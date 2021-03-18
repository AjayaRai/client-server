using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CsharpServer
{
    class Program
    {
        private const int port = 8888;

        private static readonly StockMarket stockMarket = new StockMarket();

        static void Main(string[] args)
        {
            RunServer();
        }

        private static void RunServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            Console.WriteLine("Waiting for incoming connections...");
            while (true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                new Thread(HandleIncomingConnection).Start(tcpClient);
            }
        }

        private static void HandleIncomingConnection(object param)
        {
            TcpClient tcpClient = (TcpClient) param;
            using (Stream stream = tcpClient.GetStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                StreamReader reader = new StreamReader(stream);
                int traderId = stockMarket.createNewTraderId();
                try
                {
                    stockMarket.createTrader(traderId);
                    Console.WriteLine($"New connection; customer ID {traderId}");

                    if (traderId == 1) {
                        stockMarket.setStockOwner(traderId);
                    } else {
                        stockMarket.unsetStockOwner(traderId);
                    }

                    writer.WriteLine("SUCCESS");
                    writer.Flush();

                    while (true) {
                        string line = reader.ReadLine();
                        string[] substrings = line.Split(' ');

                        switch (substrings[0].ToLower()) {
                            case "logged_in_user_id":
                                writer.WriteLine(traderId);
                                writer.Flush();
                                break;
                           
                            case "traders":
                                int[] listOfTradersId = stockMarket.getListOfTradersId();
                                writer.WriteLine(listOfTradersId.Length);
                                
                                foreach (int trader in listOfTradersId)
                                    writer.WriteLine(trader);
                                writer.Flush();
                                break;
                            
                            case "stock_owner":
                                int stockOwner = stockMarket.getStockOwner();
                                // -1 is returned when all of the traders leave, and then later others join again where no one has the Stock. So, the stock is again need to be assigned
                                
                                if (stockOwner == -1) {
                                    stockMarket.automaticallyTransferStock();
                                }
                                stockOwner = stockMarket.getStockOwner();
                                writer.WriteLine(stockOwner);
                                writer.Flush();
                                break;
                            
                            case "give_stock":
                                if (traderId == stockMarket.getStockOwner()) {
                                    int toTraderId = int.Parse(substrings[1]);
                                    Console.WriteLine("*** giverId: " + traderId);
                                    Console.WriteLine("*** receiverId: " + toTraderId);
                                    stockMarket.giveStock(traderId, toTraderId);
                                    writer.WriteLine("SUCCESS");
                                    writer.Flush();
                                } else {
                                    writer.WriteLine("NOT_SUCCESS");
                                    writer.Flush();
                                }
                                break;
                            default:
                                throw new Exception($"Unknown command: {substrings[0]}.");
                        }
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        writer.WriteLine("ERROR " + e.Message);
                        writer.Flush();
                        tcpClient.Close();
                    }
                    catch
                    {
                        Console.WriteLine("Failed to send error message.");
                    }
                }
                finally
                {
                    if (stockMarket.getStockOwner() == traderId) {
                        stockMarket.removeTraderOnceDisconnected(traderId);
                        if (stockMarket.getListOfTradersId().Length > 0) {
                            stockMarket.automaticallyTransferStock();
                        }
                    } else {
                    stockMarket.removeTraderOnceDisconnected(traderId);
                    }
                    Console.WriteLine($"Trader {traderId} disconnected.");
                }
            }
        }
    }
}

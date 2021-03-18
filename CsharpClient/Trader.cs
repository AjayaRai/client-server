using System;
using System.IO;
using System.Net.Sockets;

namespace CsharpClient {
    class Trader : IDisposable {
        const int port = 8888;

        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        public Trader() {
        // Connecting to the server and creating objects for communications
        TcpClient tcpClient = new TcpClient("localhost", port);
            NetworkStream stream = tcpClient.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            // Parsing the response
            string line = reader.ReadLine();
            if (line.Trim().ToLower() != "success")
                throw new Exception(line);
        }

        public int[] GetTradersId() {
            // Sending command
            writer.WriteLine("TRADERS");
            writer.Flush();

            // Reading the number of traders
            String line = reader.ReadLine();
            int numberOfTraders = int.Parse(line);

            // Reading the traders id
            int[] traders = new int[numberOfTraders];
            for (int i = 0; i < numberOfTraders; i++) {
                line = reader.ReadLine();
                traders[i] = int.Parse(line);
            }

            return traders;
        }

        public int GetStockOwner() {
            writer.WriteLine("STOCK_OWNER");
            writer.Flush();

            String line = reader.ReadLine();
            int stockOwner = int.Parse(line);

            return stockOwner;
        }

        public void GiveStock(String toTrader) {
            writer.WriteLine("GIVE_STOCK " + toTrader);
            writer.Flush();

            String line = reader.ReadLine();

            if (line.Trim().ToLower() != "success")
                throw new Exception(line);
        }

        public String GetUserId() {
            writer.WriteLine("LOGGED_IN_USER_ID");
            writer.Flush();

            String line = reader.ReadLine();
            return line;
        }

        public void Dispose()
        {
            reader.Close();
            writer.Close();
        }
    }
}
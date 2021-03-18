using System;

namespace CsharpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Trader trader = new Trader()) {
                    Console.WriteLine("Logged in successfully.");

                    while (true) {
                        Console.WriteLine("View my ID (M), live traders (T), Stock Owner (O), Give Stock(G):");

                        String choice = Console.ReadLine().Trim().ToUpper();

                        switch(choice) {

                            case "M":
                                Console.WriteLine("My ID: " + trader.GetUserId());
                                break;

                            case "T":
                                int[] tradersId = trader.GetTradersId();
                                Console.WriteLine("Live traders:");

                                foreach (int traderId in tradersId) {
                                    Console.WriteLine("    Trader ID " + traderId);
                                }
                                break;

                            case "O":
                                int stockOwner = trader.GetStockOwner();
                                Console.WriteLine("Stock Owner ID: " + stockOwner);
                                break;

                            case "G":
                                Console.WriteLine("Enter the trader ID whom you would like to give the Stock to:");
                                String giveStockTo = Console.ReadLine();
                                Console.WriteLine(giveStockTo);
                                trader.GiveStock(giveStockTo);
                                break;
                                
                            default:
                                Console.WriteLine("Unknown command: " + choice);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

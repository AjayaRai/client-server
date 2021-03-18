using System;
using System.Collections.Generic;

namespace CsharpServer{
    class StockMarket {
        private readonly SortedDictionary<int, Trader> traders = new SortedDictionary<int, Trader>();
        int maxTraderNumber = 0;

        public int createNewTraderId() {
            lock (traders) {


                foreach (int trader in traders.Keys) {
                    if (maxTraderNumber < trader) {
                        maxTraderNumber = trader;
                    }
                }
                int newTraderNumber = maxTraderNumber + 1;
                return newTraderNumber;
            }
        }

        public void createTrader(int traderId) {
            Trader trader = new Trader(traderId);
            traders.Add(traderId, trader);
        }

        public int[] getListOfTradersId() {
            lock (traders) {
                List<int> result = new List<int>();

                foreach (int trader in traders.Keys)
                    result.Add(trader);

                return result.ToArray();
            }
        }

        public int getStockOwner() {
            lock (traders) {
                int stockOwner = -1;
                
                foreach (KeyValuePair<int, Trader> entry in traders) {
                    if (entry.Value.GetHasStock()) {
                        stockOwner = entry.Key;
                    }
                }
                return stockOwner;
            }
        }

        public void setStockOwner(int traderId) {
            lock (traders) {
                Trader trader = traders[traderId];
                trader.SetHasStockToTrue();
            }
        }

        public void unsetStockOwner(int traderId) {
            lock (traders) {
                Trader trader = traders[traderId];
                trader.SetHasStockToFalse();
            }
        }

        public void automaticallyTransferStock() {
            lock (traders) {
                Trader trader = getNextTrader(traders);
                Console.WriteLine("Automatically transfer stock: " + trader.TraderId);
                trader.SetHasStockToTrue();
            }
        }

        // TODO make this code more efficient? 
        // public Trader getNextTrader(SortedDictionary<int, Trader> traders) {
        //     int x = 0;
        //     Trader result;
        //     using(var iter = traders.GetEnumerator()) {
        //         while (iter.MoveNext()) {
        //             if (x == 1) {
        //                 result = iter.Current.Value;
        //                 return result;
        //             }
        //             x++;
        //         }
        //     }
        //     return null;
            
        // }
        public Trader getNextTrader(SortedDictionary<int, Trader> traders) {
            foreach (KeyValuePair<int, Trader> kvp in traders) {
                return kvp.Value;
            }
            return null;
        }

        public void removeTraderOnceDisconnected(int traderId) {
            lock (traders) {
                traders.Remove(traderId);
            }
        }

        public void giveStock(int fromTraderId, int toTraderId) {
            this.unsetStockOwner(fromTraderId);
            this.setStockOwner(toTraderId);
        }
    }
}
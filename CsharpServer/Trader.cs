namespace CsharpServer
{
    class Trader {
        bool hasStock;

        public Trader(int traderId) {
            TraderId = traderId;
        }

        public int TraderId {
            get;
        }

        public void SetHasStockToTrue() {
            hasStock = true;
        }

        public void SetHasStockToFalse() {
            hasStock = false;
        }

        public bool GetHasStock() {
            return hasStock;
        }
    }
}
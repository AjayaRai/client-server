import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

public class StockMarket {
    private final Map<Integer, Trader> traders = new TreeMap<>();
    int maxTraderNumber = 0;

    public Integer createNewTraderId() {
        synchronized (traders) {


            for (Integer trader : traders.keySet()) {
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
        traders.put(traderId, trader);
    }

    public List<Integer> getListOfTradersId() {
        synchronized (traders) {
            List<Integer> result = new ArrayList<Integer>();

            for (Integer trader : traders.keySet())
                result.add(trader);

            return result;
        }
    }

    public Integer getStockOwner() {
        synchronized (traders) {
            int stockOwner = -1;
            for (Map.Entry<Integer, Trader> entry : traders.entrySet()) {
                if (entry.getValue().getHasStock()) {
                    stockOwner = entry.getKey();
                }
            }
            return stockOwner;
        }
    }

    public void setStockOwner(int traderId) {
        synchronized (traders) {
            Trader trader = traders.get(traderId);
            trader.setHasStockToTrue();
        }
    }

    public void unsetStockOwner(int traderId) {
        synchronized (traders) {
            Trader trader = traders.get(traderId);
            trader.setHasStockToFalse();
        }
    }

    public void automaticallyTransferStock() {
        synchronized (traders) {
            Map.Entry<Integer, Trader> entry = traders.entrySet().iterator().next();
            Trader trader = entry.getValue();
            trader.setHasStockToTrue();
        }
    }

    public void removeTraderOnceDisconnected(int traderId) {
        synchronized (traders) {
            traders.remove(traderId);
        }
    }

    public void giveStock(int fromTraderId, int toTraderId) {
        this.unsetStockOwner(fromTraderId);
        this.setStockOwner(toTraderId);
    }
}

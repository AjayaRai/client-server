public class Trader {
    private final int traderId;
    private boolean hasStock;

    public Trader(int traderId) {
        this.traderId = traderId;
    }

    public int getTraderId() {
        return traderId;
    }

    public void setHasStockToTrue() {
        hasStock = true;
    }

    public void setHasStockToFalse() {
        hasStock = false;
    }

    public boolean getHasStock() {
        return hasStock;
    }
}

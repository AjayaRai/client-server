import java.io.PrintWriter;
import java.net.Socket;
import java.util.List;
import java.util.Scanner;

public class TraderHandler implements Runnable {
    private final Socket socket;
    private StockMarket stockMarket;

    public TraderHandler(Socket socket, StockMarket stockMarket) {
        this.socket = socket;
        this.stockMarket = stockMarket;
    }

    @Override
    public void run() {
        int traderId = stockMarket.createNewTraderId();
        try (
                Scanner scanner = new Scanner(socket.getInputStream());
                PrintWriter writer = new PrintWriter(socket.getOutputStream(), true)
            ) {
            try {
                stockMarket.createTrader(traderId);
                System.out.println("New connection; trader ID " + traderId);
                if (traderId == 1) {
                    stockMarket.setStockOwner(traderId);
                } else {
                    stockMarket.unsetStockOwner(traderId);
                }

                writer.println("SUCCESS");

                while (true) {
                    String line = scanner.nextLine();
                    String[] substrings = line.split(" ");

                    switch (substrings[0].toLowerCase()) {
                        case "logged_in_user_id":
                            writer.println(traderId);
                            break;
                        case "traders":
                            List<Integer> listOfTradersId = stockMarket.getListOfTradersId();
                            writer.println(listOfTradersId.size());
                            for (Integer trader : listOfTradersId)
                                writer.println(trader);
                            break;
                        case "stock_owner":
                            Integer stockOwner = stockMarket.getStockOwner();
                            // -1 is returned when all of the traders leave, and then later others join again where no one has the Stock. So, the stock is again need to be assigned
                            if (stockOwner == -1) {
                                stockMarket.automaticallyTransferStock();
                            }
                            stockOwner = stockMarket.getStockOwner();
                            writer.println(stockOwner);
                            break;
                        case "give_stock":
                            if (traderId == stockMarket.getStockOwner()) {
                                int toTraderId = Integer.parseInt(substrings[1]);
                                System.out.println("*** giverId: " + traderId);
                                System.out.println("*** receiverId: " + toTraderId);
                                stockMarket.giveStock(traderId, toTraderId);
                                writer.println("SUCCESS");
                            } else {
                                writer.println("NOT_SUCCESS");
                            }
                            break;
                        default:
                            throw new Exception("Unknown command: " + substrings[0]);
                    }
                }
            } catch (Exception e) {
                writer.println("ERROR " + e.getMessage());
                socket.close();
            }
        } catch (Exception e) {

        } finally {
            if (stockMarket.getStockOwner() == traderId) {
                stockMarket.removeTraderOnceDisconnected(traderId);
                if (stockMarket.getListOfTradersId().size() > 0) {
                    stockMarket.automaticallyTransferStock();
                }
            } else {
                stockMarket.removeTraderOnceDisconnected(traderId);
            }
            System.out.println("Trader " + traderId + " disconnected.");
        }
    }
}

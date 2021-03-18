import java.util.Scanner;

public class TraderProgram {
    public static void main(String[] args) {

        try {
            Scanner in = new Scanner(System.in);

            try (Trader trader = new Trader()) {
                System.out.println("Logged in successfully.");

                while (true) {
                    System.out.println("View my ID (M), live traders (T), Stock Owner (O), Give Stock(G):");
                    String choice = in.nextLine().trim().toUpperCase();
                    switch (choice) {
                        case "M":
                            System.out.println("My ID: " + trader.getUserId());
                            break;
                        case "T":
                            int[] tradersId = trader.getTradersId();
                            System.out.println("Live traders:");
                            for (int traderId : tradersId) {
                                System.out.println("    Trader ID " + traderId);
                            }
                            break;
                        case "O":
                            int stockOwner = trader.getStockOwner();
                            System.out.println("Stock Owner ID: " + stockOwner);
                            break;
                        case "G":
                            System.out.println("Enter the trader ID whom you would like to give the Stock to:");
                            String giveStockTo = in.nextLine();
                            //System.out.println(giveStockTo);
                            trader.giveStock(giveStockTo);
                            //System.out.println("END");
                            break;
                        default:
                            System.out.println("Unknown command: " + choice);
                            break;
                    }

                }
            }
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }
    }
}

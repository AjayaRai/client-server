import java.io.*;
import java.net.Socket;
import java.util.Scanner;

public class Trader implements AutoCloseable {
    final int port = 8888;

    private final Scanner reader;
    private final PrintWriter writer;

    public Trader() throws Exception {
        // Connecting to the server and creating objects for communications
        Socket socket = new Socket("localhost", port);
        reader = new Scanner(socket.getInputStream());

        // Automatically flushes the stream with every command
        writer = new PrintWriter(socket.getOutputStream(), true);

        // Parsing the response
        String line = reader.nextLine();
        if (line.trim().compareToIgnoreCase("success") != 0)
            throw new Exception(line);
    }

    public int[] getTradersId() {
        // Sending command
        writer.println("TRADERS");

        // Reading the number of traders
        String line = reader.nextLine();
        int numberOfTraders = Integer.parseInt(line);

        // Reading the traders id
        int[] traders = new int[numberOfTraders];
        for (int i = 0; i < numberOfTraders; i++) {
            line = reader.nextLine();
            traders[i] = Integer.parseInt(line);
        }

        return traders;
    }

    public int getStockOwner() {
        writer.println("STOCK_OWNER");

        String line = reader.nextLine();
        int stockOwner = Integer.parseInt(line);

        return stockOwner;
    }

    public void giveStock(String toTrader) throws Exception {
        writer.println("GIVE_STOCK " + toTrader);

        String line = reader.nextLine();

        if (line.trim().compareToIgnoreCase("success") != 0)
            throw new Exception(line);
    }

    public String getUserId() {
        writer.println("LOGGED_IN_USER_ID");

        String line = reader.nextLine();
        return line;
    }

    @Override
    public void close() throws Exception {
        reader.close();
        writer.close();
    }
}

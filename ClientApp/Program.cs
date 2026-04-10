using System;
using System.IO;
using System.Net.Sockets;
using ClientApp.Services;

class Program
{
    static void Main()
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000);

            var encryptionService = new EncryptionService();

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            Console.Write("Enter message (e.g., SetA-Two): ");
            string message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Invalid input. Please enter something like SetA-Two");
                return;
            }

            string encryptedMessage = encryptionService.Encrypt(message);

            Console.WriteLine("Original: " + message);

            writer.WriteLine(encryptedMessage);
            writer.Flush();

            Console.WriteLine("Server response:");

            string response;
            while ((response = reader.ReadLine()) != null)
            {
                string decrypted = encryptionService.Decrypt(response);
                Console.WriteLine(decrypted);
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000);

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            Console.Write("Enter message (e.g., SetA-Two): ");
            string message = Console.ReadLine();

            string encryptedMessage = Encrypt(message);
            writer.WriteLine(encryptedMessage);

            Console.WriteLine("Server response:");

            string response;
            while ((response = reader.ReadLine()) != null)
            {
                string decrypted = Decrypt(response);
                Console.WriteLine(decrypted);
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static string Encrypt(string text)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }

    static string Decrypt(string base64Text)
    {
        byte[] bytes = Convert.FromBase64String(base64Text);
        return Encoding.UTF8.GetString(bytes);
    }
}
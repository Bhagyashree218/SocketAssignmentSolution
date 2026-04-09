using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();

        Console.WriteLine("Server started... Waiting for client...");

        var data = new Dictionary<string, Dictionary<string, int>>()
        {
            { "SetA", new Dictionary<string, int> { { "One", 1 }, { "Two", 2 } } },
            { "SetB", new Dictionary<string, int> { { "Three", 3 }, { "Four", 4 } } },
            { "SetC", new Dictionary<string, int> { { "Five", 5 }, { "Six", 6 } } },
            { "SetD", new Dictionary<string, int> { { "Seven", 7 }, { "Eight", 8 } } },
            { "SetE", new Dictionary<string, int> { { "Nine", 9 }, { "Ten", 10 } } }
        };

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();

            Thread thread = new Thread(() => HandleClient(client, data));
            thread.Start();
        }
    }

    static void HandleClient(TcpClient client, Dictionary<string, Dictionary<string, int>> data)
    {
        try
        {
            Console.WriteLine("Client connected!");

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            string encryptedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
            string clientMessage = Decrypt(encryptedMessage);

            Console.WriteLine("Received: " + clientMessage);

            string[] parts = clientMessage.Split('-');

            if (parts.Length != 2)
            {
                SendResponse(stream, "EMPTY");
                client.Close();
                return;
            }

            string setName = parts[0];
            string keyName = parts[1];

            if (!data.ContainsKey(setName))
            {
                SendResponse(stream, "EMPTY");
                client.Close();
                return;
            }

            var innerDict = data[setName];

            if (!innerDict.ContainsKey(keyName))
            {
                SendResponse(stream, "EMPTY");
                client.Close();
                return;
            }

            int value = innerDict[keyName];

            for (int i = 0; i < value; i++)
            {
                string time = DateTime.Now.ToString("HH:mm:ss");
                SendResponse(stream, time);
                Thread.Sleep(1000);
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

    static void SendResponse(NetworkStream stream, string message)
    {
        string encrypted = Encrypt(message);
        byte[] responseBytes = Encoding.UTF8.GetBytes(encrypted + "\n");
        stream.Write(responseBytes, 0, responseBytes.Length);
    }
}
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Services
{
    public class SocketService : ISocketService
    {
        private readonly IDataService _dataService;
        private readonly IEncryptionService _encryptionService;
        private readonly ILoggerService _logger;
        private readonly int _port;

        public SocketService(
            IDataService dataService,
            IEncryptionService encryptionService,
            ILoggerService logger,
            int port)
        {
            _dataService = dataService;
            _encryptionService = encryptionService;
            _logger = logger;
            _port = port;
        }

        public async Task StartAsync()
        {
            _logger.LogInfo($"Starting server on port {_port}...");
            var listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using (client)
                using (var stream = client.GetStream())
                {
                    var buffer = new byte[4096];

                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        _logger.LogInfo("Client disconnected");
                        return;
                    }

                    string encrypted = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    _logger.LogInfo("Encrypted received: " + encrypted);

                    string message = _encryptionService.Decrypt(encrypted);
                    _logger.LogInfo("Decrypted message: " + message);

                    var parts = message.Split('-');

                    if (parts.Length != 2)
                    {
                        await SendAsync(stream, "EMPTY");
                        return;
                    }

                    var value = _dataService.GetValue(parts[0], parts[1]);

                    if (value == null)
                    {
                        await SendAsync(stream, "EMPTY");
                        return;
                    }

                    for (int i = 0; i < value; i++)
                    {
                        await SendAsync(stream, DateTime.Now.ToString("HH:mm:ss"));
                        await Task.Delay(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task SendAsync(NetworkStream stream, string message)
        {
            string encrypted = _encryptionService.Encrypt(message);
            byte[] data = Encoding.UTF8.GetBytes(encrypted + "\n");

            await stream.WriteAsync(data, 0, data.Length);
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ServerApp.Services;

namespace ServerApp;
class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        int port = int.Parse(config["ServerSettings:Port"]);
        string key = config["EncryptionSettings:Key"];
        string iv = config["EncryptionSettings:IV"];

        IDataService dataService = new DataService();
        IEncryptionService encryptionService = new EncryptionService(key, iv);
        ILoggerService logger = new LoggerService();

        ISocketService socketService =
            new SocketService(dataService, encryptionService, logger, port);


        await socketService.StartAsync();
    }
}
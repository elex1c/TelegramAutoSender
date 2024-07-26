using WTelegram;

namespace TelegramSenderScript.Models;

public class Account(int app_id, string app_hash, string sessionPath)
{
    public Client TelegramClient { get; set; } 
        = new Client(app_id, app_hash, sessionPath);

    public bool IsConnected { get; private set; } = false;

    public async Task Connect()
    {
        await TelegramClient.ConnectAsync();
        
        IsConnected = TelegramClient.User != null;
    }
}
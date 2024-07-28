using WTelegram;

namespace TelegramSenderScript.Models;

public class Account : IDisposable
{
    public Client? TelegramClient { get; set; }
    public long UserId => TelegramClient?.UserId ?? 0;
    public bool IsConnected { get; private set; }
    public bool IsBanned { get; set; }
    
    public Account(int app_id, string app_hash, string sessionPath)
    {
        try
        {
            TelegramClient = new Client(app_id, app_hash, sessionPath);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("BAN") || ex.Message.Contains("DELETED_ACCOUNT")) IsBanned = true;
        }
    }
    
    public async Task Connect()
    {
        if (TelegramClient is null) return;
        
        await TelegramClient.ConnectAsync();
        
        IsConnected = TelegramClient.TLConfig is not null && TelegramClient.UserId != 0;
    }

    public void Dispose()
    {
        TelegramClient?.Dispose();
    }
}
using TelegramSenderScript.Models;

namespace TelegramSenderScript.Control;

public class AccountsControl : IDisposable
{
    public List<Account> Accounts { get; set; } = [];
    public List<long> BannedAccountUserIds { get; set; } = new();
    
    public int AccountsCount => Accounts.Count;
    public int BannedAccounts => BannedAccountUserIds.Count;
    private int CurrentAccountIndex { get; set; }

    public Account? GetNewAccount()
    {
        if (CurrentAccountIndex == AccountsCount) return null;
        
        CurrentAccountIndex++;

        return Accounts[CurrentAccountIndex - 1];
    }
    
    public void Dispose()
    {
        foreach (Account account in Accounts) account.Dispose();
    }
}
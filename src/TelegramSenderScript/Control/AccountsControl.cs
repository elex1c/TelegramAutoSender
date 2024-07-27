using TelegramSenderScript.Models;

namespace TelegramSenderScript.Control;

public class AccountsControl
{
    public List<Account> Accounts { get; set; } = [];

    public int AccountsCount => Accounts.Count;
}
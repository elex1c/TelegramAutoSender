using TelegramSenderScript;
using TelegramSenderScript.Control;
using TelegramSenderScript.ExtendedFunctions;
using TelegramSenderScript.Models;

// © 2024 Elexic. All rights reserved.
// This software is the confidential and proprietary information of Elexic Software Solutions.
// You shall not disclose such Confidential Information and shall use it
// only in accordance with the terms of the license agreement you entered into with Elexic.

// TelegramSenderScript
// Version: 1.4
// Developed by: Elexic

// This notice may not be removed or altered from any source distribution.

#region Configuration getting

    Config config = new Config();

    string? apiDataFilePath = Helper.GetPath(config.Configuration["Paths:ApiDataPath"],
        fileExceptionName: "ApiDataPath");

    if (apiDataFilePath is null) return;

    string? groupsPath = Helper.GetPath(config.Configuration["Paths:TelegramGroupsPath"],
        fileExceptionName: "TelegramGroupsPath");

    if (groupsPath is null) return;

#endregion

#region Api Data Reading

    string[] apiDataLines = File.ReadAllLines(apiDataFilePath);

    using AccountsControl accountController = new AccountsControl();

    int expectedAccountsAmount = apiDataLines.Length;
    int validApiDataCount = 0;
    int invalidApiDataCount = 0;

    for (int i = 0; i < apiDataLines.Length; i++)
    {
        TelegramConnectionData tgData = new(apiDataLines[i]);

        if (tgData.IsDataLoadedCorrectly()) validApiDataCount++;
        else
        {
            invalidApiDataCount++;
            continue;
        }
                
        accountController.Accounts.Add(new Account(tgData.ApiId, tgData.AppHash, tgData.SessionPath));
    }

    Helper.ConsoleWriteLineGreen("Valid API data lines: " + validApiDataCount);
    Helper.ConsoleWriteLineRed("Invalid API data lines: " + invalidApiDataCount);
    Console.WriteLine("Total API data lines: " + expectedAccountsAmount);

#endregion

Console.WriteLine("\n-------------------\n");
    
#region Telegram Groups Reading

    string[] groupLines = File.ReadAllLines(groupsPath);
    
    List<TelegramGroup> groups = new();
    
    for (int i = 0; i < groupLines.Length; i++)
    {
        groups.Add(new TelegramGroup(groupLines[i].Replace("@", "")));    
    }

    int validGroupsCount = groups.Count(x => !string.IsNullOrEmpty(x.GroupSource));
    
    Helper.ConsoleWriteLineGreen("Successfully read telegram groups: " + validGroupsCount);
    Console.WriteLine("Total telegram groups: " + groupLines.Length);
    
    groups = groups.Where(x => !string.IsNullOrEmpty(x.GroupSource))
        .ToList();
    
        
    
#endregion

Console.WriteLine("\n-------------------\n");

if (validApiDataCount == 0 || validGroupsCount == 0)
{
    Helper.ConsoleWriteLineRed("You can't send message when you have no valid APIs for telegram or valid groups");
    return;
}
    
#region Sending messages

    string message = "";
    while (string.IsNullOrEmpty(message))
    {
        Console.Write("Message: ");
        message = Console.ReadLine()!;
    }
    
    int sentMessagesCount = 0;
    int bannedAccounts = 0;

    Account? account = accountController.GetNewAccount();
    for (int i = 0; i < groups.Count; i++)
    {
        ConnectNewAccountOrUseOld:
            if (account is null) break;

            if (!account.IsConnected)
            {
                await account.Connect();
                if (!account.IsConnected)
                {
                    if (account.IsBanned)
                        accountController.BannedAccountUserIds?.Add(account.UserId);
                    account = accountController.GetNewAccount();
                    goto ConnectNewAccountOrUseOld;
                }
            }
        
        bool result;
        if (groups[i].IsUrl)
            result = await account.SendMessage(new Uri(groups[i].GroupSource!), message);
        else 
            result = await account.SendMessage(groups[i].GroupSource!, message);

        if (result)
        {
            sentMessagesCount++;
            Helper.ConsoleWriteLineGreen("Message was sent to " + groups[i].GroupSource);
        }
        else
        {
            Helper.ConsoleWriteLineRed("Message wasn't sent to " + groups[i].GroupSource);
        }

        if (account.IsBanned && account.UserId != 0)
        {
            Helper.ConsoleWriteLineRed("UserId " + account.UserId + " was banned");
        }
    }

#endregion
    
Console.WriteLine("\n-------------------\n");

#region End
    
    Helper.ConsoleWriteLineGreen("Successfully sent messages: " + sentMessagesCount);
    Helper.ConsoleWriteLineRed("Message weren't sent: " + (validGroupsCount - sentMessagesCount));
    Helper.ConsoleWriteLineGreen("Alive accounts: " + (validApiDataCount - bannedAccounts));
    Helper.ConsoleWriteLineRed("Banned accounts: " + bannedAccounts);
    
#endregion

Console.ReadKey();

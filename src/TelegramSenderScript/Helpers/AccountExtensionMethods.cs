using TelegramSenderScript.Models;
using TL;

namespace TelegramSenderScript.Helpers;

public static class AccountExtensionMethods
{
    public static async Task<bool> IsUserInChannel(this Account account, long channelId)
    {
        Messages_Chats? chats = await account.TelegramClient
            .Messages_GetAllChats();
        return chats.chats
            .ContainsKey(channelId);
    }

    public static async Task<bool> IsUserInChannel(this Account account, string channelUsername)
    {
        Messages_Chats? chats = await account.TelegramClient
            .Messages_GetAllChats();
        return chats.chats
            .FirstOrDefault(x => x.Value.MainUsername == channelUsername)
            .Value != null;
    }

    public static async Task<bool> JoinGroup(this Account account, string channelUsername)
    {
        UpdatesBase? basse;
        Contacts_ResolvedPeer? info;
        try
        {
            info = await account.TelegramClient.Contacts_ResolveUsername(channelUsername);
            basse = await account.TelegramClient.Channels_JoinChannel(info.Channel);
        }
        catch (Exception) { return false; }

        if (basse is not null)
            return basse.Chats.ContainsKey(info.Channel.ID);
        return false;
    }

    public static async Task<bool> SendMessage(this Account account, string channelUsername, string message)
    {
        if (!await account.IsUserInChannel(channelUsername))
            if (!await account.JoinGroup(channelUsername))
                return false;

        InputPeer? chatPeer = (await account.TelegramClient.Messages_GetAllChats())
            .chats
            .FirstOrDefault(x
                => x.Value.MainUsername == channelUsername.Replace("@", ""))
            .Value?
            .ToInputPeer();

        if (chatPeer is null) return false;
        try
        {
            await account.TelegramClient.SendMessageAsync(chatPeer, message);
            return true;
        }
        catch (Exception) { return false; }
    }
}

using TelegramSenderScript.Models;
using TL;

namespace TelegramSenderScript.ExtendedFunctions;

public static class AccountExtensionMethods
{
    public static async Task<bool> IsUserInChannel(this Account account, string channelUsername)
    {
        if (account.TelegramClient is null || !account.IsConnected) return false;

        Messages_Chats? chats;
        try
        {
            chats = await account.TelegramClient
                .Messages_GetAllChats();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("BAN") || ex.Message.Contains("DELETED_ACCOUNT"))
                account.IsBanned = true;
            
            return false;
        }
        
        return chats.chats
            .FirstOrDefault(x => x.Value.MainUsername == channelUsername)
            .Value != null;
    }

    public static async Task<Contacts_ResolvedPeer?> GetGroupInfo(this Account account, string channelUsername)
    {
        if (account.TelegramClient is null || !account.IsConnected) return null;

        Contacts_ResolvedPeer? info;
        try
        {
            info = await account.TelegramClient.Contacts_ResolveUsername(channelUsername);
        }
        catch (Exception) { return null; }
        
        return info;
    }
    
    public static async Task<ChatBase?> GetGroupInfo(this Account account, Uri uri, bool join = false)
    {
        if (account.TelegramClient is null || !account.IsConnected) return null;

        ChatBase? info;
        try
        {
            info = await account.TelegramClient.AnalyzeInviteLink(uri.AbsoluteUri, join);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("USER_ALREADY_PARTICIPANT"))
                return await account.TelegramClient.AnalyzeInviteLink(uri.AbsoluteUri);
            else if (ex.Message.Contains("BAN") || ex.Message.Contains("DELETED_ACCOUNT"))
                account.IsBanned = true;
                
            return null;
        }
        
        return info;
    }
    
    public static async Task<bool> JoinGroup(this Account account, string channelUsername)
    {
        if (account.TelegramClient is null || !account.IsConnected) return false;
        
        UpdatesBase? updatedChatBase;
        Contacts_ResolvedPeer? info;
        try
        {
            info = await account.TelegramClient.Contacts_ResolveUsername(channelUsername);
            updatedChatBase = await account.TelegramClient.Channels_JoinChannel(info.Channel);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("BAN") || ex.Message.Contains("DELETED_ACCOUNT"))
                account.IsBanned = true;
            
            return false;
        }

        if (updatedChatBase is not null)
            return updatedChatBase.Chats.ContainsKey(info.Channel.ID);
        return false;
    }

    public static async Task<bool> SendMessage(this Account account, string channelUsername, string message)
    {
        if (account.TelegramClient is null || !account.IsConnected) return false;
        
        if (!await account.IsUserInChannel(channelUsername))
            if (!await account.JoinGroup(channelUsername))
                return false;

        InputPeer? chatPeer = await GetInputPeer(account, channelUsername);

        if (chatPeer is null) return false;
        try
        {
            await account.TelegramClient.SendMessageAsync(chatPeer, message);
            return true;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("BAN") || ex.Message.Contains("DELETED_ACCOUNT"))
                account.IsBanned = true;
            
            return false;
        }
    }

    public static async Task<bool> SendMessage(this Account account, Uri uri, string message)
    {
        if (account.TelegramClient is null || !account.IsConnected) return false;
        
        ChatBase? chatInfo = await account.GetGroupInfo(uri, true);
        if (chatInfo is null || !chatInfo.IsGroup || chatInfo.IsBanned())
            return false;

        InputPeer? inputPeer = chatInfo.ToInputPeer(); 
        
        if (inputPeer is null) return false;
        try
        {
            await account.TelegramClient.SendMessageAsync(inputPeer, message);
            return true;
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("BAN") || ex.Message.Contains("DELETED_ACCOUNT"))
                account.IsBanned = true;
            
            return false;
        }
    }

    #region Private help methods

    private static async Task<InputPeer?> GetInputPeer(Account account, string channelUsername)
    {
        if (account.TelegramClient is null || !account.IsConnected) return null;
        
        return (await account.TelegramClient.Messages_GetAllChats())
            .chats
            .FirstOrDefault(x
                => x.Value.MainUsername == channelUsername)
            .Value?
            .ToInputPeer();
    }
    
    private static async Task<InputPeer?> GetInputPeer(Account account, long channelId)
    {
        if (account.TelegramClient is null || !account.IsConnected) return null;
        
        return (await account.TelegramClient.Messages_GetAllChats())
            .chats
            .FirstOrDefault(x 
                => x.Key == channelId)
            .Value?
            .ToInputPeer();
    }

    #endregion
}

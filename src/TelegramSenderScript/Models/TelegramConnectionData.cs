namespace TelegramSenderScript.Models;

public class TelegramConnectionData
{
    public int ApiId { get; set; }
    public string AppHash { get; set; } = string.Empty;
    public string SessionPath { get; set; } = string.Empty;

    public bool IsDataLoadedCorrectly() => ApiId != default
                                           && !string.IsNullOrEmpty(AppHash) 
                                           && !string.IsNullOrEmpty(SessionPath);

    public TelegramConnectionData(string dataLine)
    {
        string[] splitData = dataLine.Split(';');

        if (splitData.Length != 3) return;

        if (int.TryParse(splitData[0], null, out int apiId)) 
            ApiId = apiId;
        AppHash = splitData[1];
        SessionPath = splitData[2];
    }
}
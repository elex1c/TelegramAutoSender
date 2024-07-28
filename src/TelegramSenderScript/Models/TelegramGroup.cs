using System.Text.RegularExpressions;

namespace TelegramSenderScript.Models;

public class TelegramGroup
{
    public string? GroupSource { get; set; }
    public bool IsUrl { get; set; }
    
    public TelegramGroup(string line)
    {
        string urlPattern = @"^https:\/\/t\.me\/" + 
                         @"([a-zA-Z0-9_\/-]+)?" +
                         @"(\?[a-zA-Z0-9=&]*)?$";
        
        string usernamePattern = "^[a-zA-Z][a-zA-Z0-9_]{4,31}$";

        line = line.Trim();

        if (Regex.IsMatch(line, urlPattern)) 
        {
            GroupSource = line;
            IsUrl = true;
        }
        else if (Regex.IsMatch(line, usernamePattern))
        {
            GroupSource = line;
            IsUrl = false;
        }
    }
}


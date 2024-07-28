namespace TelegramSenderScript;

public static class Helper
{
    public static string? GetPath(string? path, string requiredFileExtension = ".txt", string fileExceptionName = "File")
    {
        if (string.IsNullOrEmpty(path))
        {
            ConsoleWriteLineRed(fileExceptionName + " parameter is empty! You can define it in appsettings.json");
            return null;
        }

        if (Path.GetExtension(path) != requiredFileExtension)
        {
            ConsoleWriteLineRed(fileExceptionName + " parameter has wrong file extension! You can change it in appsettings.json");
            return null;
        }
        
        if (path.Contains("\\"))
        {
            if (File.Exists(path)) { return path; }
            
            ConsoleWriteLineRed(fileExceptionName + " parameter is wrong! You can change it in appsettings.json");
            return null;
        }
        else
        {
            string combinedPath = Path.Combine(Directory.GetCurrentDirectory(), path);
            if (File.Exists(combinedPath)) return combinedPath;

            ConsoleWriteLineRed(fileExceptionName + " path is wrong! You can change it in appsettings.json");
            return null;
        }
    }

    public static void ConsoleWriteLineRed(string text) => ConsoleWriteLineColor(text, ConsoleColor.Red);
    public static void ConsoleWriteLineGreen(string text) => ConsoleWriteLineColor(text, ConsoleColor.Green);
    private static void ConsoleWriteLineColor(string text, ConsoleColor color)
    {
        ConsoleColor lastColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = lastColor;
    }
}
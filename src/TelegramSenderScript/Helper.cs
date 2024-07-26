namespace TelegramSenderScript;

public static class Helper
{
    public static string? GetPath(string? path, string requiredFileExtension = ".txt", string fileExceptionName = "File")
    {
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine(fileExceptionName + " path is empty! You can define it in appsettings.json");
            return null;
        }

        if (Path.GetExtension(path) != requiredFileExtension)
        {
            Console.WriteLine(fileExceptionName + " path has wrong file extension! You can change it in appsettings.json");
            return null;
        }
        
        if (path.Contains("\\"))
        {
            if (File.Exists(path)) { return path; }
            
            Console.WriteLine(fileExceptionName + " path is wrong! You can change it in appsettings.json");
            return null;
        }
        else
        {
            string combinedPath = Path.Combine(Directory.GetCurrentDirectory(), path);
            if (File.Exists(combinedPath)) return combinedPath;

            Console.WriteLine(fileExceptionName + " path is wrong! You can change it in appsettings.json");
            return null;
        }
    }
}
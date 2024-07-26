using Microsoft.Extensions.Configuration;

namespace TelegramSenderScript;

public class Config
{
    public IConfiguration Configuration { get; }
    public Config()
    {
        Configuration = ConfigSetUp();
    }

    private IConfiguration ConfigSetUp()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true);

        return builder.Build();
    }
}
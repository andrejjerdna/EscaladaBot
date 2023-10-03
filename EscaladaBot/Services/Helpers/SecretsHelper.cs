using Microsoft.Extensions.Configuration;

namespace EscaladaBot.Services.Helpers;

public static class SecretsHelper
{
    private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>()
        .Build();

    public static string GetTelegramApiToken
        => Environment.GetEnvironmentVariable("TELEGRAM_API_TOKEN")
           ?? Configuration.GetValue<string>("Telegram:ApiToken")
           ?? throw new Exception();
    
    public static string GetS3AccessKey
        => Environment.GetEnvironmentVariable("S3_ACCESS_KEY")
           ?? Configuration.GetValue<string>("S3:AccessKey")
           ?? throw new Exception();
    
    public static string GetS3SecretAccessKey
        => Environment.GetEnvironmentVariable("S3_SECRET_KEY")
           ?? Configuration.GetValue<string>("S3:SecretAccessKey")
           ?? throw new Exception();
    
    public static string GetS3Bucket
        => Environment.GetEnvironmentVariable("S3_BUCKET")
           ?? Configuration.GetValue<string>("S3:Bucket")
           ?? throw new Exception();
    
}
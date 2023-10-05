namespace backend.Utils
{
    public class ConfigHelper
    {
        public IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
    }
}

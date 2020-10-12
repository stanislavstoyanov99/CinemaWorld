namespace CinemaWorld.Services.Data.Tests
{
    using System.IO;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Configuration
    {
        public Configuration()
        {
            var serviceCollection = new ServiceCollection();

            this.ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                     path: "appsettings.json",
                     optional: false,
                     reloadOnChange: true)
               .Build();

            serviceCollection.AddSingleton<IConfiguration>(this.ConfigurationRoot);
        }

        public IConfigurationRoot ConfigurationRoot { get; private set; }
    }
}

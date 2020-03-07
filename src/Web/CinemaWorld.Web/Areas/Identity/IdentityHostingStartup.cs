using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CinemaWorld.Web.Areas.Identity.IdentityHostingStartup))]

namespace CinemaWorld.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            _ = builder.ConfigureServices((context, services) =>
              {
              });
        }
    }
}

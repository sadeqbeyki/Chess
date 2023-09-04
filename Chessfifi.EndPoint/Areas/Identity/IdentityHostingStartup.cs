using Chessfifi.EndPoint.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace Chessfifi.EndPoint.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
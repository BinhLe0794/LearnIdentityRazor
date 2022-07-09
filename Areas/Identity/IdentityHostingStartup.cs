using Microsoft.AspNetCore.Hosting;
using razorweb.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace razorweb.Areas.Identity;
public class IdentityHostingStartup : IHostingStartup
{
   public void Configure(IWebHostBuilder builder)
   {
      builder.ConfigureServices((context, services) =>
      {
      });
   }
}
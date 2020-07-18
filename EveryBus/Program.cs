using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EveryBus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSentry("https://32ca72a3968c43ad832b353620d6c018@o422150.ingest.sentry.io/5344727");
                    webBuilder.UseStartup<Startup>();
                });
    }
}

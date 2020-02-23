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
                // .ConfigureAppConfiguration((hostingContext, config) =>
                // {
                //     config.AddJsonFile(
                //         "config.json", optional: true, reloadOnChange: true);
                // })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

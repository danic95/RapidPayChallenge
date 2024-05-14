using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RapidPayChallenge.BusinessLogic;
using RapidPayChallenge.WebAPI;

namespace RapidPayChallenge
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
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices((hostContext, services) =>
                {
                    services.AddBusinessLogicDependencies();
                    services.AddWebAPIDependencies();
                });
    }
}

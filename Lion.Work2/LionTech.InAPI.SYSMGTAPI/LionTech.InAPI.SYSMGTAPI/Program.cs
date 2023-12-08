using LionTech.AspNetCore.InApi.Extensions.Hosting;
using LionTech.AspNetCore.WebApi.Extensions.DependencyInjection;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Log;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Log.Interfaces;
using LionTech.InAPI.SYSMGTAPI.HostedService;
using LionTech.InAPI.SYSMGTAPI.Service.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Utility;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LionTech.InAPI.SYSMGTAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostInApi<Program>(services =>
                {
                    services.AddServices("DataAccess", "Service");
                    services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
                    services.AddSingleton<IMisConnectionStringProvider, MisConnectionStringProvider>();
                    services.AddSingleton<IMongoConnectionProvider, MongoConnectionProvider>();
                    
                    services.AddSingleton<ISystemLogRepository, SystemLogRepository>();
                    services.AddSingleton<ISystemLogService, SystemLogService>();
                    services.AddSingleton<ISystmeLogQueues, SystmeLogQueues>();
                    services.AddHostedService<SystemLogBackgroundService>();
                });
    }
}

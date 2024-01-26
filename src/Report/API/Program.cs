using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Core.CrossCuttingConcerns.Logging.Loggers;
using DataAccess.DataContext.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API
{
    public class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables()
        .Build();

        public static void Main(string[] args)
        {
            SeriLogger.RegisterLogger(Configuration);

            try
            {
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.Migrate();
                }
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Startup Error!");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(config => config.ClearProviders()).UseSerilog()
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacBusinessModule());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

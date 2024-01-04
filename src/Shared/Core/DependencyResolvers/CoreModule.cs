using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.CrossCuttingConcerns.Localization;
using Core.CrossCuttingConcerns.Logging.Loggers;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        private IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public CoreModule(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddSingleton(WebHostEnvironment);
            services.AddSingleton<Stopwatch>();            
            services.AddSingleton<CrossCuttingConcerns.Logging.ILogger, FileLogger>();
            services.AddSingleton<IStringLocalizerFactory, StringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, StringLocalizer>();
        }
    }
}
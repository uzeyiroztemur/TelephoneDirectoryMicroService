using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.Elasticsearch;

namespace Core.CrossCuttingConcerns.Logging.Loggers
{
    public static class SeriLogger
    {
        public static void RegisterLogger(this IConfiguration configuration)
        {
            var model = configuration.GetSection("SeriLogConfig").Get<SeriLogConfig>();
            ArgumentNullException.ThrowIfNull(model);

            SelfLog.Enable(Console.Error);

            Log.Logger = new LoggerConfiguration()
                .PrepareLoggerConfig(model)
                .CreateLogger();
        }

        private static LoggerConfiguration PrepareLoggerConfig(this LoggerConfiguration loggerConfiguration, SeriLogConfig model)
        {
            return loggerConfiguration.MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Elastic.Apm", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(model.ElasticUri))
                {
                    AutoRegisterTemplate = true,
                    OverwriteTemplate = true,
                    DetectElasticsearchVersion = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    IndexFormat = $"{model.ProjectName}-{model.Environment}-logs-" + "{0:yyyy.MM.dd}",
                    ModifyConnectionSettings = s => s.BasicAuthentication(model.ElasticUser, model.ElasticPassword),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
                    FailureCallback = e => { Console.WriteLine("Unable to submit event -- " + e.RenderMessage() + " : " + e.Exception?.Message); }
                })
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", model.Environment);
        }
    }
}

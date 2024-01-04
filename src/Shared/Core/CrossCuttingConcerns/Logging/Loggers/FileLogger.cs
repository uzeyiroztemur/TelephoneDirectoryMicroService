using Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Core.CrossCuttingConcerns.Logging.Loggers
{
    public class FileLogger : ILogger
    {
        private readonly LogSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileLogger(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _settings = configuration.GetSection("Logging:Settings").Get<LogSettings>();
        }

        #region Helper
        private static string FileFormat => $"{DateTime.Now:yyyy-MM-dd}";
        private string GetMessage(Level level, string message)
        {
            var sb = new StringBuilder();

            sb.Append(DateTime.Now.ToString("HH:mm:ss")).Append('\t');
            sb.Append(_httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress).Append('\t');
            sb.Append(Enum.GetName(typeof(Level), level).ToUpper()).Append('\t');
            sb.Append(_httpContextAccessor?.HttpContext?.Request?.Method).Append('\t');
            sb.Append(_httpContextAccessor?.HttpContext?.Response?.StatusCode).Append('\t');
            sb.Append(_httpContextAccessor?.HttpContext?.Request?.Path).Append('\t');
            sb.Append(message);

            return sb.ToString();
        }
        private void Log(Level level, string logMessage)
        {
            var message = GetMessage(level, logMessage);

            _settings.Path.AppendFileWithName(FileFormat, message);
        }
        #endregion

        public void Debug(string message)
        {
            if (_settings.Enabled)
                Log(Level.Debug, message);
        }

        public void Error(string message)
        {
            if (_settings.Enabled)
                Log(Level.Error, message);
        }

        public void Fatal(string message)
        {
            if (_settings.Enabled)
                Log(Level.Fatal, message);
        }

        public void Info(string message)
        {
            if (_settings.Enabled)
                Log(Level.Info, message);
        }

        public void Warn(string message)
        {
            if (_settings.Enabled)
                Log(Level.Warn, message);
        }
    }
}

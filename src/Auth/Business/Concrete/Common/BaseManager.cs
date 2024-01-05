using Core.Utilities.IoC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Business.Concrete
{
    public class BaseManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BaseManager()
        {
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _webHostEnvironment = ServiceTool.ServiceProvider.GetService<IWebHostEnvironment>();
        }

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value;
                return userId != null ? Guid.Parse(userId) : Guid.Empty;
            }
        }

        public string LanguageCode
        {
            get
            {
                var languageCode = _httpContextAccessor.HttpContext?.Request?.Headers["Language-Code"];
                if (string.IsNullOrEmpty(languageCode))
                    languageCode = "TR";

                return languageCode.ToString().ToUpper();
            }
        }

        public string WebRootPath { get { return _webHostEnvironment.WebRootPath; } }

        private Core.CrossCuttingConcerns.Logging.ILogger logger;
        protected Core.CrossCuttingConcerns.Logging.ILogger _logger => logger ??= _httpContextAccessor.HttpContext.RequestServices.GetService<Core.CrossCuttingConcerns.Logging.ILogger>();
    }
}

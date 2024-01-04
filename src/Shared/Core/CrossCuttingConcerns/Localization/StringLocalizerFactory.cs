using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace Core.CrossCuttingConcerns.Localization
{
    public class StringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cache;

        public StringLocalizerFactory(IDistributedCache cache)
        {
            _cache = cache;
        }

        public IStringLocalizer Create(Type resourceSource) => new StringLocalizer(_cache);

        public IStringLocalizer Create(string baseName, string location) => new StringLocalizer(_cache);
    }
}

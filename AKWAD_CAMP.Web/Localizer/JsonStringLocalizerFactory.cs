using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace AKWAD_CAMP.Web.Localizer
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cashe;

        public JsonStringLocalizerFactory(IDistributedCache cashe)
        {
            _cashe = cashe;
        }
        public IStringLocalizer Create(Type resourceSource)
        {
            return new StringLocalizer(_cashe);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new StringLocalizer(_cashe);
        }
    }
}

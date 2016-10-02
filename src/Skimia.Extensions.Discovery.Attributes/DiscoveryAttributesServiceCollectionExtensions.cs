using Microsoft.Extensions.DependencyInjection;
using Skimia.Extensions.Discovery.Attributes.Abstractions;

namespace Skimia.Extensions.Discovery.Attributes
{
    public static class DiscoveryAttributesServiceCollectionExtensions
    {
        public static void AddDiscovery(this IServiceCollection services)
        {
            services.AddSingleton<IAttributeDiscover>(new AttributeDiscover());
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using SmartMapper.Core;

namespace SmartMapper.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartMapper(this IServiceCollection services)
        {
            // Tüm konfigürasyonlar burada yüklenebilir
            return services.AddSingleton<Mapper>();
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeQuality.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCodeQuality(this IServiceCollection serviceCollection,
            IConfiguration configuration) =>
            serviceCollection
                .Configure<CodeQualityRaterConfiguration>(configuration.GetSection(nameof(CodeQualityRaterConfiguration)))
                .AddScoped(sp => sp.GetService<IOptionsSnapshot<CodeQualityRaterConfiguration>>().Value)
                .AddScoped<CodeQualityRater>();
    }
}
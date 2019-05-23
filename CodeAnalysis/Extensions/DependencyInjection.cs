using CodeAnalysis.CodeAnalyzers;
using CodeAnalysis.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeAnalysis.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCodeAnalysis(this IServiceCollection serviceCollection,
            IConfiguration configuration) =>
            serviceCollection
                .AddScoped<CodeAnalyzerFactory>()
                .Configure<AnalysisConfiguration>(configuration.GetSection(nameof(AnalysisConfiguration)))
                .AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<AnalysisConfiguration>>().Value)
                .AddScoped<PhpCodeAnalyzer>() 
                .AddScoped<PascalCodeAnalyzer>()
                .AddScoped<JavaScriptCodeAnalyzer>()
                .AddScoped<CPlusPlusCodeAnalyzer>();
    }
}
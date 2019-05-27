using CodeExecution.Compilaton;
using CodeExecution.Configuration;
using CodeExecution.Contracts;
using DockerIntegration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeExecution.Extension
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCodeExecution(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .Configure<CodeExecutionConfiguration>(configuration.GetSection(nameof(CodeExecutionConfiguration)))
                .AddScoped(container => container
                    .GetService<IOptionsSnapshot<CodeExecutionConfiguration>>().Value)
                .AddScoped<ExecutableCodeFactory>()
                .AddScoped<CodeCompiler>()
                .AddScoped<CodeExecutor>()
                .AddTransient<PhpCode>()
                .AddTransient<PascalCode>()
                .AddTransient<CPlusPlusCode>()
                .AddTransient<JavaScriptCode>();
        }
    }
}
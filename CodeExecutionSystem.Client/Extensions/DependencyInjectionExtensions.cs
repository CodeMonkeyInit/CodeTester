using CodeExecutionSystem.Contracts.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeExecutionSystem.Client.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCodeExecutionSystem(this IServiceCollection serviceCollection) =>
            serviceCollection.AddScoped<ICodeExecutionApi, ExecutionApiProxy>();
    }
}
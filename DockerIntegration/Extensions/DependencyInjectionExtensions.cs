using System;
using Docker.DotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DockerIntegration.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDocker(this IServiceCollection serviceCollection, Uri dockerUri,
            IConfiguration configuration)
        {
            return serviceCollection
                .Configure<ContainerConfiguration>(configuration.GetSection(nameof(ContainerConfiguration)))
                .AddScoped(container => container.GetRequiredService<IOptionsSnapshot<ContainerConfiguration>>().Value)
                .AddSingleton(collection => new DockerClientConfiguration(dockerUri))
                .AddTransient(collection => collection.GetRequiredService<DockerClientConfiguration>().CreateClient())
                .AddTransient<DockerContainer>()
                .AddSingleton<DockerContainerExecutor>();
        }
    }
}
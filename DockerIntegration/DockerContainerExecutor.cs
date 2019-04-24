using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DockerIntegration
{
    public class DockerContainerExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public DockerContainerExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<ContainerExecutionResult> ExecuteAsync(Command command)
        {
            //TODO add load calibration
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var dockerContainer = serviceScope.ServiceProvider.GetService<DockerContainer>();

                return dockerContainer.RunAsync(command);
            }
        }
    }
}
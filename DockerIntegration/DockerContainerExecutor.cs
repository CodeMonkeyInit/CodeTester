using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DockerIntegration
{
    public class DockerContainerExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DockerContainerExecutor> _logger;
        private readonly SemaphoreSlim _semaphore;


        public DockerContainerExecutor(IServiceProvider serviceProvider, ContainerConfiguration configuration, ILogger<DockerContainerExecutor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _semaphore = new SemaphoreSlim(configuration.MaximumContainers, configuration.MaximumContainers);
        }

        public async Task<ContainerExecutionResult> ExecuteAsync(Command command)
        {
            await _semaphore.WaitAsync();

            var executionGuid = Guid.NewGuid();

            _logger.LogInformation($"Thread {Thread.CurrentThread.ManagedThreadId} acquired semaphore Id = {executionGuid}");
            
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var dockerContainer = serviceScope.ServiceProvider.GetService<DockerContainer>();

                ContainerExecutionResult executeAsync = await dockerContainer.RunAsync(command);

                _semaphore.Release();
                
                _logger.LogInformation($"Thread {Thread.CurrentThread.ManagedThreadId} released semaphore Id = {executionGuid}");
                
                return executeAsync;
            }
        }
    }
}
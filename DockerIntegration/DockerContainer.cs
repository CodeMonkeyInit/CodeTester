using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Logging;

namespace DockerIntegration
{
    public class DockerContainer
    {
        private readonly DockerClient _client;
        private readonly ContainerConfiguration _configuration;
        private readonly ILogger<DockerContainer> _logger;
        private CreateContainerResponse _createContainerResponse;

        public DockerContainer(DockerClient client, ContainerConfiguration configuration, ILogger<DockerContainer> logger)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ContainerExecutionResult> RunAsync(Command command)
        {
            _createContainerResponse = await _client.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Image = _configuration.ImageName,
                    Tty = true,
                    AttachStdin = true,
                    HostConfig = new HostConfig
                    {
                        Memory = command.Limits.MemoryLimitInBytes
                    }
                });

            await _client.Containers.StartContainerAsync(_createContainerResponse.ID, new ContainerStartParameters());

            await Task.Delay(TimeSpan.FromMilliseconds(command.Limits.TimeLimitInMs));
            
            
            if(await IsAliveAsync())
            {
                _logger.LogWarning($"Container with id {_createContainerResponse.ID} throze. Killing it now", _createContainerResponse);
                
                await _client.Containers.KillContainerAsync(_createContainerResponse.ID, new ContainerKillParameters());

                return ContainerExecutionResult.KilledByTimeout;
            }
            
            //TODO add killed by memory limit

            var errorOutputTask = GetContainerLogsAsync(new ContainerLogsParameters {ShowStderr = true});
            var standardOutputTask =  GetContainerLogsAsync(new ContainerLogsParameters{ShowStdout = true});

            return new ContainerExecutionResult
            {
                ErrorOutput = await errorOutputTask,
                StandardOutput = await standardOutputTask
            };
        }

        public async Task<bool> IsAliveAsync() => 
            await _client.Containers.InspectContainerAsync(_createContainerResponse.ID) != null;

        public async Task<string> GetContainerLogsAsync(ContainerLogsParameters logsParameters)
        {
            var stream = await _client.Containers.GetContainerLogsAsync(_createContainerResponse.ID, logsParameters);

            var streamReader = new StreamReader(stream, Encoding.UTF8);
            var result = await streamReader.ReadToEndAsync();

            return result;
        }
    }
}
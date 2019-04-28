using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
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
            var createContainerParameters = new CreateContainerParameters
            {
                Image = _configuration.ImageName,
                Tty = true,
                AttachStdin = true,
                Cmd = command.ProgramNameWithArguments,
                HostConfig = new HostConfig
                {
                    Memory = command.Limits.MemoryLimitInBytes,      
                }
            };

            if (!string.IsNullOrWhiteSpace(command.WorkingDirectory))
            {
                createContainerParameters.HostConfig.Mounts = new List<Mount>
                {
                    new Mount
                    {
                        Type = "bind",
                        Source = command.WorkingDirectory,
                        Target = _configuration.DockerWorkingDir
                    }
                };
            }
            
            _createContainerResponse = await _client.Containers.CreateContainerAsync(
                createContainerParameters);
            
            await _client.Containers.StartContainerAsync(_createContainerResponse.ID, new ContainerStartParameters());

            await Task.Delay(TimeSpan.FromMilliseconds(command.Limits.TimeLimitInMs));
            
             var containerInspection = await _client.Containers.InspectContainerAsync(_createContainerResponse.ID);
            
            //TODO add spinning to check if container is done
            if(containerInspection.State.Running)
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
                ExitCode = containerInspection.State.ExitCode,
                ErrorOutput = await errorOutputTask,
                StandardOutput = await standardOutputTask
            };
        }

        public async Task<IList<ContainerListResponse>> GetContainersAsync() 
        {
            var containers = await _client.Containers.ListContainersAsync(new ContainersListParameters());
            return containers;
        }

        public async Task<string> GetContainerLogsAsync(ContainerLogsParameters logsParameters)
        {
            var stream = await _client.Containers.GetContainerLogsAsync(_createContainerResponse.ID, logsParameters);

            var streamReader = new StreamReader(stream, Encoding.UTF8);
            var result = await streamReader.ReadToEndAsync();

            return result;
        }
    }
}
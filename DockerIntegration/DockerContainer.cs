using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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

        public DockerContainer(DockerClient client, ContainerConfiguration configuration,
            ILogger<DockerContainer> logger)
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
                Env = command.EnvironmentVariables,
                WorkingDir = command.WorkingDirectory,
                HostConfig = new HostConfig
                {
                    Memory = command.Limits.MemoryLimitInBytes
                }
            };

            if (!string.IsNullOrWhiteSpace(command.MountDirectory))
            {
                createContainerParameters.HostConfig.Mounts = new List<Mount>
                {
                    new Mount
                    {
                        Type = "bind",
                        Source = command.MountDirectory,
                        Target = _configuration.DockerWorkingDir
                    }
                };
            }

            _createContainerResponse = await _client.Containers.CreateContainerAsync(
                createContainerParameters);

            await _client.Containers.StartContainerAsync(_createContainerResponse.ID, new ContainerStartParameters());


            if (await TryKillContainerAfterTimeout(command))
                return ContainerExecutionResult.KilledByTimeout;

            var containerInspection = await _client.Containers.InspectContainerAsync(_createContainerResponse.ID);


            if (containerInspection.State.OOMKilled)
                return ContainerExecutionResult.KilledByMemoryLimit;


            var errorOutputTask = GetContainerLogsAsync(new ContainerLogsParameters {ShowStderr = true});
            var standardOutputTask = GetContainerLogsAsync(new ContainerLogsParameters {ShowStdout = true});

            return new ContainerExecutionResult
            {
                ExitCode = containerInspection.State.ExitCode,
                ErrorOutput = await errorOutputTask,
                StandardOutput = await standardOutputTask
            };
        }

        private async Task<bool> TryKillContainerAfterTimeout(Command command)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var timer = new Timer(async state =>
            {
                var containerInspect = await _client.Containers.InspectContainerAsync(_createContainerResponse.ID);

                if (!containerInspect.State.Running && !cancellationTokenSource.IsCancellationRequested)
                    cancellationTokenSource.Cancel();
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(_configuration.TimeBetweenContainerStatusChecksInMs));

            using (timer)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(command.Limits.TimeLimitInMs), cancellationTokenSource.Token);
                }
                catch(TaskCanceledException){}
            }

            var containerInspection = await _client.Containers.InspectContainerAsync(_createContainerResponse.ID);

            if (!containerInspection.State.Running)
                return false;

            _logger.LogWarning($"Container with id {_createContainerResponse.ID} throze. Killing it now",
                _createContainerResponse);

            await _client.Containers.KillContainerAsync(_createContainerResponse.ID, new ContainerKillParameters());

            return true;
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
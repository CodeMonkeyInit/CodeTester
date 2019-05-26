using System.Collections.Generic;
using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class JavaScriptCode : ExecutableCode
    {
        public override Command GetExecutionCommand(string mountDirectory) =>
            new Command
            {
                Name = "node",
                Arguments = new[] {GetCodeFilePath()},
                Limits = Limits,
                MountDirectory = mountDirectory,
                WorkingDirectory = ContainerConfiguration.DockerWorkingDir,
                EnvironmentVariables = new []{"NODE_DISABLE_COLORS=1"}
            };

        public JavaScriptCode(CodeExecutionConfiguration configuration, ContainerConfiguration containerConfiguration) :
            base(configuration, containerConfiguration)
        {
        }
    }
}
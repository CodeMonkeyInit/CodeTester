using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class PhpCode : ExecutableCode
    {
        public override Command GetExecutionCommand(string mountDirectory) =>
            new Command
            {
                Name = "php",
                Arguments = new[] {GetCodeFilePath()},
                MountDirectory = mountDirectory,
                Limits = Limits,
                WorkingDirectory = ContainerConfiguration.DockerWorkingDir
            };

        public PhpCode(CodeExecutionConfiguration configuration, ContainerConfiguration containerConfiguration) : base(
            configuration, containerConfiguration)
        {
        }
    }
}
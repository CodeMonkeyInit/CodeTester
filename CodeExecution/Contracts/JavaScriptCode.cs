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
                WorkingDirectory = ContainerConfiguration.DockerWorkingDir
            };

        public JavaScriptCode(CodeExecutionConfiguration configuration, ContainerConfiguration containerConfiguration) :
            base(configuration, containerConfiguration)
        {
        }
    }
}
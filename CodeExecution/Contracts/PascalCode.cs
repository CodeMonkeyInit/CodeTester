using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class PascalCode : NativeCompilableCode
    {
        public override Command GetCompilationCommand(string mountDirectory)
        {
            return new Command
            {
                Name = "fpc",
                Arguments = new[]
                {
                    GetCodeFilePath()
                },
                MountDirectory = mountDirectory,
                WorkingDirectory = ContainerConfiguration.DockerWorkingDir
            };
        }


        public PascalCode(CodeExecutionConfiguration configuration, ContainerConfiguration containerConfiguration) :
            base(configuration, containerConfiguration)
        {
        }
    }
}
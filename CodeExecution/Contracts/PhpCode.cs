using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class PhpCode : ExecutableCode
    {
        public PhpCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }

        public override Command GetExecutionCommand(string workingDirectory, string dockerWorkingDirectory) =>
            new Command
            {
                Name = "php",
                Arguments = new[] {GetCodeFilePath(dockerWorkingDirectory)},
                WorkingDirectory = workingDirectory,
                Limits = Limits
            };
    }
}
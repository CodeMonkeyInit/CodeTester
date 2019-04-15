using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class PhpCode : ExecutableCode
    {
        public PhpCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }

        public override Command GetExecutionCommand(string workingDirectory) =>
            new Command
            {
                Name = "php",
                Arguments = new[] {GetCodeFilePath(workingDirectory)},
                WorkingDirectory = workingDirectory,
                Limits = Limits
            };
    }
}
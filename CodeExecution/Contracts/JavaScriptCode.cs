using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class JavaScriptCode : ExecutableCode
    {
        public JavaScriptCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }

        public override Command GetExecutionCommand(string workingDirectory, string dockerWorkingDirectory) =>
            new Command
            {
                Name = "node",
                Arguments = new[] {GetCodeFilePath(dockerWorkingDirectory)},
                Limits = Limits,
                MountDirectory = workingDirectory
            };
    }
}
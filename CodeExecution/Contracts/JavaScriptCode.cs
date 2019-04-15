using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class JavaScriptCode : ExecutableCode
    {
        public JavaScriptCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }

        public override Command GetExecutionCommand(string workingDirectory) =>
            new Command
            {
                Name = "node",
                Arguments = new[] {GetCodeFilePath(workingDirectory)},
                Limits = Limits,
                WorkingDirectory = workingDirectory
            };
    }
}
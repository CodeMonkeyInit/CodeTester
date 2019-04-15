using System.IO;
using CodeExecution.Configuration;
using CodeExecution.Extension;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public abstract class CompilableCode : ExecutableCode
    {
        public abstract Command GetCompilationCommand(string workingDirectory);

        public abstract string GetExecutable(string workingDirectory);

        public CompilableCode(CodeExecutionConfiguration configuration): base(configuration)
        {
        }
    }
}
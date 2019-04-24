using System.IO;
using CodeExecution.Configuration;
using CodeExecution.Extension;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public abstract class ExecutableCode : TestingCode
    {
        protected CodeExecutionConfiguration Configuration;

        public ExecutableCode(CodeExecutionConfiguration configuration)
        {
            Configuration = configuration;
        }

        public abstract Command GetExecutionCommand(string workingDirectory);

        protected string GetExecutablePath(string workingDirectory)
        {
            return Path.Combine(workingDirectory, Configuration.CodeName);
        }

        protected string GetCodeFilePath(string workingDirectory)
        {
            return GetExecutablePath(workingDirectory) + Language.GetExtension();
        }
    }
}
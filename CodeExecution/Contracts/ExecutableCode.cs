using CodeExecution.Configuration;
using CodeExecution.Extension;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public abstract class ExecutableCode : TestingCode
    {
        protected ContainerConfiguration ContainerConfiguration { get; }
        protected readonly CodeExecutionConfiguration Configuration;

        public ExecutableCode(CodeExecutionConfiguration configuration, ContainerConfiguration containerConfiguration)
        {
            ContainerConfiguration = containerConfiguration;
            Configuration = configuration;
        }

        public string WorkingDirectory { get; set; } = string.Empty;

        public abstract Command GetExecutionCommand(string mountDirectory);

        protected string GetExecutablePath()
        {
            return Configuration.CodeName;
        }

        protected string GetCodeFilePath()
        {
            return GetExecutablePath() + Language.GetExtension();
        }
    }
}
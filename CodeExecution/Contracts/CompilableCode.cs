using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public abstract class CompilableCode : ExecutableCode
    {
        public abstract Command GetCompilationCommand(string mountDirectory);

        public abstract string GetExecutable();


        protected CompilableCode(CodeExecutionConfiguration configuration,
            ContainerConfiguration containerConfiguration) : base(configuration, containerConfiguration)
        {
        }
    }
}
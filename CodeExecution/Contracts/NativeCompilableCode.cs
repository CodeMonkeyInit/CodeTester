using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public abstract class NativeCompilableCode : CompilableCode
    {
        public override string GetExecutable()
        {
            return GetExecutablePath();
        }

        public override Command GetExecutionCommand(string mountDirectory)
        {
            return new Command
            {
                Name = $"./{GetExecutablePath()}",
                Limits = Limits,
                MountDirectory = mountDirectory,
                WorkingDirectory = ContainerConfiguration.DockerWorkingDir
            };
        }

        protected NativeCompilableCode(CodeExecutionConfiguration configuration,
            ContainerConfiguration containerConfiguration) : base(configuration, containerConfiguration)
        {
        }
    }
}
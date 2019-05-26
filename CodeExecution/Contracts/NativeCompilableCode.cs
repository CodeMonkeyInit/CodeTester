using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public abstract class NativeCompilableCode : CompilableCode
    {
        protected NativeCompilableCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }

        public override string GetExecutable(string workingDirectory)
        {
            return GetExecutablePath(workingDirectory);
        }

        public override Command GetExecutionCommand(string workingDirectory, string dockerWorkingDirectory)
        {
            return new Command
            {
                Name = GetExecutablePath(dockerWorkingDirectory),
                Limits = Limits,
                MountDirectory = workingDirectory
            };
        }
    }
}
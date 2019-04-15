using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public abstract class NativeCompilableCode: CompilableCode
    {
        public override string GetExecutable(string workingDirectory) => GetExecutablePath(workingDirectory);


        protected NativeCompilableCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }

        public override Command GetExecutionCommand(string workingDirectory) =>
            new Command
            {
                Name = GetExecutablePath(workingDirectory),
                Limits = Limits,
                WorkingDirectory = workingDirectory
            };
    }
}
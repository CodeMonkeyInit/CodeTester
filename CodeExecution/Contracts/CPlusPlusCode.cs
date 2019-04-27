using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class CPlusPlusCode : NativeCompilableCode
    {
        public CPlusPlusCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }

        public override Command GetCompilationCommand(string workingDirectory, string dockerDirectory)
        {
            return new Command
            {
                Name = "g++",
                Arguments = new[]
                {
                    GetCodeFilePath(dockerDirectory),
                    "-o",
                    GetExecutablePath(dockerDirectory)
                },
                WorkingDirectory = workingDirectory
            };
        }
    }
}
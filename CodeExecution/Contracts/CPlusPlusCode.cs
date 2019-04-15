using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class CPlusPlusCode : NativeCompilableCode
    {
        
        public override Command GetCompilationCommand(string workingDirectory)
        {
            return new Command
            {
                Name = "g++",
                Arguments = new []
                {
                    GetCodeFilePath(workingDirectory),
                    "-o",
                    GetExecutablePath(workingDirectory)
                },
                WorkingDirectory = workingDirectory
            };
        }

        public CPlusPlusCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }
    }
}
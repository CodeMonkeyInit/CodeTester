using System;
using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class PascalCode : NativeCompilableCode
    {
        public override Command GetCompilationCommand(string workingDirectory, string dockerDirectory)
        {
            return new Command
            {
                Name = "fpc",
                Arguments = new[]
                {
                    GetCodeFilePath(dockerDirectory)
                },
                WorkingDirectory = workingDirectory
            };
        }


        public PascalCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }
    }
}
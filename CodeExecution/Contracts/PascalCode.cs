using System;
using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class PascalCode : NativeCompilableCode
    {
        public override Command GetCompilationCommand(string workingDirectory)
        {
            throw new NotImplementedException();
        }


        public PascalCode(CodeExecutionConfiguration configuration) : base(configuration)
        {
        }
    }
}
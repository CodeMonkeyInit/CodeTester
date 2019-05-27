using System.Collections.Generic;
using CodeExecution.Configuration;
using DockerIntegration;

namespace CodeExecution.Contracts
{
    public class CPlusPlusCode : NativeCompilableCode
    {
        public override Command GetCompilationCommand(string mountDirectory)
        {
            return new Command
            {
                Name = "g++",
                Arguments = new[]
                {
                    GetCodeFilePath(),
                    "-o",
                    GetExecutablePath()
                },
                EnvironmentVariables = new [] {"GCC_COLORS="},
                MountDirectory = mountDirectory,
                WorkingDirectory = ContainerConfiguration.DockerWorkingDir
            };
        }

        public CPlusPlusCode(CodeExecutionConfiguration configuration, ContainerConfiguration containerConfiguration)
            : base(configuration, containerConfiguration)
        {
        }
    }
}
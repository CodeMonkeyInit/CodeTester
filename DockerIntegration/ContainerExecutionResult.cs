using System;
using System.Linq;
using CodeExecutionSystem.Contracts.Data;

namespace DockerIntegration
{
    public class ContainerExecutionResult
    {
        public long ExitCode { get; set; }
        
        public ExecutionResult Result { get; set; }
        
        public string StandardOutput { get; set; }

        public string ErrorOutput { get; set; }
        
        public static ContainerExecutionResult KilledByTimeout => new ContainerExecutionResult {Result = ExecutionResult.KilledByTimeout}; 
        
        public static ContainerExecutionResult KilledByMemoryLimit => new ContainerExecutionResult {Result = ExecutionResult.KilledByMemoryLimit};

        public bool WasSuccessful => ExitCode == 0 && Result == ExecutionResult.Success && string.IsNullOrWhiteSpace(ErrorOutput);

        public string[] StandardOutputSplit =>
            StandardOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(output => output.Trim())
                .ToArray();
    }
}
namespace DockerIntegration
{
    public class ContainerExecutionResult
    {
        public ExecutionResult Result { get; set; }
        
        public string StandardOutput { get; set; }

        public string ErrorOutput { get; set; }
        
        public static ContainerExecutionResult KilledByTimeout => new ContainerExecutionResult {Result = ExecutionResult.KilledByTimeout}; 
        
        public static ContainerExecutionResult KilledByMemoryLimit => new ContainerExecutionResult {Result = ExecutionResult.KilledByMemoryLimit};

        public bool WasSuccessful => Result == ExecutionResult.Success && string.IsNullOrWhiteSpace(ErrorOutput);
    }
}
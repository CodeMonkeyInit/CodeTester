namespace DockerIntegration
{
    public class ContainerExecutionResult
    {
        public bool WasKilled { get; set; }
        
        public string StandardOutput { get; set; }

        public string ErrorOutput { get; set; }
        
        public static ContainerExecutionResult Killed => new ContainerExecutionResult {WasKilled = true}; 
    }
}
namespace CodeExecutionSystem.Contracts.Data
{
    public class TestingCode : Code
    {
        public ExecutionData[] ExecutionData { get; set; }
        
        public Limits Limits { get; set; }
        public string WorkingDirectory { get; set; } = string.Empty;
    }
}
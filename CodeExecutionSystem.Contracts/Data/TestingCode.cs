namespace CodeExecutionSystem.Contracts.Data
{
    public class TestingCode : Code
    {
        public ExecutionData[] ExecutionData { get; set; } = new ExecutionData[0];

        public Limits Limits { get; set; } = new Limits();
    }
}
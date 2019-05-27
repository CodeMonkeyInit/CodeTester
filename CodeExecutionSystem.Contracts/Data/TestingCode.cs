using System;

namespace CodeExecutionSystem.Contracts.Data
{
    public class TestingCode : Code
    {
        public ExecutionData[] ExecutionData { get; set; } = Array.Empty<ExecutionData>();

        public Limits Limits { get; set; } = new Limits();
    }
}
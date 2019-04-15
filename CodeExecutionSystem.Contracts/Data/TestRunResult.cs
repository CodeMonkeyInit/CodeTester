namespace CodeExecutionSystem.Contracts.Data
{
    public class TestRunResult
    {
        public ExecutionResult ExecutionResult { get; set; }

        public string UserOutput { get; set; }
        
        public string ExpectedOutput { get; set; }

        public bool WasSuccessfull => UserOutput == ExpectedOutput;
    }
}
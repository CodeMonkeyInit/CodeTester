using System;

namespace CodeExecutionSystem.Contracts.Data
{
    public class TestRunResult
    {
        public ExecutionResult ExecutionResult { get; set; }

        public string UserOutput { get; set; }

        public string ExpectedOutput { get; set; }

        public bool WasSuccessful => ExecutionResult == ExecutionResult.Success && UserOutput
                                          .Trim()
                                          .Equals(ExpectedOutput.Trim(), StringComparison.InvariantCulture);
    }
}
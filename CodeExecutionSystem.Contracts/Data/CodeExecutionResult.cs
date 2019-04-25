using System.Linq;

namespace CodeExecutionSystem.Contracts.Data
{
    public class CodeExecutionResult
    {
        
        public bool WasSuccessfull => !HasCompilationError && Results.All(result => result.WasSuccessfull);
        public bool HasCompilationError => CompilationErrors.Any();

        public TestRunResult[] Results { get; set; } = new TestRunResult[0];
        
        public string[] CompilationErrors { get; set; } = new string[0];
    }
}
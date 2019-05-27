using System;
using System.Linq;

namespace CodeExecutionSystem.Contracts.Data
{
    public class CodeExecutionResult
    {
        public bool WasSuccessful => !HasCompilationError && Results.All(result => result.WasSuccessful);
        public bool HasCompilationError => CompilationErrors.Any();

        public TestRunResult[] Results { get; set; } = Array.Empty<TestRunResult>();

        public string[] CompilationErrors { get; set; } = Array.Empty<string>();
    }
}
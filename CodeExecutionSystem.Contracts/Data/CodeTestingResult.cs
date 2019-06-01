using System.ComponentModel.DataAnnotations;

namespace CodeExecutionSystem.Contracts.Data
{
    public class CodeTestingResult
    {   
        public CodeAnalysisResult CodeAnalysisResult { get; set; } = new CodeAnalysisResult();

        public CodeExecutionResult CodeExecutionResult { get; set; } = new CodeExecutionResult();

        [Range(0, 100)]
        public byte Score { get; set; }
    }
}
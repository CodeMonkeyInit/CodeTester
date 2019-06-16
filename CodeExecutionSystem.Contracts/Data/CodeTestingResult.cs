using System.ComponentModel.DataAnnotations;

namespace CodeExecutionSystem.Contracts.Data
{
    public class CodeTestingResult
    {   
        public CodeAnalysisResult CodeAnalysisResult { get; set; }

        public CodeExecutionResult CodeExecutionResult { get; set; } 

        [Range(0, 100)]
        public byte Score { get; set; }
    }
}
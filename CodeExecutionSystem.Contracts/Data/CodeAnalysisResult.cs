namespace CodeExecutionSystem.Contracts.Data
{
    public class CodeAnalysisResult
    {
        public bool IsSuccessful { get; set; }

        public AnalysisResult[] AnalysisResults { get; set; }

        public static CodeAnalysisResult ValidCode => new CodeAnalysisResult {IsSuccessful = true};
    }
}
namespace CodeAnalysis
{
    public class CodeAnalysisResult
    {
        public bool IsSuccessful { get; set; }

        public string[] Errors { get; set; }

        public string[] Warnings { get; set; }

        public CodeAnalysisResult ValidCode => new CodeAnalysisResult {IsSuccessful = true};
    }
}
namespace CodeAnalysis
{
    public class CodeAnalysisResult
    {
        public bool IsSuccessful => true;

        public string[] Errors { get; set; }

        public string[] Warnings { get; set; }
    }
}
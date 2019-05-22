namespace CodeAnalysis
{
    public class CodeAnalysisResult
    {
        public bool IsSuccessful { get; set; }

        public AnalysisResult[] AnalysisResults { get; set; }

        public static CodeAnalysisResult ValidCode => new CodeAnalysisResult {IsSuccessful = true};
    }

    public class AnalysisResult
    {
        public Level Level { get; set; }

        public string Message { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }
    }

    public enum Level
    {
        Verbose = 0,
        Warning = 1,
        Error = 2
    }
}
namespace CodeAnalysis.Configuration
{
    public class AnalysisConfiguration
    {
        public string TempFolderPath { get; set; }

        public string FileName { get; set; } = "code";
        
        public string EsLintFolder { get; set; }
    }
}
namespace CodeExecutionSystem.Contracts.Data
{
    public class AnalysisResult
    {
        public Level Level { get; set; }

        public string Message { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }
    }
}
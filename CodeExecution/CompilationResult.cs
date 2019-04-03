namespace CodeExecution
{
    public class CompilationResult
    {
        public string OutputPath { get; set; }

        public string Executable { get; set; }

        public string Errors { get; set; }

        public bool WasSuccessful => string.IsNullOrWhiteSpace(Errors);
    }
}
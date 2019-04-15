using System.Linq;

namespace CodeExecution.Compilaton
{
    public class CompilationResult
    {
        public string OutputPath { get; set; }

        public string ExecutablePath { get; set; }

        public string[] Errors { get; set; }

        public bool WasSuccessful => !Errors.Any();
    }
}
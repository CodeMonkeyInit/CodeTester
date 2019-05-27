namespace CodeExecution.Configuration
{
    public class CodeExecutionConfiguration
    {
        public string TempFolderPath { get; set; }

        public string CodeName { get; set; } = "code";

        public string InputFileName { get; set; } = "input.txt";

        public string OutputFileName { get; set; } = "output.txt";
    }
}
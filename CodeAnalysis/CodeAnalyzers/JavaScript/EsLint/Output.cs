using CodeExecutionSystem.Contracts.Data;
using Newtonsoft.Json;

namespace CodeAnalysis.CodeAnalyzers.JavaScript.EsLint
{
    public class Output
    {
        public Details[] Messages { get; set; }

        public int ErrorCount { get; set; }

        public int WarningCount { get; set; }
    }

    public class Details
    {
        public string RuleId { get; set; }

        [JsonProperty("severity")]
        public Level Level { get; set; }

        public string Message { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }
    }
}
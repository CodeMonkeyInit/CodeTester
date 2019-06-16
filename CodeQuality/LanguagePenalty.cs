using CodeExecutionSystem.Contracts.Data;

namespace CodeQuality
{
    public class LanguagePenalty
    {
        public Language Language { get; set; }

        public byte WarningPenalty { get; set; }
    }
}
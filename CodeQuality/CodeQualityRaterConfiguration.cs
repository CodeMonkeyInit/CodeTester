using System;

namespace CodeQuality
{
    public class CodeQualityRaterConfiguration
    {
        public LanguagePenalty[] CodePenalties { get; set; } = Array.Empty<LanguagePenalty>();

        public byte MinimumSuccessfulExecutionScore { get; set; }
    }
}
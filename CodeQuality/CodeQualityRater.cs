using System.Linq;
using System.Threading.Tasks;
using CodeAnalysis.CodeAnalyzers;
using CodeExecution;
using CodeExecution.Contracts;
using CodeExecutionSystem.Contracts.Data;
using Helpers;

namespace CodeQuality
{
    public class CodeQualityRater
    {
        private readonly CodeAnalyzerFactory _codeAnalyzerFactory;
        private readonly CodeExecutor _codeExecutor;
        private readonly CodeQualityRaterConfiguration _codeQualityRaterConfiguration;
        private readonly ExecutableCodeFactory _executableCodeFactory;

        public CodeQualityRater(CodeAnalyzerFactory codeAnalyzerFactory, CodeExecutor codeExecutor, CodeQualityRaterConfiguration codeQualityRaterConfiguration,  ExecutableCodeFactory executableCodeFactory)
        {
            _codeAnalyzerFactory = codeAnalyzerFactory;
            _codeExecutor = codeExecutor;
            _codeQualityRaterConfiguration = codeQualityRaterConfiguration;
            _executableCodeFactory = executableCodeFactory;
        }
        
        public async Task<CodeTestingResult> GetCodeQuality(TestingCode code)
        {
            var codeAnalyzer = _codeAnalyzerFactory.GetCodeAnalyzer(code);

            var codeAnalysisResult = await codeAnalyzer.Analyse(code);

            var codeTestingResult = new CodeTestingResult
            {
                CodeAnalysisResult = codeAnalysisResult
            };
            
            if (!codeAnalysisResult.IsSuccessful)
            {
                return codeTestingResult;
            }

            var executableCode = _executableCodeFactory.GetExecutableCode(code);

            codeTestingResult.CodeExecutionResult = await _codeExecutor.ExecuteAsync(executableCode);

            codeTestingResult.Score = GetFinalScore(codeTestingResult, code.Language);

            return codeTestingResult;
        }

        private byte GetFinalScore(CodeTestingResult testingResult, Language codeLanguage)
        {
            int finalScore = 0;

            LanguagePenalty languagePenalty = _codeQualityRaterConfiguration.CodePenalties
                .FirstOrDefault(penalty => penalty.Language == codeLanguage);

            int warningsCount = testingResult.CodeAnalysisResult.AnalysisResults
                .Count(result => result.Level == Level.Warning);

            byte warningPenalty = languagePenalty?.WarningPenalty ?? 0;

            finalScore -= warningsCount * warningPenalty;
            
            int completedTestsPercentage = GetCompletedTestsPercentage(testingResult.CodeExecutionResult);

            finalScore += completedTestsPercentage;

            var minimumSuccessfulExecutionScore = _codeQualityRaterConfiguration.MinimumSuccessfulExecutionScore;
            
            if (testingResult.CodeExecutionResult.WasSuccessful &&
                finalScore < minimumSuccessfulExecutionScore)
            {
                return minimumSuccessfulExecutionScore;
            }

            

            return finalScore > 0 ? (byte) finalScore : default;
        }

        private int GetCompletedTestsPercentage(CodeExecutionResult codeExecutionResult)
        {
            var testRuns = codeExecutionResult.Results.Length;

            var successfulRuns = codeExecutionResult.Results.Count(result => result.WasSuccessful);

            return (int) ((successfulRuns.ToDouble() / testRuns.ToDouble()) * 100);
        }
    }
}

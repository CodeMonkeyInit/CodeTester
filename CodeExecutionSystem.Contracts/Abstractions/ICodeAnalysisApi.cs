using System.Threading.Tasks;
using CodeExecutionSystem.Contracts.Data;

namespace CodeExecutionSystem.Contracts.Abstractions
{
    public interface ICodeAnalysisApi
    {
        Task<CodeAnalysisResult> AnalyzeCodeAsync(TestingCode code);
    }
}
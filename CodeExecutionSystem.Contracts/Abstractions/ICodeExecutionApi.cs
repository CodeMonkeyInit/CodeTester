using System.Threading.Tasks;
using CodeExecutionSystem.Contracts.Data;

namespace CodeExecutionSystem.Contracts.Abstractions
{
    public interface ICodeExecutionApi
    {
        Task<CodeExecutionResult> ExecuteCodeAsync(TestingCode code);
    }
}
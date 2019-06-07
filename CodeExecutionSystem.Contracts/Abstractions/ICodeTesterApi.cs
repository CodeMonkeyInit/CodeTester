using System.Threading.Tasks;
using CodeExecutionSystem.Contracts.Data;

namespace CodeExecutionSystem.Contracts.Abstractions
{
    public interface ICodeTesterApi
    {
        Task<CodeTestingResult> TestCode(TestingCode code);
    }
}
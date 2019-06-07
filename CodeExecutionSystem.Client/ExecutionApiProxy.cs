using System.Threading.Tasks;
using CodeExecutionSystem.Contracts.Abstractions;
using CodeExecutionSystem.Contracts.Data;
using Microsoft.Extensions.Configuration;

namespace CodeExecutionSystem.Client
{
    public class ExecutionApiProxy : ApiProxy, ICodeExecutionApi
    {
        private readonly string _codeExecutionApiPath;

        private const string ApiPathKey = "CodeExecutionApiPath";

        public ExecutionApiProxy(IConfiguration configuration)
        {
            _codeExecutionApiPath = GetKeyFromConfiguration(ApiPathKey, configuration);
        }

        

        public async Task<CodeExecutionResult> ExecuteCodeAsync(TestingCode code) => 
            await PostToApi<CodeExecutionResult>(_codeExecutionApiPath, code);
    }
}
using System.Threading.Tasks;
using CodeExecutionSystem.Contracts.Abstractions;
using CodeExecutionSystem.Contracts.Data;
using Microsoft.Extensions.Configuration;

namespace CodeExecutionSystem.Client
{
    public class AnalysisApiProxy : ApiProxy, ICodeAnalysisApi
    {
        private readonly string _codeAnalysisApiPath;

        private const string ApiPathKey = "CodeAnalysisApiPath";
        public AnalysisApiProxy(IConfiguration configuration)
        {
            _codeAnalysisApiPath = GetKeyFromConfiguration(ApiPathKey, configuration);
        }
        
        public async Task<CodeAnalysisResult> AnalyzeCodeAsync(TestingCode code) => 
            await PostToApi<CodeAnalysisResult>(_codeAnalysisApiPath, code);
    }
}
using System.Threading.Tasks;
using CodeExecutionSystem.Contracts.Abstractions;
using CodeExecutionSystem.Contracts.Data;
using Microsoft.Extensions.Configuration;

namespace CodeExecutionSystem.Client
{
    public class CodeTesterApiProxy : ApiProxy, ICodeTesterApi
    {
        private readonly string _codeTesterApiPath;

        private const string ApiPathKey = "CodeTesterApiPath";
        public CodeTesterApiProxy(IConfiguration configuration)
        {
            _codeTesterApiPath = GetKeyFromConfiguration(ApiPathKey, configuration);
        }
        

        public async Task<CodeTestingResult> TestCode(TestingCode code) =>
            await PostToApi<CodeTestingResult>(_codeTesterApiPath, code);
    }
}
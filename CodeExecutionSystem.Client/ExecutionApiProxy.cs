using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CodeExecutionSystem.Contracts.Abstractions;
using CodeExecutionSystem.Contracts.Data;
using Flurl.Http;
using Microsoft.Extensions.Configuration;

namespace CodeExecutionSystem.Client
{
    public class ExecutionApiProxy : ICodeExecutionApi
    {
        private readonly string _codeExecutionApiPath;

        private const string ApiPathKey = "CodeExecutionApiPath";

        public ExecutionApiProxy(IConfiguration configuration)
        {
            _codeExecutionApiPath = configuration[ApiPathKey]
                                    ?? throw new ArgumentException(
                                        $"ApiPathKey is not present in {nameof(IConfiguration)}");
        }

        public async Task<CodeExecutionResult> ExecuteCodeAsync(TestingCode code)
        {
            Validator.ValidateObject(code, new ValidationContext(code));

            return await _codeExecutionApiPath.PostJsonAsync(code).ReceiveJson<CodeExecutionResult>();
        }
    }
}
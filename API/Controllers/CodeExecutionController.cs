using System.Threading.Tasks;
using CodeExecution;
using CodeExecution.Contracts;
using CodeExecutionSystem.Contracts.Abstractions;
using CodeExecutionSystem.Contracts.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeExecutionController : ControllerBase, ICodeExecutionApi
    {
        private readonly CodeExecutor _codeExecutor;
        private readonly ExecutableCodeFactory _codeFactory;

        public CodeExecutionController(CodeExecutor codeExecutor, ExecutableCodeFactory codeFactory)
        {
            _codeExecutor = codeExecutor;
            _codeFactory = codeFactory;
        }

        // POST api/values
        [HttpPost]
        public async Task<CodeExecutionResult> ExecuteCodeAsync(TestingCode code)
        {
            var executableCode = _codeFactory.GetExecutableCode(code);

            return await _codeExecutor.ExecuteAsync(executableCode);
        }
    }
}
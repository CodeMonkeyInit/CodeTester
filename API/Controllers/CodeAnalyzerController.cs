using System.Threading.Tasks;
using CodeAnalysis;
using CodeExecutionSystem.Contracts.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeAnalyzerController : ControllerBase
    {
        private readonly CodeAnalyzerFactory _codeAnalyzerFactory;

        public CodeAnalyzerController(CodeAnalyzerFactory codeAnalyzerFactory)
        {
            _codeAnalyzerFactory = codeAnalyzerFactory;
        }

        // POST api/values
        [HttpPost]
        public async Task<CodeAnalysisResult> AnalyzeCodeAsync([FromBody] TestingCode code)
        {
            CodeAnalyzer codeAnalyzer = _codeAnalyzerFactory.GetCodeAnalyzer(code);

            return await codeAnalyzer.Analyse(code);
        }
    }
}
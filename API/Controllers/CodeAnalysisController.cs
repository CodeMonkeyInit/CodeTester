using System.Threading.Tasks;
using CodeAnalysis;
using CodeAnalysis.CodeAnalyzers;
using CodeAnalysis.CodeAnalyzers.Base;
using CodeExecutionSystem.Contracts.Abstractions;
using CodeExecutionSystem.Contracts.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeAnalysisController : ControllerBase, ICodeAnalysisApi
    {
        private readonly CodeAnalyzerFactory _codeAnalyzerFactory;

        public CodeAnalysisController(CodeAnalyzerFactory codeAnalyzerFactory)
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
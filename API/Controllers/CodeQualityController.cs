using System.Threading.Tasks;
using CodeAnalysis.CodeAnalyzers;
using CodeExecutionSystem.Contracts.Abstractions;
using CodeExecutionSystem.Contracts.Data;
using CodeQuality;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeQualityController : ControllerBase, ICodeTesterApi
    {
        private readonly CodeQualityRater _codeQualityRater;

        public CodeQualityController(CodeQualityRater codeQualityRater)
        {
            _codeQualityRater = codeQualityRater;
        }


        [HttpPost]
        public Task<CodeTestingResult> TestCode([FromBody]TestingCode code) => 
            _codeQualityRater.GetCodeQuality(code);
    }
}
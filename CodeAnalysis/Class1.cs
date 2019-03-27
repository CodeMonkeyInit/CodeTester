using System;
using System.Reflection;
using System.Threading.Tasks;
using DockerIntegration;

namespace CodeAnalysis
{
    public class CodeAnalyzer
    {
        private readonly DockerContainerExecutor _executor;

        public CodeAnalyzer(DockerContainerExecutor executor)
        {
            _executor = executor;
        }
        public async Task<CodeAnalysisResult> Analyze(Code codeForAnalysis)
        {
            var analysisResult = await _executor.Execute(codeForAnalysis.GetCommandForAnalysis());

            throw new NotImplementedException();
        }
    }
}
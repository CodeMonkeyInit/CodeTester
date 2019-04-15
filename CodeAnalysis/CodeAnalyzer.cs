using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CodeExecution;
using CodeExecution.Extension;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;

namespace CodeAnalysis
{
    public class CPlusPlusCodeAnalyzer : CodeAnalyzer
    {
        public CPlusPlusCodeAnalyzer(DockerContainerExecutor executor, AnalysisConfiguration configuration) : base(
            executor, configuration)
        {
        }

        protected override Task<CodeAnalysisResult> AnalyzeCode(Code codeForAnalysis)
        {
            throw new NotImplementedException();
        }
    }

    public class PhpCodeAnalyzer : CodeAnalyzer
    {
        public PhpCodeAnalyzer(DockerContainerExecutor executor, AnalysisConfiguration configuration) : base(executor,
            configuration)
        {
        }

        protected override Task<CodeAnalysisResult> AnalyzeCode(Code codeForAnalysis)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class CodeAnalyzer
    {
        protected readonly DockerContainerExecutor Executor;
        protected readonly AnalysisConfiguration Configuration;

        public CodeAnalyzer(DockerContainerExecutor executor, AnalysisConfiguration configuration)
        {
            Executor = executor;
            Configuration = configuration;
        }

        public async Task<CodeAnalysisResult> Analyse(Code code)
        {
            var tempFolder = Path.Combine(Configuration.TempFolderPath, Guid.NewGuid().ToString());

            await File.WriteAllTextAsync(Path.Combine(tempFolder, Configuration.FileName + code.Language.GetExtension()), code.Text);
            
            var codeAnalysis = await AnalyzeCode(code);
            
            Directory.Delete(tempFolder);

            return codeAnalysis;
        }

        protected abstract Task<CodeAnalysisResult> AnalyzeCode(Code code);
    }

    public class AnalysisConfiguration
    {
        public string TempFolderPath { get; set; }

        public string FileName { get; set; } = "code";
    }
}
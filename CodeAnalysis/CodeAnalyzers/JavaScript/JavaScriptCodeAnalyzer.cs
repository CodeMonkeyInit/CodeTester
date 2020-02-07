using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ByteSizeLib;
using CodeAnalysis.CodeAnalyzers.Base;
using CodeAnalysis.CodeAnalyzers.JavaScript.EsLint;
using CodeAnalysis.Configuration;
using CodeExecution.Contracts;
using CodeExecution.Extension;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;
using Helpers;
using Newtonsoft.Json;

namespace CodeAnalysis.CodeAnalyzers.JavaScript
{
    public class JavaScriptCodeAnalyzer : CodeAnalyzer
    {
        private readonly IMapper _mapper;

        public JavaScriptCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor, IMapper mapper)
            : base(configuration,
                codeFactory, containerConfiguration, executor)
        {
            _mapper = mapper;
        }

        protected override async Task CreateDirectoryForAnalysis(TestingCode code, string tempFolder)
        {
            await base.CreateDirectoryForAnalysis(code, tempFolder);

            Configuration.EsLintFolder.CopyFolderTo(tempFolder);
        }

        protected override Command ModifyCommandForAnalysis(Command executionCommand)
        {
            var codeName = Configuration.FileName + Language.Js.GetExtension();

            return new Command
            {
                Name = "bash",
                Arguments = new[] {"-c", $"yarn >/dev/null && npx eslint {codeName} -f json"},
                Limits = new Limits
                {
                    MemoryLimitInBytes = ByteSize.FromMegaBytes(100).Bytes.ToLong(),
                    TimeLimitInMs = TimeSpan.FromMinutes(5).TotalMilliseconds.ToLong()
                },
                WorkingDirectory = executionCommand.WorkingDirectory,
                MountDirectory = executionCommand.MountDirectory
            };
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            var output = JsonConvert.DeserializeObject<Output[]>(containerExecutionResult.StandardOutput);
            var currentFile = output.FirstOrDefault();
            var analysisResults = _mapper.Map<AnalysisResult[]>(currentFile?.Messages);

            var codeAnalysisResult = new CodeAnalysisResult
            {
                AnalysisResults = analysisResults
            };

            if (currentFile?.ErrorCount == 0)
            {
                codeAnalysisResult.IsSuccessful = true;
            }

            return codeAnalysisResult;
        }
    }
}
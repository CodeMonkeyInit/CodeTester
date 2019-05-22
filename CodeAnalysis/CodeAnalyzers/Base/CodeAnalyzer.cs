using System;
using System.IO;
using System.Threading.Tasks;
using CodeAnalysis.Configuration;
using CodeExecution.Contracts;
using CodeExecution.Extension;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;

namespace CodeAnalysis.CodeAnalyzers.Base
{
    public abstract class CodeAnalyzer
    {
        protected readonly AnalysisConfiguration Configuration;
        private readonly ExecutableCodeFactory _codeFactory;
        private readonly ContainerConfiguration _containerConfiguration;
        private readonly DockerContainerExecutor _executor;

        public CodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor)
        {
            Configuration = configuration;
            _codeFactory = codeFactory;
            _containerConfiguration = containerConfiguration;
            _executor = executor;
        }

        public async Task<CodeAnalysisResult> Analyse(TestingCode code)
        {
            var tempFolder = Path.Combine(Configuration.TempFolderPath, Guid.NewGuid().ToString());
            
            Directory.CreateDirectory(tempFolder);
            
            await File.WriteAllTextAsync(
                Path.Combine(tempFolder, Configuration.FileName + code.Language.GetExtension()), code.Text);

            ExecutableCode executableCode = _codeFactory.GetExecutableCode(code);

            Command executionCommand = GetCompilationCommand(executableCode, tempFolder);

            Command analysisCommand = ModifyCommandForAnalysis(executionCommand);

            ContainerExecutionResult containerExecutionResult = await _executor.ExecuteAsync(analysisCommand);

            CodeAnalysisResult codeAnalysis = AnalyseOutput(containerExecutionResult);

            Directory.Delete(tempFolder, true);

            return codeAnalysis;
        }

        private Command GetCompilationCommand(ExecutableCode executableCode, string tempFolder)
        {
            if (executableCode is CompilableCode compilableCode)
            {
                return compilableCode.GetCompilationCommand(tempFolder, _containerConfiguration.DockerWorkingDir);
            }
            return executableCode.GetExecutionCommand(tempFolder,
                _containerConfiguration.DockerWorkingDir);
        }

        protected virtual Command ModifyCommandForAnalysis(Command executionCommand) => executionCommand;

        protected abstract CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult);
    }
}
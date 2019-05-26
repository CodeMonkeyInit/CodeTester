using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        protected readonly ContainerConfiguration ContainerConfiguration;
        
        private readonly ExecutableCodeFactory _codeFactory;
        private readonly DockerContainerExecutor _executor;

        public CodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor)
        {
            Configuration = configuration;
            _codeFactory = codeFactory;
            ContainerConfiguration = containerConfiguration;
            _executor = executor;
        }

        public async Task<CodeAnalysisResult> Analyse(TestingCode code)
        {
            var tempFolder = Path.Combine(Configuration.TempFolderPath, Guid.NewGuid().ToString());
            
            await CreateDirectoryForAnalysis(code, tempFolder);

            ExecutableCode executableCode = _codeFactory.GetExecutableCode(code);

            Command executionCommand = GetCompilationCommand(executableCode, tempFolder);

            Command analysisCommand = ModifyCommandForAnalysis(executionCommand);

            ContainerExecutionResult containerExecutionResult = await _executor.ExecuteAsync(analysisCommand);

            if (containerExecutionResult.Result == ExecutionResult.Success)
            {
                CodeAnalysisResult codeAnalysis = AnalyseOutput(containerExecutionResult);
                
                return codeAnalysis;
            }
            
            Directory.Delete(tempFolder, true);

            return new CodeAnalysisResult
            {
                IsSuccessful = false
            };
        }

        protected virtual async Task CreateDirectoryForAnalysis(TestingCode code, string tempFolder)
        {
            Directory.CreateDirectory(tempFolder);

            await File.WriteAllTextAsync(
                Path.Combine(tempFolder, Configuration.FileName + code.Language.GetExtension()), code.Text);
        }

        private Command GetCompilationCommand(ExecutableCode executableCode, string tempFolder)
        {
            if (executableCode is CompilableCode compilableCode)
            {
                return compilableCode.GetCompilationCommand(tempFolder, ContainerConfiguration.DockerWorkingDir);
            }
            return executableCode.GetExecutionCommand(tempFolder,
                ContainerConfiguration.DockerWorkingDir);
        }
        
        protected virtual Command ModifyCommandForAnalysis(Command executionCommand) => executionCommand;

        protected abstract CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult);

        protected static CodeAnalysisResult GetCodeAnalysisResult(IEnumerable<AnalysisResult> errors, IEnumerable<AnalysisResult> warnings)
        {
            var errorsArray = errors as AnalysisResult[] ?? errors.ToArray();
            
            if (!errorsArray.Any())
            {
                return new CodeAnalysisResult
                {
                    IsSuccessful = true,
                    AnalysisResults = warnings.ToArray()
                };
            }

            return new CodeAnalysisResult
            {
                IsSuccessful = false,
                AnalysisResults = errorsArray.Union(warnings).ToArray()
            };
        }
    }
}
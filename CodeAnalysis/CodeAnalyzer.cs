using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeExecution.Contracts;
using CodeExecution.Extension;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeAnalysis
{
    public class JavaScriptCodeAnalyzer : CodeAnalyzer
    {
        public JavaScriptCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor) : base(configuration,
            codeFactory, containerConfiguration, executor)
        {
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            throw new NotImplementedException();
        }
    }

    public class PascalCodeAnalyzer : CodeAnalyzer
    {
        public PascalCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor) : base(configuration,
            codeFactory, containerConfiguration, executor)
        {
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            throw new NotImplementedException();
        }
    }

    public class CPlusPlusCodeAnalyzer : CodeAnalyzer
    {
        protected override Command ModifyCommandForAnalysis(Command executionCommand)
        {
            //TODO fix it
            return executionCommand;
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            throw new NotImplementedException();
        }

        public CPlusPlusCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor) : base(configuration,
            codeFactory, containerConfiguration, executor)
        {
        }
    }

    public class PhpCodeAnalyzer : CodeAnalyzer
    {
        protected override Command ModifyCommandForAnalysis(Command executionCommand)
        {
            executionCommand.Arguments = executionCommand.Arguments.Append("-i").ToArray();

            return executionCommand;
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            //TODO fix this
            return CodeAnalysisResult.ValidCode;
        }

        public PhpCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor) : base(configuration,
            codeFactory, containerConfiguration, executor)
        {
        }
    }

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

            await File.WriteAllTextAsync(
                Path.Combine(tempFolder, Configuration.FileName + code.Language.GetExtension()), code.Text);

            ExecutableCode executableCode = _codeFactory.GetExecutableCode(code);

            Command executionCommand = executableCode.GetExecutionCommand(Configuration.TempFolderPath,
                _containerConfiguration.DockerWorkingDir);

            Command analysisCommand = ModifyCommandForAnalysis(executionCommand);

            ContainerExecutionResult containerExecutionResult = await _executor.ExecuteAsync(analysisCommand);

            CodeAnalysisResult codeAnalysis = AnalyseOutput(containerExecutionResult);

            Directory.Delete(tempFolder);

            return codeAnalysis;
        }

        protected virtual Command ModifyCommandForAnalysis(Command executionCommand) => executionCommand;

        protected abstract CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult);
    }

    public class CodeAnalyzerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CodeAnalyzerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CodeAnalyzer GetCodeAnalyzer(TestingCode code)
        {
            switch (code.Language)
            {
                case Language.Unspecified:
                    throw new ArgumentException($"Programming language is unspecified", nameof(code));
                case Language.Js:
                    return _serviceProvider.GetRequiredService<JavaScriptCodeAnalyzer>();
                case Language.Php:
                    return _serviceProvider.GetRequiredService<PhpCodeAnalyzer>();
                case Language.Pascal:
                    return _serviceProvider.GetRequiredService<PascalCodeAnalyzer>();
                case Language.CPlusPlus:
                    return _serviceProvider.GetRequiredService<CPlusPlusCodeAnalyzer>();
                default:
                    throw new NotImplementedException("Current programming language is not supported yet");
            }
        }
    }
}
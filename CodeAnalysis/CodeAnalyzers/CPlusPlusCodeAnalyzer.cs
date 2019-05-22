using System;
using System.Linq;
using CodeAnalysis.CodeAnalyzers.Base;
using CodeAnalysis.Configuration;
using CodeExecution.Contracts;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;

namespace CodeAnalysis.CodeAnalyzers
{
    public class CPlusPlusCodeAnalyzer : CodeAnalyzer
    {
        protected override Command ModifyCommandForAnalysis(Command executionCommand)
        {
            executionCommand.Arguments = new[] {"-c", executionCommand.Arguments.First()};
            
            return executionCommand;
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            return CodeAnalysisResult.ValidCode;
        }

        public CPlusPlusCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor) : base(configuration,
            codeFactory, containerConfiguration, executor)
        {
        }
    }
}
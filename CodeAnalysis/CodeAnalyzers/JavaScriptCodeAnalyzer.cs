using System;
using CodeAnalysis.CodeAnalyzers.Base;
using CodeAnalysis.Configuration;
using CodeExecution.Contracts;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;

namespace CodeAnalysis.CodeAnalyzers
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
}
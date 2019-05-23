using System;
using System.Linq;
using CodeAnalysis.CodeAnalyzers.Base;
using CodeAnalysis.Configuration;
using CodeExecution.Contracts;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;
using Helpers;

namespace CodeAnalysis.CodeAnalyzers
{
    public class PhpCodeAnalyzer : CodeAnalyzer
    {
        private readonly string PhpError = "PHP Parse error:";
        
        protected override Command ModifyCommandForAnalysis(Command executionCommand)
        {
            executionCommand.Arguments = executionCommand.Arguments.Prepend("-l").ToArray();

            return executionCommand;
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            AnalysisResult GetError(string output)
            {
                var message = output
                    .Replace(PhpError, string.Empty)
                    .Trim()
                    .FirstCharToUpper()
                    .Split("on line", StringSplitOptions.RemoveEmptyEntries);

                return new AnalysisResult
                {
                    Level = Level.Error,
                    Message = message.FirstOrDefault(),
                    Line = message.LastOrDefault()?.ToIntOrDefault() ?? 0
                };
            }
            
            if (containerExecutionResult.Result != ExecutionResult.Success)
            {
                return new CodeAnalysisResult
                {
                    IsSuccessful = false
                };
            }

            var strings = containerExecutionResult.StandardOutputSplit;
            
            var errors = strings.Where(output => output.Contains(PhpError)).Select(GetError).ToArray();

            return GetCodeAnalysisResult(errors, Enumerable.Empty<AnalysisResult>());
        }

        public PhpCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor) : base(configuration,
            codeFactory, containerConfiguration, executor)
        {
        }
    }
}
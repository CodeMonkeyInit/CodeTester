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
    public class PascalCodeAnalyzer : CodeAnalyzer
    {
        private const int PascalHeadersCount = 4;

        private const string PascalError = "Fatal: Syntax error,";
        private const string PascalWarning = "Note:";
        
        public PascalCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor) : base(configuration,
            codeFactory, containerConfiguration, executor)
        {
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            var freePascalOutput = containerExecutionResult.StandardOutputSplit.Skip(PascalHeadersCount).ToArray();

            Func<string, AnalysisResult> GetAnalysisResultFunction(Level level, string splitBy) =>
                pascalOutput =>
                {
                    var pascalOutputWithoutLevel = pascalOutput.Split(splitBy, StringSplitOptions.RemoveEmptyEntries)
                        .Select(output => output.Trim())
                        .ToArray();

                    var nameWithPosition = pascalOutputWithoutLevel.FirstOrDefault();
                    var lineAndColumn = nameWithPosition?
                        .Split('(')
                        .LastOrDefault()?
                        .Split(')')
                        .FirstOrDefault()?
                        .Split(',')
                        .ToArray();
                    
                    var message = pascalOutputWithoutLevel.LastOrDefault().FirstCharToUpper();

                    return new AnalysisResult
                    {
                        Level = level,
                        Message = message,
                        Line = lineAndColumn?.FirstOrDefault()?.ToIntOrDefault() ?? 0,
                        Column = lineAndColumn?.LastOrDefault()?.ToIntOrDefault() ?? 0
                    };
                };

           var errors = freePascalOutput.Where(output => output.Contains(PascalError)).Select(GetAnalysisResultFunction(Level.Error, PascalError));
           var warnings = freePascalOutput.Where(output => output.Contains(PascalWarning))
               .Select(GetAnalysisResultFunction(Level.Warning, PascalWarning));

           return GetCodeAnalysisResult(errors, warnings);
        }
    }
}
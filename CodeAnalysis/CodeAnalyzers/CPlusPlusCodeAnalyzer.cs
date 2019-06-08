using System;
using System.Collections.Generic;
using System.Linq;
using CodeAnalysis.CodeAnalyzers.Base;
using CodeAnalysis.Configuration;
using CodeExecution.Contracts;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;
using Helpers;

namespace CodeAnalysis.CodeAnalyzers
{
    public class CPlusPlusCodeAnalyzer : CodeAnalyzer
    {
        private const string GccError = "error";
        private const string GccWarning = "warning";
        private const char GccSeparator = ':';

        private const int GccFilePath = 0;
        private const int GccLine = 1;
        private const int GccColumn = 2;
        private const int GccLevel = 3;
        private const int GccMessage = 4;


        protected override Command ModifyCommandForAnalysis(Command executionCommand)
        {
            var filename = executionCommand.Arguments.First();
            var latestStandardFlag = executionCommand.Arguments.Last();
            
            executionCommand.Arguments = new[] {"-c", filename, latestStandardFlag};

            return executionCommand;
        }

        protected override CodeAnalysisResult AnalyseOutput(ContainerExecutionResult containerExecutionResult)
        {
            IEnumerable<string[]> gccOutputSplit =
                containerExecutionResult.StandardOutputSplit.Select(output =>
                    output.Split(GccSeparator, StringSplitOptions.RemoveEmptyEntries).Select(gccOtput => gccOtput.Trim()).ToArray());

            Func<string[], AnalysisResult> GetAnalysisResultFunction(Level level) =>
                gccOutput => new AnalysisResult
                {
                    Level = level,
                    Message = gccOutput[GccMessage].FirstCharToUpper(),
                    Column = gccOutput[GccColumn].ToIntOrDefault(),
                    Line = gccOutput[GccLine].ToIntOrDefault()
                };

            var validGccOutput = gccOutputSplit
                .Where(message => message.Length >= GccMessage)
                .ToList();

            var errors = validGccOutput
                .Where(message => message[GccLevel] == GccError)
                .Select(GetAnalysisResultFunction(Level.Error))
                .ToArray();

            var warnings = validGccOutput
                .Where(message => message[GccLevel] == GccWarning)
                .Select(GetAnalysisResultFunction(Level.Warning));

            return GetCodeAnalysisResult(errors, warnings);
        }

        public CPlusPlusCodeAnalyzer(AnalysisConfiguration configuration, ExecutableCodeFactory codeFactory,
            ContainerConfiguration containerConfiguration, DockerContainerExecutor executor) : base(configuration,
            codeFactory, containerConfiguration, executor)
        {
        }
    }
}
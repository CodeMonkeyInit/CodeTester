using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeExecution.Compilaton;
using CodeExecution.Configuration;
using CodeExecution.Contracts;
using CodeExecution.Extension;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;

namespace CodeExecution
{
    public class CodeExecutor
    {
        private readonly CodeCompiler _compiler;
        private readonly CodeExecutionConfiguration _configuration;
        private readonly DockerContainerExecutor _executor;

        public CodeExecutor(CodeExecutionConfiguration configuration, CodeCompiler compiler,
            DockerContainerExecutor executor)
        {
            _configuration = configuration;
            _compiler = compiler;
            _executor = executor;
        }

        public async Task<CodeExecutionResult> ExecuteAsync(ExecutableCode testingCode)
        {
            //Create Environment
            var environmentPath = await CreateEnvironment(testingCode);

            //Compile
            if (testingCode is CompilableCode compilableCode)
            {
                var compilationResult = await _compiler.CompileCodeAsync(compilableCode);

                if (!compilationResult.WasSuccessful)
                    return new CodeExecutionResult
                    {
                        CompilationErrors = compilationResult.Errors
                    };
            }

            //Create Execution Environment

            var executionResults = await RunAsync(testingCode, environmentPath);

            return new CodeExecutionResult
            {
                Results = executionResults
            };
        }

        private async Task<TestRunResult[]> RunAsync(ExecutableCode testingCode, string environmentPath)
        {
            var binariesFolder = Path.Combine(environmentPath, "bin");

            Directory.Move(environmentPath, binariesFolder);

            var codeExecutionResultsTasks =
                testingCode.ExecutionData.Select(executionData =>
                    RunAsync(executionData, environmentPath, binariesFolder, testingCode));

            var executionResults = await Task.WhenAll(codeExecutionResultsTasks);

            Directory.Delete(environmentPath);
            return executionResults;
        }

        private async Task<TestRunResult> RunAsync(ExecutionData executionData, string environmentPath,
            string binariesFolder, ExecutableCode testingCode)
        {
            var testRunEnvironment = Path.Combine(environmentPath, Guid.NewGuid().ToString());

            Copy(binariesFolder, testRunEnvironment);

            var containerExecutionResult =
                await _executor.ExecuteAsync(testingCode.GetExecutionCommand(testRunEnvironment));

            var outputFile = Path.Combine(testRunEnvironment, "output.txt");
            return new TestRunResult
            {
                ExecutionResult = containerExecutionResult.Result,
                ExpectedOutput = executionData.OutputData,
                UserOutput = await GetUserOutput(outputFile, containerExecutionResult)
            };
        }

        private static async Task<string> GetUserOutput(string outputFile,
            ContainerExecutionResult containerExecutionResult)
        {
            return File.Exists(outputFile)
                ? await File.ReadAllTextAsync(outputFile)
                : containerExecutionResult.StandardOutput;
        }

        private async Task<string> CreateEnvironment(ExecutableCode testingCode)
        {
            var executingCodeFolder = Path.Combine(_configuration.TempFolderPath, Guid.NewGuid().ToString());

            var codePath = Path.Combine(executingCodeFolder,
                $"{_configuration.CodeName}{testingCode.Language.GetExtension()}");

            testingCode.WorkingDirectory = executingCodeFolder;

            Directory.CreateDirectory(testingCode.WorkingDirectory);

            await File.WriteAllTextAsync(codePath, testingCode.Text);
            return executingCodeFolder;
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (var fi in source.GetFiles()) fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);

            // Copy each subdirectory using recursion.
            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
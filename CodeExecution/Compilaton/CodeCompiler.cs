using System;
using System.IO;
using System.Threading.Tasks;
using CodeExecution.Configuration;
using CodeExecution.Contracts;
using CodeExecution.Extension;
using DockerIntegration;

namespace CodeExecution.Compilaton
{
    public class CodeCompiler
    {
        private readonly CodeExecutionConfiguration _configuration;
        private readonly DockerContainerExecutor _executor;

        public CodeCompiler(CodeExecutionConfiguration configuration, DockerContainerExecutor executor)
        {
            _configuration = configuration;
            _executor = executor;
        }
        public async Task<CompilationResult> CompileCodeAsync(CompilableCode code)
        {
            var codeFilename = _configuration.CodeName + code.Language.GetExtension();

            var pathToCode = Path.Combine(code.WorkingDirectory, codeFilename);
            
            await File.WriteAllTextAsync(pathToCode, code.Text);

            var compilationCommand = code.GetCompilationCommand(code.WorkingDirectory);

            var execute = await _executor.ExecuteAsync(compilationCommand);

            if (execute.WasSuccessful)
            {
                return new CompilationResult
                {
                    OutputPath = pathToCode,
                    ExecutablePath = code.GetExecutable(pathToCode)
                };
            }
            
            return new CompilationResult
            {
                Errors = execute.ErrorOutput.Split(Environment.NewLine)
            };
        }
    }
}
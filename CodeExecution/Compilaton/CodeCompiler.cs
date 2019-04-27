using System;
using System.IO;
using System.Linq;
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
        private readonly ContainerConfiguration _containerConfiguration;
        private readonly DockerContainerExecutor _executor;

        public CodeCompiler(CodeExecutionConfiguration configuration, ContainerConfiguration containerConfiguration,
         DockerContainerExecutor executor)
        {
            _configuration = configuration;
            _containerConfiguration = containerConfiguration;
            _executor = executor;
        }
        public async Task<CompilationResult> CompileCodeAsync(CompilableCode code)
        {
            var codeFilename = _configuration.CodeName + code.Language.GetExtension();

            var pathToCode = Path.Combine(code.WorkingDirectory, codeFilename);
            
            //await File.WriteAllTextAsync(pathToCode, code.Text);

            var compilationCommand = code.GetCompilationCommand(code.WorkingDirectory, _containerConfiguration.DockerWorkingDir);

            var execute = await _executor.ExecuteAsync(compilationCommand);

            if (execute.WasSuccessful && (!(code is CPlusPlusCode) || string.IsNullOrWhiteSpace(execute.StandardOutput)))
            {
                return new CompilationResult
                {
                    OutputPath = pathToCode,
                    ExecutablePath = code.GetExecutable(pathToCode)
                };
            }
            
            return new CompilationResult
            {
                Errors =  execute.ErrorOutput.Split(Environment.NewLine)
                .Union(execute.StandardOutput.Split(Environment.NewLine))
                .ToArray()
            };
        }
    }
}
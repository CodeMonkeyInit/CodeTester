using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CodeAnalysis;
using DockerIntegration;

namespace CodeExecution
{
    public abstract class CompilableCode: ExecutionCode
    {
        public abstract Command GetCompilationCommand(string pathToCode);
        public abstract string GetExecutable(string pathToCode);
    }
    
    public class CPlusPlusCode : CompilableCode
    {
        public override Command GetCompilationCommand(string pathToCode)
        {
        }

        public override string GetExecutable(string pathToCode)
        {
            throw new NotImplementedException();
        }
    }

    public class PascalCode : CompilableCode
    {
        public override Command GetCompilationCommand(string pathToCode)
        {
            throw new NotImplementedException();
        }

        public override string GetExecutable(string pathToCode)
        {
            throw new NotImplementedException();
        }
    }
    

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

            var compilationCommand = code.GetCompilationCommand(pathToCode);

            var execute = await _executor.ExecuteAsync(compilationCommand);

            if (execute.WasSuccessful)
            {
                return new CompilationResult
                {
                    OutputPath = pathToCode,
                    Executable = code.GetExecutable(pathToCode)
                };
            }
            
            return new CompilationResult
            {
                Errors = execute.ErrorOutput
            };
        }
    }

    public class CodeExecutor
    {
        private readonly CodeExecutionConfiguration _configuration;
        private readonly CodeCompiler _compiler;
        private readonly DockerContainerExecutor _executor;

        public CodeExecutor(CodeExecutionConfiguration configuration, CodeCompiler compiler, DockerContainerExecutor executor)
        {
            _configuration = configuration;
            _compiler = compiler;
            _executor = executor;
        }

        public async Task<CodeExecutionResult> Execute(ExecutionCode code)
        {
            //Create Environment
            var executingCodeFolder = Path.Combine(_configuration.TempFolderPath, Guid.NewGuid().ToString());
            
            
            
            //

            //Compile
            if (code is CompilableCode compilableCode)
            {
                CompilationResult compilationResult = await _compiler.CompileCodeAsync(compilableCode);
                
            }
            
            //Create Execution Environment

            await _executor.ExecuteAsync(new Command {Name = code.Text});

            //Execute


            //Remove Environment
        }
    }

    public class ExecutionCode : Code
    {
        public string[] InputData { get; set; }

        public string[] OutputData { get; set; }
    }

    public class CodeExecutionResult
    {
         
        
        public string[] CompilationErrors { get; set; }
    }
}
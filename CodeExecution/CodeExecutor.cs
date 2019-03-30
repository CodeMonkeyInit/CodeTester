using System;
using System.IO;
using System.Threading.Tasks;
using CodeAnalysis;

namespace CodeExecution
{
    public class CodeExecutor
    {
        private readonly CodeExecutorConfiguration _configuration;

        public CodeExecutor(CodeExecutorConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CodeExecutionResult> Execute(ExecutionCode code)
        {
            //Create Environment
            var executingCodeFolder = Path.Combine(_configuration.TempFolderPath, Guid.NewGuid().ToString());

            var codeFilename = _configuration.CodeName + code.Language.GetExtension();
            
            await File.WriteAllTextAsync(Path.Combine(executingCodeFolder, codeFilename), code.Text);
            
            File.WriteAllText();

            //Compile

            //Execute


            //Remove Environment
        }
    }

    public class CodeExecutorConfiguration
    {
        public string TempFolderPath { get; set; }

        public string CodeName { get; set; } = "code";
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
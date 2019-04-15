using System;
using System.IO;
using AutoMapper;
using CodeExecution.Configuration;
using CodeExecution.Extension;
using CodeExecutionSystem.Contracts;
using CodeExecutionSystem.Contracts.Data;
using DockerIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeExecution.Contracts
{
    

    public class ExecutableCodeFactory
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public ExecutableCodeFactory(ServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }
        
        public ExecutableCode GetExecutableCode(TestingCode code)
        {
            switch (code.Language)
            {
                case Language.Js:
                    return GetCodeType<JavaScriptCode>(code);
                case Language.Php:
                    return GetCodeType<PhpCode>(code);
                case Language.Pascal:
                    return GetCodeType<PascalCode>(code);
                case Language.CPlusPlus:
                    return GetCodeType<CPlusPlusCode>(code);
                default:
                    throw new ArgumentOutOfRangeException();
            }
       }

        private ExecutableCode GetCodeType<T>(TestingCode code) where T : ExecutableCode
        {
            var executableCode = _serviceProvider.GetRequiredService<T>();

            _mapper.Map(executableCode, code);

            return executableCode;
        }
    }
    
    public abstract class ExecutableCode : TestingCode
    {
        public ExecutableCode(CodeExecutionConfiguration configuration)
        {
            Configuration = configuration;
        }

        public abstract Command GetExecutionCommand(string workingDirectory);
        
        protected CodeExecutionConfiguration Configuration;
        
        protected string GetExecutablePath(string workingDirectory) => 
            Path.Combine(workingDirectory, Configuration.CodeName);

        protected string GetCodeFilePath(string workingDirectory) =>
            GetExecutablePath(workingDirectory) + Language.GetExtension();
    }
}
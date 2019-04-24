using System;
using AutoMapper;
using CodeExecutionSystem.Contracts.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CodeExecution.Contracts
{
    public class ExecutableCodeFactory
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public ExecutableCodeFactory(IServiceProvider serviceProvider, IMapper mapper)
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

            _mapper.Map(code, executableCode);

            return executableCode;
        }
    }
}
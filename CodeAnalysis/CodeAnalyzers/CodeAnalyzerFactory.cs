using System;
using CodeAnalysis.CodeAnalyzers.Base;
using CodeExecutionSystem.Contracts.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CodeAnalysis.CodeAnalyzers
{
    public class CodeAnalyzerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CodeAnalyzerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CodeAnalyzer GetCodeAnalyzer(TestingCode code)
        {
            switch (code.Language)
            {
                case Language.Unspecified:
                    throw new ArgumentException($"Programming language is unspecified", nameof(code));
                case Language.Js:
                    return _serviceProvider.GetRequiredService<JavaScriptCodeAnalyzer>();
                case Language.Php:
                    return _serviceProvider.GetRequiredService<PhpCodeAnalyzer>();
                case Language.Pascal:
                    return _serviceProvider.GetRequiredService<PascalCodeAnalyzer>();
                case Language.CPlusPlus:
                    return _serviceProvider.GetRequiredService<CPlusPlusCodeAnalyzer>();
                default:
                    throw new NotImplementedException("Current programming language is not supported yet");
            }
        }
    }
}
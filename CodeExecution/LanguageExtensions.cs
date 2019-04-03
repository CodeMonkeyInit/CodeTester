using System;
using CodeAnalysis;
using CodeExecutionSystem.Contracts;

namespace CodeExecution
{
    public static class LanguageExtensions
    {
        public static string GetExtension(this Language language)
        {
            switch (language)
            {
                case Language.Js:
                    return ".js";
                case Language.Php:
                    return ".php";
                case Language.Pascal:
                    return ".pas";
                case Language.CPlusPlus:
                    return ".cpp";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool CanBeCompiled(this Language language)
        {
            switch (language)
            {
                case Language.Js:
                case Language.Php:
                    return false;
                
                case Language.Pascal:
                case Language.CPlusPlus:
                    return true;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }

    public static class CodeExtensions
    {
        public static string GetCompilationCommand(this Code code)
        {
            if(!code.Language.CanBeCompiled())
                throw new InvalidOperationException("Code cannot be compiled");
            
            switch (code.Language)
            {
                case Language.Pascal:
                    //command
                    break;
                case Language.CPlusPlus:
                    //command
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code.Language), code.Language, null);
            }
        }
    }
}
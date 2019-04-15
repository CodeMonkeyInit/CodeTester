using System;
using CodeExecutionSystem.Contracts;
using CodeExecutionSystem.Contracts.Data;

namespace CodeExecution.Extension
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
    
}
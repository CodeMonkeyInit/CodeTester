using System;
using ByteSizeLib;
using CodeExecutionSystem.Contracts;
using DockerIntegration;

namespace CodeAnalysis
{
    public class Code
    {
        public string Text { get; set; }

        public Language Language { get; set; }

        public Command GetCommandForAnalysis()
        {
            var memoryLimit = ByteSize.FromMegaBytes(100).Bytes;

            switch (Language)
            {
                case Language.Js:
                    
                    break;
                case Language.Php:
                    break;
                case Language.Pascal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
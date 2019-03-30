using System;
using CodeExecutionSystem.Contracts;

namespace CodeAnalysis
{
    

    public abstract class Code
    {
        public string Author { get; set; }
        
        public string Text { get; set; }

        public Language Language { get; set; }

        public string PathToCode { get; set; } = string.Empty;
    }
}
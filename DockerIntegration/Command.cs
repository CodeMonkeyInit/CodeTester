using System.Collections.Generic;
using System.Linq;
using CodeExecutionSystem.Contracts.Data;

namespace DockerIntegration
{
    public class Command
    {
        public string Name { get; set; }
        
        public string[] Arguments { get; set; }
        
        public List<string> ProgramNameWithArguments => Arguments.Prepend(Name).ToList();
        
        public Limits Limits { get; set; } = new Limits();

        public string WorkingDirectory { get; set; }
    }
}
using System.Collections.Generic;
using System.Linq;
using CodeExecutionSystem.Contracts.Data;

namespace DockerIntegration
{
    public class Command
    {
        public string Name { get; set; }

        public string[] Arguments { get; set; } = new string[0];

        public List<string> ProgramNameWithArguments => Arguments.Prepend(Name).ToList();

        public Limits Limits { get; set; } = new Limits();

        public string WorkingDirectory { get; set; }

        public IList<string> EnvironmentVariables { get; set; }
    }
}
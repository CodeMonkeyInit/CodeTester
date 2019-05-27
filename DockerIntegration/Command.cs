using System.Collections.Generic;
using System.Linq;
using CodeExecutionSystem.Contracts.Data;

namespace DockerIntegration
{
    public class Command
    {
        public string Name { get; set; }

        public string[] Arguments { get; set; } = new string[0];

        public List<string> ProgramNameWithArguments => string.IsNullOrWhiteSpace(StdinFilename)
            ? Arguments.Prepend(Name).ToList()
            : new List<string> {"bash", "-c", $"{string.Join(' ', Arguments.Prepend(Name))} < {StdinFilename}"};

        public Limits Limits { get; set; } = new Limits();

        public string MountDirectory { get; set; }

        public string WorkingDirectory { get; set; }

        public string[] EnvironmentVariables { get; set; }

        public string StdinFilename { get; set; }
    }
}
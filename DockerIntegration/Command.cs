using System.Collections.Generic;

namespace DockerIntegration
{
    public class Command
    {
        public string Name { get; set; }
        
        public Dictionary<string, string> Arguments { get; set; }
        
        public Limits Limits { get; set; }
    }
}
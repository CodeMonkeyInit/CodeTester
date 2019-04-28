namespace DockerIntegration
{
    public class ContainerConfiguration
    {
        public string ImageName { get; set; }

        public long TimeBetweenContainerStatusChecksInMs { get; set; } = 100;

        public string DockerWorkingDir { get; set; } = "/mnt/docker";

        public string DockerEngineUri { get; set; } = "unix:///var/run/docker.sock";
    }
}
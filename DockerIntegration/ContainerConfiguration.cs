namespace DockerIntegration
{
    public class ContainerConfiguration
    {
        public string ImageName { get; set; }

        public string DockerWorkingDir { get; set; } = "/mnt/docker";
    }
}
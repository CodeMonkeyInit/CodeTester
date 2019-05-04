# CodeTester

## Installation

1. Download and install Docker https://www.docker.com
2. Run "docker pull codemonkeyinit/sevsu_tesing_app"
3. (For Windows) Edit "DockerEngineUri" in appsettings.json to

```json
"ContainerConfiguration": 
{   
  "DockerEngineUri": "npipe://./pipe/docker_engine" 
}
```

4. Run app

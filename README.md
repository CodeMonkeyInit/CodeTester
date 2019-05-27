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



##Code examples

```json
{
	"Text": "console.log(\"pitty\")",
	"ExecutionData": [
		{"InputData": "", "OutputData": "pitty"}
    ],
	"Language": "Js"
}

```

```json
{
	"Text": "#include <stdio.h>\n int main() { printf(\"pitty\"); return 0; }",
	"ExecutionData": [
		{"InputData": "", "OutputData": "pitty"}
    ],
	"Language": "CPlusPlus"
}

```

```json
{
	"Text": "begin write('pitty') end.",
	"ExecutionData": [
		{"InputData": "", "OutputData": "pitty"}
    ],
	"Language": "Pascal"
}
```

```json
{
	"Text": "<?php echo 'pitty'; ?>",
	"ExecutionData": [
		{"InputData": "", "OutputData": "pitty"}
    ],
	"Language": "Php"
}
```
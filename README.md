# SimonVossTask
Simon Voss Task

## Step: 1
-> First clone the repository and checkout to master branch(default)  :

```bash
https://github.com/HassanMehmoodBU/SimonVossTask.git
```

## Step: 2
-> Open Visual Studio and load "SimonVossTask.sln" (in the root directory/ master branch)

## Step: 3
-> Verify API address, open appsettings.json in SimonVossTask (webapp/ subfolder in root)

```bash
"API": "http://localhost:25699/api/search/"
```
## Step: 4 (1) run directly with docker-compose
-> Build the Project<br />
-> Start/Run (Docker Compose)

## Step: 4 (2) run IIS Express without docker
Goto Main project/ Solution properties<br />
-> Startup Project<br />
-> Set to Multiple Startup projects<br />
-> Select docker-compose to None<br />
-> Select SearchAPI to Start<br />
-> Select SimonVossTask to Start<br />
-> Build the Project<br />
-> Start/Run (IIS Express) 

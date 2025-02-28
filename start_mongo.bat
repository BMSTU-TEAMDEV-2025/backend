@echo off
setlocal

if not exist "mongo.id" (
    docker pull mongo:latest
    docker run -i -d -p 27017:27017 --pull=never mongo:latest >> mongo.id
    if errorlevel 1 (
        del mongo.id >nul 2>&1
        exit /b 1
    )
    exit /b 0
)

set /p id=<mongo.id

if "%id%"=="" (
    docker pull mongo:latest
    docker run -i -d -p 27017:27017 --pull=never mongo:latest >> mongo.id
    if errorlevel 1 (
        del mongo.id >nul 2>&1
        exit /b 1
    )
    exit /b 0
)

docker start %id% >nul 2>&1

endlocal

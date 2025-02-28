@echo off
setlocal

if not exist "mongo.id" (
    echo No mongo.id
    exit /b 1
)

set /p id=<mongo.id

if "%id%"=="" (
    del mongo.id
    echo No mongo.id
    exit /b 1
)

if "%~1"=="-d" (
    del mongo.id
    docker stop %id% >nul 2>&1
    docker rm -v -f %id% >nul 2>&1
    exit /b 0
)

docker stop %id% >nul 2>&1

endlocal

@echo off
setlocal

if not exist "mongo.id" (
    docker run -i -d -p 27017:27017 mongo:latest >> mongo.id
    exit /b 0
)

set /p id=<mongo.id

docker start %id% >nul 2>&1

endlocal

@echo off

docker-compose build 
docker-compose up --detach --wait

exit /b %errorlevel%

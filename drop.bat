@echo off

docker-compose down --rmi local -v
docker volume prune -f

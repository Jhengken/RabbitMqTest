## 環境
IDE: Visual Studio 2022
Docker Desktop

## rabbitmq server  
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management

## 問題
Headers 看資料還踹不出來
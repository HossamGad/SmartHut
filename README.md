# smarthut-client-smarthut-group2

## The applikation

This is an exercise to learn how to setup an front end client who's communicating with a backend SignalR hub (Websocket).

This application is built with **Microsoft Blazor Server**. Authentication and authorization is done with Azure AD, where a user needs a domain specific email to access the application.

The application is using **Microsoft.AspNetCore.SignalR.Client** in order to setup and listen to messages sent from the SignalR hub.

## Run in development

- Clone or download
- cd into root folder
- run `dotnet restore`
- then run `dotnet run`
- the application will only work on https://localhost:5001 due to CORS when communikating with an external backend

## When publishing the applikation

- **dotnet publish -c Release --self-contained -r linux-x64**
- ZIP the folder _bin/release/netcoreapp3.1/linux-x64/publish_
- Deliver...

## How to run the published app (PREQ. the host must run a Linux based distribution)

- In the root folder (linux-x64)
- run: **./SmarthutPOC --urls https://localhost:5001**
- _The server is being served with Kestrel, not the optimal solution, but it works for this purpose. Consider configure NGINX (reverse proxy) for serving the application._
- _Note that no SSL is configured, but since this applikation is served from an internal network, this shouldnt pose any issues_

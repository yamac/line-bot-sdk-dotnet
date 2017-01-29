# LinebotEcho - Echo bot

A simple example which shows a basic usage of the LINE bot.

# Getting Started

## Open solution or project

Open solution(sln) file in the Visual Studio 2017 or project(csproj) file in the Visual Studio Code.
You can also open in the command prompt or terminal.

## Configuration

Configure properties of the LINE Messaging API by one of environment variables, appsettings.json and user secrets.

### Environment variables

Set environment variables as shown in the following example.

On Windows.
```
Line:ChannelId=[Your channel id]
Line:ChannelSecret=[Your channel secret]
Line:ChannelAccessToken=[Your channel access token]
Line:WebhookPath=[Your webhook path]
```

On Mac OS X / Linux.
```
$ export Line__ChannelId=[Your channel id]
$ export Line__ChannelSecret=[Your channel secret]
$ export Line__ChannelAccessToken=[Your channel access token]
$ export Line__WebhookPath=[Your webhook path]
```
*(Note: Double underscore)*

### appsettings.json

Edit the appsettings.json file as shown in the following example.

***Caution:*** Please be careful not to commit or publish your confidential information to the Internet. It is strongly recommended that you store your confidential information as environment variables.

```
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "Line": {
    "ChannelId": "[Your channel id]",
    "ChannelSecret": "[Your channel secret]",
    "ChannelAccessToken": "[Your channel access token]",
    "WebhookPath":  "[Your webhook path]"
  }
}
```

### User secrets

It can also be stored as user secrets in the development environment.

## Restore, Build and Run

Restore packages, build and start debugging in the Visual Studio 2017 or Visual Studio Code.
You can also use the following dotnet command in the project directory.

```
$ dotnet restore
$ dotnet build
$ dotnet run
```

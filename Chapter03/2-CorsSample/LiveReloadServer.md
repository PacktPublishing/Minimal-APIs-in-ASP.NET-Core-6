
Install Live reload server for startup static file in In-Memory WebServer.
-g options consent to install tool globally on machine

```powershell
dotnet tool install -g LiveReloadServer
```

Launch Webserver in Frontend folder. Please, insert base path to command!
```
livereloadserver "{BasePath}\Chapter03\2-CorsSample\Frontend"
```


Run Backend and try to call backend from frontend app by click button
```
dotnet run .\Backend\CorsSample.csproj
```
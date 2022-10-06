
dotnet clean --configuration Release

dotnet build --configuration Release

$destination = "HttpKeyboardMouse\bin\Release\net6.0-windows"

Copy-Item -Path "config.yaml" -Destination $destination -Force

Copy-Item -Path "www" -Destination $destination -Recurse -Force

Compress-Archive -Update -Path $destination -DestinationPath package.zip

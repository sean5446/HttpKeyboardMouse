
$destination = "HttpKeyboardMouse\bin\Release\net6.0-windows"
$packageName = "package.zip"

# don't dot net clean, remove the whole directory in case things have been renamed
if (Test-Path $destination ) {
  Remove-Item -Recurse -Force $destination 
}
if (Test-Path $destination ) {
    Remove-Item -Force $packageName
}

dotnet build --configuration Release

Copy-Item -Path "config.yaml" -Destination $destination -Force
Copy-Item -Path "www" -Destination $destination -Recurse -Force

Compress-Archive -Path $destination -DestinationPath $packageName

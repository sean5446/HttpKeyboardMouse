
$destination = "HttpKeyboardMouse\bin\Release\net6.0-windows"
$packageName = "package.zip"
$packageFolder = "package"

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

# for asset (Publish Release in workflow)
Compress-Archive -Path $destination/* -DestinationPath $packageName

# for artifact (Upload Artifact in workflow)
# New-Item $packageFolder -ItemType directory
# Copy-Item -Path $destination/* -Destination $packageFolder -Recurse -Force


$binaryPath = "HttpKeyboardMouse\bin\Release\net6.0-windows"
$packageName = "package.zip"
$packageFolder = "package"

# don't dot net clean, remove the whole directory in case things have been renamed
if (Test-Path $binaryPath ) {
  Remove-Item -Recurse -Force $binaryPath
}
if (Test-Path $packageName ) {
    Remove-Item -Force $packageName
}
if (Test-Path $packageFolder ) {
    Remove-Item -Recurse -Force $packageFolder
}

dotnet build --configuration Release

Copy-Item -Force -Path "config.yaml" -Destination $binaryPath 
Copy-Item -Recurse -Force -Path "www" -Destination $binaryPath 

# for asset (Publish Release in workflow)
Compress-Archive -Path $binaryPath/* -DestinationPath $packageName

# for artifact (Upload Artifact in workflow)
# New-Item $packageFolder -ItemType directory
# Copy-Item -Path $binaryPath/* -Destination $packageFolder -Recurse -Force

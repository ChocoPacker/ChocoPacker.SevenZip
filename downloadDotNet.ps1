function Download-DotNet {
    $DOTNET_SDK_URL = 'https://download.microsoft.com/download/A/3/8/A38489F3-9777-41DD-83F8-2CBDFAB2520C/packages/DotNetCore.1.0.0-SDK.Preview2-x64.exe'
    Invoke-WebRequest -Uri $DOTNET_SDK_URL -OutFile dotnet.zip
    Add-Type -assembly 'system.io.compression.filesystem'
    if (Test-Path '.\dist-dotnet'){
        Remove-Item '.\dist-dotnet' -Force -Recurse    
    }
    
    [io.compression.zipfile]::ExtractToDirectory('dotnet.zip', 'dist-dotnet')
    $env:PATH=(Convert-Path .) + '\dist-dotnet;' + $env:PATH
}
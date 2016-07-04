function Download-DotNet {
    $DOTNET_SDK_URL = 'https://download.microsoft.com/download/1/5/2/1523EBE1-3764-4328-8961-D1BD8ECA9295/dotnet-dev-win-x64.1.0.0-preview2-003121.zip'
    Invoke-WebRequest -Uri $DOTNET_SDK_URL -OutFile dotnet.zip
    Add-Type -assembly 'system.io.compression.filesystem'
    if (Test-Path '.\dist-dotnet'){
        Remove-Item '.\dist-dotnet' -Force -Recurse    
    }
    
    [io.compression.zipfile]::ExtractToDirectory('dotnet.zip', 'dist-dotnet')
    $env:PATH=(Convert-Path .) + '\dist-dotnet;' + $env:PATH
}
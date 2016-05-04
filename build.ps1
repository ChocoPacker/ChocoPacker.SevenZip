param($buildNumber = 1,
    [switch]
    $localDotNet)
    
# If specified we will prepend PATH with local dotnet CLI SDK
if ($localDotNet){
    . .\downloadDotNet.ps1
    Download-DotNet
}

& dotnet restore ChocoPacker.SevenZip
if ($LASTEXITCODE -ne 0){
    Write-Output 'Failed to restore dotnet dependencies'
    exit 1
}

& dotnet pack ChocoPacker.SevenZip
if ($LASTEXITCODE -ne 0){
    Write-Output 'Failed to create SevenZip nuget'
    exit 1
}

& dotnet restore ChocoPacker.SevenZip.Tests
if ($LASTEXITCODE -ne 0){
    Write-Output 'Failed to restore dotnet test dependencies'
    exit 1
}

& dotnet test ChocoPacker.SevenZip.Tests
if ($LASTEXITCODE -ne 0){
    Write-Output 'Failed to run unit tests'
    exit 1
}

exit 0
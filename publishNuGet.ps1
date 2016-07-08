param(
    [Parameter(Mandatory=$true)]
    [string]
    $repoUrl,
    [Parameter(Mandatory=$true)]
    [string]
    $repoKey
)

$NUGET_EXE_URL = 'https://dist.nuget.org/win-x86-commandline/v3.4.4/NuGet.exe'
Invoke-WebRequest -Uri $NUGET_EXE_URL -OutFile NuGet.exe

$nugetPackages = Get-ChildItem -Recurse | Where { $_.PSPath -match 'bin\\Release\\.+(?<!symbols)\.nupkg' }
foreach ($package in $nugetPackages){
    Write-Output ('Publishing: ' + $package.FullName)
    .\nuget.exe push $package.FullName -ApiKey $repoKey -Source $repoUrl
    if ($LASTEXITCODE -ne 0){
        Write-Output 'Failed to publish nuget'
        exit 1
    }

    Write-Output 'Successfully published'
}
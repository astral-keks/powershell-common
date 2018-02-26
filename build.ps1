$version = "0.3.0"

$src = "$PSScriptRoot\src"
$package = "$PSScriptRoot\package"
$artifact = "$PSScriptRoot\artifact"

# Build binaries
if (Test-Path $artifact) { Remove-Item $artifact -Recurse }
if (Test-Path $package) { Remove-Item $package -Recurse }
dotnet restore $src\Common\PowerShell.Common.csproj
dotnet pack $src\Common\PowerShell.Common.csproj --configuration Release -o $artifact --version-suffix $version
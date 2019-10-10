param (
	[string]$BuildVersionNumber=$(throw "-BuildVersionNumber is required."),
	[string]$TagVersionNumber
)

& msbuild /t:restore Dispenser.sln

#foreach ($src in ls $PSScriptRoot\..\src/*) {
#    Push-Location $src
#
#	Write-Output "build: Building & packaging project in $src"
#
#    if ($TagVersionNumber -ne $null) {
#        $version = $TagVersionNumber
#    }
#    else {
#        $version = $BuildVersionNumber
#    }
#
#    & dotnet build -c Release
#    & dotnet pack -c Release --include-symbols -o ..\..\artifacts --no-build /p:PackageVersion=$version
#    if($LASTEXITCODE -ne 0) { exit 1 }    
#
#    Pop-Location
#}

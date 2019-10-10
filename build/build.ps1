param (
	[string]$BuildVersionNumber=$(throw "-BuildVersionNumber is required."),
	[string]$TagVersionNumber
)

& msbuild -t:restore /p:Configuration=Release Dispenser.sln

foreach ($src in ls $PSScriptRoot\..\src/*) {
    Push-Location $src

	Write-Output "build: Building & packaging project in $src"

    if ($TagVersionNumber -ne $null) {
        $version = $TagVersionNumber
    }
    else {
        $version = $BuildVersionNumber
    }

    & msbuild /p:Configuration=Release
    & msbuild -t:pack $src.csproj /p:Configuration=Release -p:IncludeSymbols=true -p:BuildOutputTargetFolder=..\..\artifacts /p:PackageVersion=$version
    if($LASTEXITCODE -ne 0) { exit 1 }    

    Pop-Location
}

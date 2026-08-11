[CmdletBinding()]
param(
    [string]$RepoRoot = "C:\Users\Administrator\source\repos\Medicals\AestheticClinic",
    [string]$ProjectPath = "C:\Users\Administrator\source\repos\Medicals\AestheticClinic\CrystalReportWebAPI\CrystalReportWebAPI\CrystalReportWebAPI.csproj",
    [string]$PublishPath = "C:\inetpub\wwwroot\AestheticEMR\CrystalReportWebAPI",
    [string]$Configuration = "Release",
    [string]$AppPoolName = "CrystalReportWebAPI",
    [switch]$SkipIis
)

$ErrorActionPreference = 'Stop'

function Resolve-MsBuildPath {
    $cmd = Get-Command msbuild.exe -ErrorAction SilentlyContinue
    if ($cmd) { return $cmd.Source }

    $vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
    if (Test-Path $vswhere) {
        $installPath = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
        if ($installPath) {
            $msbuild = Join-Path $installPath "MSBuild\Current\Bin\MSBuild.exe"
            if (Test-Path $msbuild) { return $msbuild }
        }
    }

    throw "MSBuild not found. Run from Developer PowerShell or install Visual Studio build tools."
}

function Resolve-AppPoolFromPhysicalPath {
    param([string]$PhysicalPath)

    Import-Module WebAdministration -ErrorAction Stop

    $normalized = [System.IO.Path]::GetFullPath($PhysicalPath).TrimEnd('\\').ToLowerInvariant()

    foreach ($site in Get-ChildItem IIS:\Sites) {
        if ($site.physicalPath) {
            $sitePath = [System.IO.Path]::GetFullPath($site.physicalPath).TrimEnd('\\').ToLowerInvariant()
            if ($sitePath -eq $normalized -and $site.applicationPool) {
                return $site.applicationPool
            }
        }

        foreach ($app in Get-WebApplication -Site $site.Name) {
            if ($app.physicalPath) {
                $appPath = [System.IO.Path]::GetFullPath($app.physicalPath).TrimEnd('\\').ToLowerInvariant()
                if ($appPath -eq $normalized -and $app.applicationPool) {
                    return $app.applicationPool
                }
            }
        }
    }

    return $null
}

if (-not (Test-Path $ProjectPath)) {
    throw "Project file not found: $ProjectPath"
}

$msbuild = Resolve-MsBuildPath
$tempPublishPath = Join-Path $env:TEMP ("crystal-publish-" + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $tempPublishPath -Force | Out-Null

$resolvedAppPool = $AppPoolName?.Trim()
if (-not $SkipIis -and [string]::IsNullOrWhiteSpace($resolvedAppPool)) {
    try {
        $resolvedAppPool = Resolve-AppPoolFromPhysicalPath -PhysicalPath $PublishPath
    }
    catch {
        Write-Warning "Could not auto-resolve IIS app pool: $($_.Exception.Message)"
    }
}

if (-not $SkipIis) {
    if ([string]::IsNullOrWhiteSpace($resolvedAppPool)) {
        Write-Warning "No IIS app pool resolved. Deployment will continue without app pool restart."
    }
    else {
        Write-Host "Using IIS app pool: $resolvedAppPool"
    }
}

try {
    if (-not $SkipIis -and -not [string]::IsNullOrWhiteSpace($resolvedAppPool)) {
        Import-Module WebAdministration -ErrorAction Stop
        $poolState = (Get-WebAppPoolState -Name $resolvedAppPool).Value
        if ($poolState -eq 'Started') {
            Write-Host "Stopping app pool: $resolvedAppPool"
            Stop-WebAppPool -Name $resolvedAppPool
        }
    }

    Write-Host "Publishing CrystalReportWebAPI to temp path: $tempPublishPath"
    & $msbuild $ProjectPath /t:Build /p:Configuration=$Configuration /p:DeployOnBuild=true /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=true /p:publishUrl="$tempPublishPath" /nologo /v:m
    if ($LASTEXITCODE -ne 0) {
        throw "MSBuild publish failed with exit code $LASTEXITCODE"
    }

    Push-Location $RepoRoot
    $commit = (& git rev-parse --short HEAD 2>$null)
    if (-not $commit) { $commit = "unknown" }
    Pop-Location

    $buildInfo = @(
        "Project=CrystalReportWebAPI",
        "Commit=$commit",
        "PublishedUtc=$([DateTime]::UtcNow.ToString('yyyy-MM-ddTHH:mm:ssZ'))",
        "Machine=$env:COMPUTERNAME",
        "User=$env:USERNAME",
        "Configuration=$Configuration"
    )
    Set-Content -Path (Join-Path $tempPublishPath "build-info.txt") -Value $buildInfo -Encoding UTF8

    if (-not (Test-Path $PublishPath)) {
        New-Item -ItemType Directory -Path $PublishPath -Force | Out-Null
    }

    Write-Host "Syncing files to IIS path: $PublishPath"
    & robocopy $tempPublishPath $PublishPath /MIR /R:2 /W:2 /NFL /NDL /NP | Out-Null
    $rc = $LASTEXITCODE
    if ($rc -gt 7) {
        throw "Robocopy failed with exit code $rc"
    }

    if (-not $SkipIis -and -not [string]::IsNullOrWhiteSpace($resolvedAppPool)) {
        Import-Module WebAdministration -ErrorAction Stop
        Write-Host "Starting app pool: $resolvedAppPool"
        Start-WebAppPool -Name $resolvedAppPool
    }

    $marker = Join-Path $PublishPath "build-info.txt"
    if (Test-Path $marker) {
        Write-Host "Deployment marker:`n$(Get-Content $marker | Out-String)"
    }

    Write-Host "CrystalReportWebAPI deployment completed successfully."
}
finally {
    if (Test-Path $tempPublishPath) {
        Remove-Item -Path $tempPublishPath -Recurse -Force -ErrorAction SilentlyContinue
    }
}

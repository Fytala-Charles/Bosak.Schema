#Requires -Version 7
<#
.SYNOPSIS
    Full local rebuild for Bosak.Schema: clean, restore, build, test.

.DESCRIPTION
    Runs a complete clean → restore → build (Release) → test cycle for the
    solution. Used locally before committing and mirrors the CI workflow in
    .github/workflows/build.yml.

.EXAMPLE
    .\tools\rebuild.ps1
#>
[CmdletBinding()]
param ()

$ErrorActionPreference = "Stop"
$root = Split-Path $PSScriptRoot -Parent

Push-Location $root
try {
    dotnet clean "Bosak.Schema.slnx"
    dotnet restore "Bosak.Schema.slnx"
    dotnet build "Bosak.Schema.slnx" --configuration Release --no-restore
    dotnet test "Bosak.Schema.slnx" --configuration Release --no-build
}
finally {
    Pop-Location
}

#Requires -Version 7
<#
.SYNOPSIS
    Validates that a project follows the FYTALA canonical documentation structure.

.DESCRIPTION
    Checks for required and recommended files, required sections in AGENTS.md, stale
    placeholders, old prefixed document names, sequential section numbering in the
    canonical docs, referenced-but-missing documents, and recent update dates.

.PARAMETER ProjectPath
    Path to the project root to validate. Defaults to the current directory.

.PARAMETER Strict
    If set, the script exits with code 1 on any warning or error.

.EXAMPLE
    .\check-docs.ps1 -ProjectPath D:\Development\Berlin

.EXAMPLE
    .\check-docs.ps1 -Strict
#>
[CmdletBinding()]
param (
    [string]$ProjectPath = ".",
    [switch]$Strict
)

$ErrorActionPreference = "Stop"
$ProjectPath = Resolve-Path $ProjectPath | Select-Object -ExpandProperty Path

$exitCode = 0
$errors = 0
$warnings = 0

function Write-Result {
    param (
        [string]$Message,
        [ValidateSet("Pass", "Fail", "Warn", "Info")]
        [string]$Level = "Info"
    )

    switch ($Level) {
        "Pass"  { Write-Host "  [PASS]  $Message" -ForegroundColor Green }
        "Fail"  { Write-Host "  [FAIL]  $Message" -ForegroundColor Red;   $script:errors++ }
        "Warn"  { Write-Host "  [WARN]  $Message" -ForegroundColor Yellow; $script:warnings++ }
        "Info"  { Write-Host "  [INFO]  $Message" -ForegroundColor Cyan }
    }
}

function Test-FileExists {
    param ([string]$RelativePath)
    $full = Join-Path $ProjectPath $RelativePath
    if (Test-Path $full) {
        Write-Result "Found $RelativePath" -Level Pass
        return $true
    }
    else {
        Write-Result "Missing $RelativePath" -Level Fail
        return $false
    }
}

function Test-FileRecommended {
    param ([string]$RelativePath)
    $full = Join-Path $ProjectPath $RelativePath
    if (Test-Path $full) {
        Write-Result "Found $RelativePath" -Level Pass
        return $true
    }
    else {
        Write-Result "Recommended file missing: $RelativePath" -Level Warn
        return $false
    }
}

function Test-DateFreshness {
    param ([string]$RelativePath)
    $full = Join-Path $ProjectPath $RelativePath
    if (-not (Test-Path $full)) { return }
    $content = Get-Content -Raw -Path $full
    $match = [regex]::Match($content, "[Uu]pdated:\s*(\d{4}-\d{2}-\d{2})")
    if ($match.Success) {
        $lastUpdated = [DateTime]::ParseExact($match.Groups[1].Value, "yyyy-MM-dd", $null)
        $daysAgo = ([DateTime]::Now - $lastUpdated).Days
        if ($daysAgo -le 7) {
            Write-Result "$RelativePath last updated $daysAgo day(s) ago ($($match.Groups[1].Value))" -Level Pass
        }
        elseif ($daysAgo -le 30) {
            Write-Result "$RelativePath last updated $daysAgo day(s) ago ($($match.Groups[1].Value))" -Level Warn
        }
        else {
            Write-Result "$RelativePath last updated $daysAgo day(s) ago ($($match.Groups[1].Value)) — consider updating" -Level Warn
        }
    }
    else {
        Write-Result "$RelativePath missing 'Last updated:' header" -Level Warn
    }
}

# =================================================================================================
# Header
# =================================================================================================
Write-Host "`n========================================" -ForegroundColor White
Write-Host "Documentation Validation" -ForegroundColor White
Write-Host "Project: $ProjectPath" -ForegroundColor White
Write-Host "========================================" -ForegroundColor White

# =================================================================================================
# 1. Required files
# =================================================================================================
Write-Host "`n1. Required Files" -ForegroundColor White
$required = @(
    "README.md",
    "AGENTS.md",
    "ROADMAP.md",
    "LICENSE.md",
    "docs\ARCHITECTURE.md",
    "docs\FEATURE_REQUESTS.md",
    "docs\INTEGRATION.md",
    "docs\AGENT_HANDOVER.md",
    "docs\DOCUMENTATION_STYLE_GUIDE.md",
    ".kimi\HANDOVER.md"
)
foreach ($f in $required) {
    Test-FileExists $f | Out-Null
}

# =================================================================================================
# 2. Recommended files
# =================================================================================================
Write-Host "`n2. Recommended Files" -ForegroundColor White
$recommended = @(
    "Directory.Build.props",
    ".editorconfig",
    "global.json",
    ".env.example",
    "docs\ADR-000-TEMPLATE.md",
    "tools\rebuild.ps1",
    "tools\check-docs.ps1",
    ".github\workflows\build.yml"
)
foreach ($f in $recommended) {
    Test-FileRecommended $f | Out-Null
}

# =================================================================================================
# 3. AGENTS.md required sections
# =================================================================================================
Write-Host "`n3. AGENTS.md Required Sections" -ForegroundColor White
$agentsPath = Join-Path $ProjectPath "AGENTS.md"
if (Test-Path $agentsPath) {
    $agentsContent = Get-Content -Raw -Path $agentsPath

    $sections = @(
        @{ Pattern = "File Headers?";                          Description = "File Headers section" },
        @{ Pattern = "XML Documentation";                      Description = "XML Documentation section" },
        @{ Pattern = "Testing Requirements?";                  Description = "Testing Requirements section" },
        @{ Pattern = "Versioning";                             Description = "Versioning section" },
        @{ Pattern = "Architecture Decision Records";          Description = "Architecture Decision Records section" },
        @{ Pattern = "Documentation Sync Checklist";           Description = "Documentation Sync Checklist section" }
    )

    foreach ($sec in $sections) {
        if ($agentsContent -match $sec.Pattern) {
            Write-Result "AGENTS.md contains $($sec.Description)" -Level Pass
        }
        else {
            Write-Result "AGENTS.md missing $($sec.Description)" -Level Fail
        }
    }

    # Check for file header template
    if ($agentsContent -match "AUTHOR\s*:") {
        Write-Result "AGENTS.md contains a file header template" -Level Pass
    }
    else {
        Write-Result "AGENTS.md missing a file header template" -Level Fail
    }

    # Check for pre-handover checklist
    if ($agentsContent -match "Pre-Handover Checklist") {
        Write-Result "AGENTS.md contains Pre-Handover Checklist" -Level Pass
    }
    else {
        Write-Result "AGENTS.md missing Pre-Handover Checklist" -Level Fail
    }
}
else {
    Write-Result "AGENTS.md not found — skipping section checks" -Level Fail
}

# =================================================================================================
# 4. Stale placeholders
# =================================================================================================
Write-Host "`n4. Stale Placeholders" -ForegroundColor White
$allMd = Get-ChildItem -Path $ProjectPath -Recurse -Filter "*.md" -File |
    Where-Object { $_.FullName -notmatch "\.git" -and $_.FullName -notmatch "node_modules" }

$placeholderFound = $false
foreach ($md in $allMd) {
    $content = Get-Content -Raw -Path $md.FullName
    if ($content -match "\{\{[A-Z][A-Za-z0-9]*\}\}") {
        $rel = $md.FullName.Substring($ProjectPath.Length + 1)
        Write-Result "$rel contains unresolved placeholder: $($matches[0])" -Level Fail
        $placeholderFound = $true
    }
}
if (-not $placeholderFound) {
    Write-Result "No stale placeholders found in any .md file" -Level Pass
}

# =================================================================================================
# 5. Old prefixed document names
# =================================================================================================
Write-Host "`n5. Old Prefixed Document Names" -ForegroundColor White
$oldPrefixPattern = "\b(BERLIN_|ROSETTA_|BOSAK_|SILK_)(FEATURE_REQUESTS|INTEGRATION|ARCHITECTURE|AGENT_HANDOVER)\.md\b"
$prefixFound = $false
foreach ($md in $allMd) {
    $content = Get-Content -Raw -Path $md.FullName
    $matchesList = [regex]::Matches($content, $oldPrefixPattern)
    if ($matchesList.Count -gt 0) {
        $rel = $md.FullName.Substring($ProjectPath.Length + 1)
        foreach ($m in $matchesList) {
            $referencedFile = $m.Value
            # If a physical file with that exact name exists in docs/, assume it's a legitimate cross-project guide
            $physicalPath = Join-Path $ProjectPath "docs" $referencedFile
            if (Test-Path $physicalPath) {
                continue
            }
            Write-Result "$rel references old/stale name: $($m.Value)" -Level Fail
            $prefixFound = $true
        }
    }
}
if (-not $prefixFound) {
    Write-Result "No stale prefixed document references found" -Level Pass
}

# Also check for physical files with old prefixes (shouldn't exist after renames)
$prefixes = @("BERLIN", "ROSETTA", "BOSAK", "SILK")
$oldPhysical = @()
foreach ($prefix in $prefixes) {
    $oldPhysical += Get-ChildItem -Path (Join-Path $ProjectPath "docs") -Filter "${prefix}_FEATURE_REQUESTS.md" -File -ErrorAction SilentlyContinue
    $oldPhysical += Get-ChildItem -Path (Join-Path $ProjectPath "docs") -Filter "${prefix}_INTEGRATION.md" -File -ErrorAction SilentlyContinue
    $oldPhysical += Get-ChildItem -Path (Join-Path $ProjectPath "docs") -Filter "${prefix}_ARCHITECTURE.md" -File -ErrorAction SilentlyContinue
}
foreach ($f in $oldPhysical) {
    $rel = $f.FullName.Substring($ProjectPath.Length + 1)
    Write-Result "Old physical file exists: $rel" -Level Fail
    $prefixFound = $true
}
if (-not $prefixFound -and $oldPhysical.Count -eq 0) {
    Write-Result "No old prefixed physical files in docs/" -Level Pass
}

# =================================================================================================
# 6. Sequential section numbering in canonical docs
# =================================================================================================
Write-Host "`n6. Section Numbering" -ForegroundColor White
$canonical = @(
    "docs\ARCHITECTURE.md",
    "docs\INTEGRATION.md",
    "docs\FEATURE_REQUESTS.md",
    "docs\AGENT_HANDOVER.md"
)
foreach ($doc in $canonical) {
    $full = Join-Path $ProjectPath $doc
    if (-not (Test-Path $full)) { continue }
    $content = Get-Content -Raw -Path $full
    $nums = @([regex]::Matches($content, '(?m)^##\s+(\d+)\.') | ForEach-Object { [int]$_.Groups[1].Value })

    if ($nums.Count -lt 2) {
        Write-Result "$doc — fewer than 2 numbered sections; numbering check skipped" -Level Info
        continue
    }

    $dupes = @($nums | Group-Object | Where-Object { $_.Count -gt 1 } | ForEach-Object { $_.Name })
    $isAscending = $true
    for ($i = 1; $i -lt $nums.Count; $i++) {
        if ($nums[$i] -le $nums[$i - 1]) { $isAscending = $false; break }
    }

    if ($dupes.Count -gt 0) {
        Write-Result "$doc has duplicate section number(s): $($dupes -join ', ')" -Level Fail
    }
    elseif (-not $isAscending) {
        Write-Result "$doc section numbers out of order ($($nums -join ', '))" -Level Warn
    }
    else {
        Write-Result "$doc section numbering is sequential ($($nums.Count) sections)" -Level Pass
    }
}

# =================================================================================================
# 7. Referenced documents exist
# =================================================================================================
Write-Host "`n7. Referenced Documents Exist" -ForegroundColor White
$missingRefFound = $false
foreach ($md in $allMd) {
    $content = Get-Content -Raw -Path $md.FullName
    $rel = $md.FullName.Substring($ProjectPath.Length + 1)

    # References of the form docs/SOMETHING.md (from any file)
    foreach ($m in [regex]::Matches($content, '(?<![A-Za-z0-9_/.-])docs/([A-Za-z0-9_.-]+\.md)')) {
        $target = Join-Path $ProjectPath ("docs\" + $m.Groups[1].Value)
        if (-not (Test-Path $target)) {
            Write-Result "$rel references missing document: docs/$($m.Groups[1].Value)" -Level Fail
            $missingRefFound = $true
        }
    }

    # Markdown links to sibling files from inside docs/, e.g. [x](./INTEGRATION.md)
    if ($md.DirectoryName -eq (Join-Path $ProjectPath "docs")) {
        foreach ($m in [regex]::Matches($content, '\]\((?:\./)?([A-Za-z0-9_.-]+\.md)\)')) {
            $target = Join-Path $md.DirectoryName $m.Groups[1].Value
            if (-not (Test-Path $target)) {
                Write-Result "$rel links to missing document: $($m.Groups[1].Value)" -Level Fail
                $missingRefFound = $true
            }
        }
    }
}
if (-not $missingRefFound) {
    Write-Result "All referenced documents exist" -Level Pass
}

# =================================================================================================
# 8. Last-updated date freshness
# =================================================================================================
Write-Host "`n8. Last-Updated Date Freshness" -ForegroundColor White
Test-DateFreshness "docs\FEATURE_REQUESTS.md"
Test-DateFreshness "docs\AGENT_HANDOVER.md"
Test-DateFreshness ".kimi\HANDOVER.md"

# =================================================================================================
# Summary
# =================================================================================================
Write-Host "`n========================================" -ForegroundColor White
if ($errors -eq 0 -and $warnings -eq 0) {
    Write-Host "RESULT: ALL CHECKS PASSED" -ForegroundColor Green
}
elseif ($errors -eq 0) {
    Write-Host "RESULT: PASSED WITH WARNINGS ($warnings warning(s))" -ForegroundColor Yellow
}
else {
    Write-Host "RESULT: FAILED ($errors error(s), $warnings warning(s))" -ForegroundColor Red
    $exitCode = 1
}
Write-Host "========================================" -ForegroundColor White

if ($Strict -and ($errors -gt 0 -or $warnings -gt 0)) {
    exit 1
}
elseif ($errors -gt 0) {
    exit 1
}
else {
    exit 0
}

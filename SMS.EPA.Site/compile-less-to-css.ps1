# Script to compile layout.less to layout.css

Write-Host "Compiling layout.less to layout.css..."

# Read variables.less
$variables = Get-Content "wwwroot/Content/Site/variables.less" -Raw

# Read mixins.less  
$mixins = Get-Content "wwwroot/Content/Site/mixins.less" -Raw

# Read layout.less
$layout = Get-Content "wwwroot/Content/Site/layout.less" -Raw

# Extract variables and create replacements
$variableReplacements = @{}
$variables -split "`n" | ForEach-Object {
    if ($_ -match "^@([^:]+):\s*(.+);$") {
        $variableReplacements["@$($matches[1])"] = $matches[2].Trim()
    }
}

# Extract mixins and create replacements
$mixinReplacements = @{}
$mixins -split "`n" | ForEach-Object {
    if ($_ -match "^\.([^(]+)\(([^)]*)\)\s*\{") {
        $mixinName = $matches[1]
        $mixinParams = $matches[2]
        # This is a simplified approach - in reality you'd need to parse the full mixin body
        $mixinReplacements[".$mixinName"] = "/* Mixin: .$mixinName */"
    }
}

# Start with variables CSS
$css = $variables

# Add mixins CSS
$css += "`n`n" + $mixins

# Process each import in layout.less
$imports = $layout -split "`n" | Where-Object { $_ -match "^@import" }

foreach ($import in $imports) {
    if ($import -match '"(.*?)"') {
        $importPath = $matches[1]
        $fullPath = "wwwroot/Content/Site/$importPath"
        
        if (Test-Path $fullPath) {
            Write-Host "Processing import: $importPath"
            $importContent = Get-Content $fullPath -Raw
            
            # Replace variables
            foreach ($var in $variableReplacements.Keys) {
                $importContent = $importContent -replace [regex]::Escape($var), $variableReplacements[$var]
            }
            
            # Replace mixins (simplified - just remove mixin definitions for now)
            $importContent = $importContent -replace "^\..*?\{[^}]*\}", ""
            
            $css += "`n`n/* ===== $importPath ===== */`n"
            $css += $importContent
        } else {
            Write-Host "Warning: File not found: $fullPath"
        }
    }
}

# Add the remaining layout.less content (excluding imports)
$remainingLayout = $layout -split "`n" | Where-Object { $_ -notmatch "^@import" } | Out-String
$css += "`n`n/* ===== layout.less remaining content ===== */`n"
$css += $remainingLayout

# Replace variables in the final CSS
foreach ($var in $variableReplacements.Keys) {
    $css = $css -replace [regex]::Escape($var), $variableReplacements[$var]
}

# Write the compiled CSS
Set-Content "wwwroot/Content/Site/layout.css" -Value $css -NoNewline

Write-Host "Compilation complete! Created wwwroot/Content/Site/layout.css"

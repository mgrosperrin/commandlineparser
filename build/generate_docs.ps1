Param(
    [switch]$Serve
)
$ARTIFACTS_DIR = Join-Path $PSScriptRoot "artifacts"
$TOOLS_DIR = Join-Path $ARTIFACTS_DIR "tools"
$docfxRoot = Join-Path $TOOLS_DIR "docfx.console"
$docfx = Join-Path (Join-Path $docfxRoot "tools") "docfx.exe"
if (-not (Test-Path $docfx)) {
    mkdir -p $docfxRoot -ErrorAction Ignore | Out-Null
    $temp = (New-TemporaryFile).FullName + ".zip"
    Invoke-WebRequest "https://www.nuget.org/api/v2/package/docfx.console/2.52.0" -O $temp
    Expand-Archive $temp -DestinationPath $docfxRoot
    Remove-Item $temp
}
[string[]] $arguments = @()
if ($Serve) {
    $arguments += '--serve'
}
& $docfx docs/docfx.json @arguments

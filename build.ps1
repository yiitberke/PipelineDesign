# Rebuilds PipelineDesign.exe with the C# compiler that ships with Windows (.NET Framework 4). No extra install needed.
# PipelineDesign.exe dosyasini Windows ile gelen C# derleyicisiyle yeniden derler. Ek kurulum gerekmez.
$csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path $csc)) { Write-Error 'C# compiler not found (.NET Framework 4).'; exit 1 }
Set-Location $PSScriptRoot
& $csc /nologo /codepage:65001 /target:winexe /optimize+ '/out:PipelineDesign.exe' '/win32icon:src\app.ico' '/resource:pipeline-design.html,app.html' /r:System.Windows.Forms.dll 'src\launcher.cs'
if ($LASTEXITCODE -eq 0) { Write-Host 'Built: PipelineDesign.exe' }

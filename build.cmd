@echo off
cd %~dp0

if("%1"=="") goto noVersion
set version=%1

SETLOCAL
SET GLOBAL_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %GLOBAL_NUGET% goto copyNuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%GLOBAL_NUGET%'"

:copyNuget
IF EXIST build\nuget.exe goto runBuild
md build
copy %GLOBAL_NUGET% build\nuget.exe > nul

:runBuild
build\nuget restore

"%ProgramFiles(x86)%\MSBuild\14.0\bin\msbuild.exe" src\MGR.CommandLineParser\MGR.CommandLineParser.csproj /p:Configuration=Release /t:Rebuild
build\nuget pack build\MGR.CommandLineParser.nuspec -symbols -Version %version%

goto:eof

:noVersion
echo A version should be provided.

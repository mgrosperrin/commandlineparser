@echo off
cd %~dp0

if("%1"=="") goto noVersion
set version=%1

SETLOCAL
SET LOCAL_NUGET=build\NuGet.exe

IF EXIST %LOCAL_NUGET% goto runBuild
echo Downloading latest version of NuGet.exe...
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile '%LOCAL_NUGET%'"

:runBuild
build\nuget restore

build\nuget pack src\MGR.CommandLineParser\MGR.CommandLineParser.csproj -Properties Configuration=Release -Build -Symbols -MSBuildVersion 14
rem build\nuget pack build\MGR.CommandLineParser.nuspec -symbols -Version %version%

pause
goto:eof

:noVersion
echo A version should be provided.

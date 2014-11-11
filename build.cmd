@echo Off
build\nuget.exe restore

msbuild MGR.CommandLineParser.sln /p:Configuration=Debug /p:Platform="Any CPU"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var absoluteRootDir = MakeAbsolute(Directory("../"));
var rootDir = Directory(absoluteRootDir.FullPath);
var toolsDir = rootDir + Directory("artifacts/tools");
var nugetFile = toolsDir + File("nuget.exe");
var xunitRunnerFile = toolsDir + Directory("xunit.runner.console") + Directory("tools") + File("xunit.console.exe");
var outputBinariesDir = rootDir + Directory("artifacts/bin/");
var packagesDir = rootDir + Directory("packages/");
var solutionFile = rootDir + File("MGR.CommandLineParser.sln");
var testsDlls = GetFiles(outputBinariesDir.Path + "/*Tests.dll");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(outputBinariesDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile, new NuGetRestoreSettings { PackagesDirectory = packagesDir, ToolPath = nugetFile });
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration)
                .WithProperty("OutputPath", new string[]{outputBinariesDir}));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NuGetInstall("xunit.runner.console", new NuGetInstallSettings { OutputDirectory = toolsDir, ExcludeVersion = true, ToolPath = nugetFile });
    /*XUnit2(testsDlls, new XUnit2Settings {
        MaxThreads = 1,
        ToolPath = xunitRunnerFile,
        OutputDirectory = outputBinariesDir
        });*/
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests")
	;

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

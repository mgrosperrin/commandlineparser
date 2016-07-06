//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var mygetFeed = Argument("mygetFeed", "");
var buildNumber = Argument("buildNumber", "");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var absoluteRootDir = MakeAbsolute(Directory("../"));
var rootDir = Directory(absoluteRootDir.FullPath);
var artifactsDir = rootDir + Directory("artifacts");
var toolsDir = artifactsDir + Directory("tools");
var nugetFile = toolsDir + File("nuget.exe");
var xunitRunnerFile = toolsDir + Directory("xunit.runner.console") + Directory("tools") + File("xunit.console.exe");
var gitVersionFile = toolsDir + Directory("GitVersion.CommandLine") + Directory("tools") + File("GitVersion.exe");
var gitLinkFile = toolsDir + Directory("GitLink") + Directory("lib") + Directory("net45") + File("GitLink.exe");
var outputBinariesDir = artifactsDir + Directory("bin");
var packagesDir = rootDir + Directory("packages/");
var solutionFile = rootDir + File("MGR.CommandLineParser.sln");
var versionAssemblyFile = rootDir + Directory("src") + Directory("CommonFiles") + File("VersionAssemblyInfo.cs");
var commandLineParserNuspecFile = rootDir + Directory("src") + Directory("MGR.CommandLineParser") + File("MGR.CommandLineParser.nuspec");
var commandLineParserDllFile = outputBinariesDir + File("MGR.CommandLineParser.dll");
var commandLineParserPdbFile = outputBinariesDir + File("MGR.CommandLineParser.pdb");
var commandLineParserXmlFile = outputBinariesDir + File("MGR.CommandLineParser.xml");

var branchName = "";
var nugetFeed = "";
var version = "0.0.0";
var subVersion = "";
var shaHash = "";
var publishPackage = false;
var isApiKeyDefined = HasEnvironmentVariable("NUGET_API_KEY");
var mygetFeedDefined = !string.IsNullOrEmpty(mygetFeed);

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
    NuGetInstall("GitVersion.CommandLine", new NuGetInstallSettings {
        OutputDirectory = toolsDir,
        ExcludeVersion = true,
        ToolPath = nugetFile
    });
    NuGetInstall("xunit.runner.console", new NuGetInstallSettings {
        OutputDirectory = toolsDir,
        ExcludeVersion = true,
        ToolPath = nugetFile
    });
    NuGetInstall("gitlink", new NuGetInstallSettings {
        OutputDirectory = toolsDir,
        ExcludeVersion = true,
		Prerelease = true,
        ToolPath = nugetFile
    });
});

Task("Prepare-Build")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
{
	var gitVersion = GitVersion(new GitVersionSettings
	{
		ToolPath = gitVersionFile
	});
	version = gitVersion.MajorMinorPatch;
	branchName = gitVersion.BranchName;
	shaHash = gitVersion.Sha;
	if (branchName == "dev")
	{
		nugetFeed = mygetFeed;
		subVersion = "-alpha" + buildNumber;
		publishPackage = isApiKeyDefined && mygetFeedDefined;
	}
	else if (branchName.StartsWith("release-"))
	{
		publishPackage = isApiKeyDefined;
		subVersion = "-beta" + buildNumber;
	}
	else if (branchName == "master")
	{
		publishPackage = isApiKeyDefined;
	}
	CreateAssemblyInfo(versionAssemblyFile, new AssemblyInfoSettings {
		Version = version,
		FileVersion = version,
		InformationalVersion = version + subVersion
	});
});

Task("Build")
    .IsDependentOn("Prepare-Build")
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
	var testsDlls = GetFiles(outputBinariesDir.Path + "/*Tests.dll");
    XUnit2(testsDlls, new XUnit2Settings {
        MaxThreads = 1,
        ToolPath = xunitRunnerFile,
        OutputDirectory = outputBinariesDir
        });
});

Task("Create-Package")
	.IsDependentOn("Run-Unit-Tests")
	//.WithCriteria(() => publishPackage)
	.Does(() =>
{
	GitLink(rootDir, new GitLinkSettings {
		PdbDirectoryPath = outputBinariesDir,
		ToolPath = gitLinkFile,
		ShaHash = shaHash
	});
	var nuGetPackSettings = new NuGetPackSettings{
		ToolPath = nugetFile,
		Version = version + subVersion,
		Files = new List<NuSpecContent> {
			new NuSpecContent{ Source = commandLineParserDllFile, Target = "lib/net40" },
			new NuSpecContent{ Source = commandLineParserPdbFile, Target = "lib/net40" },
			new NuSpecContent{ Source = commandLineParserXmlFile, Target = "lib/net40" }
		},
		OutputDirectory = artifactsDir
	};
	var commandLineParserResourceFiles = GetFiles(MakeAbsolute(outputBinariesDir).FullPath + "\\*\\MGR.CommandLineParser.resources.dll");
	foreach(var resourceFile in commandLineParserResourceFiles)
	{
		var resourceFileFullPath = resourceFile.FullPath;
		var resourceName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(resourceFileFullPath));
		nuGetPackSettings.Files.Add(new NuSpecContent{ Source = resourceFile.FullPath, Target = "lib/net40/" + resourceName });
	}
	NuGetPack(commandLineParserNuspecFile, nuGetPackSettings);
});

Task("Publish-Package")
	.IsDependentOn("Create-Package")
	.WithCriteria(() => publishPackage)
	.Does(() =>
{
	var nugetPackageFiles = GetFiles(artifactsDir.Path + "\\MGR.CommandLineParser.*.nupkg");
	var commandLineParserNuGetFile = nugetPackageFiles.FirstOrDefault();
	if(commandLineParserNuGetFile != null)
	{
		NuGetPush(commandLineParserNuGetFile, new NuGetPushSettings {
			Source = nugetFeed,
			ApiKey = EnvironmentVariable("NUGET_API_KEY"),
			ToolPath = nugetFile
		});
	}
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Publish-Package")
	;

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

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
var nugetPackagePublicationFeed = "";
var version = "0.0.0";
var subVersion = "";
var shaHash = "";
var publishPackage = true;
var packagePublishingApiKeyName = "NUGET_API_KEY";
var isBuildingPR = HasEnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(outputBinariesDir);
});

Task("Install-Tools-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
	var defaultInstallSettings = new NuGetInstallSettings {
        OutputDirectory = toolsDir,
        ExcludeVersion = true,
        ToolPath = nugetFile
    };
    NuGetInstall("GitVersion.CommandLine", defaultInstallSettings);
    NuGetInstall("xunit.runner.console", defaultInstallSettings);
	var gitLinkInstallSettings =  new NuGetInstallSettings {
        OutputDirectory = toolsDir,
        ExcludeVersion = true,
		Prerelease = true,
        ToolPath = nugetFile
    };
    NuGetInstall("gitlink", gitLinkInstallSettings);
});

Task("Prepare-Build")
	.IsDependentOn("Install-Tools-Packages")
	.Does(() =>
{
	var gitVersionSettings = new GitVersionSettings
	{
		ToolPath = gitVersionFile
	};
	var gitVersion = GitVersion(gitVersionSettings);
	version = gitVersion.MajorMinorPatch;
	branchName = gitVersion.BranchName;
	shaHash = gitVersion.Sha;
	if (branchName == "dev")
	{
		subVersion = "-alpha" + buildNumber;
		nugetPackagePublicationFeed = mygetFeed;
		packagePublishingApiKeyName = "MYGET_API_KEY";
		publishPackage = !string.IsNullOrEmpty(nugetPackagePublicationFeed);
	}
	else if (branchName.StartsWith("release-"))
	{
		subVersion = "-beta" + buildNumber;
	}
	else if (branchName != "master")
	{
		publishPackage = false;
	}
	publishPackage = publishPackage && HasEnvironmentVariable(packagePublishingApiKeyName) && !isBuildingPR;
	
	var assemblyInfoSettings = new AssemblyInfoSettings {
		Version = version,
		FileVersion = version,
		InformationalVersion = version + subVersion
	};
	CreateAssemblyInfo(versionAssemblyFile, assemblyInfoSettings);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Prepare-Build")
    .Does(() =>
{
	var nugetRestoreSettings = new NuGetRestoreSettings {
		PackagesDirectory = packagesDir,
		ToolPath = nugetFile
	};
    NuGetRestore(solutionFile, nugetRestoreSettings);
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
	var testsDlls = GetFiles(outputBinariesDir.Path + "/*Tests.dll");
	var xunit2Settings = new XUnit2Settings {
        MaxThreads = 1,
        ToolPath = xunitRunnerFile,
        OutputDirectory = outputBinariesDir
	};
    XUnit2(testsDlls, xunit2Settings);
});

Task("Create-Package")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() =>
{
	var gitLinkSettings = new GitLinkSettings {
		PdbDirectoryPath = outputBinariesDir,
		ToolPath = gitLinkFile,
		ShaHash = shaHash
	};
	GitLink(rootDir, gitLinkSettings);
	var nuGetPackSettings = new NuGetPackSettings{
		ToolPath = nugetFile,
		Version = version + subVersion,
		Files = new List<NuSpecContent> {
			new NuSpecContent{
				Source = commandLineParserDllFile,
				Target = "lib/net40"
			},
			new NuSpecContent{
				Source = commandLineParserPdbFile,
				Target = "lib/net40"
			},
			new NuSpecContent{
				Source = commandLineParserXmlFile,
				Target = "lib/net40"
			}
		},
		OutputDirectory = artifactsDir
	};
	var commandLineParserResourceFiles = GetFiles(MakeAbsolute(outputBinariesDir).FullPath + "\\*\\MGR.CommandLineParser.resources.dll");
	foreach(var resourceFile in commandLineParserResourceFiles)
	{
		var resourceFileFullPath = resourceFile.FullPath;
		var resourceName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(resourceFileFullPath));
		nuGetPackSettings.Files.Add(new NuSpecContent{
			Source = resourceFile.FullPath,
			Target = "lib/net40/" + resourceName
		});
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
		var nugetPushSettings = new NuGetPushSettings {
			Source = nugetPackagePublicationFeed,
			ApiKey = EnvironmentVariable(packagePublishingApiKeyName),
			ToolPath = nugetFile
		};
		NuGetPush(commandLineParserNuGetFile, nugetPushSettings);
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

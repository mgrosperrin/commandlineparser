//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var mygetFeed = Argument("mygetFeed", "");

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

var nugetPackagePublicationFeed = "https://www.nuget.org/api/v2/package";
var informationalVersion = "1.0.0";
var sha1Hash = "";
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
        Version = "2.4.0",
        ToolPath = nugetFile
    };
    NuGetInstall("gitlink", gitLinkInstallSettings);
});

public string GetBranchName(GitVersion gitVersion)
{
	var branchName = EnvironmentVariable("APPVEYOR_REPO_BRANCH") ?? gitVersion.BranchName ?? "<Not Set>";
	return branchName;
}

public string GetPreReleaseNumber(string branchName, GitVersion gitVersion)
{
	if(branchName != gitVersion.BranchName)
	{
		return "1";
	}
	return gitVersion.PreReleaseNumber;
}

Task("Prepare-Build")
    .IsDependentOn("Install-Tools-Packages")
    .Does(() =>
{
	var assemblyInfoSettings = new AssemblyInfoSettings {
        Version = informationalVersion,
        FileVersion = informationalVersion,
        InformationalVersion = informationalVersion
    };
	if(!isBuildingPR)
    {
		var gitVersionSettings = new GitVersionSettings
		{
			ToolPath = gitVersionFile
		};
		var gitVersion = GitVersion(gitVersionSettings);
		var subVersion = "";
		var version = gitVersion.MajorMinorPatch;
		var branchName = GetBranchName(gitVersion);
		var buildNumber = GetPreReleaseNumber(branchName, gitVersion);
		sha1Hash = gitVersion.Sha;
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
		informationalVersion = version + subVersion;
		assemblyInfoSettings = new AssemblyInfoSettings {
			Version = version,
			FileVersion = version,
			InformationalVersion = informationalVersion
		};
	}
	else
	{
		publishPackage = false;
	}
	publishPackage &= HasEnvironmentVariable(packagePublishingApiKeyName);
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
        ToolPath = xunitRunnerFile,
        OutputDirectory = outputBinariesDir
    };
    XUnit2(testsDlls, xunit2Settings);
});

Task("Create-Package")
    .IsDependentOn("Run-Unit-Tests")
    .WithCriteria(() => !isBuildingPR)
    .Does(() =>
{
    var gitLinkSettings = new GitLinkSettings {
        PdbDirectoryPath = outputBinariesDir,
        ToolPath = gitLinkFile,
        ShaHash = sha1Hash
    };
    GitLink(rootDir, gitLinkSettings);
    var nuGetPackSettings = new NuGetPackSettings{
        ToolPath = nugetFile,
        Version = informationalVersion,
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

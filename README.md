MGR.Commandlineparser
=================

_**Build status**_

| dev        | [![Build status][build-status_dev_img]][build-status_dev_url] |
|------------|---------------------------------------------------------|
| **master** | [![Build status][build-status_master_img]][build-status_master_url] |


***MyGet*** *(preview bits)*:

| MGR.CommandLineParser | [![MyGet package version][myget_commandlineparser_img]][myget_commandlineparser_url] | [![Number of MyGet downloads][myget-download_commandlineparser_img]][myget-download_commandlineparser_url] |
|-|-|-|
| **MGR.CommandLineParser.Command.Lambda** | [![MyGet package version][myget_commandlineparser-command-lambda_img]][myget_commandlineparser-command-lambda_url] | [![Number of MyGet downloads][myget-download_commandlineparser-command-lambda_img]][myget-download_commandlineparser-command-lambda_url] |
| **MGR.CommandLineParser.Hosting** | [![MyGet package version][myget_commandlineparser-hosting_img]][myget_commandlineparser-hosting_url] | [![Number of MyGet downloads][myget-download_commandlineparser-hosting_img]][myget-download_commandlineparser-hosting_url] |

***Nuget***:

| MGR.CommandLineParser | [![NuGet package version][nuget_commandlineparser_img]][nuget_commandlineparser_url] | [![Number of NuGet downloads][nuget-download_commandlineparser_img]][nuget-download_commandlineparser_url] |
|-|-|-|
| **MGR.CommandLineParser.Command.Lambda** | [![NuGet package version][nuget_commandlineparser-command-lambda_img]][nuget_commandlineparser-command-lambda_url] | [![Number of NuGet downloads][nuget-download_commandlineparser-command-lambda_img]][nuget-download_commandlineparser-command-lambda_url] |
| **MGR.CommandLineParser.Hosting** | [![NuGet package version][nuget_commandlineparser-hosting_img]][nuget_commandlineparser-hosting_url] | [![Number of NuGet downloads][nuget-download_commandlineparser-hosting_img]][nuget-download_commandlineparser-hosting_url] |

[![Number of open issues][github-issues_img]][github-issues_url]
[![Number of open PR][github-pr_img]][github-pr_url]

MGR.CommandLineParser is a multi-command line parser. It uses [System.ComponentModel.DataAnnotations](http://msdn.microsoft.com/fr-fr/library/system.componentmodel.dataannotations.aspx) to declare and validate the commands.

# How to use it ?
You can find **more docs [here](docs/index.md)**

**I. Install MGR.CommandLineParser**

MGR.CommandLineParser is available through [NuGet][nuget]:

    PM> Install-Package MGR.CommandLineParser

**II. Declare your own commands**

After adding `MGR.CommandLineParser` to your project, you have to define your own commands:

* by implementing the interface `MGR.CommandLineParser.Command.ICommand`;
* by extending the abstract class `MGR.CommandLineParser.Command.CommandBase`.

To personnalize your commands, you add some properties to your class, and implement `Execute` (if you directly implement `ICommand`), or override `ExecuteCommand` (if you override `CommandBase`).

For example :
via `MGR.CommandLineParser.Command.ICommand`;
``` c#
public class HelloWorldCommand : ICommand
{
    [Display(ShortName = "n", Description = "The name to display")]
    [Required]
    public string Name {get; set;}

    public IList<string> Arguments {get; set;}

    public Task<int> ExecuteAsync()
    {
        Console.WriteLine("Hello world {0} !", Name);
        if(Arguments.Count > 0)
        {
            Console.WriteLine("Arguments : {0}", string.Join("," Arguments));
        }
        return Task.FromResult(0);
    }
}
```

Via `MGR.CommandLineParser.Command.CommandBase`.
```c#
public class HelloWorldCommand : CommandBase
{
    [Display(ShortName = "n", Description = "The name to display")]
    [Required]
    public string Name {get; set;}

    protected override Task<int> ExecuteCommandAsync()
    {
        Console.WriteLine("Hello world {0} !", Name);
        if(Arguments.Count > 0)
        {
            Console.WriteLine("Arguments : {0}", string.Join("," Arguments));
        }
        return Task.FromResult(0);
    }
}
```

**III. Parse the command line**

The simplest way to parse the command line is to call the `Parse` method on a `IParser` instance :
```c#
var parserBuilder = new ParserBuilder(new ParserOptions())
                .AddCommands(builder => builder.AddCommands<HelloWorldCommand>());
IParser parser = parserBuilder.BuildParser();
CommandResult<ICommand> commandResult = parser.Parse(args);
if(commandResult.IsValid)
{
    return commandResult.ExecuteAsync();
}
return commandResult.ReturnCode;
```

Or if you have define only one command for your program :
```c#
var parserBuilder = new ParserBuilder(new ParserOptions())
                .AddCommands(builder => builder.AddCommands<HelloWorldCommand>());
IParser parser = parserBuilder.BuildParser();
CommandResult<HelloWorldCommand> commandResult = parser.Parse<HelloWorldCommand>(args);
if(commandResult.IsValid)
{
    return commandResult.ExecuteAsync();
}
return commandResult.ReturnCode;
```

In the first case, the first item in the `args` parameter must be the name of the command (the name of the type, minus the suffix `Command` if present).
In the other case, the name of the command should be omitted.

Depending on the value of `args`, the result will be (when not providing the type of the command to the `Parse` method):

| Value of args | Result |
|------|--------|
|`null`|return code is `CommandResultCode.NoArgs` (-100)|
|empty enumeration of string|return code is `CommandResultCode.NoCommandName` (-200) and the global help is printed to the console|
|doesn't begin by `HelloWorld` or `Help` (the default help command)|return code is `CommandResultCode.NoCommandFound` (-300) and the global help is printed to the console|
|`HelloWorld`|return code is `CommandResultCode.CommandParameterNotValid` (-400) and the help for the `HelloWorldCommand` is printed to the console|
|`HelloWorld --name Matthias` or `HelloWorld -n Matthias`|return code is `CommandResultCode.Ok` (0) and `Hello world Matthias !` is printed to the console|


   [build-status_dev_img]: https://dev.azure.com/mgrosperrin/github/_apis/build/status/MGR.CommandLineParser?branchName=dev
   [build-status_dev_url]: https://dev.azure.com/mgrosperrin/github/_build?definitionId=14&_a=summary&repositoryFilter=4&branchFilter=22
   [build-status_master_img]: https://dev.azure.com/mgrosperrin/github/_apis/build/status/MGR.CommandLineParser?branchName=master
   [build-status_master_url]: https://dev.azure.com/mgrosperrin/github/_build?definitionId=14&_a=summary&repositoryFilter=4&branchFilter=39
   [myget_commandlineparser_img]: https://img.shields.io/myget/mgrosperrin/vpre/MGR.CommandLineParser.svg
   [myget_commandlineparser_url]: https://www.myget.org/feed/mgrosperrin/package/nuget/MGR.CommandLineParser/
   [myget-download_commandlineparser_img]: https://img.shields.io/myget/mgrosperrin/dt/MGR.CommandLineParser.svg
   [myget-download_commandlineparser_url]: https://www.myget.org/feed/mgrosperrin/package/nuget/MGR.CommandLineParser/
   [myget_commandlineparser-command-lambda_img]: https://img.shields.io/myget/mgrosperrin/vpre/MGR.CommandLineParser.Command.Lambda.svg
   [myget_commandlineparser-command-lambda_url]: https://www.myget.org/feed/mgrosperrin/package/nuget/MGR.CommandLineParser.Command.Lambda/
   [myget-download_commandlineparser-command-lambda_img]: https://img.shields.io/myget/mgrosperrin/dt/MGR.CommandLineParser.Command.Lambda.svg
   [myget-download_commandlineparser-command-lambda_url]: https://www.myget.org/feed/mgrosperrin/package/nuget/MGR.CommandLineParser.Command.Lambda/
   [myget_commandlineparser-hosting_img]: https://img.shields.io/myget/mgrosperrin/vpre/MGR.CommandLineParser.Hosting.svg
   [myget_commandlineparser-hosting_url]: https://www.myget.org/feed/mgrosperrin/package/nuget/MGR.CommandLineParser.Hosting/
   [myget-download_commandlineparser-hosting_img]: https://img.shields.io/myget/mgrosperrin/dt/MGR.CommandLineParser.Hosting.svg
   [myget-download_commandlineparser-hosting_url]: https://www.myget.org/feed/mgrosperrin/package/nuget/MGR.CommandLineParser.Hosting/
   [nuget_commandlineparser_img]: https://img.shields.io/nuget/v/MGR.CommandLineParser.svg
   [nuget_commandlineparser_url]: https://www.nuget.org/packages/MGR.CommandLineParser/
   [nuget-download_commandlineparser_img]: https://img.shields.io/nuget/dt/MGR.CommandLineParser.svg
   [nuget-download_commandlineparser_url]: https://www.nuget.org/stats/packages/MGR.CommandLineParser?groupby=Version
   [nuget_commandlineparser-command-lambda_img]: https://img.shields.io/nuget/v/MGR.CommandLineParser.Command.Lambda.svg
   [nuget_commandlineparser-command-lambda_url]: https://www.nuget.org/packages/MGR.CommandLineParser.Command.Lambda/
   [nuget-download_commandlineparser-command-lambda_img]: https://img.shields.io/nuget/dt/MGR.CommandLineParser.Command.Lambda.svg
   [nuget-download_commandlineparser-command-lambda_url]: https://www.nuget.org/stats/packages/MGR.CommandLineParser.Command.Lambda?groupby=Version
   [nuget_commandlineparser-hosting_img]: https://img.shields.io/nuget/v/MGR.CommandLineParser.Hosting.svg
   [nuget_commandlineparser-hosting_url]: https://www.nuget.org/packages/MGR.CommandLineParser.Hosting/
   [nuget-download_commandlineparser-hosting_img]: https://img.shields.io/nuget/dt/MGR.CommandLineParser.Hosting.svg
   [nuget-download_commandlineparser-hosting_url]: https://www.nuget.org/stats/packages/MGR.CommandLineParser.Hosting?groupby=Version
   [github-issues_img]: http://img.shields.io/github/issues/mgrosperrin/commandlineparser.svg
   [github-issues_url]: https://github.com/mgrosperrin/commandlineparser/issues
   [github-pr_img]: http://img.shields.io/github/issues-pr/mgrosperrin/commandlineparser.svg
   [github-pr_url]: https://github.com/mgrosperrin/commandlineparser/pulls

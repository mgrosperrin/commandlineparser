MGR.Commandlineparser
=================

_**Build status**_

| dev        | [![Build status][appveyor-dev-svg]][appveyor-dev]       |
|------------|---------------------------------------------------------|
| **master** | [![Build status][appveyor-master-svg]][appveyor-master] |


[![NuGet package version][nuget-svg]][nuget]
[![Number of NuGet downloads][nugetDownload-svg]][nugetDownload]
[![Number of open issues][githubIssues-svg]][githubIssues]

MGR.CommandLineParser is a multi-command line parser. It uses [System.ComponentModel.DataAnnotations](http://msdn.microsoft.com/fr-fr/library/system.componentmodel.dataannotations.aspx) to declare and validate the commands.

# How to use it ?
You can find more docs [here](docs/index.md)

I. **Install MGR.CommandLineParser**

MGR.CommandLineParser is available through [NuGet][nuget]:

    PM> Install-Package MGR.CommandLineParser

II. **Declare your own commands**

After adding MGR.CommandLineParser to your project, you have to define your own commands:

* by implementing the interface `MGR.CommandLineParser.Command.ICommand`;
* by extending the abstract class `MGR.CommandLineParser.Command.CommandBase`.

To personnalize your commands, you add some properties to your class, and implement `Execute` (if you directly implement `ICommand`), or override `ExecuteCommand` (if you override `CommandBase`).
For example :
via `MGR.CommandLineParser.Command.ICommand`;
```
public class HelloWorldCommand : ICommand
{
    [Display(ShortName = "n", Description = "The name to display")]
    [Required]
    public string Name {get; set;}

    public IList<string> Arguments {get; set;}

    public int Execute()
    {
        Console.WriteLine("Hello world {0} !", Name);
        if(Arguments.Count > 0)
        {
            Console.WriteLine("Arguments : {0}", string.Join("," Arguments));
        }
        return 0;
    }
}
```
Via `MGR.CommandLineParser.Command.CommandBase`.
```
public class HelloWorldCommand : CommandBase
{
    [Display(ShortName = "n", Description = "The name to display")]
    [Required]
    public string Name {get; set;}

    protected override int ExecuteCommand()
    {
        Console.WriteLine("Hello world {0} !", Name);
        if(Arguments.Count > 0)
        {
            Console.WriteLine("Arguments : {0}", string.Join("," Arguments));
        }
        return 0;
    }
}
```

III. **Parse the command line**

Then  simplest way to parse the command line is to call the `Parse` method on a `IParser` instance :
Then call the `Parse` method on a `IParser` instance :
```
var parserBuilder = new ParserBuilder();
IParser parser = parserBuilder.BuildParser();
CommandResult<ICommand> commandResult = parser.Parse(args);
if(commandResult.IsValid)
{
    return commandResult.Execute();
}
return commandResult.ReturnCode;
```
Or if you have define only one command for your program :
```
var parserBuilder = new ParserBuilder();
IParser parser = parserBuilder.BuildParser();
CommandResult<HelloWorldCommand> commandResult = parser.Parse<HelloWorldCommand>(args);
if(commandResult.IsValid)
{
    return commandResult.Execute();
}
return commandResult.ReturnCode;
```

In the first case, the first item in the `args` parameter must be the name of the command (the name of the type, minus the suffix `Command` if present).
In the other case, the name of the command should be omitted.

Depending on the value of `args`, the result will be (when not providing the type of the command to the `Parse` method) :

* `args` is null : return code is `CommandResultCode.NoArgs` (-100);
* `args` is an empty enumeration of string : return code is `CommandResultCode.NoCommandName` (-200) and the global help is printed to the console;
* `args` doesn't begin by `HelloWorld` or `Help` (the default help command) : return code is `CommandResultCode.NoCommandFound` (-300) and the global help is printed to the console;
* `args` is just `HelloWorld` : return code is `CommandResultCode.CommandParameterNotValid` (-400) and the help for the `HelloWorldCommand` is printed to the console;
* `args` is `HelloWorld -n Matthias` : return code is `CommandResultCode.Ok` (0) and `Hello world Matthias !` is printed to the console.


   [appveyor-dev]: https://ci.appveyor.com/project/mgrosperrin/commandlineparser
   [appveyor-dev-svg]: https://ci.appveyor.com/api/projects/status/lfp2jw7xle8vmpo3/branch/dev?svg=true
   [appveyor-master]: https://ci.appveyor.com/project/mgrosperrin/commandlineparser
   [appveyor-master-svg]: https://ci.appveyor.com/api/projects/status/lfp2jw7xle8vmpo3/branch/master?svg=true
   [nuget]: http://www.nuget.org/packages/MGR.CommandLineParser/
   [nuget-svg]: http://img.shields.io/nuget/v/MGR.CommandLineParser.svg
   [nugetDownload]: http://www.nuget.org/stats/packages/MGR.CommandLineParser?groupby=Version
   [nugetDownload-svg]: http://img.shields.io/nuget/dt/MGR.CommandLineParser.svg
   [githubIssues]: https://github.com/mgrosperrin/commandlineparser/issues
   [githubIssues-svg]: http://img.shields.io/github/issues/mgrosperrin/commandlineparser.svg

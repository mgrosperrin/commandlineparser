MGR.Commandlineparser
=================
[![Build status](http://img.shields.io/appveyor/ci/mgrosperrin/commandlineparser.svg)](https://ci.appveyor.com/project/mgrosperrin/commandlineparser)  
[![NuGet package version](http://img.shields.io/nuget/v/MGR.CommandLineParser.svg)](http://www.nuget.org/packages/MGR.CommandLineParser/)  
[![Number of NuGet downloads](http://img.shields.io/nuget/dt/MGR.CommandLineParser.svg)](http://www.nuget.org/stats/packages/MGR.CommandLineParser?groupby=Version)  
[![Number of open issues](http://img.shields.io/github/issues/mgrosperrin/commandlineparser.svg)](https://github.com/mgrosperrin/commandlineparser/issues)

MGR.CommandLineParser is a multi-command line parser. It uses [System.ComponentModel.DataAnnotations](http://msdn.microsoft.com/fr-fr/library/system.componentmodel.dataannotations.aspx) to declare and validate the commands.

#How to use it ?
I. **Get MGR.CommandLineParser**

MGR.CommandLineParser is available through [NuGet](https://www.nuget.org/packages/MGR.CommandLineParser/).

II. **Declare your own commands**

After adding MGR.CommandLineParser to your project, you have to define your own commands :

* by implementing the interface `MGR.CommandLineParser.Command.ICommand`;
* by extending the abstract class `MGR.CommandLineParser.Command.CommandBase`.

To personnalize your commands, you add some properties to your class, and implement `Execute` (if you directly implements `ICommand`), or override `ExecuteCommand` (if you override `CommandBase`).
For example :
```
public class HelloWorldCommand : CommandBase
{
    [Display(ShortName = "n", Description = "The name to display")]
    [Required]
    public string Name {get; set;}

    protected override int ExecuteCommand()
    {
        Console.WriteLine("Hello world {0} !", Name);
        return 0;
    }
}
```

III. **Parse the command line**

The simplest way to parse the command line is to call the `Parse` method on a `IParser` instance :
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

Depending on the value of `args`, the result will be :

* `args` is null : return code is `CommandResultCode.NoArgs` (-100);
* `args` is an empty enumeration of string : return code is `CommandResultCode.NoCommandName` (-200) and the global help is printed to the console;
* `args` doesn't begin by `HelloWorld` or `Help` (the default help command) : return code is `CommandResultCode.NoCommandFound` (-300) and the global help is printed to the console;
* `args` is just `HelloWorld` : return code is `CommandResultCode.CommandParameterNotValid` (-400) and the help for the `HelloWorldCommand` is printed to the console;
* `args` is `HelloWorld -n Matthias` : return code is `CommandResultCode.Ok` (0) and `Hello world Matthias !` is printed to the console.
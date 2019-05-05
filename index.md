MGR.Commandlineparser
=================

MGR.CommandLineParser is a multi-command line parser. It uses [System.ComponentModel.DataAnnotations](http://msdn.microsoft.com/fr-fr/library/system.componentmodel.dataannotations.aspx) to declare and validate the commands.

# How to use it ?
You can find **more docs [here](docs/index.md)**

## I. Install MGR.CommandLineParser

MGR.CommandLineParser is available through [NuGet][nuget]:

    PM> Install-Package MGR.CommandLineParser

## II. Declare your own commands

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

    protected override Task<int> ExecuteCommandasync()
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

## III. Parse the command line

The simplest way to parse the command line is to call the `Parse` method on a `IParser` instance :
```c#
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
```c#
var parserBuilder = new ParserBuilder();
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
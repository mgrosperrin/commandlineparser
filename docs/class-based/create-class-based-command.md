﻿# Create a class-based command

## Implement `ICommand`

To create a class-based command, you have to create a class that implements the `MGR.CommandLineParser.Command.ICommand` (or [inherits `CommandBase`](#Inherits-from-CommandBase)).
The interface provides two members:

1. a property `Arguments` (of type `IList<string>`) that will receive all arguments not mapped to an option,
2. a method `ExecuteAsync` (that take no parameters and return a `Task<int>`) that is called to execute the command. The returned value is the status code that should be returned as exit code.

You can add options to your command by simply adding properties to your class.
The properties can have any type, you just have to be sure there is a [converter](../extensibility/converter.md) for this type (see the [list of built-in converters](../extensibility/built-in-converters.md)).

You can customize the behavior of the options with some annotations (the parser supports the annotations from [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/dotnet/api/system.componentmodel.dataannotations)):

- Use `System.ComponentModel.DataAnnotations.DisplayAttribute` and some of its properties:
  - `Name` and `ShortName` to respectively change the long and short form of the option
  - `Description` to define a description in the auto-generated help for the option,
- Use any of the `System.ComponentModel.DataAnnotations.ValidationAttribute` derived class to validate the value provided for the option.

## Inherits from `CommandBase`
You can inherits from `MGR.CommandLineParser.Command.CommandBase`.
This abstract class provides:

- implementation of an `--help` option,
- access to the current `IServiceProvider`,
- access to the current [IConsole](../console.md)
- access to the [ICommandType](../extensibility/icommandtype.md) (which describes the current command)


When deriving from this class, you have to implement the `ExecuteCommandAsync` that is called when the value of the `--help` option is ```false```.


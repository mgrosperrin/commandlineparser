# Create a class-based command

To create a class-based command, you have to create two classes:
1. one that defines the data associated with the command,
2. one that implements the logic of the command.

Depending on what basic features you need for your command, you can choose to implement the logic of the command by inheriting from the abstract class `CommandBase<THelpedCommandData>` or by implementing the interface `ICommandHandler<TCommandData>`.

## Inherit from `CommandBase<THelpedCommandData>`
The abstract class `CommandBase<THelpedCommandData>` provides implementation for :

- an `--help` option,
- access to the current `IServiceProvider` via the property `ServiceProvider`,
- access to the current [IConsole](../extensibility/console.md) via the property `Console`.

When deriving from this class, you have to implement the `ExecuteCommandAsync` that is called when the value of the `--help` option is `false`.

You also have to make your command data class inherit from `MGR.CommandLineParser.Command.HelpedCommandData` to have the help option implemented.
It will also give you access to the [ICommandType](/api/MGR.CommandLineParser.Extensibility.Command.ICommandType.html) which describes the current command.

## Implement `ICommandHandler` and `CommandData`

If you don't need these features, you can implement the interface `ICommandHandler<TCommandData>`.

The interface defines a method `ExecuteAsync` to implement which is called to execute the command.
It takes two parameters:
1. The data of the command, of type of the generic type argument `TCommandData`,
2. A `CancellationToken` used to stop the execution of the command.

The method returns a `Task<int>` that represent the status code of the execution of the command (usually the exit code of the program).

The "data" class must inherit `CommandData`, that provides a property `Arguments` (of type `IList<string>`) that will receive all arguments not mapped to an option.
To define your own opptions, you have to add properties to this class.

## Customize your command

[Learn how to customize your command](customize-class-based-command.md).

## Add options to you command

You can add options to your command by simply adding properties to your "command data" class.
The properties can have any type, you just have to be sure there is a [converter](../extensibility/converter.md) for this type (see the [list of built-in converters](../extensibility/built-in-converters.md)).

You can customize the behavior of the options with some annotations (the parser supports the annotations from [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/dotnet/api/system.componentmodel.dataannotations)):

- Use `System.ComponentModel.DataAnnotations.DisplayAttribute` and some of its properties:
  - `Name` and `ShortName` to respectively change the long and short form of the option
  - `Description` to define a description in the auto-generated help for the option,
- Use any of the `System.ComponentModel.DataAnnotations.ValidationAttribute` derived class to validate the value provided for the option.

[See all details about customizing your options](customize-class-based-option.md).
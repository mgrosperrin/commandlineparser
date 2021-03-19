# Customize your class-based option

## Implement `ICommand`

You can add options to your command by simply adding properties to your class.
The properties can have any type, you just have to be sure there is a [converter](../extensibility/converter.md) for this type (see the [list of built-in converters](../extensibility/built-in-converters.md)).

You can customize the behavior of the options with some annotations (the parser supports the annotations from [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/dotnet/api/system.componentmodel.dataannotations)):

- Use `System.ComponentModel.DataAnnotations.DisplayAttribute` and some of its properties:
  - `Name` and `ShortName` to respectively change the long and short form of the option
  - `Description` to define a description in the auto-generated help for the option,
- Use any of the `System.ComponentModel.DataAnnotations.ValidationAttribute` derived class to validate the value provided for the option.

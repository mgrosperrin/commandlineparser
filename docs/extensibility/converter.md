# Converters

Converters is the mechanism to convert the arguments provided as `string`
to the strongly type expected by your option.

There is multiple [built-in converters](built-in-converters.md),
but you can create your own if you don't find the one you need.

Converter are implementation of the interface `MGR.CommandLineParser.Extensibility.Converters.IConverter`.

That interface defines the following:

- a property `TargetType` that let the parser knows for which type the converter is for,
- a method `Convert` that take the provided `string` and the type of the option for which the value is provided for.
The method returns an `object` that can be set to the option.

To add your own converter, you have to register it in the DI container used by the library.
If you want to replace one of the built-in converter, you have to manually remove the built-in converter
from the DI container.
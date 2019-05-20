# MGR.CommandLineParser documentation

<<<<<<< HEAD
`MGR.CommandLineParser` is an extensible commandline parser library.

It provides multiple ways to define the commands and their options (without counting your own providers for the commands)
and is able to automatically generate help/usage output:
=======
`MGR.CommandLineParser` is a multi-command line parser library.
It provides an extensible mechanism to provide define commands
(with multiple out-of-the-box providers)
and is able to automatically generate help/usage output for all commands.
Built-in providers lets you define command by:
>>>>>>> feb22c0 (Add more documentation)

- [creating a class](class-based/create-class-based-command.md) that implements `MGR.CommandLineParser.Command.ICommand` or inherits `MGR.CommandLineParser.Command.CommandBase` (provides some basic behavior for commands like support of `--help` option)
- [dynamically defining a command that uses a lambda](lambda/create-a-lambda-based-command.md) as execution (via the package `MGR.CommandLineParser.Command.Lambda)

<<<<<<< HEAD
The [expected syntax for the commandline](syntax/index.md) arguments follows the GNU convention (double dash for long form of options, simple dash for short form of options).

The extensible part of the library allow you to customize some part of the parsing engine, and mainly add your own command provider.

## Usage

This s

### [Learn how to create class-based commands](create-class-based-command.md)
###
=======
The general syntax on the command line is:
`<commandName> <options> <arguments>`

Options can be specified by their _normal/long_ form (`--full-option`)
or with its short form (`-fo`).
The value of the option can be separated of the name of the option
by a space (` `) or a colon (`:`).

Arguments is a list of non-option string that is passed to the command.


There is also some ways to customize others parts of the parser:

- how to display information to the user: the [`IConsole` interface](extensibility/console.md)
- how the value of the options are converted: the [`IConverter` interface](extensibility/converter.md)

This library use [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) to retrieve the implementation of all this service, so you just have to register your own implementation to override/augment the default behavior.



---
>>>>>>> feb22c0 (Add more documentation)
## [Customize command display](customize-command-display.md)
## [Call command's options](call-command-options.md)
## [Customize the internal of the parser](customize-internal-of-parser.md)
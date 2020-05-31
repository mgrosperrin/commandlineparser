# MGR.CommandLineParser documentation

`MGR.CommandLineParser` is a multi-command line parser library.
It provides multiple ways to define the commands and their options (without counting your own providers for the commands)
and is able to automatically generate help/usage output:

- [create a class](class-based/create-class-based-command.md) that implements `MGR.CommandLineParser.Command.ICommand` or inherits `MGR.CommandLineParser.Command.CommandBase` (provides some basic behavior for commands like support of `--help` option)
- [dynamically defines a command that uses a lambda](create-a-lambda-bsed-command as execution

### [Learn how to create class-based commands](create-class-based-command.md)
###
## [Customize command display](customize-command-display.md)
## [Call command's options](call-command-options.md)
## [Customize the internal of the parser](customize-internal-of-parser.md)
# Command based on a lambda

If you need to define command with some dynamism,
you can define command that are based on a lambda for its execution.

To define the command, you have to call the extension method `AddCommand` on the `CommandLineParserBuilder`.
The method expects:
- the name of the command
- an `Action<CommandBuilder>` to define the options of the command (and other parameters of the command)
- a `Func<CommandExecutionContext, Task<int>>` that holds the execution of the command. The `CommandExecutionContext` is the parameter that allows you to get the value of the different options.

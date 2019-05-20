# Customize your class-based command

## Add attribute `CommandAttribute`

The first way to customize your command is to add the attribute
 `MGR.CommandLineParser.Command.CommandAttribute` to your class.
This attribute allows you to define:

- the description of the command (displayed in the help) (`Description` property)
- the *global* usage of the command (displayed in the help) (`Usage` property). This is used to display how to use your command, and is the specific part for your command: the name of the executable and the name of the command should not be included!
- some samples for the command (displayed in the help) (`Samples` property)
- if the command should be hide from the listing of commands in the help (`HideFromHelpListing`). This can be useul for *preview* command because even if the command is not listed, it can be invoked!


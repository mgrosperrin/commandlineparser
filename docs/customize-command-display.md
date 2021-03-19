# Customize command display
## Command display
You can customize some command display attributes in the help with the `MGR.CommandLineParser.Command.CommandAttribute`.

You can add a `Description`, `Usage` and some `Samples`.

You can set `HideFromHelpListing` to hide a command the help listing.

## Option display
You can customize the display of the option's command by two ways:
1. Using the `System.ComponentModel.DataAnnotations.DisplayAttribute`
 and it's `Name`, `ShortName` and `Description` properties.
2. Registrying an implementation of
 `MGR.CommandLineParser.Extensibility.Command.IPropertyOptionAlternateNameGenerator`.

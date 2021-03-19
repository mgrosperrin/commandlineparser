# Displaying info to the user: IConsole

In order to ease the testability of your commands,
writing information to the users can be done though the `IConsole` interface.

This interface is also used by the parser itself to display info to the user (for example the help).
The internal logs are **not** written to the console.

As for others interface, you can provide your own implementation
for example if you want to use the library in an other type of GUI (like WPF or Web App).

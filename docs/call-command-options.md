# Call command's options

To call an option of a command, you can use the following syntaxes:
1. /LongOptionName
2. -LongOptionName
3. -l

when your option have defined as:
``` c#
[Display(ShortName = "l")]
public YourType LongOptionName { get; set; }
```

The short form of the option name can only be used with a single dash.
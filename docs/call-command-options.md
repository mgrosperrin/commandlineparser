# Call command's options

To call an option of a command, you can use the following syntaxes:
1. --long-option-name
2. -l

when your option have defined as:
``` c#
[Display(ShortName = "l")]
public YourType LongOptionName { get; set; }
```

The short form of the option name can only be used with a single dash.

Boolean options can be used combined in their short form.

for example, if you have defined the following options:
``` c#
[Display(ShortName = "v")]
public bool Verbose { get; set; }

[Display(ShortName = "f")]
public bool Force { get; set; }
```
Then you can use `-vf` (or `-fv`)
to specify the value of the options `Verbose` and `Force`
(the value can be specified like _classics_ boolean options).
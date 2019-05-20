# Built-in converters

The following converters are built-into the parser library (they are all in the namespace `MGR.CommandLineParser.Extensibility.Converters`):

|Converter type | Target type(s) |
|---|---|
| `BooleanConverter` | `bool` |
| `ByteConverter` | `byte` |
| `CharConverter` | `char` |
| `DateTimeConverter` | `DateTime` |
| `DecimalConverter` | `decimal` |
| `DoubleConverter` | `doube` |
| `EnumConverter` | An enum |
| `FileSystemInfoConverter` | `FileInfo` or a `DirectoryInfo` |
| `GuidConverter` | `Guid` |
| `Int16Converter` | `short` |
| `Int32Converter` | `int` |
| `Int64Converter` | `long` |
| `SingleConverter` | `float` |
| `StringConverter` | `string` |
| `TimeSpanConverter` | `TimeSpan` |
| `UriConverter` | `Uri` |
| `KeyValueConverter` | `KeyValuePair<T,V>` (this converter is used when the type of the option is a dictionary) |.

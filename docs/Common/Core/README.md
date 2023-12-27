## About
The Common Core package contains useful classes and interfaces for .NET components.

See [Change Log](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Core) for release notes.

## Key Features
It provides:
- Classes
- Converters
- Interfaces
- Models

## Main Types
- `Classes.JsonHelper` - Provides methods for serialization of data files and custom object types.
- `Classes.ModelBase` - Base class for models that require the `INotifyPropertyChanged` interface.
- `Classes.ModelDataError` - Base class for models _(and view models)_ that require the `INotifyPropertyChanged` and `INotifyDataErrorInfo` interfaces.
- `Converters.JsonBoolean` - Additionally converts `1/0`. `y/n`, or 'yes/no` string values to a boolean.
- `Converters.StringConverter` - Provides Try/Parse static methods to convert strings to other data types.

## Feedback
This is provided as open source under the MIT license.\
Bug reports and contributions are welcome [at the GitHub repository](https://github.com/KevinDHeath/HomeBase).
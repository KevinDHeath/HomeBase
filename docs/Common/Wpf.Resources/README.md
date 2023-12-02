## About
The Wpf.Resources package contains Windows Desktop resources which can be includes in .NET WPF projects.

## Key Features
Custom styles for `Buttons`, `CheckBoxes`, `ListViews`, `TabControls`, and `TextBoxes`.

## Change Log
- v1.0.3
  - Now targets .NET 8
  - Updated project website and source repository links.
- v1.0.2
  - Package name prefix changed to kdheath.
  - Name changed from `Common.Wpf` with no changes to any of the namespace names.
  - Removed the `DateOnlyToString`, `DateTimeToString`, and `UIntegerToString` converters.
- v1.0.1 Removed the `ComboBox.xaml` and `DatePicker.xaml` styles as they have been replaced with Custom controls.
- v1.0.0 Package added.

## How to Use
To include the resources in a WPF project:
- Add a reference to the `Common.Wpf.Resources` package.
- Include the required resource dictionaries in the `App.xaml` file.

```xml
<ResourceDictionary>
  <ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="pack://application:,,,/Common.Wpf.Resources;component/Themes/Common.xaml"/>
  </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

## Main Types
- `RelayCommand` - Classes to relay functionality to other objects by invoking delegates.
- `ConverterBase` - Base class for `IValueConverter` classes.
- `RuleBase` - Base class for validation rules.

## Feedback
This is provided as open source under the MIT license.\
Bug reports and contributions are welcome [at the GitHub repository](https://github.com/KevinDHeath/MyProjects).
# Common.Core Change Log

- v2.0.3
  - Fixed issue in `ModelDataError` when an unsupported annotation is applied to a property. For example `StringLength` for a `DataTime` property will result in an `InvalidCastException` exception.
- v2.0.2
  - Added the `AddressFactoryBase` class to support multi-country address data.
- v2.0.1
  - Added `Postcode` model to replace `USZipCode`\
  Column order changed in data file\
  ZipCode -> Code _(size changed from 5 to 10)_\
  State -> Province _(size changed from 2 to 10)_\
  County - no longer required\
  City - no longer required
  - Added `Province` model to replace `USState`\
  Alpha -> Code _(size changed from 2 to 10)_\
  Capital - Removed
- v2.0.0
  - GitHub repository name changed from `MyProjects` to `HomeBase`
  - Updated to .NET 8
- v1.0.3
  - Added Converters for Json.
  - Added `Interfaces.IDataFactory`
  - Added `Classes.DataFactoryBase`
  - Renamed `Common.Core.Classes.EditableHelper` to `ReflectionHelper`
- v1.0.2
  - Added the `ModelData` base class and the `ResultsSet` class to provide data set paging information
  - Moved the `Common.Data.Interfaces` to this module so they can be referenced without having to include the large `Common.Data` component which has the JSON data files embedded
  - Moved the `Common.Data.Models` classes for the same reason
  - Updated to target .NET 7
- v1.0.1 - Package created
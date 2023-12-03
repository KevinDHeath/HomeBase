# Common.Core
The Common Core local package contains classes and interfaces for .NET components.

## Change Log
- v2.0.0
  - GitHub repository name changed from `MyProjects` to `HomeBase`.
  - Updated to .NET 8
- v1.0.3
  - Added Converters for Json.
  - Added `Interfaces.IDataFactory`.
  - Added `Classes.DataFactoryBase`.
  - Rename `Common.Core.Classes.EditableHelper` to `ReflectionHelper`.
- v1.0.2
  - Added the `ModelData` base class and the `ResultsSet` class to provide data set paging information.
  - Moved the `Common.Data.Interfaces` to this module so they can be referenced without having to include the large `Common.Data` component which has the JSON data files embedded.
  - Moved the `Common.Data.Models` classes for the same reason.
  - Updated to target .NET 7
- v1.0.1 - Package created.
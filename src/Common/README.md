# Common Data
Implements 3 methods of providing data.

## Common.Data.API
Uses a restful API to access data using the Entity Framework provided by Microsoft.

- Caches zip code data
- Can maintain data

Dependencies:
- [kdheath.Common.Core](https://www.nuget.org/packages/kdheath.Common.Core)

## Common.Data.Json
Uses JSON data embedded in the binary file and is therefore not changeable.

Dependencies:
- [kdheath.Common.Core](https://www.nuget.org/packages/kdheath.Common.Core)

References:
- [Using embedded files in NET Core](https://josef.codes/using-embedded-files-in-dotnet-core/)

## Common.Data.Sql
Uses data directly from SQL Server.

- Caches zip code data
- Can maintain data

Dependencies:
- [kdheath.Common.Core](https://www.nuget.org/packages/kdheath.Common.Core)
- [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient)

# Json.Converters
Framework: .NET 6.0 _and_ .NET 8.0

> Note: This is not yet available on NuGet.

Dependencies:
- None

References:
- [JSON serialization and de-serialization in .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview)\
[Compare Newtonsoft.Json to System.Text.Json, and migrate to System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft)
- [De-serializing generic interfaces with System.Text.Json](https://www.mrlacey.com/2019/10/deserializing-generic-interfaces-with.html)
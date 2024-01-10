# Common Data
Implements 5 methods of providing data.

## Common.Data.API
Uses a restful API _(Representational State Transfer)_ to access data using the Entity Framework provided by Microsoft.

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

## Common.Data.SQLite
Uses data from a SQLite database using the Entity Framework provided by Microsoft.

- Caches zip code data
- Can maintain data

Dependencies:
- [kdheath.Common.Core](https://www.nuget.org/packages/kdheath.Common.Core)
- [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
- [Microsoft.Extensions.Configuration.Binder](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)
- [Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json)

## Common.Data.SqlServer
Uses data from a SQL Server database using the Entity Framework provided by Microsoft.

- Caches zip code data
- Can maintain data

Dependencies:
- [kdheath.Common.Core](https://www.nuget.org/packages/kdheath.Common.Core)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)
- [Microsoft.Extensions.Configuration.Binder](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)
- [Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json)

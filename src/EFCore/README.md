# Entity Framework Projects
- [Using a Separate Migrations Project](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects)
- [Design-time DbContext Creation](https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation)

> The [Entity Framework Core tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) must be installed to perform migrations.

```shell
dotnet-ef # Displays the current version
dotnet tool install --global dotnet-ef # Installs the tools
dotnet tool update --global dotnet-ef # Updates the tools version
```

## EFCore Data
This folder contains the SQL scripts, and data files used to define and populate tables in an existing database. 

References:
- [SQL Data Files](https://github.com/KevinDHeath/HomeBase/tree/main/src/EFCore/Data)


## EFCore.Database.Address
This contains the Address context code to create and populate a database with the necessary tables and indexes for Address data.

Dependencies:
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
- [Common.Data.SQLite](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Data/SQLite)

## EFCore.Database.Entity
This contains the Entity context code to create and populate a database with the necessary tables and indexes for Entity data.

Dependencies:
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
- [Common.Data.SQLite](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Data/SQLite)

## EFCore.Database.Full
This contains the Full context code to create and populate a database with the necessary tables and indexes for all data.

Dependencies:
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
- [Common.Data.SqlServer](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Data/SqlServer)


## EFCore RestApi
This ASP.NET REST API exposes the data stored in a Full context database as JSON using the HTTP protocol.

The connection string to access the database is stored in `appsettings.json`.

Dependencies:
- [Common.Data.SqlServer](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Data/SqlServer)
- [Microsoft.AspNetCore.OpenApi](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi)
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore)

References:
- [Get started with Swashbuckle and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle)
- [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Using multiple appsettings.json files in .NET](https://dev.to/rogeliogamez92/using-multiple-appsettingsjson-to-release-to-different-platforms-in-dotnet-2554)

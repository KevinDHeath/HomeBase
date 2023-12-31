# Entity Framework Projects

> For SQLServer databases use [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).

## EFCore Data
This contains the code, Sql scripts, and data files used to define and populate tables in an existing database. 

Dependencies:
- [kdheath.Common.Core](https://www.nuget.org/packages/kdheath.Common.Core)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)

References:
- [SQL Data Scripts](https://github.com/KevinDHeath/HomeBase/tree/main/src/EFCore/Data/Scripts)

## EFCore Database
This contains the Entity Framework code to populate an existing SQL Server database with the necessary tables and indexes.
> The [Entity Framework Core tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) must be installed to perform migrations.

The connection string to access the database is stored in project user secrets. For example:
``` json
{
  "ConnectionStrings": {
    "CommonData": "Server=localhost;Database=EFCore;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```
Dependencies:
- [EFCore.Data](https://github.com/KevinDHeath/HomeBase/tree/main/src/EFCore/Data)
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
- [Microsoft.Extensions.Configuration.Binder](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)
- [Microsoft.Extensions.Configuration.UserSecrets](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.UserSecrets)

References:
- [Using a Separate Migrations Project](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects)
- [Design-time DbContext Creation](https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation)

## EFCore.Database.Address
This contains the Entity Framework code to create and populate a database with the necessary tables and indexes for Address data.

Dependencies:
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
- _The Common.Data project for the database type being used._

## EFCore.Database.Entity
This contains the Entity Framework code to create and populate a database with the necessary tables and indexes for Entity data.

Dependencies:
- [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
- _The Common.Data project for the database type being used._

## EFCore RestApi
This ASP.NET REST API exposes the data stored in a Sql database as JSON using the HTTP protocol.

The connection string to access the database is stored in `appsettings.json`.

Dependencies:
- [EFCore.Data](https://github.com/KevinDHeath/HomeBase/tree/main/src/EFCore/Data)
- [Microsoft.AspNetCore.OpenApi](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi)
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore)

References:
- [Get started with Swashbuckle and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle)
- [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Using multiple appsettings.json files in .NET](https://dev.to/rogeliogamez92/using-multiple-appsettingsjson-to-release-to-different-platforms-in-dotnet-2554)

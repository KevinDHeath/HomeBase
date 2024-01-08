# EFCore.Database
This builds the migrations to update the EFCore SQL Server database used by `Common.Data.Sql`.

> The `EFCore.Data` project defines the actual Data Context, this is only responsible for establishing the SQL Server database connection.

To generate a migration, start a NuGet Package Manager PowerShell session in Visual Studio and enter the following:
```shell
cd src/EFCore/Database
dotnet-ef migrations add [migration name]

dotnet-ef migrations remove # remove the latest migration
-or-
dotnet-ef database update # applies the latest migration
```

The project has a User Secret associated with it that contains the connection string which points to the database where the migrations will be applied.
~~~json
{
  "ConnectionStrings": {
    "CommonData": "Server=localhost;Database=EFCore8;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
~~~

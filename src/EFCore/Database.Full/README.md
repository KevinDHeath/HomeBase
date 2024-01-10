# EFCore.Database
This builds the migrations to update the EFCore Sql Server database used by `Common.Data.SqlServer`.

To generate a migration, start a NuGet Package Manager PowerShell session in Visual Studio and enter the following:
```shell
cd src/EFCore/Database.Full
dotnet-ef migrations add [migration name]

dotnet-ef migrations remove # remove the latest migration
-or-
dotnet-ef database update # applies the latest migration
```

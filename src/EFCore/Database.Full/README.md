# EFCore.Database.Full
This builds the migrations to update the EFCore Sql Server database used by `Common.Data.SqlServer`.

**Note:** The database _must exist_ before any migrations are generated. The default name of the database is `EFCommonData`.\
Please see [Common.Data.SqlServer](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Data/SqlServer) for information on how to use a different database.

To generate a migration, start a NuGet Package Manager PowerShell session in Visual Studio and enter the following:

```shell
cd src/EFCore/Database.Full
dotnet-ef migrations add [migration name]

dotnet-ef database update # applies the latest migration
-or-
dotnet-ef migrations remove # remove the latest migration
```
The JSON files in the Data\Seed folder are used during the create process to automatically seed the data in the majority of the tables.

**Important:** Because the Company and Person data cannot be automatically seeded due to them having a 'owns one' relationship to `Address`, import the data manually into the tables with the tab-delimited files supplied in the `Data` folder. See [SQL Data](https://github.com/KevinDHeath/HomeBase/tree/main/src/EFCore/Data) for information on how to import flat-file data.
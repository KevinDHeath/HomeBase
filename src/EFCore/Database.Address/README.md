# EFCore.Database.Address
This builds the migrations to update an EFCore SQLite Address database used by `Common.Data.SQLite`.

To create the Address database, start a PowerShell session and run the following commands:

~~~shell
cd src/EFCore/Database.Address

dotnet-ef database update # applies the latest migration
-or-
dotnet-ef migrations remove # remove the latest migration
~~~
The JSON files in the Data\Seed folder are used during the create process to automatically seed the data in the tables.

> Note: There is also a very large flat file that contains all the US Postcodes that can be used to replace the default data.\
> **Important**: The `Postcodes` table must be cleared before importing this file.

References:
- [Data Seeding](https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding)
- [Collations and Case Sensitivity](https://learn.microsoft.com/en-us/ef/core/miscellaneous/collations-and-case-sensitivity)
- [EFCore.Database.Entity](../Database.Entity/README.md)

Testing:

To test the data has been populated correctly use the following SQL scripts:
```sql
SELECT [County] FROM [Postcodes] WHERE [Province]='FL' GROUP BY [County] ORDER BY [County]
SELECT [City] FROM [Postcodes] WHERE [Province]='FL' AND [County]='seminole' GROUP BY [City] ORDER BY [City]
SELECT [Code] FROM [Postcodes] WHERE [Province]='FL' AND [County]='seminole' AND [City]='lake mary' ORDER BY [Code]
```
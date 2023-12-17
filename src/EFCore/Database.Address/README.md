# EFCore.Database.Address

To create the Address database, start a PowerShell session and run the following commands:

~~~shell
cd src/EFCore/Database.Address

dotnet-ef migrations add InitialCreate
dotnet-ef database update
~~~
The JSON files in the Data folder are used during the create process to automatically seed the data in the tables.

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
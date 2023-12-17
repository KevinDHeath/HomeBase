# EFCore.Database.Entity

To create the Entity database, start a PowerShell session and run the following commands:

~~~shell
cd src/EFCore/Database.Entity

dotnet-ef migrations add InitialCreate
dotnet-ef database update
~~~

Because the Company and Person data cannot be automatically seeded due to them having a 'owns one' relationship to Address, import directly into the database tables with the tab-delimited files supplied in the `Data` folder.\
Use the following options when importing:
- Data source type: `Flat file (csv)`
- Input file: `[repo location]\src\EFCore\Database.Entity\Data`
- Text encoding: `UTF-8`
- Ignore errors: `False`
- First line represents column names: `True`
- Column separator: `\t (tab)`
- NULL values: `True`

References:
- [SQL Server Import and Export Wizard](https://learn.microsoft.com/en-us/sql/integration-services/import-export-data/import-and-export-data-with-the-sql-server-import-and-export-wizard)
- [SQLiteStudio Import](https://github.com/pawelsalawa/sqlitestudio/wiki/CsvImport)
- [EFCore.Database.Address](../Database.Address/README.md)
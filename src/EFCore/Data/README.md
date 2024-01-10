## SQL Data
<table>
<tr><th align="left">Data file</th><th align="left">Import</th><th align="left">SQL script</th></tr>
<tr><td>Companies.txt</td><td>Flat File</td><td>Scripts/Companies.sql</td></tr>
<tr><td>ISOCountries.txt</td><td>Data</td><td>Scripts/ISOCountries.sql</td></tr>
<tr><td>Movies.txt</td><td>Flat File</td><td>Scripts/Movies.sql</td></tr>
<tr><td>People.txt</td><td>Flat File</td><td>Scripts/People.sql</td>
<tr><td>USPostcodes.txt</td><td>Flat File</td><td>Scripts/Postcodes.sql</td></tr>
<tr><td>USProvinces.txt</td><td>Flat File</td><td>Scripts/Provinces.sql</td></tr>
<tr><td>SuperHeroes.txt</td><td>Flat File</td><td>Scripts/SuperHeroes.sql</td></tr>
</table>

The following Tasks rely on having [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) installed.

### Importing data
If the table doesn't already exist in the target database, create it using the SQL script provided, otherwise clear the contents using:\
```TRUNCATE TABLE [table name];```

Use Tasks>Import **Data** to load a text file with no NULL values directly into the table:
  - Data Source: Flat File Source
  - Destination: SQL Server Native Client 11.0

> **Note**: To preserve NULL values SSMS v17.3 or higher must be installed.
- Use Tasks>Import **Flat File** to load a text file to a new table named [_table name_temp_].

  Make sure that the column data types are set correctly.

  Companies:\
  Address_Postode: nvarchar(20)\
  NaicsCode: nvarchar(20)
 
  People:\
  Address_Postode: nvarchar(20)

- Then copy the data from the generated temporary table to the actual table\
  ```INSERT INTO [table name] SELECT * FROM [table name_temp];```

- Finally, remove the temporary table\
   ```DROP TABLE [table name_temp];```

References:
- [Import Flat File to SQL Wizard](https://learn.microsoft.com/en-us/sql/relational-databases/import-export/import-flat-file-wizard)
- [Copy Columns from One Table to Another](https://learn.microsoft.com/en-us/sql/relational-databases/tables/copy-columns-from-one-table-to-another-database-engine)
## SQL Data
<table>
<tr><th align="left">SQL script</th><th align="left">Data file</th><th align="left">Import</th></tr>
<tr><td>Companies.sql</td><td>Data/Companies.txt</td><td>Flat File</td></tr>
<tr><td>ISOCountries.sql</td><td>Data/ISOCountries.txt</td><td>Data</td></tr>
<tr><td>Movies.sql</td><td>Data/Movies.txt</td><td>Flat File</td></tr>
<tr><td>People.sql</td><td>Data/People.txt</td><td>Flat File</td>
<tr><td>*Postcodes.sql</td><td>Data/USPostcodes.txt</td><td>Flat File</td></tr>
<tr><td>*Provinces.sql</td><td>Data/USProvinces.txt</td><td>Flat File</td></tr>
<tr><td>SuperHeroes.sql</td><td>Data/SuperHeroes.txt</td><td>Flat File</td></tr>
<tr><td>USStates.sql</td><td>Data/USStates.txt</td><td>Flat File</td></tr>
<tr><td>USZipCodes.sql</td><td>Data/USZipCodes.txt</td><td>Data</td></tr>
<tr><td colspan="3">* <i>new as of Common.Core v2.0.1</i></td></tr>
</table>

The following Tasks rely on having [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) installed.

### Importing data
If the table doesn't already exist in the target database, create it using the SQL script provided, otherwise clear the contents using:\
```TRUNCATE TABLE [table name];```

Use Tasks>Import **Data** to load a text file with no NULL values directly into the table:
  - Data Source: Flat File Source
  - Destination: SQL Server Native Client 11.0

> **Note**: To preserve NULL values SSMS v17.3 or higher must be installed.
- Use Tasks>Import **Flat File** to load a text file to a new table named [_table name_temp_].\
  Make sure that the column data types are set correctly.\
  \
  Companies:\
  Address_ZipCode: nvarchar(20)\
  NaicsCode: nvarchar(20)

- Then copy the data from the import table to the actual table\
   ```INSERT INTO [table name] SELECT * FROM [table name_temp];```

- Finally, remove the temporary table\
   ```DROP TABLE [table name_temp];```

References:
- [Import Flat File to SQL Wizard](https://learn.microsoft.com/en-us/sql/relational-databases/import-export/import-flat-file-wizard)
- [Copy Columns from One Table to Another](https://learn.microsoft.com/en-us/sql/relational-databases/tables/copy-columns-from-one-table-to-another-database-engine)
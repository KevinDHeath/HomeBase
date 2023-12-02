## SQL Data
| SQL script | Data file | Import |
| :--------- | :-------- | :----- |
| Companies.sql | Data/Companies.txt | Flat File |
| ISOCountries.sql | Data/ISOCountries.txt | Data |
| Movies.sql | Data/Movies.txt | Flat File |
| People.sql | Data/People.txt | Flat File |
| SuperHeroes.sql | Data/SuperHeroes.txt | Flat File |
| USStates.sql | Data/USStates.txt | Flat File |
| USZipCodes.sql | Data/USZipCodes.txt | Data |

The following Tasks rely on having [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) installed.

### Importing data
If the table doesn't already exist in the target database, create it using the SQL script provided, otherwise clear the contents using:\
```TRUNCATE TABLE [table name];```

Use Tasks>Import **Data** to load a text file with no NULL values directly into the table:
  - Data Source: Flat File Source
  - Destination: SQL Server Native Client 11.0

To preserve NULL values SSMS v17.3 or higher must be installed.
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
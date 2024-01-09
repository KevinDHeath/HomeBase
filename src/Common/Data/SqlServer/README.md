# Common.Data.SqlServer
Microsoft SQL Server is a relational database management system (RDBMS). Applications and tools connect to a SQL Server _instance_ or _database_, and communicate using Transact-SQL (T-SQL).

> For Sql Server databases use [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).

References:
- [What is SQL Server?](https://learn.microsoft.com/en-us/sql/sql-server/what-is-sql-server)
- [Microsoft SQL Server EF Core Database Provider](https://learn.microsoft.com/en-us/ef/core/providers/sql-server)

Notes:

The default name of the SQL Server database is `EFCommonData`. To point to another location or different database names use a `appsettings.json` file and set the following connection strings:
~~~json
{
  "ConnectionStrings": {
    "CommonData": "Server=localhost;Database=EFCommonData;TrustServerCertificate=true;MultipleActiveResultSets=true;Integrated Security=SSPI;"
  }
}
~~~

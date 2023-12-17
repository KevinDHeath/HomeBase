# Common.Data.SQLite
SQLite is a C-language library that implements a small, fast, self-contained, high-reliability, full-featured, SQL database engine.

> The best tool to manage the databases is [SQLite Studio](https://sqlitestudio.pl/).

References:
- [SQLite documentation](https://www.sqlite.org/docs.html)
- [Microsoft.Data.Sqlite overview](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/)
- [EF Core Database Provider Limitations](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/limitations)

Notes:

The default location for the SQLite databases is in a `Data` sub-folder. To point to another location or different database names use a `appsettings.json` file and set the following connection strings:
~~~json
{
  "ConnectionStrings": {
    "AddressData": "Data Source=C:\\Databases\\usAddressData.db",
    "EntityData": "Data Source=C:\\Databases\\usEntityData.db"
  }
}
~~~

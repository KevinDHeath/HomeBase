# Common Core
The Common Core local package contains classes and interfaces for .NET components.

Dependencies:
- None

References:
- [Get Application Version in .NET Core](https://edi.wang/post/2018/9/27/get-app-version-net-core)
- [Async and Await in C#](https://www.c-sharpcorner.com/article/async-and-await-in-c-sharp/)
- [Understanding and Using ConfigureAwait](https://dev.to/this-is-learning/understanding-and-using-configureawait-in-asynchronous-programming-2da3)
- [Null Forgiving Operator in C#](https://jeremybytes.blogspot.com/2022/07/null-forgiving-operator-in-c.html)
- [Serialize Interface Instances With System.Text.Json](https://khalidabuhakmeh.com/serialize-interface-instances-system-text-json)

## Common Data
Implements 3 methods of providing data.

### Common.Data.API
Uses a restful API to access data using the Entity Framework provided by Microsoft.

- Caches zip code data
- Can maintain data

Dependencies:
- [kdheath.Common.Core](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Core)

### Common.Data.Json
Uses JSON data embedded in the binary file and is therefore not changeable.

Dependencies:
- [kdheath.Common.Core](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Core)

References:
- [Using embedded files in NET Core](https://josef.codes/using-embedded-files-in-dotnet-core/)

### Common.Data.Sql
Uses data directly from SQL Server.

- Caches zip code data
- Can maintain data

Dependencies:
- [kdheath.Common.Core](https://github.com/KevinDHeath/HomeBase/tree/main/src/Common/Core)
- [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient)

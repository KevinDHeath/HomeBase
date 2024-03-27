## About
The Json.Converters package provides null, string, and interface converters for JSON serialization classes that are provided in the System.Text.Json namespace.

## Change Log
- v1.0.0 - Package created but not released.

## How to Use
To use in a C# program:
```c#
using System.Text.Json;
using Json.Converters;

public static JsonSerializerOptions GetSerializerOptions()
{
  var rtn = new JsonSerializerOptions();
  rtn.Converters.Add( new InterfaceFactory( typeof( Person ), typeof( IPerson ) ) );
  rtn.Converters.Add( new BooleanNull() );
  rtn.Converters.Add( new DateOnlyString() );
  ...
  return rtn;
}
```

## Main Types
The converters implemented in this library are:

| **Type** | **Description** |
| :------- | :-------------- |
| bool | Boolean (true or false) value. |
| byte | 8-bit unsigned integer ranging from 0 to 255. |
| DateOnly | Date ranging from January 1, 0001 to December 31, 9999. _(Introduced in Net 6, supported in System.Text.Json in Net 7)_ |
| DateTime | An point in time, typically expressed as a date and time of day. |
| DateTimeOffset | A point in time, typically expressed as a date and time of day, relative to Coordinated Universal Time (UTC). |
| decimal | 16 byte floating-point with precision of 28-29 digits. |
| double | 8 bytes floating-point with precision of 15-17 digits. |
| float | 4 bytes floating-point with precision of 6-9 digits. |
| Guid | Globally unique identifier (GUID). |
| int  | Signed 32-bit integer ranging from -2,147,483,648 to 2,147,483,647. |
| long  | Signed 64-bit integer ranging from -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807. |
| sbyte | Signed 8-bit integer ranging from -128 to 127. |
| short | Signed 16-bit integer ranging from -32,768 to 32,767. |
| TimeOnly | Time of day ranging from 00:00:00 to 23:59:59.9999999. _(Introduced in Net 6, supported in System.Text.Json in Net 7)_ |
| TimeSpan | A time interval. |
| uint | Unsigned 32-bit integer ranging from 0 to 4,294,967,295. |
| ulong | Unsigned 64-bit integer ranging from 0 to 18,446,744,073,709,551,615. |
| ushort | Unsigned 16-bit integer ranging from 0 to 65,535. |

The converters **NOT** implemented in this library are:

| **Type** | **Description** |
| :------- | :-------------- |
| char | Character as a UTF-16 code unit. |

## Feedback
This is provided as open source under the MIT license.\
Bug reports and contributions are welcome [at the GitHub repository](https://github.com/KevinDHeath/NuGetPackages).

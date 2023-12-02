using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Json.Converters;

/// <summary>
/// Converts a nullable long (System.Int64) object or value to or from JSON.
/// </summary>
public class LongNull : JsonConverter<long?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type long.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override long? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( long? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetInt64();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( LongString.ParseString( ref input, out long rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new long?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, long? value, JsonSerializerOptions options )
	{
		if( value.HasValue )
		{
			writer.WriteNumberValue( value.Value );
		}
		else
		{
			writer.WriteNullValue();
		}
	}

	#endregion
}

/// <summary>
/// Converts a long (System.Int64) string to or from JSON.
/// </summary>
public class LongString : JsonConverter<long>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a long.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override long Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( long ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetInt64();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out long rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new long();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, long value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out long rtn )
	{
		// Try to convert Signed 64-bit integer, range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( long.TryParse( input,
				NumberStyles.Integer | NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign,
				NumberFormatInfo.CurrentInfo, out rtn ) ) return true;
		}
		return false;
	}
}
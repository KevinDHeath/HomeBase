using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Json.Converters;

/// <summary>
/// Converts a nullable DateTimeOffset object or value to or from JSON.
/// </summary>
public class DateTimeOffsetNull : JsonConverter<DateTimeOffset?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type DateTimeOffset.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override DateTimeOffset? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( DateTimeOffset? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				var input = reader.GetString();
				if( DateTimeOffsetString.ParseString( ref input, out DateTimeOffset rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new DateTime?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options )
	{
		if( value.HasValue )
		{
			writer.WriteStringValue( value.Value );
		}
		else
		{
			writer.WriteNullValue();
		}
	}

	#endregion
}

/// <summary>
/// Converts a DateTimeOffset string to or from JSON.
/// </summary>
public class DateTimeOffsetString : JsonConverter<DateTimeOffset>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a DateTimeOffset.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override DateTimeOffset Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( DateTimeOffset ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out DateTimeOffset rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new DateTime();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options )
	{
		writer.WriteStringValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out DateTimeOffset rtn )
	{
		// Try to convert date/time offset
		rtn = DateTimeOffset.MinValue;
		if( null != input && input.Length > 0 )
		{
			if( DateTimeOffset.TryParse( input, out rtn ) ) return true;
		}
		return false;
	}
}
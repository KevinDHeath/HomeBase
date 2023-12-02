using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Json.Converters;

/// <summary>
/// Converts a nullable TimeOnly object or value to or from JSON.
/// </summary>
public class TimeOnlyNull : JsonConverter<TimeOnly?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type TimeOnly.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override TimeOnly? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( TimeOnly? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				var input = reader.GetString();
				if( TimeOnlyString.ParseString( ref input, out TimeOnly rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new TimeOnly?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options )
	{
		if( value.HasValue )
		{
			writer.WriteStringValue( value.Value.ToString( TimeOnlyString.cFormat ) );
		}
		else
		{
			writer.WriteNullValue();
		}
	}

	#endregion
}

/// <summary>
/// Converts a TimeOnly string to or from JSON.
/// </summary>
public class TimeOnlyString : JsonConverter<TimeOnly>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a TimeOnly.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override TimeOnly Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( TimeOnly ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out TimeOnly rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new TimeOnly();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options )
	{
		writer.WriteStringValue( value.ToString( cFormat ) );
	}

	#endregion

	internal const string cFormat = @"HH:mm:ss";

	internal static bool ParseString( ref string? input, out TimeOnly rtn )
	{
		// Try to convert time, range: 00:00:00 to 23:59:59.9999999
		rtn = TimeOnly.MinValue;
		if( null != input && input.Length > 0 )
		{
			if( TimeOnly.TryParse( input, out rtn ) ) return true;
		}
		return false;
	}
}
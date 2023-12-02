using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Json.Converters;

/// <summary>
/// Converts a nullable TimeSpan object or value to or from JSON.
/// </summary>
public class TimeSpanNull : JsonConverter<TimeSpan?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type TimeSpan.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override TimeSpan? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( TimeSpan? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				var input = reader.GetString();
				if( TimeSpanString.ParseString( ref input, out TimeSpan rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new TimeSpan?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options )
	{
		if( value.HasValue )
		{
			writer.WriteStringValue( value.Value.ToString( TimeSpanString.cFormat ) );
		}
		else
		{
			writer.WriteNullValue();
		}
	}

	#endregion
}

/// <summary>
/// Converts a TimeSpan string to or from JSON.
/// </summary>
public class TimeSpanString : JsonConverter<TimeSpan>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a TimeSpan.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override TimeSpan Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( TimeSpan ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out TimeSpan rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new TimeSpan();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options )
	{
		writer.WriteStringValue( value.ToString( cFormat ) );
	}

	#endregion

	// Outputs the ten-millionths of a second (or the fractional number of ticks).
	// If there are any trailing fractional zeros, they aren't included in the result string
	internal const string cFormat = @"d\.hh\:mm\:ss\.FFFFFFF";

	internal static bool ParseString( ref string? input, out TimeSpan rtn )
	{
		// Try to convert time span
		rtn = TimeSpan.MinValue;
		if( null != input && input.Length > 0 )
		{
			input = input.Trim();
			if( input.EndsWith( "-" ) ) // Handle trailing negative sign
				input = string.Concat( "-", input.AsSpan( 0, input.Length - 1 ) );
		
				if( TimeSpan.TryParse( input, out rtn ) ) return true;
		}
		return false;
	}
}
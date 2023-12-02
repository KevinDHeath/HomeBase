using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable decimal (System.Decimal) object or value to or from JSON.
/// </summary>
public class DecimalNull : JsonConverter<decimal?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type decimal.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override decimal? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( decimal? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetDecimal();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( DecimalString.ParseString( ref input, out decimal rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new decimal?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options )
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
/// Converts a decimal (System.Decimal) string to or from JSON.
/// </summary>
public class DecimalString : JsonConverter<decimal>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a decimal.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override decimal Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( decimal ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetDecimal();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out decimal rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new decimal();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, decimal value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out decimal rtn )
	{
		// Try to convert 16 byte floating-point, precision: 28-29 digits
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( decimal.TryParse( input, NumberStyles.Currency, NumberFormatInfo.CurrentInfo,
				out rtn ) ) return true;
		}
		return false;
	}
}
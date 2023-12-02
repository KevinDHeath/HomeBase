using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable double (System.Double) object or value to or from JSON.
/// </summary>
public class DoubleNull : JsonConverter<double?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type double.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override double? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( double? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetDouble();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( DoubleString.ParseString( ref input, out double rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new double?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, double? value, JsonSerializerOptions options )
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
/// Converts a double (System.Double) string to or from JSON.
/// </summary>
public class DoubleString : JsonConverter<double>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a double.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override double Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( double ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetDouble();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out double rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new double();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, double value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out double rtn )
	{
		// Try to convert 8 bytes floating-point, precision: 15-17 digits
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( double.TryParse( input, NumberStyles.Number, NumberFormatInfo.CurrentInfo,
				out rtn ) ) return true;
		}
		return false;
	}
}
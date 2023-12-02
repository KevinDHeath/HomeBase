using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable short (System.Int16) object or value to or from JSON.
/// </summary>
public class ShortNull : JsonConverter<short?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type short.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override short? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( short? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetInt16();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ShortString.ParseString( ref input, out short rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new short?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, short? value, JsonSerializerOptions options )
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
/// Converts a short (System.Int16) string to or from JSON.
/// </summary>
public class ShortString : JsonConverter<short>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a short.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override short Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( short ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetInt16();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out short rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new short();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, short value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out short rtn )
	{
		// Try to convert Signed 16-bit integer, range: -32,768 to 32,767
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( short.TryParse( input,
				NumberStyles.Integer | NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign,
				NumberFormatInfo.CurrentInfo, out rtn ) ) return true;
		}
		return false;
	}
}
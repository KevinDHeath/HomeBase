using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable Byte (System.Byte) object or value to or from JSON.
/// </summary>
public class ByteNull : JsonConverter<byte?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type byte.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override byte? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( byte? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetByte();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ByteString.ParseString( ref input, out byte rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new byte?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, byte? value, JsonSerializerOptions options )
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
/// Converts a Byte (System.Byte) string to or from JSON.
/// </summary>
public class ByteString : JsonConverter<byte>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a byte.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override byte Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( byte ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetByte();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out byte rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new byte();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, byte value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out byte rtn )
	{
		// Try to convert Unsigned 8-bit integer, range: 0 to 255
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( byte.TryParse( input,
				NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite,
				NumberFormatInfo.CurrentInfo, out rtn ) ) return true;
		}
		return false;
	}
}
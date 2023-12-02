using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable Signed Byte (System.SByte) object or value to or from JSON.
/// </summary>
public class SByteNull : JsonConverter<sbyte?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type signed byte.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override sbyte? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( sbyte? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetSByte();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( SByteString.ParseString( ref input, out sbyte rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new sbyte?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, sbyte? value, JsonSerializerOptions options )
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
/// Converts a Signed Byte (System.SByte) string to or from JSON.
/// </summary>
public class SByteString : JsonConverter<sbyte>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a signed byte.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override sbyte Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( sbyte ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetSByte();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out sbyte rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new sbyte();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, sbyte value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out sbyte rtn )
	{
		// Try to convert Signed 8-bit integer, range: -128 to 127
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( sbyte.TryParse( input, NumberStyles.Integer | NumberStyles.AllowTrailingSign,
				NumberFormatInfo.CurrentInfo, out rtn ) ) return true;
		}
		return false;
	}
}
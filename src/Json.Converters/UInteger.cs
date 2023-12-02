using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Json.Converters;

/// <summary>
/// Converts a nullable Unsigned Integer (System.UInt32) object or value to or from JSON.
/// </summary>
public class UIntegerNull : JsonConverter<uint?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type unsigned-integer.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override uint? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( uint? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetUInt32();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( UIntegerString.ParseString( ref input, out uint rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new uint?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, uint? value, JsonSerializerOptions options )
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
/// Converts an Unsigned Integer (System.UInt32) string to or from JSON.
/// </summary>
public class UIntegerString : JsonConverter<uint>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to an unsigned-integer.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override uint Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( uint ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetUInt32();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out uint rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new uint();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, uint value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out uint rtn )
	{
		// Try to convert Unsigned 32-bit integer, range: 0 to 4,294,967,295
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( uint.TryParse( input,
				NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite | NumberStyles.AllowThousands,
				NumberFormatInfo.CurrentInfo, out rtn ) ) return true;
		}
		return false;
	}
}
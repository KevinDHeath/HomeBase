using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable Unsigned Short (System.UInt16) object or value to or from JSON.
/// </summary>
public class UShortNull : JsonConverter<ushort?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type unsigned-short.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override ushort? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( ushort? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetUInt16();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( UShortString.ParseString( ref input, out ushort rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new ushort?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, ushort? value, JsonSerializerOptions options )
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
/// Converts a null-able Unsigned Short (System.UInt16) object or value to or from JSON.
/// </summary>
public class UShortString : JsonConverter<ushort>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a unsigned-short.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override ushort Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( ushort ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetUInt16();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out ushort rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new ushort();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, ushort value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out ushort rtn )
	{
		// Try to convert Unsigned 16-bit integer, range: 0 to 65,535
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( ushort.TryParse( input,
				NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite | NumberStyles.AllowThousands,
				NumberFormatInfo.CurrentInfo, out rtn ) ) return true;
		}
		return false;
	}
}
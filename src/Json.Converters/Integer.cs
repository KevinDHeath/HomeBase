using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Json.Converters;

/// <summary>
/// Converts a nullable integer (System.Int32) object or value to or from JSON.
/// </summary>
public class IntegerNull : JsonConverter<int?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type integer.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override int? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( int? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetInt32();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( IntegerString.ParseString( ref input, out int rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new int?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, int? value, JsonSerializerOptions options )
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
/// Converts a integer (System.Int32) string to or from JSON.
/// </summary>
public class IntegerString : JsonConverter<int>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a integer.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override int Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( int ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetInt32();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out int rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new int();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, int value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out int rtn )
	{
		// Try to convert Signed 32-bit integer, range: -2,147,483,648 to 2,147,483,647
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( int.TryParse( input,
				NumberStyles.Integer | NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign,
				NumberFormatInfo.CurrentInfo, out rtn ) ) return true;
		}
		return false;
	}
}
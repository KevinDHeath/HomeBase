using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable float (System.Single) object or value to or from JSON.
/// </summary>
public class FloatNull : JsonConverter<float?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type float.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override float? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( float? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetSingle();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( FloatString.ParseString( ref input, out float rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new float?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, float? value, JsonSerializerOptions options )
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
/// Converts a (System.Single) string to or from JSON.
/// </summary>
public class FloatString : JsonConverter<float>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a float.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override float Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( float ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetSingle();

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out float rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new float();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, float value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out float rtn )
	{
		// Try to convert 4 bytes floating-point, precision: 6-9 digits
		rtn = 0;
		if( null != input && input.Length > 0 )
		{
			if( float.TryParse( input, NumberStyles.Number, NumberFormatInfo.CurrentInfo,
				out rtn ) ) return true;
		}
		return false;
	}
}
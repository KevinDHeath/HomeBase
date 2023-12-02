using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable boolean (System.Boolean) object or value to or from JSON.
/// </summary>
public class BooleanNull : JsonConverter<bool?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a three-valued boolean.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override bool? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( bool? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.False:
				return false;

			case JsonTokenType.True:
				return true;

			case JsonTokenType.String:
				var input = reader.GetString();
				if( BooleanString.ParseString( ref input, out bool rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new bool?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, bool? value, JsonSerializerOptions options )
	{
		if( value.HasValue )
		{
			writer.WriteBooleanValue( value.Value );
		}
		else
		{
			writer.WriteNullValue();
		}
	}

	#endregion
}

/// <summary>
/// Converts a boolean (System.Boolean) string to or from JSON.
/// </summary>
public class BooleanString : JsonConverter<bool>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a boolean.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override bool Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( bool ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.False:
				return false;

			case JsonTokenType.True:
				return true;

			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out bool rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return false;
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, bool value, JsonSerializerOptions options )
	{
		writer.WriteBooleanValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out bool rtn )
	{
		// Try to convert Boolean, which can be true or false
		rtn = false;
		if( null != input )
		{
			input = input.Trim().ToLower();
			if( input.Length == 1 )
			{
				// Check for 1, 0, Y, N values
				if( input == "0" || input == "n" ) input = bool.FalseString;
				else if( input == "1" || input == "y" ) input = bool.TrueString;
			}
			else if( input.Length <= 3 )
			{
				// Check for Yes, No values
				if( input == "no" ) input = bool.FalseString;
				else if( input == "yes" ) input = bool.TrueString;
			}

			if( bool.TryParse( input, out rtn ) ) return true;
		}
		return false;
	}
}
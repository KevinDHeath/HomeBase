using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a nullable Guid (System.Guid) object or value to or from JSON.
/// </summary>
public class GuidNull : JsonConverter<Guid?>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a null type Guid.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override Guid? Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( Guid? ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				var input = reader.GetString();
				if( GuidString.ParseString( ref input, out Guid rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return new Guid?();
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options )
	{
		if( value.HasValue )
		{
			writer.WriteStringValue( value.Value );
		}
		else
		{
			writer.WriteNullValue();
		}
	}

	#endregion
}

/// <summary>
/// Converts a Guid (System.Guid) string to or from JSON.
/// </summary>
public class GuidString : JsonConverter<Guid>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON to a Guid.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted value. If the value is empty or could not be converted a
	/// null value is returned.</returns>
	public override Guid Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		Debug.Assert( TypeToConvert == typeof( Guid ) );
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				var input = reader.GetString();
				if( ParseString( ref input, out Guid rtn ) ) return rtn;
				break;

			default:
				break;
		}

		return Guid.Empty;
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, Guid value, JsonSerializerOptions options )
	{
		writer.WriteStringValue( value );
	}

	#endregion

	internal static bool ParseString( ref string? input, out Guid rtn )
	{
		// Try to globally unique identifier
		rtn = Guid.Empty;
		if( null != input && input.Length > 0 )
		{
			if( Guid.TryParse( input, out rtn ) ) return true;
		}
		return false;
	}
}
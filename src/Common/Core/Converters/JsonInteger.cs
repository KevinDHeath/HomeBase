using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Core.Converters;

#region Nullable Integer

/// <summary>Converts a nullable integer (System.Int32) string to or from JSON.</summary>
public class JsonIntegerString : JsonConverter<int?>
{
	/// <summary>Reads and converts the JSON to a nullable integer.</summary>
	/// <inheritdoc/>
	public override int? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetInt32();

			case JsonTokenType.String:
				string? value = reader.GetString();
				if( value is not null && StringConverter.TryParse( ref value, out int result ) )
				{ return result; }
				break;

			default:
				break;
		}

		return null;
	}

	/// <inheritdoc/>
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
}

#endregion
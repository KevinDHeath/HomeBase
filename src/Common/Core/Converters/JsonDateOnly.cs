using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Core.Converters;

#region Nullable DateOnly

/// <summary>Converts a nullable DateOnly string to or from JSON.</summary>
public class JsonDateOnlyString : JsonConverter<DateOnly?>
{
	/// <summary>Reads and converts the JSON to a nullable DateOnly.</summary>
	/// <inheritdoc/>
	public override DateOnly? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		switch( reader.TokenType )
		{
			case JsonTokenType.String:
				string? value = reader.GetString();
				if( value is not null && StringConverter.TryParse( ref value, out DateOnly result ) )
				{ return result; }
				break;

			default:
				break;
		}

		return null;
	}

	/// <inheritdoc/>
	public override void Write( Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options )
	{
		if( value.HasValue )
		{
			writer.WriteStringValue( value.Value.ToString() );
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}

#endregion
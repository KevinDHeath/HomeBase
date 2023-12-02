using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Core.Converters;

#region Nullable Boolean

/// <summary>Converts a nullable boolean (System.Boolean) string to or from JSON.</summary>
public class JsonBooleanString : JsonConverter<bool?>
{
	/// <summary>Reads and converts the JSON to a nullable boolean.</summary>
	/// <inheritdoc/>
	public override bool? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		switch( reader.TokenType )
		{
			case JsonTokenType.False:
				return false;

			case JsonTokenType.True:
				return true;

			case JsonTokenType.String:
				string? value = reader.GetString();
				if( value is not null && StringConverter.TryParse( ref value, out bool result ) )
				{ return result; }
				break;

			default:
				break;
		}

		return null;
	}

	/// <inheritdoc/>
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
}

#endregion
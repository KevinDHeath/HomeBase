using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Core.Converters;

#region Nullable Decimal

/// <summary>Converts a decimal (System.Decimal) string to or from JSON.</summary>
public class JsonDecimalString : JsonConverter<decimal?>
{
	/// <summary>Reads and converts the JSON to a nullable decimal.</summary>
	/// <inheritdoc/>
	public override decimal? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		switch( reader.TokenType )
		{
			case JsonTokenType.Number:
				return reader.GetDecimal();

			case JsonTokenType.String:
				string? value = reader.GetString();
				if( value is not null && StringConverter.TryParse( ref value, out decimal result ) )
				{ return result; }
				break;

			default:
				break;
		}

		return null;
	}

	/// <summary>Writes a specified value as JSON.</summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, decimal? value, JsonSerializerOptions options )
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
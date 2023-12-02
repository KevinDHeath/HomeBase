using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Core.Classes;

/// <summary>Converts a concrete class object from Json and returns it as an interface.</summary>
/// <typeparam name="M">The concrete class type.</typeparam>
/// <typeparam name="I">The interface type.</typeparam>
internal class InterfaceConverter<M, I> : JsonConverter<I> where M : class, I
{
	#region Overridden Methods

	/// <summary>Reads and converts the Json as a class and returns it as an interface.</summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted class as an interface.</returns>
	public override I Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		var result = JsonSerializer.Deserialize<M>( ref reader, options );
		return result ?? (M)Activator.CreateInstance<M>();
	}

	/// <summary>Writes a specified value as Json.</summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to Json.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, I value, JsonSerializerOptions options )
	{
		// Process as Json document
		using var jsonDoc = JsonDocument.Parse( JsonSerializer.Serialize( value ) );

		writer.WriteStartObject();
		foreach( var element in jsonDoc.RootElement.EnumerateObject() )
		{
			element.WriteTo( writer );
		}
		writer.WriteEndObject();
	}

	#endregion
}
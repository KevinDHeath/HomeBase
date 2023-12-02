using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Converts a list of class objects from JSON and returns it as an interface.
/// </summary>
/// <typeparam name="M">The concrete class type.</typeparam>
public class IListConverter<M> : JsonConverter<IList<M>>
{
	#region Overridden Methods

	/// <summary>
	/// Reads and converts the JSON as a class and returns it as an interface.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="TypeToConvert">The type to convert.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	/// <returns>The converted class object list as an interface.</returns>
	public override IList<M> Read( ref Utf8JsonReader reader, Type TypeToConvert, JsonSerializerOptions options )
	{
		return JsonSerializer.Deserialize<List<M>>( ref reader, options )!;
	}

	/// <summary>
	/// Writes a specified value as JSON.
	/// </summary>
	/// <param name="writer">The writer to write to.</param>
	/// <param name="value">The value to convert to JSON.</param>
	/// <param name="options">An object that specifies serialization options to use.</param>
	public override void Write( Utf8JsonWriter writer, IList<M> value, JsonSerializerOptions options )
	{
		// Interface lists cannot be written as JSON
		throw new NotImplementedException();
	}

	#endregion
}
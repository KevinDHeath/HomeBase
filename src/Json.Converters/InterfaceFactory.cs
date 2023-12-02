using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Supports converting Interface types by using a factory pattern.
/// </summary>
public class InterfaceFactory : JsonConverterFactory
{
	#region Properties and Constructor

	/// <summary>
	/// Gets the concrete class type.
	/// </summary>
	public Type ConcreteType { get; }

	/// <summary>
	/// Gets the interface type.
	/// </summary>
	public Type InterfaceType { get; }

	/// <summary>
	/// Initializes a new instance of the JsonConverterFactory class.
	/// </summary>
	/// <param name="concrete">The concrete class type.</param>
	/// <param name="interfaceType">The interface type.</param>
	public InterfaceFactory( Type concrete, Type interfaceType )
	{
		ConcreteType = concrete;
		InterfaceType = interfaceType;
	}

	#endregion

	#region Overridden Methods

	/// <summary>
	/// Determines whether the converter instance can convert the specified object type.
	/// </summary>
	/// <param name="typeToConvert">The type of the object to check whether it can be
	/// converted by this converter instance.</param>
	/// <returns>True if the instance can convert the specified object type otherwise False.</returns>
	public override bool CanConvert( Type typeToConvert )
	{
		return typeToConvert == InterfaceType;
	}

	/// <summary>
	/// Creates a converter for a specified type.
	/// </summary>
	/// <param name="typeToConvert">The type handled by the converter.</param>
	/// <param name="options">The serialization options to use.</param>
	/// <returns>A converter compatible with typeToConvert.</returns>
	public override JsonConverter CreateConverter( Type typeToConvert, JsonSerializerOptions options )
	{
		var converterType = typeof( InterfaceConverter<,> ).MakeGenericType( ConcreteType, InterfaceType );

		return (JsonConverter)Activator.CreateInstance( converterType )!;
	}

	#endregion
}
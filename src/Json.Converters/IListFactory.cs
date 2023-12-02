using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Converters;

/// <summary>
/// Supports converting IList Interface types by using a factory pattern.
/// </summary>
public class IListFactory : JsonConverterFactory
{
	#region Properties and Constructor

	/// <summary>
	/// Gets the interface type.
	/// </summary>
	public Type InterfaceType { get; }

	/// <summary>
	/// Initializes a new instance of the JsonConverterFactory class.
	/// </summary>
	/// <param name="interfaceType">The interface type.</param>
	public IListFactory( Type interfaceType )
	{
		InterfaceType = interfaceType;
	}

	#endregion

	#region Overridden Methods

	/// <summary>
	/// Determines whether the converter instance can convert the specified object type.
	/// </summary>
	/// <param name="typeToConvert">The type of the object to check whether it can be
	/// converted by this converter instance.</param>
	/// <returns>True if the instance can convert the specified object type, otherwise False.</returns>
	public override bool CanConvert( Type typeToConvert )
	{
		if( typeToConvert.Equals( typeof( IList<> ).MakeGenericType( InterfaceType ) )
			&& typeToConvert.GenericTypeArguments[0].Equals( InterfaceType ) )
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Creates a converter for a specified type.
	/// </summary>
	/// <param name="typeToConvert">The type handled by the converter.</param>
	/// <param name="options">The serialization options to use.</param>
	/// <returns>A converter compatible with typeToConvert.</returns>
	public override JsonConverter CreateConverter( Type typeToConvert, JsonSerializerOptions options )
	{
		return (JsonConverter)Activator.CreateInstance(
			typeof( IListConverter<> ).MakeGenericType( InterfaceType ) )!;
	}

	#endregion
}
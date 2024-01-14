using System.Reflection;
using System.Text.Json;
using Common.Core.Classes;
using Common.Data.Helpers;

namespace Common.Data.Json;

/// <summary>Base class to provide common embedded Json data access functionality.</summary>
public abstract class Factory : DataFactoryBase
{
	private const string cAssembly = "Common.Data.Json";
	private const string cPrefix = "Json.";

	#region Constructors and Variables

	private static readonly EmbeddedResource sEmbeddedResourceQuery;

	static Factory()
	{
		var assembliesToPreload = new List<Assembly> { Assembly.Load( cAssembly ) };
		sEmbeddedResourceQuery = new EmbeddedResource( assembliesToPreload );
	}

	/// <summary>Initializes a new instance of the Factory class.</summary>
	protected Factory()
	{}

	#endregion

	/// <summary>Returns the Json from a resource in an assembly.</summary>
	/// <param name="resourceName">Resource name.</param>
	/// <param name="assembly">Assembly name. If not supplied the default name is used.</param>
	/// <returns>Null is returned if the resource could not be loaded.</returns>
	internal static string? GetEmbeddedResource( string resourceName, string assembly = cAssembly )
	{
		var stream = sEmbeddedResourceQuery.Read( assembly, cPrefix + resourceName );
		if( null != stream )
		{
			StreamReader reader = new( stream );
			return reader.ReadToEnd();
		}
		return null;
	}

	/// <summary>Reads a Json resource and populates a factory object.</summary>
	/// <typeparam name="T">Type of factory to populate.</typeparam>
	/// <param name="resource">Resource name.</param>
	/// <param name="options">Json serializer options.</param>
	/// <returns>Null is returned if the object could not be populated.</returns>
	protected static T? DeserializeJson<T>( string resource, JsonSerializerOptions options ) where T : Factory
	{
		var json = GetEmbeddedResource( resource );
		if( json is null ) { return null; }
		return JsonHelper.DeserializeJson<T>( ref json, options );
	}
}
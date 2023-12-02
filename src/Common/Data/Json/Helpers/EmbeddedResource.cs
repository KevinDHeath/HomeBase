// Ignore Spelling: Preload

using System.Reflection;

namespace Common.Data.Helpers;

internal class EmbeddedResource : IEmbeddedResource
{
	private readonly Dictionary<Assembly, string> _assemblyNames;

	#region Constructors

	/// <summary>
	/// Initializes a new instance of the EmbeddedResource class.
	/// </summary>
	public EmbeddedResource() : this( Array.Empty<Assembly>() )
	{ }

	/// <summary>
	/// Initializes a new instance of the EmbeddedResource class.
	/// </summary>
	/// <param name="assembliesToPreload">Collection of assemblies.</param>
	public EmbeddedResource( IEnumerable<Assembly> assembliesToPreload )
	{
		_assemblyNames = new Dictionary<Assembly, string>();
		foreach( var assembly in assembliesToPreload )
		{
			var name = assembly.GetName()?.Name;
			if( null != name )
				_assemblyNames.Add( assembly, name );
		}
	}

	#endregion

	#region Public Methods

	/// <inheritdoc/>
	public Stream Read<T>( string resource )
	{
		var assembly = typeof( T ).Assembly;
		return ReadInternal( assembly, resource );
	}

	/// <inheritdoc/>
	public Stream Read( Assembly assembly, string resource )
	{
		return ReadInternal( assembly, resource );
	}

	/// <inheritdoc/>
	public Stream Read( string assemblyName, string resource )
	{
		var assembly = Assembly.Load( assemblyName );
		return ReadInternal( assembly, resource );
	}

	#endregion

	#region Internal Methods

	/// <summary>
	/// Uses the GetManifestResourceStream method on the assembly to get a stream to read from.
	/// </summary>
	/// <param name="assembly">Assembly containing the resource.</param>
	/// <param name="resource">Name of the resource.</param>
	/// <returns>The resource as a stream.</returns>
	/// <remarks>Important thing here is how the path is built up to read a resource.
	/// We need to specify the assembly name and then use dot notation to create the
	/// correct path as in assembly name.Folder.Filename
	/// For example: Private.Data.File.PersonData.json
	/// </remarks>
	internal Stream ReadInternal( Assembly assembly, string resource )
	{
		if( !_assemblyNames.ContainsKey( assembly ) )
		{
			var name = assembly.GetName()?.Name;
			if( null != name )
				_assemblyNames[assembly] = name;
		}

		// Assembly name.Folder.Filename
		var stream = assembly.GetManifestResourceStream( $"{_assemblyNames[assembly]}.{resource}" );
		return stream ?? new MemoryStream();
	}

	#endregion
}
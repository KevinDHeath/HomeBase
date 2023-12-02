using System.Reflection;

namespace Common.Data.Helpers;

internal interface IEmbeddedResource
{
	/// <summary>
	/// Read from some type in the assembly.
	/// </summary>
	/// <typeparam name="T">Some type in the assembly.</typeparam>
	/// <param name="resource">Name of the resource.</param>
	/// <returns>The resource as a stream.</returns>
	Stream Read<T>( string resource );

	/// <summary>
	/// Read from some assembly.
	/// </summary>
	/// <param name="assembly">Some assembly.</param>
	/// <param name="resource">Name of the resource.</param>
	/// <returns>The resource as a stream.</returns>
	Stream Read( Assembly assembly, string resource );

	/// <summary>
	/// Read from some preloaded assembly.
	/// </summary>
	/// <param name="assemblyName">The name of the assembly.</param>
	/// <param name="resource">Name of the resource.</param>
	/// <returns>The resource as a stream.</returns>
	Stream Read( string assemblyName, string resource );
}
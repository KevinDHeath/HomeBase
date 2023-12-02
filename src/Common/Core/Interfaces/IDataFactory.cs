namespace Common.Core.Interfaces;

/// <summary>Interface for a DataFactory implementation class.</summary>
/// <typeparam name="T">Generic class or interface.</typeparam>
public interface IDataFactory<T> where T : class
{
	#region Properties

	/// <summary>Gets or sets the collection of objects.</summary>
	List<T> Data { get; set; }

	/// <summary>Gets the total number of objects available.</summary>
	int TotalCount { get; }

	#endregion

	#region Methods

	/// <summary>Gets a collection of objects.</summary>
	/// <param name="max">Maximum number of objects to return. Zero indicates all available.</param>
	/// <returns>A collection of objects.</returns>
	IList<T> Get( int max = 0 );

	/// <summary>Gets a collection of objects from an external file.</summary>
	/// <param name="path">Location of the data file.</param>
	/// <param name="file">Name of the file. If not supplied the default name is used.</param>
	/// <param name="max">Maximum number of objects to return. Zero indicates all available.</param>
	/// <returns>A collection of objects.</returns>
	IList<T> Get( string path, string? file, int max = 0 );

	/// <summary>Serialize a collection of objects to a specified file location.</summary>
	/// <param name="path">Location for the file.</param>
	/// <param name="file">Name of the file. If not supplied the default name is used.</param>
	/// <param name="list">The collection to serialize.</param>
	/// <returns>True if the file was saved, otherwise false is returned.</returns>
	/// <remarks>There must be data already loaded and the path must exist.</remarks>
	bool Serialize( string path, string? file, IList<T>? list );

	/// <summary>Updates an object with data from another of the same kind.</summary>
	/// <param name="obj">Object containing the original values.</param>
	/// <param name="mod">Object containing the modified values.</param>
	/// <returns>False is returned if there were any failures during the update.</returns>
	bool Update( T obj, T mod );

	#endregion
}
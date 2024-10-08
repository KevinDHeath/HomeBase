global using System.ComponentModel;
global using System.Diagnostics;
global using Library.Data.Models;
global using Common.Core.Classes;

namespace Library.Data;

/// <summary>Data storage interface.</summary>
public interface IStorage
{
	/// <summary>Loads data from a file.</summary>
	/// <param name="fileName">Full name of the file.</param>
	/// <returns>A populated Library if the data was loaded, otherwise it is empty.</returns>
	DataStorage Load( string? fileName = null );

	/// <summary>Saves data to a file.</summary>
	/// <param name="data">Data model object.</param>
	/// <param name="fileName">Full name of the file.</param>
	/// <returns>True if the data was saved, otherwise false is returned.</returns>
	bool Save( DataStorage data, string? fileName = null );
}
namespace Common.Core.Interfaces;

/// <summary>Defines a generalized method that a model class implements to create a method for
/// updating of instances.</summary>
public interface IEditable
{
	/// <summary>Updates changes in another object of the same type to this object.</summary>
	/// <param name="obj">An object to use as the data source.</param>
	void Update( object? obj );
}
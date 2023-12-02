using Common.Core.Interfaces;

namespace Common.Core.Classes;

/// <summary>Base class for models that allow editing.</summary>
public abstract class ModelEdit : ModelData, ICloneable, IEditable, IEquatable<object>
{
	/// <inheritdoc/>
	public abstract object Clone();

	/// <inheritdoc/>
	/// <param name="obj">An object to compare with this object.</param>
	public abstract new bool Equals( object? obj );

	/// <summary>Updates the current object properties from an object of the same type.</summary>
	/// <param name="obj">An object with the changed values.</param>
	public abstract void Update( object? obj );
}
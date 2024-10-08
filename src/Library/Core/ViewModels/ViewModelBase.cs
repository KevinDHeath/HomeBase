namespace Library.Core.ViewModels;

/// <summary>Base class for view models.</summary>
public class ViewModelBase : ModelDataError, IDisposable
{
	/// <summary>Performs application-defined tasks associated with freeing, releasing,
	/// or resetting unmanaged resources.</summary>
	public virtual void Dispose()
	{
		GC.SuppressFinalize( this );
	}
}
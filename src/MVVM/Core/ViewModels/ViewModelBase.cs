using CommunityToolkit.Mvvm.ComponentModel;

namespace MVVM.Core.ViewModels;

/// <summary>Base class for view models.</summary>
public class ViewModelBase : ObservableObject, IDisposable
{
	#region IDisposable Implementation

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting
	/// unmanaged resources.
	/// </summary>
	public virtual void Dispose()
	{
		GC.SuppressFinalize( this );
	}

	#endregion
}
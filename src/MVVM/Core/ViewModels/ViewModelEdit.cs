using System.Collections;
using System.ComponentModel;
using Common.Core.Classes;

namespace MVVM.Core.ViewModels;

/// <summary>Base class for editable view models.</summary>
public class ViewModelEdit : ViewModelBase, INotifyDataErrorInfo
{
	#region Regular Expression Constants

	/// <summary>Government number format for Company.</summary>
	public const string cEINRegex = @"^\d{2}-?\d{7}$";

	///// <summary>International E.164 Phone number format.</summary>
	//internal const string cPhoneIntRegex = @"^[\+]?[0-9]{1,3}[\s.-]?[0-9]{1,3}[\s.-]?[0-9]{1,4}[\s.-]?[0-9]{1,4}$";

	/// <summary>Government number format for Person.</summary>
	/// <remarks>Allows leading "666" for test data purposes.</remarks>
	public const string cSSNRegex = @"^(?!000|9\d{2})\d{3}-?(?!00)\d{2}-?(?!0{4})\d{4}$";

	/// <summary>Simple e-mail address.</summary>
	public const string cEmailRegex = @".+@.+\..+";

	#endregion

	#region Constructor and Variables

	/// <summary>Validation executor.</summary>
	protected readonly ModelDataError _validation;

	/// <summary>Initializes a new instance of the ViewModelEdit class.</summary>
	protected ViewModelEdit()
	{
		_validation = new ModelDataError( this );
		_validation.ErrorsChanged += Core_ErrorsChanged;
	}

	#endregion

	#region INotifyDataErrorInfo Implementation

	/// <inheritdoc/>
	public bool HasErrors => _validation.HasErrors;

	/// <inheritdoc/>
	public IEnumerable GetErrors( string? propertyName )
	{
		return _validation.GetErrors( propertyName );
	}

	/// <inheritdoc/>
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	private void Core_ErrorsChanged( object? sender, DataErrorsChangedEventArgs e )
	{
		ErrorsChanged?.Invoke( this, e );
	}

	#endregion
}
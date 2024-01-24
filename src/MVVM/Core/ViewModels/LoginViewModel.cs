using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.Input;
using Common.Core.Classes;
using MVVM.Core.Services;
using MVVM.Core.Stores;
using MVVM.Core.Validations;

namespace MVVM.Core.ViewModels;

/// <summary>Login view model.</summary>
public sealed partial class LoginViewModel : ViewModelEdit
{
	#region Properties

	/// <summary>Gets or sets the user name.</summary>
	[Required( ErrorMessage = "Please enter name." )]
	public string? UserName
	{
		get => _name;
		set
		{
			if( value != _name )
			{
				_validation.ValidateProperty( value );
				SetProperty( ref _name, value );
				LoginCommand.NotifyCanExecuteChanged();
			}
		}
	}

	/// <summary>Gets or sets the user e-mail address.</summary>
	[Required( ErrorMessage = "Please enter email." )]
	[RegularExpression( RegExAttribute.cEmail, ErrorMessage = "Format not valid." )]
	[UserEmail]
	public string? UserEmail
	{
		get => _userEmail;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _userEmail, value );
			LoginCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the user date of birth.</summary>
	[Required( ErrorMessage = "Please enter date of birth." )]
	[NoFutureDate]
	public DateOnly? BirthDate
	{
		get => _dob;
		set
		{
			if( value != _dob )
			{
				_validation.ValidateProperty( value );
				SetProperty( ref _dob, value );
				LoginCommand.NotifyCanExecuteChanged();
			}
		}
	}

	/// <summary>Gets or sets the password.</summary>
	[Required( ErrorMessage = "Please enter a password." )]
	public string? Password
	{
		get => _password;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _password, value );
			LoginCommand.NotifyCanExecuteChanged();
		}
	}
	/// <summary>Indicates whether a User has not been selected.</summary>
	public bool IsNew { get; private set; }

	#endregion

	#region Commands

	/// <summary>Login command.</summary>
	[RelayCommand( CanExecute = nameof( CanLogin ) )]
	public void Login()
	{
		if( IsNew )
		{
			_store.CurrentLogin = new()
			{
				Name = UserName,
				Email = UserEmail
			};
			if( BirthDate is not null )
			{
				_store.CurrentLogin.BirthDate = BirthDate;
				_store.CurrentLogin.Age = ModelBase.CalculateAge( BirthDate );
			}
			_store.Users.Add( _store.CurrentLogin );
			_store.CurrentUser = _store.CurrentLogin;
		}
		else
		{
			_store.CurrentLogin = _store.CurrentUser;
		}
		_service.Navigate();
	}

	/// <summary>Cancel Login command.</summary>
	[RelayCommand]
	public void CancelLogin()
	{
		_service.Navigate();
	}

	#endregion

	#region Constructor and Variables

	internal readonly UsersStore _store;
	private readonly INavigationService _service;
	private string? _name;
	private string? _userEmail;
	private DateOnly? _dob;
	private string? _password;

	/// <summary>Initializes a new instance of the LoginViewModel class.</summary>
	/// <param name="store">Users storage.</param>
	/// <param name="service">Navigation service.</param>
	public LoginViewModel( UsersStore store, INavigationService service )
	{
		_store = store;
		_service = service;

		IsNew = true;
		if( store.CurrentUser is not null )
		{
			_name = store.CurrentUser.Name;
			_userEmail = store.CurrentUser.Email;
			_dob = store.CurrentUser.BirthDate;
			IsNew = false;
		}

		_validation.ValidateAllProperties();
	}

	#endregion

	#region Private Methods

	private bool CanLogin() => !_validation.HasErrors && Password is not null && Password.Length > 0;

	#endregion
}
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Common.Core.Classes;
using Common.Core.Models;
using MVVM.Core.Stores;
using MVVM.Core.Validations;

namespace MVVM.Core.ViewModels;

/// <summary>User view model.</summary>
public sealed partial class UsersViewModel : ViewModelEdit
{
	#region Properties

	/// <summary>Gets the list of users.</summary>
	public ObservableCollection<User> UsersList => _store.Users;

	/// <summary>Gets the collection of Genders.</summary>
	public IEnumerable<Genders> GenderValues => _store.GenderValues;

	/// <summary>Gets or sets the current user.</summary>
	public User? CurrentUser
	{
		get { return _store.CurrentUser; }
		set
		{
			_store.CurrentUser = value;
			OnPropertyChanged();
		}
	}

	internal bool IsNew => CurrentUser is null || !UsersList.Contains( CurrentUser );

	/// <summary>Gets or sets the list filter text.</summary>
	[ObservableProperty]
	private string _filterText;

	#endregion

	#region Commands

	/// <summary>Command to apply the user changes.</summary>
	public IRelayCommand SaveUserCommand { get; }

	/// <summary>Command to cancel the user changes.</summary>
	public IRelayCommand CancelUserEditCommand { get; }

	/// <summary>Command to create a new user.</summary>
	public IRelayCommand NewUserCommand { get; }

	#endregion

	#region Constructor and Variables

	internal readonly UsersStore _store;
	private string? _name;
	private int? _age;
	private string? _email;
	private DateOnly? _dob;
	private Genders _gender;
	private bool _changes;

	/// <summary>Initializes a new instance of the UserViewModel class.</summary>
	/// <param name="store">Users storage.</param>
	public UsersViewModel( UsersStore store )
	{
		_store = store;
		_filterText = string.Empty;

		// Must initialize commands before setting the initial user
		NewUserCommand = new RelayCommand( NewUser );
		CancelUserEditCommand = new RelayCommand( RollbackUser, AllowRollback );
		SaveUserCommand = new RelayCommand( SaveUser, AllowApply );

		// Set the initial user
		_store.CurrentUserChanged += RollbackUser;
		if( _store.CurrentLogin is not null ) { CurrentUser = _store.CurrentLogin; }
		else if( UsersList.Count > 0 ) { CurrentUser = UsersList[0]; } else { RollbackUser(); }
	}

	#endregion

	#region List Filtering

	/// <summary>List filter predicate.</summary>
	public Func<object, bool> ListFilter
	{
		get
		{
			return ( item ) =>
			{
				if( FilterText.Length == 0 ) { return true; }

				if( item is User user )
				{
					string age = user.Age.HasValue ? user.Age.Value.ToString() : string.Empty;
					string dob = user.BirthDate.HasValue ? user.BirthDate.Value.ToString() : string.Empty;
					string name = user.Name is not null ? user.Name : string.Empty;
					string email = user.Email is not null ? user.Email : string.Empty;

					if( name.Contains( FilterText, StringComparison.OrdinalIgnoreCase ) ||
						( age.Length > 0 && age.Contains( FilterText ) ) ||
						( dob.Length > 0 && dob.Contains( FilterText ) ) ||
						( email.Length > 0 && email.Contains( FilterText ) ) )
						return true;
				}

				return false;
			};
		}
	}

	#endregion

	#region Overridden Methods

	/// <inheritdoc/>
	public override void Dispose()
	{
		_store.CurrentUserChanged -= RollbackUser;
		base.Dispose();
	}

	#endregion

	#region Private Methods

	private void EvaluateCommands()
	{
		CancelUserEditCommand.NotifyCanExecuteChanged();
		SaveUserCommand.NotifyCanExecuteChanged();
	}

	private void NewUser()
	{
		CurrentUser = new User();
	}

	private bool AllowRollback()
	{
		if( CurrentUser is null ) return true;

		_changes = !CurrentUser.Equals( this );
		return _changes;
	}

	private void RollbackUser()
	{
		CurrentUser ??= new User();

		Name = CurrentUser.Name;
		Age = CurrentUser.Age;
		Email = CurrentUser.Email;
		BirthDate = CurrentUser.BirthDate;
		Gender = CurrentUser.Gender;
	}

	private bool AllowApply() => !HasErrors & _changes;

	private void SaveUser()
	{
		if( CurrentUser is null ) { return; }

		CurrentUser.Update( this );
		_store.Save();

		EvaluateCommands();
	}

	#endregion
}

#region IUser Implementation

public sealed partial class UsersViewModel : ViewModelEdit, Common.Core.Interfaces.IUser
{
	/// <summary>Gets or sets the unique identifier.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the name.</summary>
	[Required( ErrorMessage = "{0} cannot be empty." )]
	[StringLength( 50, ErrorMessage = "{0} cannot be greater than 50 chars." )]
	[UserRule()]
	public string? Name
	{
		get => _name;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _name, value );
			EvaluateCommands();
		}
	}

	/// <summary>Gets or sets the age.</summary>
	[Required( ErrorMessage = "{0} cannot be empty." )]
	[Range( 0, 140, ErrorMessage = "{0} must be between {1} and {2}" )]
	public int? Age
	{
		get => _age;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _age, value );
			if( _name is not null ) { _validation.ValidateProperty( _name, nameof( Name ) ); }
			EvaluateCommands();
		}
	}

	/// <summary>Gets or sets the email address.</summary>
	[Required( ErrorMessage = "{0} cannot be empty." )]
	[RegularExpression( RegExAttribute.cEmail, ErrorMessage = "Format not valid." )]
	[UserEmail]
	public string? Email
	{
		get => _email;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _email, value );
			EvaluateCommands();
		}
	}

	/// <summary>Gets or sets the date of birth.</summary>
	[Display( Name = "Date of Birth" )]
	[Required( ErrorMessage = "{0} cannot be empty." )]
	[NoFutureDate]
	public DateOnly? BirthDate
	{
		get => _dob;
		set
		{
			_validation.ValidateProperty( value );
			if( value != _dob )
			{
				SetProperty( ref _dob, value );
				if( _dob is not null ) { Age = ModelBase.CalculateAge( _dob ); }
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the gender.</summary>
	public Genders Gender
	{
		get => _gender;
		set
		{
			SetProperty( ref _gender, value );
			EvaluateCommands();
		}
	}
}

#endregion
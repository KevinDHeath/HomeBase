using System.Collections.ObjectModel;
using Common.Core.Models;

namespace MVVM.Core.Stores;

/// <summary>Stores the Users data used by the view model.</summary>
public class UsersStore
{
	#region Properties

	/// <summary>Gets the list of users.</summary>
	public ObservableCollection<User> Users { get; private set; }

	/// <summary>Gets or sets the current user information.</summary>
	public User? CurrentUser
	{
		get => _currentUser;
		set
		{
			_currentUser = value;
			CurrentUserChanged?.Invoke();
		}
	}

	/// <summary>Gets or sets the current login information.</summary>
	public User? CurrentLogin
	{
		get => _currentLogin;
		set
		{
			_currentLogin = value;
			CurrentLoginChanged?.Invoke();
		}
	}

	/// <summary>Gets the collection of Genders.</summary>
	public IEnumerable<Genders> GenderValues
	{
		get
		{
			_genders ??= GetGenders();
			return _genders;
		}
	}

	/// <summary>Determines if a user is logged in.</summary>
	public bool IsLoggedIn => CurrentLogin is not null;

	#endregion

	#region Constructor and Variables

	private User? _currentUser;
	private User? _currentLogin;
	private IEnumerable<Genders>? _genders;

	/// <summary>Initializes a new instance of the UserStore class.</summary>
	/// <param name="useAsync">True to use asynchronous loading.</param>
	public UsersStore( bool useAsync = false )
	{
		Users = new ObservableCollection<User>( useAsync ? User.GetUsersAsync().Result : User.GetUsers() );
	}

	#endregion

	#region Internal Methods

	internal static IEnumerable<Genders> GetGenders()
	{
		Genders[] values = (Genders[])Enum.GetValues( typeof( Genders ) );
		return values.OrderBy( v => v.ToString() );
	}

	internal bool DoesEmailExist( string email ) => Users.Any( x => x.Email is not null &&
		x.Email.Equals( email, StringComparison.OrdinalIgnoreCase ) );

	internal void Save()
	{
		if( CurrentUser is not null && !Users.Contains( CurrentUser ) )
		{
			Users.Add( CurrentUser );
		}
	}

	internal void Logout()
	{
		CurrentLogin = null;
	}

	#endregion

	#region Public Events

	/// <summary>Occurs when the current user changes.</summary>
	public event Action? CurrentUserChanged;

	/// <summary>Occurs when the current login changes.</summary>
	public event Action? CurrentLoginChanged;

	#endregion
}
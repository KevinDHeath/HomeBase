using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Common.Core.Classes;
using Common.Core.Interfaces;

namespace Common.Core.Models;

/// <summary>This class contains details of a User.</summary>
public class User : ModelEdit, IUser
{
	#region IUser Properties

	/// <inheritdoc/>
	public override int Id { get; set; }

	/// <inheritdoc/>
	[MaxLength( 50 )]
	public string? Name { get; set; }

	/// <inheritdoc/>
	[Range( 0, 150 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public int? Age { get; set; }

	/// <inheritdoc/>
	[Column( TypeName = "date" )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public DateOnly? BirthDate { get; set; }

	/// <inheritdoc/>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Email { get; set; }

	/// <inheritdoc/>
	[Required]
	public Genders Gender { get; set; }

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the User class.</summary>
	public User()
	{ }

	#endregion

	#region Public Methods

	/// <inheritdoc/>
	public override object Clone()
	{
		return MemberwiseClone();
	}

	/// <inheritdoc/>
	/// <param name="obj">An object implementing the IUser interface to compare with this object.</param>
	public override bool Equals( object? obj )
	{
		if( obj is null || obj is not IUser user ) { return false; }

		if( user.Name != Name ) { return false; }
		if( user.Email != Email ) { return false; }
		if( user.Age != Age ) { return false; }
		if( user.BirthDate != BirthDate ) { return false; }
		if( user.Gender != Gender ) { return false; }

		return true;
	}

	/// <inheritdoc/>
	/// <param name="obj">An object implementing the IUser interface with the changed values.</param>
	public override void Update( object? obj )
	{
		if( obj is null || obj is not IUser other ) { return; }

		if( other.Name != Name ) { Name = other.Name; OnPropertyChanged( nameof( Name ) ); }
		if( other.Age != Age ) { Age = other.Age; OnPropertyChanged( nameof( Age ) ); }
		if( other.Email != Email ) { Email = other.Email; OnPropertyChanged( nameof( Email ) ); }
		if( other.BirthDate != BirthDate ) { BirthDate = other.BirthDate; OnPropertyChanged( nameof( BirthDate ) ); }
		if( other.Gender != Gender ) { Gender = other.Gender; OnPropertyChanged( nameof( Gender ) ); }
	}

	#endregion

	#region Static Methods

	/// <summary>Retrieves a list of users.</summary>
	/// <returns>A list of users.</returns>
	public static IList<User> GetUsers()
	{
		var items = new List<User>();

		User wrk = new() { Name = "John Doe", Email = "john@doe-family.com", BirthDate = new DateOnly( 1981, 10, 14 ) };
		wrk.Age = CalculateAge( wrk.BirthDate );
		wrk.Gender = Genders.Male;
		items.Add( wrk );

		wrk = new() { Name = "Jane Doe", Email = "jane@doe-family.com", BirthDate = new DateOnly( 1984, 7, 5 ) };
		wrk.Age = CalculateAge( wrk.BirthDate );
		wrk.Gender = Genders.Female;
		items.Add( wrk );

		wrk = new() { Name = "Chris Doe", Email = "chris.doe@gmail.com", BirthDate = new DateOnly( 2002, 1, 19 ) };
		wrk.Age = CalculateAge( wrk.BirthDate );
		wrk.Gender = Genders.Neutral;
		items.Add( wrk );

		wrk = new() { Name = "Donna Doe", Email = "donna.doe@gmail.com", BirthDate = new DateOnly( 2005, 12, 8 ) };
		wrk.Age = CalculateAge( wrk.BirthDate );
		wrk.Gender = Genders.Fluid;
		items.Add( wrk );

		return items;
	}

	/// <summary>Asynchronously retrieves a list of users.</summary>
	/// <returns>A list of users.</returns>
	public static async Task<IList<User>> GetUsersAsync()
	{
		IList<User> rtn = new List<User>();
		await Task.Run( () => { rtn = GetUsers(); } );
		return rtn;
	}

	#endregion
}
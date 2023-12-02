using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Core.Classes;
using Common.Core.Converters;
using Common.Core.Interfaces;

namespace Common.Core.Models;

/// <summary>This class contains details of a Person.</summary>
public class Person : ModelEdit, IPerson
{
	/// <summary>Default name of the Person data file.</summary>
	public const string cDefaultFile = "Person.json";

	#region Private Variables

	private int _id;
	private string? _firstName;
	private string? _middleName;
	private string? _lastName;
	private Address _address;
	private string? _governmentNumber;
	private string? _idState;
	private string? _idNumber;
	private string? _homePhone;
	private DateOnly? _birthDate;

	#endregion

	#region IPerson Properties

	/// <inheritdoc/>
	public override int Id
	{
		get => _id;
		set
		{
			if( value != _id )
			{
				_id = value;
				OnPropertyChanged( nameof( Id ) );
			}
		}
	}

	/// <inheritdoc/>
	[Required]
	[MaxLength( 50 )]
	public string FirstName
	{
		get => ( _firstName is not null ) ? _firstName : string.Empty;
		set
		{
			if( !value.Equals( _firstName ) )
			{
				_firstName = value;
				OnPropertyChanged( nameof( FirstName ) );
				OnPropertyChanged( nameof( FullName ) );
			}
		}
	}

	/// <inheritdoc/>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	[MaxLength( 50 )]
	public string? MiddleName
	{
		get => _middleName;
		set
		{
			if( value != _middleName )
			{
				_middleName = SetNullString( value );
				OnPropertyChanged( nameof( MiddleName ) );
				OnPropertyChanged( nameof( FullName ) );
			}
		}
	}

	/// <inheritdoc/>
	[Required]
	[MaxLength( 50 )]
	public string LastName
	{
		get => ( _lastName is not null ) ? _lastName : string.Empty;
		set
		{
			if( !value.Equals( _lastName ) )
			{
				_lastName = value;
				OnPropertyChanged( nameof( LastName ) );
				OnPropertyChanged( nameof( FullName ) );
			}
		}
	}

	/// <inheritdoc/>
	public Address Address
	{
		get => _address;
		set
		{
			if( value != _address )
			{
				_address = value;
				OnPropertyChanged( nameof( Address ) );
			}
		}
	}

	/// <inheritdoc/>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? GovernmentNumber
	{
		get => _governmentNumber;
		set
		{
			if( value != _governmentNumber )
			{
				_governmentNumber = SetNullString( value );
				OnPropertyChanged( nameof( GovernmentNumber ) );
			}
		}
	}

	/// <inheritdoc/>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? IdState
	{
		get => _idState;
		set
		{
			if( value != _idState )
			{
				_idState = SetNullString( value );
				OnPropertyChanged( nameof( IdState ) );
			}
		}
	}

	/// <inheritdoc/>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? IdNumber
	{
		get => _idNumber;
		set
		{
			if( value != _idNumber )
			{
				_idNumber = SetNullString( value );
				OnPropertyChanged( nameof( IdNumber ) );
			}
		}
	}

	/// <inheritdoc/>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? HomePhone
	{
		get => _homePhone;
		set
		{
			if( value != _homePhone )
			{
				_homePhone = SetNullString( value );
				OnPropertyChanged( nameof( HomePhone ) );
			}
		}
	}

	/// <inheritdoc/>
	[Column( TypeName = "date" )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	[JsonConverter( typeof( JsonDateOnlyString ) )]
	public DateOnly? BirthDate
	{
		get => _birthDate;
		set
		{
			if( value != _birthDate )
			{
				_birthDate = value;
				OnPropertyChanged( nameof( BirthDate ) );
			}
		}
	}

	/// <inheritdoc/>
	[NotMapped]
	[JsonIgnore]
	public string FullName
	{
		get
		{
			var values = new[] { FirstName.Trim(), MiddleName?.Trim(), LastName.Trim() };
			return string.Join( " ", values.Where( s => !string.IsNullOrEmpty( s ) ) );
		}
	}

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the Person class.</summary>
	public Person()
    {
		_address = new Address();
	}

	#endregion

	#region Public Methods

	/// <inheritdoc/>
	public override object Clone()
	{
		return ReflectionHelper.CreateDeepCopy( this ) as Person ?? new Person();
	}

	/// <inheritdoc/>
	/// <param name="obj">An object implementing the IPerson interface to compare with this object.</param>
	public override bool Equals( object? obj )
	{
		if( obj is null || obj is not IPerson other ) { return false; }

		if( other.Id != Id ) { return false; }
		if( other.FirstName != FirstName ) { return false; }
		if( other.MiddleName != MiddleName ) { return false; }
		if( other.LastName != LastName ) { return false; }
		if( other.GovernmentNumber != GovernmentNumber ) { return false; }
		if( other.IdState != IdState ) { return false; }
		if( other.IdNumber != IdNumber ) { return false; }
		if( other.HomePhone != HomePhone ) { return false; }
		if( other.BirthDate != BirthDate ) { return false; }
		if( !other.Address.Equals( Address ) ) { return false; }

		return true;
	}

	/// <inheritdoc/>
	/// <param name="obj">An object implementing the IPerson interface with the changed values.</param>
	public override void Update( object? obj )
	{
		if( obj is null || obj is not IPerson other || other is not Person person ) { return; }

		ReflectionHelper.ApplyChanges( person, this );
	}

	/// <summary>Gets the Json serializer options for Person objects.</summary>
	/// <returns>A JsonSerializerOptions object.</returns>
	public static JsonSerializerOptions GetSerializerOptions()
	{
		var rtn = JsonHelper.DefaultSerializerOptions();
		rtn.Converters.Add( new InterfaceFactory( typeof( Person ), typeof( IPerson ) ) );
		rtn.NumberHandling = JsonNumberHandling.AllowReadingFromString;

		return rtn;
	}

	/// <summary>Builds a Person object from a database table row.</summary>
	/// <param name="row">Database row containing the Person columns.</param>
	/// <param name="addPrefix">Table column prefix for Address fields if required.</param>
	/// <returns>Person object containing the database values.</returns>
	/// <remarks>This method assumes that the table column names are the same as the property names.</remarks>
	public static Person Read( DataRow row, string addPrefix = "" )
	{
		return new Person()
		{
			Id = row.Field<int>( nameof( Id ) ),
			FirstName = row.Field<string>( nameof( FirstName ) )!,
			MiddleName = row.Field<string?>( nameof( MiddleName ) ),
			LastName = row.Field<string>( nameof( LastName ) )!,
			Address = Address.BuildAddress( row, addPrefix ),
			GovernmentNumber = row.Field<string?>( nameof( GovernmentNumber ) ),
			IdState = row.Field<string?>( nameof( IdState ) ),
			IdNumber = row.Field<string?>( nameof( IdNumber ) ),
			HomePhone = row.Field<string?>( nameof( HomePhone ) ),
			BirthDate = Generic.DateTimeToDateOnly( row[nameof( BirthDate )] ),
		};
	}

	/// <summary>Builds the SQL script for any value changes.</summary>
	/// <param name="row">Database row containing the current Person data.</param>
	/// <param name="obj">IPerson object containing the original values.</param>
	/// <param name="mod">IPerson object containing the modified values.</param>
	/// <param name="addPrefix">Table column name prefix for Address fields (if required).</param>
	/// <returns>An empty string is returned if no changes were found.</returns>
	/// <remarks>This method assumes that the table column names are the same as the property names.</remarks>
	public static string UpdateSQL( DataRow row, IPerson obj, IPerson mod, string addPrefix = "" )
	{
		IList<string> sql = new List<string>();
		Person cur = Read( row, addPrefix );
		if( cur.Id != mod.Id ) { return string.Empty; }

		if( Changed( nameof( FirstName ), sql, obj.FirstName, mod.FirstName, cur.FirstName ) )
		{ mod.FirstName = cur.FirstName; }

		if( Changed( nameof( MiddleName ), sql, obj.MiddleName, mod.MiddleName, cur.MiddleName ) )
		{ mod.MiddleName = cur.MiddleName; }

		if( Changed( nameof( LastName ), sql, obj.LastName, mod.LastName, cur.LastName ) )
		{ mod.LastName = cur.LastName; }

		Address.UpdateAddress( obj.Address, mod.Address, cur.Address, sql, addPrefix );

		if( Changed( nameof( GovernmentNumber ), sql, obj.GovernmentNumber, mod.GovernmentNumber, cur.GovernmentNumber ) )
		{ mod.GovernmentNumber = cur.GovernmentNumber; }

		if( Changed( nameof( IdState ), sql, obj.IdState, mod.IdState, cur.IdState ) )
		{ mod.IdState = cur.IdState; }

		if( Changed( nameof( IdNumber ), sql, obj.IdNumber, mod.IdNumber, cur.IdNumber ) )
		{ mod.IdNumber = cur.IdNumber; }

		if( Changed( nameof( HomePhone ), sql, obj.HomePhone, mod.HomePhone, cur.HomePhone ) )
		{ mod.HomePhone = cur.HomePhone; }

		// Special handling for DateOnly format
		if( obj.BirthDate != mod.BirthDate && obj.BirthDate == cur.BirthDate )
		{
			string? val = mod.BirthDate?.ToString( "yyyy-MM-dd" );
			SetSQLColumn( nameof( BirthDate ), val, sql );
		}
		else if( mod.BirthDate != cur.BirthDate ) { mod.BirthDate = cur.BirthDate; }

		return string.Join( ", ", sql );
	}

	#endregion
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;
using Common.Core.Classes;

namespace Common.Core.Models;

/// <summary>This class contains details of an Address.</summary>
public class Address : ModelBase, IEquatable<object>
{
	#region Private Variables

	private string? _street;
	private string? _city;
	private string? _state;
	private string? _zipCode;
	private string? _country;

	#endregion

	#region Properties

	/// <summary>Gets or sets the Street.</summary>
	[MaxLength(50)]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Street
	{
		get => _street;
		set
		{
			if( value != _street )
			{
				_street = SetNullString( value );
				OnPropertyChanged( nameof( Street ) );
				OnPropertyChanged( nameof( FullAddress ) );
			}
		}
	}

	/// <summary>Gets or sets the City.</summary>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? City
	{
		get => _city;
		set
		{
			if( value != _city )
			{
				_city = SetNullString( value );
				OnPropertyChanged( nameof( City ) );
				OnPropertyChanged( nameof( FullAddress ) );
			}
		}
	}

	/// <summary>Gets or sets the State.</summary>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? State
	{
		get => _state;
		set
		{
			if( value != _state )
			{
				_state = SetNullString( value );
				OnPropertyChanged( nameof( State ) );
				OnPropertyChanged( nameof( FullAddress ) );
			}
		}
	}

	/// <summary>Gets or sets the Postal Code.</summary>
	[MaxLength( 20 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? ZipCode
	{
		get => _zipCode;
		set
		{
			if( value != _zipCode )
			{
				_zipCode = SetNullString( value );
				OnPropertyChanged( nameof( ZipCode ) );
			}
		}
	}

	/// <summary>Gets or sets the Country.</summary>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Country
	{
		get => _country;
		set
		{
			if( value != _country )
			{
				_country = SetNullString( value );
				OnPropertyChanged( nameof( Country ) );
			}
		}
	}

	/// <summary>Gets the Full Address.</summary>
	[NotMapped]
	[JsonIgnore]
	public string FullAddress
	{
		get
		{
			var values = new[] { Street?.Trim(), City?.Trim(), State?.Trim() };
			return string.Join( ", ", values.Where( s => !string.IsNullOrEmpty( s ) ) );
		}
	}

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the Address class.</summary>
	public Address()
    { }

	#endregion

	#region Internal Methods

	/// <inheritdoc/>
	internal new bool Equals( object? obj )
	{
		if( obj is null || obj is not Address other ) { return false; }

		if( other.Street != Street ) { return false; }
		if( other.City != City ) { return false; }
		if( other.State != State ) { return false; }
		if( other.ZipCode != ZipCode ) { return false; }
		if( other.Country != Country ) { return false; }

		return true;
	}

	internal static Address BuildAddress( DataRow row, string prefix = "" )
	{
		return new Address()
		{
			Street = row.Field<string?>( prefix + nameof( Street ) ),
			City = row.Field<string?>( prefix + nameof( City ) ),
			State = row.Field<string?>( prefix + nameof( State ) ),
			ZipCode = row.Field<string?>( prefix + nameof( ZipCode ) ),
			Country = row.Field<string?>( prefix + nameof( Country ) )
		};
	}

	internal static void UpdateAddress( Address obj, Address mod, Address cur, IList<string> sql,
		string prefix = "" )
	{
		if( ModelData.Changed( prefix + nameof( Street ), sql, obj.Street, mod.Street, cur.Street ) )
		{ mod.Street = cur.Street; }

		if( ModelData.Changed( prefix + nameof( City ), sql, obj.City, mod.City, cur.City ) )
		{ mod.City = cur.City; }

		if( ModelData.Changed( prefix + nameof( State ), sql, obj.State, mod.State, cur.State ) )
		{ mod.State = cur.State; }

		if( ModelData.Changed( prefix + nameof( ZipCode ), sql, obj.ZipCode, mod.ZipCode, cur.ZipCode ) )
		{ mod.ZipCode = cur.ZipCode; }

		if( ModelData.Changed( prefix + nameof( Country ), sql, obj.Country, mod.Country, cur.Country ) )
		{ mod.Country = cur.Country; }
	}

	#endregion
}
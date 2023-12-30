using System.ComponentModel.DataAnnotations;
using Common.Core.Models;
using Common.Core.Classes;
using MVVM.Core.Validations;

namespace MVVM.Core.ViewModels;

/// <summary>Address view model.</summary>
public abstract class AddressViewModel : ViewModelEdit
{
	#region Properties

	/// <summary>Gets a list of US State data.</summary>
	public static IList<Province> States => AddressFactoryBase.Provinces;

	/// <summary>Gets a sorted list of ISO Country data.</summary>
	public static IList<CountryCode> Countries => AddressFactoryBase.Countries;

	/// <summary>Gets or sets the Street.</summary>
	[Required( ErrorMessage = "Street cannot be empty." )]
	public string Street
	{
		get => ( _ad.Street is null ) ? string.Empty : _ad.Street;
		set
		{
			if( !value.Equals( _ad.Street ) )
			{
				_validation.ValidateProperty( value );
				_ad.Street = value;
			}
		}
	}

	/// <summary>Gets or sets the City name.</summary>
	public string? City
	{
		get => _ad.City;
		set
		{
			if( value != _ad.City )
			{
				_validation.ValidateProperty( value );
				_ad.City = value;
				if( _ad.ZipCode is not null ) _validation.ValidateProperty( _ad.ZipCode, nameof( ZipCode ) );
				OnPropertyChanged(); // Need this as it may change in Zip code validation
			}
		}
	}

	private string? _county;
	/// <summary>Gets or sets the County name.</summary>
	public string? County
	{
		get => _county;
		set
		{
			if( value != _county )
			{
				_county = value;
				OnPropertyChanged(); // Need this as it is not in base model
			}
		}
	}

	/// <summary>Gets or sets the State code.</summary>
	[StateCode()]
	public string? State
	{
		get => _ad.State;
		set
		{
			value = value?.ToUpper();
			if( value != _ad.State )
			{
				_validation.ValidateProperty( value );
				_ad.State = value;
				if( _ad.ZipCode is not null ) _validation.ValidateProperty( _ad.ZipCode, nameof( ZipCode ) );
				OnPropertyChanged(); // Need this as it may change in Zip code validation
			}
		}
	}

	/// <summary>Gets or sets the Zip code.</summary>
	[ZipCode]
	public string? ZipCode
	{
		get => _ad.ZipCode;
		set
		{
			if( value != _ad.ZipCode )
			{
				_validation.ValidateProperty( value );
				_ad.ZipCode = value;
			}
		}
	}

	/// <summary>Gets or sets the Country code.</summary>
	[CountryCode()]
	public string? Country
	{
		get => _ad.Country;
		set
		{
			value = value?.ToUpper();
			if( value != _ad.Country )
			{
				_validation.ValidateProperty( value );
				_ad.Country = value;
			}
		}
	}

	#endregion

	#region Constructor and Variables

	/// <summary>Internal address data.</summary>
	protected Address _ad;

	/// <summary>Initializes a new instance of the AddressViewModel class.</summary>
	protected AddressViewModel()
	{
		_ad = new Address();
	}

	#endregion
}
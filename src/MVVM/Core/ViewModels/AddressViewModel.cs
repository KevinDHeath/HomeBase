using System.ComponentModel.DataAnnotations;
using Common.Core.Models;
using Common.Core.Classes;
using MVVM.Core.Validations;

namespace MVVM.Core.ViewModels;

/// <summary>Address view model.</summary>
public abstract class AddressViewModel : ViewModelEdit
{
	#region Properties

	/// <summary>Gets a list of Province data.</summary>
	public static IList<Province> Provinces => AddressFactoryBase.Provinces;

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
				if( _ad.Postcode is not null ) _validation.ValidateProperty( _ad.Postcode, nameof( Postcode ) );
				OnPropertyChanged(); // Need this as it may change in Postcode validation
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

	/// <summary>Gets or sets the Province code.</summary>
	[Province()]
	public string? Province
	{
		get => _ad.Province;
		set
		{
			value = value?.ToUpper();
			if( value != _ad.Province )
			{
				_validation.ValidateProperty( value );
				_ad.Province = value;
				if( _ad.Postcode is not null ) _validation.ValidateProperty( _ad.Postcode, nameof( Postcode ) );
				OnPropertyChanged(); // Need this as it may change in Postcode validation
			}
		}
	}

	/// <summary>Gets or sets the Postcode.</summary>
	[Postcode]
	public string? Postcode
	{
		get => _ad.Postcode;
		set
		{
			if( value != _ad.Postcode )
			{
				_validation.ValidateProperty( value );
				_ad.Postcode = value;
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
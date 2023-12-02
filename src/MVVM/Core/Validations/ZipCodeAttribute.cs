using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Common.Core.Classes;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Validations;

internal partial class ZipCodeAttribute : ValidationAttribute
{
	[GeneratedRegex( @"^[0-9]{5}(?:-[0-9]{4})?$" )]
	private static partial Regex ZipCodeRegex();

	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext context )
	{
		if( value is not null && value is string val && val.Length > 0 &&
			context.MemberName is not null && context.ObjectInstance is AddressViewModel avm )
		{
			avm.County = null;
			if( AddressFactory.DefaultCountry.Equals( avm.Country, StringComparison.OrdinalIgnoreCase ) )
			{
				if( !ZipCodeRegex().IsMatch( val ) )
				{
					return new( "Format not valid.", new string[] { context.MemberName } );
				}
				else if( AddressData.ZipCodeCount > 0 )
				{
					var zip = AddressData.GetZipCode( val );
					if( zip is null )
					{
						return new( "Zip code is not valid.", new string[] { context.MemberName } );
					}
					else
					{
						avm.County = zip.County;
						if( !zip.State.Equals( avm.State, StringComparison.OrdinalIgnoreCase ) )
						{
							var name = AddressFactory.GetStateName( zip.State );
							return new( "Zip code is for '" + name + "'", new string[] { "State" } );
						}
						if( !zip.City.Equals( avm.City, StringComparison.OrdinalIgnoreCase ) )
						{
							return new( "Zip code is for '" + zip.City + "'", new string[] { "City" } );
						}
					}
				}
			}
		}

		return ValidationResult.Success;
	}
}
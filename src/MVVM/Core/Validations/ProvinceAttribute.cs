using System.ComponentModel.DataAnnotations;
using Common.Core.Classes;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Validations;

internal class ProvinceAttribute : ValidationAttribute
{
	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext context )
	{
		if( value is not null && value is string val && val.Length > 0 &&
			context.MemberName is not null && context.ObjectInstance is AddressViewModel avm )
		{
			if( AddressFactoryBase.DefaultCountry.Equals( avm.Country, StringComparison.OrdinalIgnoreCase ) )
			{
				if( AddressFactoryBase.Provinces.Count > 0 && !AddressFactoryBase.CheckProvinceCode( val ) )
				{
					return new( $"Province code '{val}' is not valid.", new string[] { context.MemberName } );
				}
			}
		}

		return ValidationResult.Success;
	}
}
using System.ComponentModel.DataAnnotations;
using Common.Core.Classes;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Validations;

internal class StateCodeAttribute : ValidationAttribute
{
	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext context )
	{
		if( value is not null && value is string val && val.Length > 0 &&
			context.MemberName is not null && context.ObjectInstance is AddressViewModel avm )
		{
			if( AddressFactory.DefaultCountry.Equals( avm.Country, StringComparison.OrdinalIgnoreCase ) )
			{
				if( AddressFactory.States.Count > 0 && !AddressFactory.CheckStateCode( val ) )
				{
					return new( $"State code '{val}' is not valid.", new string[] { context.MemberName } );
				}
			}
		}

		return ValidationResult.Success;
	}
}
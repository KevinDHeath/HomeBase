using System.ComponentModel.DataAnnotations;
using Common.Core.Classes;

namespace MVVM.Core.Validations;

internal class CountryCodeAttribute : ValidationAttribute
{
	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext context )
	{
		if( value is not null && value is string val && val.Length > 0 &&
			context.MemberName is not null )
		{
			if( !AddressFactory.CheckCountryCode( val ) )
			{
				return new( $"Country code '{val}' is not valid.", new string[] { context.MemberName } );
			}
		}

		return ValidationResult.Success;
	}
}
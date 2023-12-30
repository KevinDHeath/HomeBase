using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Common.Core.Classes;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Validations;

internal partial class PhoneNumberAttribute : ValidationAttribute
{
	[GeneratedRegex( @"^\(?\d{3}\)?-? *\d{3}-? *-?\d{4}$" )]
	private static partial Regex USPhoneRegex();

	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext context )
	{
		if( value is not null && value is string val && val.Length > 0 &&
			context.MemberName is not null )
		{
			if( context.ObjectInstance is AddressViewModel avm &&
				!AddressFactoryBase.DefaultCountry.Equals( avm.Country, StringComparison.OrdinalIgnoreCase ) )
			{
				// If Country is not US validate min 11 max 15 digits
				string onlyNumbers = new( val.ToCharArray().Where( c => char.IsDigit( c ) ).ToArray() );
				if( onlyNumbers.Length < 11 || onlyNumbers.Length > 15 )
				{
					return new( $"Format not valid.", new string[] { context.MemberName } );
				}
			}
			else
			{
				if( !USPhoneRegex().IsMatch( val ) )
				{
					return new( "Format not valid.", new string[] { context.MemberName } );
				}

			}
		}

		return ValidationResult.Success;
	}
}
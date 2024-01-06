using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Common.Core.Classes;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Validations;

internal partial class PostcodeAttribute : ValidationAttribute
{
	[GeneratedRegex( @"^[0-9]{5}(?:-[0-9]{4})?$" )]
	private static partial Regex ZipCodeRegex();

	internal static readonly string[] provinceMember = ["Province"];
	internal static readonly string[] cityMember = ["City"];

	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext context )
	{
		if( value is not null && value is string val && val.Length > 0 &&
			context.MemberName is not null && context.ObjectInstance is AddressViewModel avm )
		{
			avm.County = null;
			if( AddressFactoryBase.DefaultCountry.Equals( avm.Country, StringComparison.OrdinalIgnoreCase ) )
			{
				if( !ZipCodeRegex().IsMatch( val ) )
				{
					return new( "Format not valid.", new string[] { context.MemberName } );
				}
				else if( AddressFactoryBase.PostcodeCount > 0 )
				{
					var zip = AddressData.GetPostcode( val );
					if( zip is null )
					{
						return new( "Postcode is not valid.", new string[] { context.MemberName } );
					}
					else
					{
						avm.County = zip.County;
						if( !zip.Province.Equals( avm.Province, StringComparison.OrdinalIgnoreCase ) )
						{
							string name = AddressFactoryBase.GetProvinceName( zip.Province );
							return new( "Postcode is for '" + name + "'", provinceMember );
						}
						if( zip.City is not null && !zip.City.Equals( avm.City, StringComparison.OrdinalIgnoreCase ) )
						{
							return new( "Postcode is for '" + zip.City + "'", cityMember );
						}
					}
				}
			}
		}

		return ValidationResult.Success;
	}
}
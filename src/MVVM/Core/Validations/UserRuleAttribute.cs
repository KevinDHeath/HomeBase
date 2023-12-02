using System.ComponentModel.DataAnnotations;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Validations;

internal sealed class UserRuleAttribute : ValidationAttribute
{
	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext context )
	{
		if( context.ObjectInstance is UsersViewModel vm && context.MemberName is not null &&
			value is not null )
		{
			var age = context.MemberName == "Age" ? value as int? : vm.Age;
			var name = context.MemberName == "Name" ? value?.ToString() : vm.Name;
			if( age is not null && name?.Length > 0 )
			{
				if( ( age < 69 ) && name.StartsWith( "Kev", StringComparison.OrdinalIgnoreCase ) )
				{
					return new( $"{name} cannot be less that sixty nine!",
						new string[] { context.MemberName } );
				}
			}
		}

		return ValidationResult.Success;
	}
}
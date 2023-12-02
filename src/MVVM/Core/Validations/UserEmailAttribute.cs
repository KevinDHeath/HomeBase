using System.ComponentModel.DataAnnotations;
using MVVM.Core.Stores;
using MVVM.Core.ViewModels;

namespace MVVM.Core.Validations;

internal class UserEmailAttribute : ValidationAttribute
{
	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext context )
	{
		if( context.ObjectInstance is not null && context.MemberName is not null &&
			value is not null && value is string val )
		{
			if( val.Length > 0 )
			{
				bool isNew = false;
				UsersStore? store = null;

				if( context.ObjectInstance is UsersViewModel uvm )
				{
					isNew = uvm.IsNew;
					store = uvm._store;
				}
				else if( context.ObjectInstance is LoginViewModel lvm )
				{
					isNew = lvm.IsNew;
					store = lvm._store;
				}

				if( isNew && store is not null && store.DoesEmailExist( val ) )
				{
					return new( "Email not allowed.", new string[] { context.MemberName } );
				}
			}
		}

		return ValidationResult.Success;
	}
}
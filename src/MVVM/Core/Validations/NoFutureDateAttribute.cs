using System.ComponentModel.DataAnnotations;

namespace MVVM.Core.Validations;

/// <summary>Validates that a date is not in the future.</summary>
/// <remarks>This works for both <see cref="DateTime">DateTime</see> and <see cref="DateOnly">DateOnly</see> values.</remarks>
[AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
public sealed class NoFutureDateAttribute : ValidationAttribute
{
	/// <inheritdoc/>
	public override string FormatErrorMessage( string name )
	{
		return $"{name} cannot be in the future.";
	}

	/// <inheritdoc/>
	protected override ValidationResult? IsValid( object? value, ValidationContext validationContext )
	{
		if( validationContext.ObjectInstance is not null && validationContext.MemberName is not null &&
			value is not null )
		{
			DateOnly? dateOnly = null;
			if( value is DateTime dt ) { dateOnly = DateOnly.FromDateTime( dt ); }
			else if( value is DateOnly d ) { dateOnly = d; }

			if( dateOnly is not null )
			{
				if( dateOnly > DateOnly.FromDateTime( DateTime.Now ) )
				{
					return new( FormatErrorMessage( validationContext.DisplayName ),
						new string[] { validationContext.MemberName } );
				}
			}
		}

		return ValidationResult.Success;
	}
}
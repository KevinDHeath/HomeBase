using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Common.Core.Classes;

/// <summary>Base class for models that required the INotifyDataErrorInfo and
/// INotifyPropertyChanged interfaces.</summary>
/// <remarks>It's base class is <see cref="ModelBase"/> so that functionality is included.<br/>
/// It can be used as a base class, or as an instance variable.</remarks>
/// <example>
/// In a class using it as a base class, the protected constructor will be used:
/// <code language="C#">
/// using System.ComponentModel.DataAnnotations;
/// using Common.Core.Classes;
///
/// public class TestModel : ModelDataError
/// {
///     public bool IsValid => !HasErrors;
///
///     private string? _name;
///     [Display( Name = "Full Name" )]
///     [Required( ErrorMessage = "Name cannot be empty." )]
///     public string Name
///     {
///         get => ( _name is not null ) ? _name : string.Empty;
///         set
///         {
///             if( value.Equals( _name ) ) return;
///
///             name = value;
///             ValidateProperty( value );
///             OnPropertyChanged();
///         }
///     }
///
///     public TestModel()
///     { }
/// }
/// </code>
/// In a class using it as an instance variable, the public constructor must be used.
/// <br/>The INotifyDataErrorInfo interface must be implemented and the event handler needs to
/// subscribe to the core error version so that the ErrorsChanged event is seen as being used:
/// <code language="C#">
/// using System.Collections;
/// using System.ComponentModel;
/// using System.ComponentModel.DataAnnotations;
/// using Common.Core.Classes;
///
/// public class TestModel : INotifyDataErrorInfo
/// {
///     public bool IsValid => !_validator.HasErrors;
///
///     private string? _name;
///     [Display( Name = "Full Name" )]
///     [Required( ErrorMessage = "Name cannot be empty." )]
///     public string Name
///     {
///         get => ( _name is not null ) ? _name : string.Empty;
///         set
///         {
///             if( value.Equals( _name ) ) return;
///             name = value;
///             _validator.ValidateProperty( value );
///             OnPropertyChanged();
///         }
///     }
///
///     private readonly ModelDataError _validator;
///
///     public TestModel()
///     {
///         // Do this before anything else
///         _validator = new ModelDataError( this );
///         _validator.ErrorsChanged += Core_ErrorsChanged;
///     }
///
///     public bool HasErrors => _validator.HasErrors;
///
///     public IEnumerable GetErrors( string? propertyName )
///     {
///         return _validator.GetErrors( propertyName );
///     }
///
///     public event EventHandler&lt;DataErrorsChangedEventArgs&gt;? ErrorsChanged;
/// 
///     private void Core_ErrorsChanged( object? sender, DataErrorsChangedEventArgs e )
///     {
///         ErrorsChanged?.Invoke( this, e );
///     }
/// }
/// </code>
/// </example>
public class ModelDataError : ModelBase, INotifyDataErrorInfo
{
	#region INotifyDataErrorInfo Implementation

	/// <inheritdoc/>
	public bool HasErrors => _errors.Any( propErrors => propErrors.Value.Count > 0 );

	/// <inheritdoc/>
	public IEnumerable GetErrors( string? propertyName )
	{
		// Get entity-level errors when the target property is null or empty
		if( string.IsNullOrEmpty( propertyName ) )
		{
			// Local function to gather all the entity-level errors
			[MethodImpl( MethodImplOptions.NoInlining )]
			IEnumerable<ValidationResult> GetAllErrors()
			{
				return _errors.Values.SelectMany( static errors => errors );
			}

			return GetAllErrors();
		}

		// Property-level errors, if any
		if( propertyName is not null && _errors.ContainsKey( propertyName ) )
		{
			return _errors[propertyName];
		}

		// Property not found
		return Array.Empty<ValidationResult>();
	}

	/// <inheritdoc/>
	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	#endregion

	#region Constructors and Variables

	private readonly Dictionary<string, List<ValidationResult>> _errors = new();
	private readonly ConditionalWeakTable<Type, Dictionary<string, string>> _displayNamesMap = new();
	private readonly PropertyInfo[] _properties;
	private readonly ValidationContext _context;

	/// <summary>Initializes a new instance of the ModelDataError class.</summary>
	protected ModelDataError()
	{
		_properties = GetProperties( GetType() );
		_context = new ValidationContext( this );
	}

	/// <summary>Initializes a new instance of the ModelDataError class.</summary>
	/// <param name="instance">Object being validated.</param>
	public ModelDataError( object instance )
	{
		_properties = GetProperties( instance.GetType() );
		_context = new ValidationContext( instance );
	}

	#endregion

	#region Public Methods

	/// <summary>Validates all the properties in the current instance.</summary>
	public void ValidateAllProperties()
	{
		foreach( var propertyInfo in _properties )
		{
			ValidateProperty( propertyInfo.GetValue( _context.ObjectInstance ), propertyInfo.Name );
		}
	}

	/// <summary>Validates a property with a specified name and a given input value.</summary>
	/// <param name="value">The value to test for the specified property.</param>
	/// <param name="propertyName">The name of the property to validate.</param>
	/// <returns>True if the value is valid, false if any errors are found.</returns>
	public bool ValidateProperty( object? value, [CallerMemberName] string propertyName = "" )
	{
		if( string.IsNullOrEmpty( propertyName ) ) return true;

		_context.MemberName = propertyName;
		_context.DisplayName = GetDisplayNameForProperty( propertyName );

		var results = new List<ValidationResult>();
		Validator.TryValidateProperty( value, _context, results );

		ClearErrors( propertyName );
		AddValidationResults( results );

		return results.Count == 0;
	}

	/// <summary>Clears the validation errors for a specified property or for the entire entity.</summary>
	/// <param name="propertyName">The name of the property to clear validation errors for.<br/>
	/// If a <see langword="null"/> or empty name is used, all entity-level errors will be cleared.
	/// </param>
	public void ClearErrors( string? propertyName = null )
	{
		if( !string.IsNullOrEmpty( propertyName ) )
		{
			if( _errors.Remove( propertyName ) )
			{
				OnErrorsChanged( propertyName );
			}
		}
		else
		{
			foreach( KeyValuePair<string, List<ValidationResult>> property in _errors )
			{
				ClearErrors( property.Key );
			}
		}
	}

	#endregion

	#region Private Methods

	private void OnErrorsChanged( string? propertyName )
	{
		ErrorsChanged?.Invoke( this, new DataErrorsChangedEventArgs( propertyName ) );
	}

	private void AddValidationResults( List<ValidationResult> results )
	{
		if( results.Count == 0 ) { return; }

		// Group validation results by property names
		var resultsByPropName = from res in results
								from mname in res.MemberNames
								group res by mname into g
								select g;

		foreach( var property in resultsByPropName )
		{
			results = property.Select( res => res ).ToList();
			if( results.Count > 0 )
			{
				foreach( var result in results )
				{
					AddError( property.Key, result );
				}
			}
		}
	}

	private void AddError( string propertyName, ValidationResult error )
	{
		if( !_errors.ContainsKey( propertyName ) )
		{
			_errors[propertyName] = new List<ValidationResult>();
		}

		_errors[propertyName].Add( error );
		OnErrorsChanged( propertyName );
	}

	private static PropertyInfo[] GetProperties<T>( T type ) where T : Type
	{
		// Get all properties with data annotations
		PropertyInfo[] validationProperties = (
			from propInfo in type.GetProperties( BindingFlags.Instance | BindingFlags.Public )
			where propInfo.GetIndexParameters().Length == 0 &&
				   propInfo.GetCustomAttributes<ValidationAttribute>( true ).Any()
			select propInfo ).ToArray();

		return validationProperties;
	}

	private string GetDisplayNameForProperty( string propertyName )
	{
		static Dictionary<string, string> GetDisplayNames( Type type )
		{
			Dictionary<string, string> displayNames = new();

			foreach( PropertyInfo property in type.GetProperties( BindingFlags.Instance | BindingFlags.Public ) )
			{
				if( property.GetCustomAttribute<DisplayAttribute>() is DisplayAttribute attribute &&
					attribute.GetName() is string displayName )
				{
					displayNames.Add( property.Name, displayName );
				}
			}

			return displayNames;
		}

		_ = _displayNamesMap.GetValue( _context.ObjectInstance.GetType(),
				static t => GetDisplayNames( t ) ).TryGetValue( propertyName, out string? displayName );

		return displayName ?? propertyName;
	}

	#endregion
}
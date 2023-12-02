namespace Common.Core.Converters;

/// <summary>Helper methods to convert data types to other data types.</summary>
public static class Generic
{
	/// <summary>Safe converting to any type.</summary>
	/// <typeparam name="T">Type of object to convert.</typeparam>
	/// <param name="value">Value to try and convert.</param>
	/// <param name="defaultValue">Default value, sets the type of the returned value.</param>
	/// <returns>The converted value, or default if any error occurs.</returns>
	private static T? Convert<T>( object? value, T defaultValue )
	{
		try
		{
			return value is null || value is DBNull ? default :
				(T?)(T)System.Convert.ChangeType( value,
					Nullable.GetUnderlyingType( typeof( T ) ) ?? typeof( T ) );
		}
		catch { return defaultValue; }
	}

	/// <summary>Converts a char object to a 3-state boolean type.</summary>
	/// <param name="value">Value to try and convert.</param>
	/// <returns>Null is returned if the value could not be converted,
	/// true if the value is Y, y, or 1, otherwise false.</returns>
	public static bool? CharToBool( object value )
	{
		char? data = Convert<char?>( value, null );
		if( data is null ) {  return null; }
		string strVal = data.ToString()!;
		bool ok = StringConverter.TryParse( ref strVal, out bool result );
		return ok ? result : null;
	}

	/// <summary>Converts a DateTime object to a DateOnly type.</summary>
	/// <param name="value">Value to try and convert.</param>
	/// <returns>Null is returned if the value could not be converted.</returns>
	public static DateOnly? DateTimeToDateOnly( object value )
	{
		DateTime? data = Convert<DateTime?>( value, null );
		if( data is null ) { return null; }
		return DateOnly.FromDateTime( data.Value );
	}
}

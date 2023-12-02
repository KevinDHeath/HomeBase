using System.Globalization;

namespace Common.Core.Converters;

// https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles

/// <summary>Helper class to convert strings to other data types.</summary>
public static class StringConverter
{
	/// <summary>Tries to convert the specified string to its System.Boolean equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out bool result )
	{
		value = value.Trim().ToLower();
		if( value.Length == 1 )
		{
			// Check for 1, 0, Y, N values
			if( value == "0" || value == "n" ) { value = bool.FalseString; }
			else if( value == "1" || value == "y" ) { value = bool.TrueString; }
		}
		else if( value.Length <= 3 )
		{
			// Check for Yes, No values
			if( value == "no" ) { value = bool.FalseString; }
			else if( value == "yes" ) { value = bool.TrueString; }
		}

		return bool.TryParse( value, out result );
	}

	/// <summary>Tries to convert the specified string to its System.Byte equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out byte result, CultureInfo? culture = null )
	{
		return byte.TryParse( value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite,
			culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.DateOnly equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out DateOnly result, CultureInfo? culture = null )
	{
		return DateOnly.TryParse( value, culture, DateTimeStyles.None, out result );
	}

	/// <summary>Tries to convert the specified string to its System.DateTime equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out DateTime result, CultureInfo? culture = null )
	{
		return DateTime.TryParse( value, culture, DateTimeStyles.None, out result );
	}

	/// <summary>Tries to convert the specified string to its System.DateTimeOffset equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out DateTimeOffset result, CultureInfo? culture = null )
	{
		return DateTimeOffset.TryParse( value, culture, DateTimeStyles.None, out result );
	}

	/// <summary>Tries to convert the specified string to its System.Decimal equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out decimal result, CultureInfo? culture = null )
	{
		return decimal.TryParse( value, NumberStyles.Currency, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.Double equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out double result, CultureInfo? culture = null )
	{
		result = 0;
		if( string.IsNullOrWhiteSpace( value ) ) { return false; }

		return double.TryParse( value, NumberStyles.Number, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.Single (float) equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out float result, CultureInfo? culture = null )
	{
		result = 0;
		if( string.IsNullOrWhiteSpace( value ) ) { return false; }

		return float.TryParse( value, NumberStyles.Number, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.Guid equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out Guid result )
	{
		result = Guid.Empty;
		if( string.IsNullOrWhiteSpace( value ) ) { return false; }

		return Guid.TryParse( value, out result );
	}

	/// <summary>Tries to convert the specified string to its System.Int32 (integer) equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out int result, CultureInfo? culture = null )
	{
		return int.TryParse( value, NumberStyles.Integer | NumberStyles.AllowThousands |
			NumberStyles.AllowTrailingSign, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.Int64 (long) equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out long result, CultureInfo? culture = null )
	{
		return long.TryParse( value, NumberStyles.Integer | NumberStyles.AllowThousands |
			NumberStyles.AllowTrailingSign, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.SByte equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out sbyte result, CultureInfo? culture = null )
	{
		return sbyte.TryParse( value, NumberStyles.None, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.Int16 (short) equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out short result, CultureInfo? culture = null )
	{
		return short.TryParse( value, NumberStyles.Integer | NumberStyles.AllowThousands |
			NumberStyles.AllowTrailingSign, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.TimeOnly equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out TimeOnly result, CultureInfo? culture = null )
	{
		return TimeOnly.TryParse( value, culture, DateTimeStyles.None, out result );
	}

	/// <summary>Tries to convert the specified string to its System.TimeSpan equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out TimeSpan result, CultureInfo? culture = null )
	{
		value = value.Trim();
		if( value.EndsWith( "-" ) ) // Handle trailing negative sign
		{ value = string.Concat( "-", value.AsSpan( 0, value.Length - 1 ) ); }

		return TimeSpan.TryParse( value, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.UInt32 (unsigned integer) equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out uint result, CultureInfo? culture = null )
	{
		return uint.TryParse( value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite |
			NumberStyles.AllowThousands, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.UInt64 (unsigned long) equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out ulong result, CultureInfo? culture = null )
	{
		return ulong.TryParse( value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite |
			NumberStyles.AllowThousands, culture, out result );
	}

	/// <summary>Tries to convert the specified string to its System.UInt16 (unsigned short) equivalent.</summary>
	/// <param name="value">A string containing the value to convert.</param>
	/// <param name="result">If the conversion succeeded, contains the value.</param>
	/// <param name="culture">An object that provides culture-specific formatting information.</param>
	/// <returns>True if value was converted successfully.</returns>
	public static bool TryParse( ref string value, out ushort result, CultureInfo? culture = null )
	{
		return ushort.TryParse( value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite |
			NumberStyles.AllowThousands, culture, out result );
	}
}
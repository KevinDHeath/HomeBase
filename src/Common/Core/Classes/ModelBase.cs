using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.Core.Classes;

/// <summary>Base class for models that require the INotifyPropertyChanged interface.</summary>
public abstract class ModelBase : INotifyPropertyChanged
{
	#region INotifyPropertyChanged Implementation

	/// <summary>Method that will handle the PropertyChanged event raised when a property is
	/// changed on a component.</summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>Invoked whenever the effective value has been updated.</summary>
	/// <param name="property">Name of the property that has changed.</param>
	public void OnPropertyChanged( [CallerMemberName] string property = "" )
	{
		PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( property ) );
	}

	#endregion

	#region Public Methods

	/// <summary>Calculate the current age based on a DateTime value.</summary>
	/// <param name="date">Date to use.</param>
	/// <returns>Null is returned if no date is provided.</returns>
	public static int? CalculateAge( DateTime? date )
	{
		// https://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-based-on-a-datetime-type-birthday
		return date.HasValue ? (int)( ( DateTime.Now - date.Value ).TotalDays / 365.242199 ) : null;
	}

	/// <summary>Calculate the current age based on a DateOnly value.</summary>
	/// <param name="date">Date to use.</param>
	/// <returns>Null is returned if no date is provided.</returns>
	public static int? CalculateAge( DateOnly? date )
	{
		return date.HasValue ? CalculateAge( date.Value.ToDateTime( TimeOnly.MinValue ) ) : null;
	}

	/// <summary>Sets a string as null if the length is zero.</summary>
	/// <param name="value">Value to check.</param>
	/// <returns>Null is returned if the string length is zero.</returns>
	public static string? SetNullString( string? value )
	{
		return value is not null && value.Length > 0 ? value : null;
	}

	#endregion
}
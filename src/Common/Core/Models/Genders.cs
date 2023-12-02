namespace Common.Core.Models;

/// <summary>Genders of a person.</summary>
public enum Genders
{
	/// <summary>Unknown</summary>
	Unknown,
	/// <summary>Male.</summary>
	Male,
	/// <summary>Female.</summary>
	Female,
	/// <summary>Differs from the identity assigned to their physical sex.</summary>
	Trans,
	/// <summary>Does not conform to the gender binary.</summary>
	NonBinary,
	/// <summary>Does not identify as male or female but rather as one or the other depending on the day.</summary>
	Fluid,
	/// <summary>Not relating or specific to people of one particular gender.</summary>
	Neutral
};
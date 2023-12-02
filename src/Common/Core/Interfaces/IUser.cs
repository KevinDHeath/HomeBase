using Common.Core.Models;

namespace Common.Core.Interfaces;

/// <summary>Interface for a User implementation class.</summary>
public interface IUser
{
	#region Properties

	/// <summary>Gets or sets the unique identifier.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the User Name.</summary>
	string? Name { get; set; }

	/// <summary>Gets or sets the Age.</summary>
	int? Age { get; set; }

	/// <summary>Gets or sets the Date of Birth.</summary>
	DateOnly? BirthDate { get; set; }

	/// <summary>Gets or sets the E-mail address.</summary>
	string? Email { get; set; }

	/// <summary>Gets or sets the Gender.</summary>
	Genders Gender { get; set; }

	#endregion
}
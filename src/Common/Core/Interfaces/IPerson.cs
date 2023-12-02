using System.Text.Json.Serialization;
using Common.Core.Models;

namespace Common.Core.Interfaces;

/// <summary>Interface for a Person implementation class.</summary>
public interface IPerson
{
	#region Properties

	/// <summary>Gets or sets the unique identifier.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the First Name.</summary>
	string FirstName { get; set; }

	/// <summary>Gets or sets the Middle Name.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? MiddleName { get; set; }

	/// <summary>Gets or sets the Last Name.</summary>
	string LastName { get; set; }

	/// <summary>Gets or sets the Primary Address.</summary>
	Address Address { get; set; }

	/// <summary>Gets or sets the Government Number.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? GovernmentNumber { get; set; }

	/// <summary>Gets or sets the Identification State.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? IdState { get; set; }

	/// <summary>Gets or sets the Identification Number.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? IdNumber { get; set; }

	/// <summary>Gets or sets the Home Phone Number.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? HomePhone { get; set; }

	/// <summary>Gets or sets the Date of Birth.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	DateOnly? BirthDate { get; set; }

	/// <summary>Gets the Full Name.</summary>
	[JsonIgnore]
	string FullName { get; }

	#endregion
}
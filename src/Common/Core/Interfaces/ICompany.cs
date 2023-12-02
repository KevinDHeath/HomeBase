using System.Text.Json.Serialization;
using Common.Core.Models;

namespace Common.Core.Interfaces;

/// <summary>Interface for a Company implementation class.</summary>
public interface ICompany
{
	#region Properties

	/// <summary>Gets or sets the unique identifier.</summary>
	int Id { get; set; }

	/// <summary>Gets or sets the Company Name.</summary>
	string Name { get; set; }

	/// <summary>Gets or sets the Primary Address.</summary>
	Address Address { get; set; }

	/// <summary>Gets or sets the Government Number.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? GovernmentNumber { get; set; }

	/// <summary>Gets or sets the Primary Phone Number.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? PrimaryPhone { get; set; }

	/// <summary>Gets or sets the Secondary Phone Number.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? SecondaryPhone { get; set; }

	/// <summary>Gets or sets the Primary Email.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? Email { get; set; }

	/// <summary>Gets or sets the NAICS Code.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? NaicsCode { get; set; }

	/// <summary>Gets or sets the Private Company indicator.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	bool? Private { get; set; }

	/// <summary>Gets or sets the number of Deposit Accounts.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	int? DepositsCount { get; set; }

	/// <summary>Gets or sets the Deposit Accounts Balance.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	decimal? DepositsBal { get; set; }

	#endregion
}
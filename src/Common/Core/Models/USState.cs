using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;
using Common.Core.Classes;

namespace Common.Core.Models;

/// <summary>US State details.</summary>
public class USState : ModelData
{
	#region Properties

	/// <inheritdoc/>
	public override int Id { get; set; }

	/// <summary>2-digit US Postal Service State abbreviation.</summary>
	[MaxLength( 2 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Alpha { get; set; }

	/// <summary>State name.</summary>
	[Required]
	[MaxLength( 50 )]
	public string Name { get; set; } = string.Empty;

	/// <summary>State capital.</summary>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Capital { get; set; }

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the USState class.</summary>
	public USState()
	{ }

	#endregion

	#region Public Methods

	/// <summary>Builds a USState object from a database table row.</summary>
	/// <param name="row">Database row containing the USState columns.</param>
	/// <returns>USState object containing the database values.</returns>
	/// <remarks>This method assumes that the table column names are the same as the property names.</remarks>
	public static USState Read( DataRow row )
	{
		return new USState()
		{
			Id = row.Field<int>( nameof( Id ) ),
			Alpha = row.Field<string?>( nameof( Alpha ) ),
			Name = row.Field<string>( nameof( Name ) )!,
			Capital = row.Field<string?>( nameof( Capital ) )
		};
	}

	#endregion
}
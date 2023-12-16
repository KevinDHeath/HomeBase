using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;
using Common.Core.Classes;

namespace Common.Core.Models;

/// <summary>Province details.</summary>
public class Province : ModelData
{
	/// <inheritdoc/>
	public override int Id { get; set; }

	/// <summary>Province code.</summary>
	[MaxLength( 10 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Code { get; set; }

	/// <summary>Province name.</summary>
	[Required]
	[MaxLength( 50 )]
	public string Name { get; set; } = string.Empty;

	/// <summary>Initializes a new instance of the Province class.</summary>
	public Province()
	{ }

	/// <summary>Builds a Province object from a database table row.</summary>
	/// <param name="row">Database row containing the Province columns.</param>
	/// <returns>Province object containing the database values.</returns>
	/// <remarks>This method assumes that the table column names are the same as the property names.</remarks>
	public static Province Read( DataRow row )
	{
		return new Province()
		{
			Id = row.Field<int>( nameof( Id ) ),
			Code = row.Field<string?>( nameof( Code ) ),
			Name = row.Field<string>( nameof( Name ) )!
		};
	}
}
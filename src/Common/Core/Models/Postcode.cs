using System.ComponentModel.DataAnnotations;
using System.Data;
using Common.Core.Classes;

namespace Common.Core.Models;

/// <summary>Post code details.</summary>
public class Postcode : ModelData
{
	/// <inheritdoc/>
	public override int Id { get; set; }

	/// <summary>Gets or sets the Postal Service code.</summary>
	[Required]
	[MaxLength( 10 )]
	public string Code { get; set; } = string.Empty;

	/// <summary>Gets or sets the Province code.</summary>
	[Required]
	[MaxLength( 10 )]
	public string Province { get; set; } = string.Empty;

	/// <summary>Gets or sets the County name.</summary>
	[MaxLength( 50 )]
	public string? County { get; set; }

	/// <summary>Gets or sets the City name.</summary>
	[MaxLength( 50 )]
	public string? City { get; set; }

	/// <summary>Initializes a new instance of the Postcode class.</summary>
	public Postcode()
	{ }

	/// <summary>Builds a Postcode object from a database table row.</summary>
	/// <param name="row">Database row containing the Postcode columns.</param>
	/// <returns>Postcode object containing the database values.</returns>
	/// <remarks>This method assumes that the table column names are the same as the property names.</remarks>
	public static Postcode Read( DataRow row )
	{
		return new Postcode()
		{
			Id = row.Field<int>( nameof( Id ) ),
			Code = row.Field<string>( nameof( Code ) )!,
			Province = row.Field<string>( nameof( Province ) )!,
			County = row.Field<string>( nameof( County ) )!,
			City = row.Field<string>( nameof( City ) )!
		};
	}
}
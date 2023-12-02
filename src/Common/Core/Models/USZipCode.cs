using System.ComponentModel.DataAnnotations;
using System.Data;
using Common.Core.Classes;

namespace Common.Core.Models;

/// <summary>US Zip code details.</summary>
public class USZipCode : ModelData
{
	#region Properties

	/// <inheritdoc/>
	public override int Id { get; set; }

	/// <summary>Gets or sets the City name.</summary>
	[Required]
	[MaxLength( 50 )]
	public string City { get; set; } = string.Empty;

	/// <summary>Gets or sets the County name.</summary>
	[Required]
	[MaxLength( 50 )]
	public string County { get; set; } = string.Empty;

	/// <summary>Gets or sets the 2-digit US Postal Service State abbreviation.</summary>
	[Required]
	[MaxLength( 2 )]
	public string State { get; set; } = string.Empty;

	/// <summary>Gets or sets the 5-digit US Postal Service Zip code.</summary>
	[Required]
	[MaxLength( 5 )]
	public string ZipCode { get; set; } = string.Empty;

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the USZipCode class.</summary>
	public USZipCode()
	{ }

	#endregion

	#region Public Methods

	/// <summary>Builds a USZipCode object from a database table row.</summary>
	/// <param name="row">Database row containing the USZipCode columns.</param>
	/// <returns>USZipCode object containing the database values.</returns>
	/// <remarks>This method assumes that the table column names are the same as the property names.</remarks>
	public static USZipCode Read( DataRow row )
	{
		return new USZipCode()
		{
			Id = row.Field<int>( nameof( Id ) ),
			City = row.Field<string>( nameof( City ) )!,
			County = row.Field<string>( nameof( County ) )!,
			State = row.Field<string>( nameof( State ) )!,
			ZipCode = row.Field<string>( nameof( ZipCode ) )!
		};
	}

	#endregion
}
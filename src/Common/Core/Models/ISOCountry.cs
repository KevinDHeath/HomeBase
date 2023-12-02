using System.ComponentModel.DataAnnotations;
using System.Data;
using Common.Core.Classes;

namespace Common.Core.Models;

/// <summary>ISO-3166 Country details.</summary>
public class ISOCountry : ModelData
{
	#region Properties

	/// <inheritdoc/>
	public override int Id { get; set; }

	/// <summary>2-digit ISO-3166 code.</summary>
	[Required]
	[MaxLength( 2 )]
	public string Alpha2 { get; set; } = string.Empty;

	/// <summary>3-digit ISO-3166 code.</summary>
	[Required]
	[MaxLength( 3 )]
	public string Alpha3 { get; set; } = string.Empty;

	/// <summary>Country name.</summary>
	[Required]
	[MaxLength( 50 )]
	public string Name { get; set; } = string.Empty;

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the ISOCountry class.</summary>
	public ISOCountry()
	{ }

	#endregion

	#region Public Methods

	/// <summary>Builds an ISOCountry object from a database table row.</summary>
	/// <param name="row">Database row containing the ISOCountry columns.</param>
	/// <returns>ISOCountry object containing the database values.</returns>
	/// <remarks>This method assumes that the table column names are the same as the property names.</remarks>
	public static ISOCountry Read( DataRow row )
	{
		return new ISOCountry()
		{
			Id = row.Field<int>( nameof( Id ) ),
			Alpha2 = row.Field<string>( nameof( Alpha2 ) )!,
			Alpha3 = row.Field<string>( nameof( Alpha3 ) )!,
			Name = row.Field<string>( nameof( Name ) )!
		};
	}

	#endregion
}
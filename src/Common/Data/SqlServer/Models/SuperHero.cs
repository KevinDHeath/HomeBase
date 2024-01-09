using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Common.Core.Classes;

namespace Common.Data.SqlServer.Models;

/// <summary>This class contains details of a Super Hero.</summary>
public class SuperHero : ModelData
{
	#region Publishers

	/// <summary>Super Hero publishers.</summary>
	[JsonConverter( typeof( JsonStringEnumConverter<Publishers> ) )]
	public enum Publishers
	{
		/// <summary>DC comics.</summary>
		DC = 0,
		/// <summary>Marvel comics.</summary>
		Marvel
	}

	#endregion

	#region Properties

	/// <summary>Gets or sets the primary key.</summary>
	public override int Id { get; set; }

	/// <summary>Super Hero name.</summary>
	[Required]
	[MaxLength( 50 )]
	public string Name { get; set; }

	/// <summary>Super Hero identity first name.</summary>
	[Required]
	[MaxLength( 50 )]
	public string FirstName { get; set; }

	/// <summary>Super Hero identity last name.</summary>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? LastName { get; set; }

	/// <summary>Place where the Super Hero resides.</summary>
	[MaxLength( 50 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Place { get; set; }

	/// <summary>Comics publisher.</summary>
	public Publishers Publisher { get; set; }

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the SuperHero class.</summary>
	public SuperHero()
	{
		Name = string.Empty;
		FirstName = string.Empty;
	}

	#endregion
}

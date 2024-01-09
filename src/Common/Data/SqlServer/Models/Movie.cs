using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Common.Core.Classes;

namespace Common.Data.SqlServer.Models;

/// <summary>This class contains details of a Movie.</summary>
public class Movie : ModelData
{
	#region Properties

	/// <summary>Gets or sets the primary key.</summary>
	public override int Id { get; set; }

	/// <summary>Movie title.</summary>
	[Required]
	[MaxLength( 80 )]
	public string Title { get; set; }

	/// <summary>Year the movie was released.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public int? ReleaseYear { get; set; }

	/// <summary>Comma separated list of genres.</summary>
	[MaxLength( 100 )]
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Genre { get; set; }

	/// <summary>The duration in minutes.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public int? Duration { get; set; }

	#endregion

	#region Constructor

	/// <summary>Initializes a new instance of the Movie class.</summary>
	public Movie()
	{
		Title = string.Empty;
	}

	#endregion
}
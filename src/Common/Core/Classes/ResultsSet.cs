using System.Text.Json.Serialization;

namespace Common.Core.Classes;

/// <summary>Class that contains the properties of a data result set.</summary>
/// <typeparam name="T">Generic class or interface.</typeparam>
public class ResultsSet<T> where T : class
{
	/// <summary>Gets or sets the total count of the results.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public int? Total { get; set; }

	/// <summary>Gets or sets the previous start position.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Previous { get; set; }

	/// <summary>Gets or sets the next start position.</summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	public string? Next { get; set; }

	/// <summary>Gets or sets the results collection.</summary>
	public ICollection<T> Results { get; set; } = new List<T>();

	/// <summary>Gets the maximum numbers of results to return.</summary>
	[JsonIgnore]
	public int Max { get; private set; } = 10;

	#region Constructors

	/// <summary>Initializes a new instance of the ResultsSet class.</summary>
	public ResultsSet()
	{ }

	/// <summary>Initializes a new instance of the ResultsSet class.</summary>
	/// <param name="max">Maximum numbers of results to return.</param>
	public ResultsSet( int? max = 0 )
	{
		if( max is not null and > 0 ) { Max = max.Value; }
	}

	#endregion
}
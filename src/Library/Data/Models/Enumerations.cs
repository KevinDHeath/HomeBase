namespace Library.Data.Models;

/// <summary>Enumeration of categories.</summary>
public enum Categories
{
	/// <summary>Reviewed item.</summary>
	Reviewed,
	/// <summary>Popular item.</summary>
	Popular,
	/// <summary>New item.</summary>
	New,
	/// <summary>Merge item.</summary>
	Merge
}

/// <summary>Enumeration helpers.</summary>
public class Enumerations
{
	private static readonly IOrderedEnumerable<Categories> noMerge = ExcludeMerge().OrderBy( v => v.ToString() );
	private static readonly IOrderedEnumerable<Categories> mergeOnly = MergeOnly().OrderBy( v => v.ToString() );

	/// <summary>Gets a collection of categories.</summary>
	/// <param name="status">Categories to return, Merge if item can be merged, anything else for normal item.</param>
	/// <returns>Collection of category values.</returns>
	public static IEnumerable<Categories> GetCategories( Categories status ) => status == Categories.Merge ? mergeOnly : noMerge;

	internal static Categories ConvertCategory( string? value )
	{
		if( value is null ) { return Categories.Reviewed; }
		return value.ToLower() switch
		{
			"hot" => Categories.Popular,
			"new" => Categories.New,
			"merge" => Categories.Merge,
			_ => Categories.Reviewed,
		};
	}

	internal static string? ConvertCategory( Categories value )
	{
		return value switch
		{
			Categories.Reviewed => null,
			Categories.Popular => "hot",
			_ => value.ToString().ToLower(),
		};
	}

	private static Categories[] ExcludeMerge()
	{
		Categories[] values = (Categories[])Enum.GetValues( typeof( Categories ) );
		int idx = Array.IndexOf( values, Categories.Merge );
		if( idx < 0 ) { return values; }

		Categories[] temp = new Categories[values.Length - 1];
		Array.Copy( values, 0, temp, 0, idx );
		Array.Copy( values, idx + 1, temp, idx, values.Length - idx - 1 );

		return temp;
	}

	private static Categories[] MergeOnly() => [Categories.Merge];
}
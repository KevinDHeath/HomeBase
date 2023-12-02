namespace EFCore.RestApi.Services;

internal static class BaseService
{
	private const string cSize = @"?count=";
	private const string cLast = "&last=";

	internal static void ApplyPaging<T>( ResultsSet<T> set, int last, string uri ) where T : ModelData
	{
		if( last < 0 ) { last = 0; }
		if( last - set.Max >= 0 )
		{
			set.Previous = $"{uri}{cSize}{set.Max}";
			set.Previous += $"{cLast}{last - set.Max}";
		}

		if( set.Results.Count == set.Max )
		{
			set.Next = $"{uri}{cSize}{set.Max}";
			set.Next += $"{cLast}{set.Results.LastOrDefault()?.Id}";
		}
	}

	internal static string GetRequestUri( HttpRequest rq )
	{
		return $"{rq.Scheme}://{rq.Host}{rq.PathBase}{rq.Path}";
	}
}
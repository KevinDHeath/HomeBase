using System.Text.Json;
using Common.Core.Classes;

namespace Common.Data.Api;

/// <summary>Base class to provide common REST API access functionality.</summary>
/// <remarks>This uses endpoint strings contained in a Json application settings file.<br/>
/// The default file name is "appsettings.json".
/// <code language="JSON">
/// {
///   "Endpoints": {
///     "CommonData": "[scheme]://[host][:port]/api/"
///   }
/// }
/// </code>
/// </remarks>
public abstract class Factory : DataFactoryBase
{
	internal const string cEndpointKey = "CommonData";
	private static string? endpointSection = "Endpoints";

	#region Constructor and Initialization

	private static DataServiceBase? _service;

	/// <summary>REST API service for consuming JSON data.</summary>
	protected static DataServiceBase? ServiceBase => _service;

	/// <summary>Initializes a new instance of the Factory class.</summary>
	/// <param name="endpointName">The connection string settings name.</param>
	/// <param name="configFile">The name of the configuration file.</param>
	protected Factory( string endpointName, string configFile = "" )
	{
		if( string.IsNullOrWhiteSpace( configFile ) ) { configFile = cConfigFile; }
		_service = GetDataService( endpointName, ref configFile );
	}

	internal static DataServiceBase? GetDataService( string endpointName, ref string configFile )
	{
		if( _service is not null ) { return _service; }
		if( string.IsNullOrWhiteSpace( endpointName ) ) { return null; }

		Dictionary<string, string?> config = JsonHelper.ReadAppSettings( ref configFile, ref endpointSection );
		config.TryGetValue( endpointName, out string? setting );
		if( !string.IsNullOrWhiteSpace( setting ) )
		{
			if( Uri.TryCreate( setting, UriKind.Absolute, out var uri ) )
			{
				DataServiceBase rtn = new( uri.AbsoluteUri );
				return rtn;
			}
		}
		return null;
	}

	#endregion

	internal static int GetRowCount<T>( string resource, JsonSerializerOptions options ) where T : class
	{
		string uri = $"{resource}?count=1";
		string? json = _service?.GetResource( uri );
		if( json is not null )
		{
			ResultsSet<T>? obj = JsonHelper.DeserializeJson<ResultsSet<T>>( ref json, options );
			if( obj is not null && obj.Total is not null ) { return obj.Total.Value; }
		}
		return 0;
	}

	internal static IList<T> GetResource<T>( string resource, JsonSerializerOptions options,
		int count, int total ) where T : class
	{
		string uri = resource + $"?count={count}&last={GetStartIndex( total, count )}";
		string? json = _service?.GetResource( uri );
		if( json is not null )
		{
			ResultsSet<T>? obj = JsonHelper.DeserializeJson<ResultsSet<T>>( ref json, options );
			if( obj is not null ) { return obj.Results.ToList(); }
		}
		return new List<T>();
	}

	internal static bool PutResource<T>( string resourceName, int Id, T data,
		JsonSerializerOptions? options = null ) where T : class
	{
		string uri = resourceName + $"/{Id}";
		T? rtn = _service?.PutResource( uri, data, options );
		return rtn is not null;
	}
}
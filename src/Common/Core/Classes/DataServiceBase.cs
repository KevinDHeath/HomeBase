using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Common.Core.Classes;

/// <summary>Base class for services consuming data from a RESTful API.</summary>
public class DataServiceBase
{
	private readonly HttpClient _httpClient;
	private readonly string _mediaType = "application/json";

	#region Constructor and Initialization

	/// <summary>Initializes a new instance of the DataServiceBase class.</summary>
	/// <param name="baseUri">Base Uniform Resource Identifier.</param>
	public DataServiceBase( string baseUri )
	{
		_httpClient = CreateSharedClient( baseUri );
		_httpClient.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( _mediaType ) );
	}

	#endregion

	/// <summary>Creates a resource.</summary>
	/// <typeparam name="T">Generic class or interface.</typeparam>
	/// <param name="uri">Uniform Resource Identifier.</param>
	/// <param name="obj">Resource to create.</param>
	/// <param name="options">Json serializer options.</param>
	/// <returns>The created resource, or null if there were errors while processing the request.</returns>
	public T? PostResource<T>( string uri, T obj, JsonSerializerOptions? options = null ) where T : class
	{
		string? json = JsonHelper.Serialize( obj );
		if( json is null ) { return null; }

		CheckUri( ref uri );
		try
		{
			HttpRequestMessage rq = new( HttpMethod.Post, uri )
			{
				Content = new StringContent( json, Encoding.UTF8, _mediaType )
			};
			json = GetResponseAsync( rq ).Result;
			if( !string.IsNullOrEmpty( json ) ) { return JsonHelper.DeserializeJson<T>( ref json, options ); }
		}
		catch( HttpRequestException ) { }  // Synchronous exception
		catch( AggregateException ) { } // Asynchronous exception
		return null;
	}

	/// <summary>Retrieves a resource.</summary>
	/// <param name="uri">Uniform Resource Identifier.</param>
	/// <returns>Resource as a string, or null if there were errors while processing the request.</returns>
	public string? GetResource( string uri )
	{
		CheckUri( ref uri );
		try
		{
			HttpRequestMessage rq = new( HttpMethod.Get, uri );
			return GetResponseAsync( rq ).Result;
		}
		catch( HttpRequestException ) { }  // Synchronous exception
		catch( AggregateException ) { } // Asynchronous exception
		return null;
	}

	/// <summary>Updates a resource.</summary>
	/// <typeparam name="T">Generic class or interface.</typeparam>
	/// <param name="uri">Uniform Resource Identifier.</param>
	/// <param name="obj">Resource to update.</param>
	/// <param name="options">Json serializer options.</param>
	/// <returns>The updated resource, or null if there were errors while processing the request.</returns>
	public T? PutResource<T>( string uri, T obj, JsonSerializerOptions? options = null ) where T : class
	{
		string? json = JsonHelper.Serialize( obj, options );
		if( json is null ) { return null; }

		CheckUri( ref uri );
		try
		{
			HttpRequestMessage rq = new( HttpMethod.Put, uri )
			{
				Content = new StringContent( json, Encoding.UTF8, _mediaType )
			};
			json = GetResponseAsync( rq ).Result;
			if( !string.IsNullOrEmpty( json ) ) { return JsonHelper.DeserializeJson<T>( ref json, options ); }
		}
		catch( HttpRequestException ) { }  // Synchronous exception
		catch( AggregateException ) { } // Asynchronous exception
		return null;
	}

	/// <summary>Deletes a resource.</summary>
	/// <typeparam name="T">Generic class or interface.</typeparam>
	/// <param name="uri">Uniform Resource Identifier.</param>
	/// <param name="options">Json serializer options.</param>
	/// <returns>The deleted resource, or null if there were errors while processing the request.</returns>
	public T? DeleteResource<T>( string uri, JsonSerializerOptions? options = null ) where T : class
	{
		CheckUri( ref uri );
		try
		{
			HttpRequestMessage rq = new( HttpMethod.Delete, uri );
			string json = GetResponseAsync( rq ).Result;
			if( !string.IsNullOrEmpty( json ) ) { return JsonHelper.DeserializeJson<T>( ref json, options ); }
		}
		catch( HttpRequestException ) { }  // Synchronous exception
		catch( AggregateException ) { } // Asynchronous exception
		return null;
	}

	#region Private Methods

	private static HttpClient CreateSharedClient( string siteUri )
	{
		SocketsHttpHandler handler = new()
		{
			PooledConnectionLifetime = TimeSpan.FromMinutes( 15 ) // Recreate every 15 minutes
		};

		if( siteUri.Contains( '\\' ) ) { siteUri = siteUri.Replace( '\\', '/' ); }
		if( !siteUri.EndsWith( '/' ) ) { siteUri += "/"; }

		return new HttpClient( handler ) { BaseAddress = new Uri( siteUri ) };
	}

	private void CheckUri( ref string uri )
	{
		if( !uri.ToLowerInvariant().StartsWith( Uri.UriSchemeHttp ) )
		{
			uri = _httpClient.BaseAddress?.OriginalString + uri;
		}
	}

	// https://devblogs.microsoft.com/dotnet/configureawait-faq/
	private async Task<string> GetResponseAsync( HttpRequestMessage request )
	{
		HttpResponseMessage response = await _httpClient.SendAsync( request ).ConfigureAwait( false );
		_ = response.EnsureSuccessStatusCode();

		using StreamReader reader = new( response.Content.ReadAsStream() );
		return await reader.ReadToEndAsync().ConfigureAwait( false );
	}

	#endregion
}
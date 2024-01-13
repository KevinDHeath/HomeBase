using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Common.Core.Interfaces;
using Common.Core.Models;
using MVVM.Core.Models;

namespace MVVM.Core.Stores;

/// <summary>Companies storage.</summary>
public class CompaniesStore : Companies
{
	#region Properties

	internal ObservableCollection<ICompany> Companies { get; private set; }

	/// <summary>Gets the total number of companies available.</summary>
	public int Count => _context.Companies.Count();

	internal ICompany? CurrentCompany
	{
		get => _currentCompany;
		set
		{
			_currentCompany = value;
			CurrentCompanyChanged?.Invoke();
		}
	}

	internal string DataPath => !string.IsNullOrWhiteSpace( _dataFolder ) ? _dataFolder : Path.GetTempPath();

	#endregion

	#region Public Event

	/// <summary>Occurs when the current company changes.</summary>
	public event Action? CurrentCompanyChanged;

	#endregion

	#region Constructor and Variables

	private readonly EntityContextBase _context;
	private ICompany? _currentCompany;
	private string? _dataFolder;
	private bool _useExternal;

	/// <summary>Initializes a new instance of the CompaniesStore class.</summary>
	/// <param name="settingsStore">Settings storage.</param>
	/// <param name="context">Database context.</param>
	public CompaniesStore( SettingsStore settingsStore, EntityContextBase context )
	{
		settingsStore.SettingsChanged += SettingsPropertyChanged;
		_useExternal = settingsStore.CurrentSettings?.UseExternal ?? false;
		_context = context;

		Initialize( settingsStore.CurrentSettings );
		Companies ??= [];
	}

	#endregion

	#region Internal Methods

	internal void AddCompany( int max = 0 )
	{
		foreach( var company in Get( max ) )
		{
			Companies.Add( company );
		}
	}

	internal void UpdateCompany( ICompany company )
	{
		Company? res = null;
		if( CurrentCompany is not null && CurrentCompany is Company cur )
		{
			if( !_useExternal ) { res = Update( CurrentCompany.Id, company ).Result; }
			if( res is not null ) { cur.Update( company ); }
		}
	}

	internal bool Export( string? folder, string? fileName )
	{
		if( Companies.Count == 0 ) return false;

		if( !string.IsNullOrWhiteSpace( folder ) && !string.IsNullOrWhiteSpace( fileName ) )
		{
			return Serialize( folder, fileName, new List<ICompany>( Companies ) );
		}

		return false;
	}

	#endregion

	#region Private Methods

	private new List<ICompany> Get( int max = 0 )
	{
		if( max > 0 )
		{
			int start = GetStartIndex( Count, max );
			return [.. _context.Companies.Where( p => p.Id > start && p.Id <= start + max )];
		}

		List<ICompany> rtn = [];
		return rtn;
	}

	private void Initialize( Settings? settings )
	{
		_dataFolder = settings?.ExternalData;
		var fileName = string.Empty;
		var recs = settings?.MaxCompanies ?? 1;

		if( settings is not null && _useExternal && !string.IsNullOrWhiteSpace( _dataFolder ) &&
			!string.IsNullOrWhiteSpace( settings.ExtCompanies ) )
		{
			fileName = settings.ExtCompanies;
		}

		Initialize( recs, fileName );
	}

	private void Initialize( int? recs, string? fileName = "" )
	{
		int max = recs is null ? 1 : recs.Value;
		Data.Clear();

		Companies = !string.IsNullOrWhiteSpace( _dataFolder ) && !string.IsNullOrWhiteSpace( fileName )
			? new ObservableCollection<ICompany>( Get( _dataFolder, fileName, max ) )
			: new ObservableCollection<ICompany>( Get( max ) );
	}

	private void SettingsPropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if( sender is Settings settings )
		{
			switch( e.PropertyName )
			{
				case nameof( Settings.ExtCompanies ):
				case nameof( Settings.UseExternal ):
					_useExternal = settings.UseExternal ?? false;
					Initialize( settings );
					break;
				case nameof( Settings.ExternalData ):
					_dataFolder = settings.ExternalData;
					Initialize( settings );
					break;

				default: break;
			}
		}
	}

	#endregion

	#region CRUD Operations

	/// <summary>Creates a Company.</summary>
	/// <param name="details">The Company details.</param>
	/// <returns>The created Company details.</returns>
	public async Task<Company> Create( Company details )
	{
		_context.Companies.Add( details );
		_ = await _context.SaveChangesAsync();
		return details;
	}

	/// <summary>Gets a specific Company.</summary>
	/// <param name="id">Unique Company Id.</param>
	/// <returns>The Company details.</returns>
	public async Task<Company?> Read( int id )
	{
		return await _context.Companies.Include( x => x.Address ).FirstOrDefaultAsync( x => x.Id == id );
	}

	/// <summary>Updates a specific Company.</summary>
	/// <param name="id">Unique Company Id.</param>
	/// <param name="changes">The Company detail changes.</param>
	/// <returns>The updated Company details.</returns>
	public async Task<Company?> Update( int id, ICompany changes )
	{
		Company? current = await Read( id );
		if( current is null ) { return current; }

		current.Update( changes );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	/// <summary>Deletes a Company.</summary>
	/// <param name="id">Unique Company Id.</param>
	/// <returns>The deleted Company details.</returns>
	public async Task<Company?> Delete( int id )
	{
		Company? current = await Read( id );
		if( current is null ) return null;

		_context.Companies.Remove( current );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	#endregion
}
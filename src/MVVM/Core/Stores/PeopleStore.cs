using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Common.Core.Interfaces;
using Common.Core.Models;
using MVVM.Core.Models;

namespace MVVM.Core.Stores;

/// <summary>People storage.</summary>
public class PeopleStore : People
{
	#region Properties

	internal ObservableCollection<IPerson> People { get; private set; }

	/// <summary>Gets the total number of people available.</summary>
	public int Count => _context.People.Count();

	internal IPerson? CurrentPerson
	{
		get => _currentPerson;
		set
		{
			_currentPerson = value;
			CurrentPersonChanged?.Invoke();
		}
	}

	internal string DataPath => !string.IsNullOrWhiteSpace( _dataFolder ) ? _dataFolder : Path.GetTempPath();

	#endregion

	#region Public Event

	/// <summary>Occurs when the current person changes.</summary>
	public event Action? CurrentPersonChanged;

	#endregion

	#region Constructor and Variables

	private readonly EntityContextBase _context;
	private IPerson? _currentPerson;
	private string? _dataFolder;
	private bool _useExternal;

	/// <summary>Initializes a new instance of the PeopleStore class.</summary>
	/// <param name="settingsStore">Settings storage.</param>
	/// <param name="context">Database context.</param>
	public PeopleStore( SettingsStore settingsStore, EntityContextBase context )
	{
		settingsStore.SettingsChanged += SettingsPropertyChanged;
		_useExternal = settingsStore.CurrentSettings?.UseExternal ?? false;
		_context = context;

		Initialize( settingsStore.CurrentSettings );
		People ??= [];
	}

	#endregion

	#region Internal Methods

	internal void AddPerson( int max = 0 )
	{
		foreach( var person in Get( max ) )
		{
			People.Add( person );
		}
	}

	internal void UpdatePerson( IPerson person )
	{
		Person? res = null;
		if( CurrentPerson is not null && CurrentPerson is Person cur )
		{
			if( !_useExternal ) { res = Update( CurrentPerson.Id, person ).Result; }
			if( res is not null ) { cur.Update( person ); }
		}
	}

	internal bool Export( string? folder, string? fileName )
	{
		if( People.Count == 0 ) return false;

		if( !string.IsNullOrWhiteSpace( folder ) && !string.IsNullOrWhiteSpace( fileName ) )
		{
			Serialize( folder, fileName, new List<IPerson>( People ) );
		}

		return false;
	}

	#endregion

	#region Private Methods

	private List<IPerson> Get( int max = 0 )
	{
		if( max > 0 )
		{
			int start = GetStartIndex( Count, max );
			return [.. _context.People.Where( p => p.Id > start && p.Id <= start + max )];
		}

		List<IPerson> rtn = [];
		return rtn;
	}

	private void Initialize( Settings? settings )
	{
		_dataFolder = settings?.ExternalData;
		var fileName = string.Empty;
		var recs = settings?.MaxPeople ?? 1;

		if( settings is not null && _useExternal && !string.IsNullOrWhiteSpace( _dataFolder ) &&
			!string.IsNullOrWhiteSpace( settings.ExtPeople ) )
		{
			fileName = settings.ExtPeople;
		}

		Initialize( recs, fileName );
	}

	private void Initialize( int? recs, string? fileName = "" )
	{
		int max = recs is null ? 1 : recs.Value;
		Data.Clear();

		People = !string.IsNullOrWhiteSpace( _dataFolder ) && !string.IsNullOrWhiteSpace( fileName )
			? new ObservableCollection<IPerson>( Get( _dataFolder, fileName, max ) )
			: new ObservableCollection<IPerson>( Get( max ) );
	}

	private void SettingsPropertyChanged( object? sender, PropertyChangedEventArgs e )
	{
		if( sender is Settings settings )
		{
			switch( e.PropertyName )
			{
				case nameof( Settings.ExtPeople ):
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

	/// <summary>Creates a Person.</summary>
	/// <param name="details">The Person details.</param>
	/// <returns>The created Person details.</returns>
	public async Task<Person> Create( Person details )
	{
		_ = _context.People.Add( details );
		_ = await _context.SaveChangesAsync();
		return details;
	}

	/// <summary>Gets a specific Person.</summary>
	/// <param name="id">Unique Person Id.</param>
	/// <returns>The Person details.</returns>
	private async Task<Person?> Read( int id )
	{
		return await _context.People.Include( x => x.Address ).FirstOrDefaultAsync( x => x.Id == id );
	}

	/// <summary>Updates a specific Person.</summary>
	/// <param name="id">Unique Person Id.</param>
	/// <param name="changes">The Person detail changes.</param>
	/// <returns>The updated Person details.</returns>
	public async Task<Person?> Update( int id, IPerson changes )
	{
		Person? current = await Read( id );
		if( current is null ) { return current; }

		current.Update( changes );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	/// <summary>Deletes a Person.</summary>
	/// <param name="id">Unique Person Id.</param>
	/// <returns>The deleted Person details.</returns>
	public async Task<Person?> Delete( int id )
	{
		Person? current = await Read( id );
		if( current is null ) return null;

		_context.People.Remove( current );
		_ = await _context.SaveChangesAsync();
		return current;
	}

	#endregion
}
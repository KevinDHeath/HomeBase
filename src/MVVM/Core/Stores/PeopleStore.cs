using System.Collections.ObjectModel;
using System.ComponentModel;
using Common.Core.Interfaces;
using MVVM.Core.Models;

namespace MVVM.Core.Stores;

/// <summary>People storage.</summary>
public class PeopleStore
{
	#region Properties

	internal ObservableCollection<IPerson> People { get; private set; }

	internal int Count => _factory.TotalCount;

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

	private readonly IDataFactory<IPerson> _factory;
	private IPerson? _currentPerson;
	private string? _dataFolder;
	private bool _useExternal;

	/// <summary>Initializes a new instance of the PeopleStore class.</summary>
	/// <param name="settingsStore">Settings storage.</param>
	/// <param name="factory">Person data factory.</param>
	public PeopleStore( SettingsStore settingsStore, IDataFactory<IPerson> factory )
	{
		settingsStore.SettingsChanged += SettingsPropertyChanged;
		_useExternal = settingsStore.CurrentSettings?.UseExternal ?? false;
		_factory = factory;

		Initialize( settingsStore.CurrentSettings );
		People ??= new();
	}

	#endregion

	#region Internal Methods

	internal void AddPerson( int max = 0 )
	{
		if( _factory is null ) return;

		foreach( var person in _factory.Get( max ) )
		{
			People.Add( person );
		}
	}

	internal bool Export( string? folder, string? fileName )
	{
		if( People.Count == 0 ) return false;

		if( !string.IsNullOrWhiteSpace( folder ) && !string.IsNullOrWhiteSpace( fileName ) )
		{
			_factory.Serialize( folder, fileName, new List<IPerson>( People ) );
		}

		return false;
	}

	internal void UpdatePerson( IPerson person )
	{
		bool ok = true;
		if( CurrentPerson is not null && CurrentPerson is Common.Core.Models.Person cur )
		{
			if( !_useExternal ) { ok = _factory.Update( CurrentPerson, person ); }
			if( ok ) { cur.Update( person ); }
		}
	}

	#endregion

	#region Private Methods

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
		_factory.Data.Clear();

		People = !string.IsNullOrWhiteSpace( _dataFolder ) && !string.IsNullOrWhiteSpace( fileName )
			? new ObservableCollection<IPerson>( _factory.Get( _dataFolder, fileName, max ) )
			: new ObservableCollection<IPerson>( _factory.Get( max ) );
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
}
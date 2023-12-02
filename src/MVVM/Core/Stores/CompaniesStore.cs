using System.Collections.ObjectModel;
using System.ComponentModel;
using Common.Core.Interfaces;
using MVVM.Core.Models;

namespace MVVM.Core.Stores;

/// <summary>Companies storage.</summary>
public class CompaniesStore
{
	#region Properties

	internal ObservableCollection<ICompany> Companies { get; private set; }

	internal int Count => _factory.TotalCount;

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

	private readonly IDataFactory<ICompany> _factory;
	private ICompany? _currentCompany;
	private string? _dataFolder;
	private bool _useExternal;

	/// <summary>Initializes a new instance of the CompaniesStore class.</summary>
	/// <param name="settingsStore">Settings storage.</param>
	/// <param name="factory">Company data factory.</param>
	public CompaniesStore( SettingsStore settingsStore, IDataFactory<ICompany> factory )
	{
		settingsStore.SettingsChanged += SettingsPropertyChanged;
		_useExternal = settingsStore.CurrentSettings?.UseExternal ?? false;
		_factory = factory;

		Initialize( settingsStore.CurrentSettings );
		Companies ??= new();
	}

	#endregion

	#region Internal Methods

	internal void AddCompany( int max = 0 )
	{
		if( _factory is null ) return;

		foreach( var company in _factory.Get( max ) )
		{
			Companies.Add( company );
		}
	}

	internal void UpdateCompany( ICompany company )
	{
		bool ok = true;
		if( CurrentCompany is not null && CurrentCompany is Common.Core.Models.Company cur )
		{
			if( !_useExternal ) { ok = _factory.Update( CurrentCompany, company ); }
			if( ok ) { cur.Update( company ); }
		}
	}

	internal bool Export( string? folder, string? fileName )
	{
		if( Companies.Count == 0 ) return false;

		if( !string.IsNullOrWhiteSpace( folder ) && !string.IsNullOrWhiteSpace( fileName ) )
		{
			return _factory.Serialize( folder, fileName, new List<ICompany>( Companies ) );
		}

		return false;
	}

	#endregion

	#region Private Methods

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
		_factory.Data.Clear();

		Companies = !string.IsNullOrWhiteSpace( _dataFolder ) && !string.IsNullOrWhiteSpace( fileName )
			? new ObservableCollection<ICompany>( _factory.Get( _dataFolder, fileName, max ) )
			: new ObservableCollection<ICompany>( _factory.Get( max ) );
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
}
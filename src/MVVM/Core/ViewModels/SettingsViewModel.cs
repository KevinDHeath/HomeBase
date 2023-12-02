using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Core.Services;
using MVVM.Core.Stores;

namespace MVVM.Core.ViewModels;

/// <summary>Settings view model.</summary>
public sealed partial class SettingsViewModel : ViewModelEdit
{
	#region Properties

	/// <summary>Gets or sets the font size.</summary>
	[Display( Name = "Font Size" )]
	[Required( ErrorMessage = "{0} is required." )]
	[Range( 10, 18, ErrorMessage = "{0} must be between {1} and {2}." )]
	public int? FontSize
	{
		get => _fontSize;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _fontSize, value );
			SaveSettingsCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the external data file for companies.</summary>
	[ObservableProperty]
	[NotifyCanExecuteChangedFor( nameof( SaveSettingsCommand ) )]
	private string? _extCompanies;

	/// <summary>Gets or sets the external data file for people.</summary>
	[ObservableProperty]
	[NotifyCanExecuteChangedFor( nameof( SaveSettingsCommand ) )]
	private string? _extPeople;

	/// <summary>Gets or sets the folder containing external data.</summary>
	[Required( ErrorMessage = "Location is required." )]
	public string? ExternalData
	{
		get => _externalData;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _externalData, value );
			SaveSettingsCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the maximum number of companies to load.</summary>
	[Display( Name = "Max Companies" )]
	[Required( ErrorMessage = "{0} is required." )]
	[Range( 1, 15, ErrorMessage = "{0} must be between {1} and {2}." )]
	public int? MaxCompanies
	{
		get => _maxCompanies;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _maxCompanies, value );
			SaveSettingsCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Gets or sets the maximum number of people to load.</summary>
	[Display( Name = "Max People" )]
	[Required( ErrorMessage = "{0} is required." )]
	[Range( 1, 15, ErrorMessage = "{0} must be between {1} and {2}." )]
	public int? MaxPeople
	{
		get => _maxPeople;
		set
		{
			_validation.ValidateProperty( value );
			SetProperty( ref _maxPeople, value );
			SaveSettingsCommand.NotifyCanExecuteChanged();
		}
	}

	/// <summary>Indicates whether to use external data files.</summary>
	[ObservableProperty]
	[NotifyCanExecuteChangedFor( nameof( SaveSettingsCommand ) )]
	private bool? _useExternal;

	#endregion

	#region Commands

	/// <summary>Navigate to the Home View command.</summary>
	[RelayCommand]
	public void NavigateHome() => _service.Navigate();

	/// <summary>Save Settings view command.</summary>
	[RelayCommand( CanExecute = nameof( CanSaveSettings ) )]
	public void SaveSettings()
	{
		if( _store.CurrentSettings is not null )
		{
			// Update the current settings and save
			_store.CurrentSettings.FontSize = FontSize;
			_store.CurrentSettings.ExtCompanies = ExtCompanies;
			_store.CurrentSettings.ExtPeople = ExtPeople;
			_store.CurrentSettings.ExternalData = ExternalData;
			_store.CurrentSettings.MaxCompanies = MaxCompanies;
			_store.CurrentSettings.MaxPeople = MaxPeople;
			_store.CurrentSettings.UseExternal = UseExternal;
			_store.Save();
		}

		_service.Navigate();
	}

	#endregion

	#region Constructor and Variables

	private int? _fontSize;
	private int? _maxCompanies;
	private int? _maxPeople;
	private string? _externalData;

	private readonly SettingsStore _store;
	private readonly INavigationService _service;

	/// <summary>Initializes a new instance of the SettingsViewModel class.</summary>
	/// <param name="store">Settings storage.</param>
	/// <param name="service">Navigation service.</param>
	public SettingsViewModel( SettingsStore store, INavigationService service )
	{
		_store = store;
		_fontSize = _store.CurrentSettings?.FontSize;
		_extCompanies = _store.CurrentSettings?.ExtCompanies;
		_extPeople = _store.CurrentSettings?.ExtPeople;
		_externalData = _store.CurrentSettings?.ExternalData;
		_maxCompanies = _store.CurrentSettings?.MaxCompanies;
		_maxPeople = _store.CurrentSettings?.MaxPeople;
		_useExternal = _store.CurrentSettings?.UseExternal;

		_service = service;
	}

	#endregion

	#region Private Methods

	private bool CanSaveSettings()
	{
		if( _store.CurrentSettings is null ) return false;

		// Check for any errors
		if( HasErrors ) return false;

		// Check for any data changes
		return ( FontSize != _store.CurrentSettings.FontSize ) ||
			( ExtCompanies != _store.CurrentSettings.ExtCompanies ) ||
			( ExtPeople != _store.CurrentSettings.ExtPeople ) ||
			( ExternalData != _store.CurrentSettings.ExternalData ) ||
			( MaxCompanies != _store.CurrentSettings.MaxCompanies ) ||
			( MaxPeople != _store.CurrentSettings.MaxPeople ) ||
			( UseExternal != _store.CurrentSettings.UseExternal );
	}

	#endregion
}
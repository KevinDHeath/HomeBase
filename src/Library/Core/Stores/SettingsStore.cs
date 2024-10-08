namespace Library.Core.Stores;

/// <summary>Class to handle the runtime settings.</summary>
public sealed class SettingsStore
{
	private readonly string _settingsFile = string.Empty;
	private Settings? _settings;

	/// <summary>Initializes a new instance of the SettingsStore class for serialization.</summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public SettingsStore() { }

	/// <summary>Initializes a new instance of the SettingsStore class.</summary>
	/// <param name="settingsFile">Settings file name.</param>
	public SettingsStore( string settingsFile )
	{
		if( string.IsNullOrWhiteSpace( settingsFile ) ) { return; }
		_settingsFile = ReflectionHelper.AddCurrentPath( settingsFile );

		SettingsStore? json = JsonHelper.DeserializeFile<SettingsStore>( _settingsFile );
		if( json?.Settings is not null )
		{
			Settings = json.Settings;
			Settings.CheckIfValid();
		}
	}

	/// <summary>Occurs when the settings property changes.</summary>
	public event Action? SettingsChanged;

	/// <summary>Gets or sets the settings.</summary>
	[JsonPropertyName( "appSettings" )]
	public Settings Settings
	{
		get => _settings is not null ? _settings : new Settings() { FontSize = 12 };
		set
		{
			_settings = value;
			OnSettingsChanged();
		}
	}

	/// <summary>Saves the settings file.</summary>
	internal bool Save()
	{
		if( string.IsNullOrWhiteSpace( _settingsFile ) ) { return false; }

		JsonSerializerOptions options = JsonHelper.DefaultSerializerOptions();
		options.WriteIndented = true;
		return JsonHelper.Serialize( this, _settingsFile, options );
	}

	private void OnSettingsChanged() => SettingsChanged?.Invoke();
}
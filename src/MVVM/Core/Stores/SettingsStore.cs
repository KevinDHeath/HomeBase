using System.ComponentModel;
using Common.Core.Classes;
using MVVM.Core.Models;

namespace MVVM.Core.Stores;

/// <summary>Settings storage.</summary>
public class SettingsStore
{
	private readonly string _settingsFile;

	/// <summary>Gets the current settings.</summary>
	public Settings? CurrentSettings { get; private set; }

	/// <summary>Occurs when any settings property changes.</summary>
	public event PropertyChangedEventHandler? SettingsChanged;

	#region Constructor

	/// <summary>Initializes a new instance of the SettingsStore class.</summary>
	/// <param name="settingsFile">Name of the settings file.</param>
	public SettingsStore( string settingsFile )
	{
		_settingsFile = settingsFile;
		string fullFile = ReflectionHelper.AddCurrentPath( _settingsFile );
		CurrentSettings = JsonHelper.DeserializeFile<Settings>( fullFile );

		if( CurrentSettings is not null )
		{
			CurrentSettings.PropertyChanged += PropertiesChanged;
		}
	}

	#endregion

	internal bool Save()
	{
		System.Text.Json.JsonSerializerOptions options = JsonHelper.DefaultSerializerOptions();
		options.WriteIndented = true;
		return JsonHelper.Serialize( CurrentSettings, _settingsFile, options );
	}

	private void PropertiesChanged( object? sender, PropertyChangedEventArgs e )
	{
		SettingsChanged?.Invoke( sender, e );
	}
}
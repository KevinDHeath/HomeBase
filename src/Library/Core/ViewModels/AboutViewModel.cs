using System.Reflection;
using System.Runtime.Versioning;

namespace Library.Core.ViewModels;

/// <summary>Creates a new instance of the ItemViewModel class.</summary>
public sealed class AboutViewModel : ViewModelBase
{
	private readonly LibraryStore _store;

	/// <summary>Creates a new instance of the ItemViewModel class.</summary>
	/// <param name="store">Library store.</param>
	/// <param name="navigation">Settings navigation service.</param>
	public AboutViewModel( LibraryStore store, INavigationService navigation )
	{
		_store = store;
		_store.SettingsChanged += SettingsPropertyChanged;
		NavigateSettingsCommand = new NavigateCommand( navigation );
		InitAppInfo();
	}

	/// <summary>Gets the navigation command for the Settings view.</summary>
	public ICommand NavigateSettingsCommand { get; }

	/// <summary>Gets the section count.</summary>
	public int SectionsCount => _store.Sections.Count;

	/// <summary>Gets the author count.</summary>
	public int AuthorsCount => _store.Authors.Count;

	/// <summary>Gets the item count.</summary>
	public int ItemsCount => _store.ItemsCount;

	/// <summary>Gets the merge count.</summary>
	public int MergeCount => _store.Merges.Count;

	/// <summary>Gets the collection name.</summary>
	public string? CollectionName => _store.Settings.GetCollectionName();

	/// <summary>Gets the library file name.</summary>
	public string? MainFile => _store.Settings.GetMainFileName();

	/// <summary>Gets the items path.</summary>
	public string? ItemsPath => _store.Settings.GetItemsPath();

	/// <summary>Gets the items to merge path.</summary>
	public string? MergePath => _store.Settings.GetMergePath();

	/// <summary>Gets a string value specifying a company name.</summary>
	public string Company { get; private set; } = string.Empty;

	/// <summary>Gets the build configuration, such as retail or debug.</summary>
	public string Configuration { get; private set; } = string.Empty;

	/// <summary>Gets a string value specifying copyright information.</summary>
	public string Copyright { get; private set; } = string.Empty;

	/// <summary>Gets the text description.</summary>
	public string Description { get; private set; } = string.Empty;

	/// <summary>Gets a string value specifying product information.</summary>
	public string Product { get; private set; } = string.Empty;

	/// <summary>Gets a string value specifying a friendly name.</summary>
	public string Title { get; private set; } = string.Empty;

	/// <summary>Gets the version of .NET that the assembly was compiled against.</summary>
	public string FrameworkName { get; private set; } = string.Empty;

	/// <summary>Where other assemblies that reference the assembly will look. If this number
	/// changes, other assemblies must update their references to the assembly.</summary>
	public string AssemblyVersion { get; private set; } = string.Empty;

	/// <summary>This can increase for every deployment. It is used for setup programs.
	/// Used to mark assemblies that have the same AssemblyVersion, but are generated
	/// from different builds.</summary>
	public string FileVersion { get; private set; } = string.Empty;

	/// <summary>Gets a string value of the Product version.<br/>
	/// This is the version you would use when talking to customers or for display on your website.
	/// This version can be a string, like '1.0 Release Candidate'.</summary>
	public string InfoVersion { get; private set; } = string.Empty;

	private void InitAppInfo()
	{
		var assembly = Assembly.GetEntryAssembly();
		string? val;

		val = GetCustomAttribute<AssemblyTitleAttribute>( assembly )?.Title;
		Title = val ?? string.Empty;

		val = GetCustomAttribute<AssemblyCompanyAttribute>( assembly )?.Company;
		Company = val ?? string.Empty;

		val = GetCustomAttribute<AssemblyConfigurationAttribute>( assembly )?.Configuration;
		Configuration = val ?? string.Empty;

		val = GetCustomAttribute<AssemblyCopyrightAttribute>( assembly )?.Copyright;
		Copyright = val ?? string.Empty;

		val = GetCustomAttribute<AssemblyDescriptionAttribute>( assembly )?.Description;
		Description = val ?? string.Empty;

		val = GetCustomAttribute<AssemblyProductAttribute>( assembly )?.Product;
		Product = val ?? string.Empty;

		val = GetCustomAttribute<TargetFrameworkAttribute>( assembly )?.FrameworkDisplayName;
		FrameworkName = val ?? string.Empty;

		val = assembly?.GetName()?.Version?.ToString();
		AssemblyVersion = val ?? string.Empty;

		val = GetCustomAttribute<AssemblyFileVersionAttribute>( assembly )?.Version;
		FileVersion = val ?? string.Empty;

		val = GetCustomAttribute<AssemblyInformationalVersionAttribute>( assembly )?.InformationalVersion;
		InfoVersion = val ?? string.Empty;
	}

	private static T? GetCustomAttribute<T>( Assembly? assembly ) where T : Attribute
	{
		try { return assembly?.GetCustomAttribute<T>(); }
		catch { return null; }
	}

	private void SettingsPropertyChanged()
	{
		string? mainFile = _store.Settings.GetMainFileName();
		if( _store.Settings.IsValid && !string.IsNullOrWhiteSpace( mainFile ) )
		{
			OnPropertyChanged( nameof( SectionsCount ) );
			OnPropertyChanged( nameof( AuthorsCount ) );
			OnPropertyChanged( nameof( ItemsCount ) );
			OnPropertyChanged( nameof( MergeCount ) );
			OnPropertyChanged( nameof( CollectionName ) );
			OnPropertyChanged( nameof( MainFile ) );
			OnPropertyChanged( nameof( ItemsPath ) );
			OnPropertyChanged( nameof( MergePath ) );
		}
	}

	/// <inheritdoc/>
	public override void Dispose()
	{
		_store.SettingsChanged -= SettingsPropertyChanged;
		base.Dispose();
	}
}
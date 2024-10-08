global using Library.Core.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Library.Core.Services;
using Library.Core.Stores;

namespace Library.Wpf;

public partial class App : Application
{
	internal static new App Current => (App)Application.Current;

	private IServiceProvider Services { get; }

	private SettingsStore Settings { get; }

	public App()
	{
		IServiceCollection services = RegisterServices.Build();
		services.AddSingleton( s => new MainWindow() { DataContext = s.GetRequiredService<MainViewModel>() } );
		Services = services.BuildServiceProvider();
		Settings = Services.GetRequiredService<SettingsStore>();
		Settings.SettingsChanged += SettingsPropertyChanged;
	}

	protected override void OnStartup( StartupEventArgs e )
	{
		SettingsPropertyChanged();
		Services.GetRequiredService<NavigationLayout<LibraryViewModel>>().Navigate();
		Services.GetRequiredService<MainWindow>().Show();
		base.OnStartup( e );
	}

	#region Change Font Size

	private void SettingsPropertyChanged() => ChangeFontSize( Settings?.Settings.FontSize );

	public static double AppFontSize
	{
		get => (double)Current.Resources["Common.FontSize"];
		private set => Current.Resources["Common.FontSize"] = value;
	}

	private const double cMinFontSize = 10;
	private const double cMaxFontSize = 18;

	public static void ChangeFontSize( double? value )
	{
		// Font size can be between 10 and 18
		if( value is not null && value != AppFontSize && value >= cMinFontSize && value <= cMaxFontSize )
		{
			AppFontSize = value.Value;
		}
	}

	#endregion

	internal bool CheckSave( string title)
	{
		LibraryStore store = Services.GetRequiredService<LibraryStore>();
		if( store.ItemChanges.Count > 0 )
		{
			MessageBoxResult result = MessageBox.Show( $"Do you want to save {store.ItemChanges.Count} changes?",
				title, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes );
			switch( result )
			{
				case MessageBoxResult.Cancel:
					return true;
				case MessageBoxResult.Yes:
					_ = store.Save();
					break;
			}
		}
		return false;
	}
}
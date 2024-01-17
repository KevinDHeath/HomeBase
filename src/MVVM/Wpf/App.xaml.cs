using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MVVM.Core.Services;
using MVVM.Core.ViewModels;

namespace MVVM.Wpf;

/// <summary>Interaction logic for App.xaml</summary>
public partial class App : Application
{
	#region Properties

	// 1. Add a double resource into the Application resources
	// 2. Add a static property in the App class
	// 3. Use this app resource as DynamicResource anywhere it's needed
	//    FontSize="{DynamicResource Common.FontSize}"

	/// <summary>Gets or sets the global font size.</summary>
	public static double GlobalFontSize
	{
		get => (double)Current.Resources["Common.FontSize"];
		set => Current.Resources["Common.FontSize"] = value;
	}

	#endregion

	#region Constructor and Variables

	private readonly IServiceProvider _serviceProvider;

	/// <summary>Initializes a new instance of the App class.</summary>
	public App()
	{
		_serviceProvider = ServiceProviderHelper.Create( "appsettings.json" );
	}

	#endregion

	#region Overridden Methods

	/// <summary>Raises the Startup event.</summary>
	/// <param name="e">A StartupEventArgs that contains the event data.</param>
	protected override void OnStartup( StartupEventArgs e )
	{
		INavigationService navigationService = _serviceProvider.GetRequiredService<INavigationService>();
		navigationService.Navigate();

		var settings = _serviceProvider.GetRequiredService<Core.Stores.SettingsStore>();
		if( settings.CurrentSettings is not null && settings.CurrentSettings.FontSize is not null )
			GlobalFontSize = (double)settings.CurrentSettings.FontSize;

		MainWindow = new MainWindow
		{
			DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
		};
		MainWindow.Show();

		base.OnStartup( e );
	}

	#endregion
}
using System.Windows;
using System.Windows.Controls;
using MVVM.Core.ViewModels;

namespace MVVM.Wpf.Components;

/// <summary>Interaction logic for NavigationMenu.xaml</summary>
public partial class NavigationMenu : UserControl
{
	/// <summary>Identifies the ContentView dependency property.</summary>
	public readonly static DependencyProperty ContentViewProperty = DependencyProperty.Register(
		nameof( ContentView ), typeof( ViewModelBase ), typeof( NavigationMenu ),
		new PropertyMetadata( defaultValue: null, propertyChangedCallback: ViewChangedCallback ) );

	/// <summary>Gets or sets the navigation bar view model.</summary>
	public ViewModelBase? ContentView
	{
		get => (ViewModelBase)GetValue( ContentViewProperty );
		set => SetValue( ContentViewProperty, value );
	}

	/// <summary>Initializes a new instance of the NavigationMenu class.</summary>
	public NavigationMenu()
	{
		InitializeComponent();
	}

	private static void ViewChangedCallback( DependencyObject d, DependencyPropertyChangedEventArgs e )
	{
		var input = (NavigationMenu)d;
		input.ContentView = e.NewValue as ViewModelBase;
	}
}
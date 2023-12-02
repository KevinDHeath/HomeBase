using System.Windows.Controls;

namespace MVVM.Wpf.Components;

/// <summary>Interaction logic for Layout.xaml</summary>
public partial class Layout : UserControl
{
	/// <summary>Initializes a new instance of the Layout class.</summary>
	public Layout()
	{
		InitializeComponent();
	}

	// When the data context changes update the ContentViewModel binding for the navigation menu
	private void ContextChanged( object sender, System.Windows.DependencyPropertyChangedEventArgs e )
	{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
		if( e.NewValue is not null && e.NewValue is MVVM.Core.ViewModels.LayoutViewModel vm )
		{
			UserControl style = Format;
			if( style is NavigationMenu )
			{
				Format.SetBinding( NavigationMenu.ContentViewProperty, new System.Windows.Data.Binding
				{
					Source = DataContext,
					Path = new System.Windows.PropertyPath( nameof( vm.ContentViewModel ) ),
					Mode = System.Windows.Data.BindingMode.OneWay
				} );
			}
		}
#pragma warning restore IDE0059 // Unnecessary assignment of a value
	}
}
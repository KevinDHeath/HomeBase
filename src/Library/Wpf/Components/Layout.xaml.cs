using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Library.Wpf.Components;

public partial class Layout : UserControl
{
	public Layout()
	{
		InitializeComponent();
	}

	// When the data context changes update the ContentViewModel binding for the navigation menu
	// This handles the extra DataContextChanged="ContextChanged" set in the xaml
	private void ContextChanged( object sender, DependencyPropertyChangedEventArgs e )
	{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
		if( e.NewValue is not null && e.NewValue is LayoutViewModel vm )
		{
			UserControl style = Format;
			if( style is NavigationMenu )
			{
				Format.SetBinding( NavigationMenu.ContentViewProperty, new Binding
				{
					Source = DataContext,
					Path = new PropertyPath( nameof( vm.ContentViewModel ) ),
					Mode = BindingMode.OneWay
				} );
			}
		}
#pragma warning restore IDE0059 // Unnecessary assignment of a value
	}
}
using System.Windows;
using System.Windows.Controls;

namespace MVVM.Wpf.Views;

/// <summary>Interaction logic for SettingsView.xaml</summary>
public partial class SettingsView : UserControl
{
	/// <summary>Initializes a new instance of the SettingsView class.</summary>
	public SettingsView()
	{
		InitializeComponent();
	}

	private string? _orgFontSize;

	private void UserControl_Loaded( object sender, RoutedEventArgs e )
	{
		_orgFontSize = tbFontSize.Text;
	}

	private void Apply_Click( object sender, RoutedEventArgs e )
	{
		if( _orgFontSize is not null && tbFontSize.Text != _orgFontSize )
		{
			if( double.TryParse( tbFontSize.Text, out double val ) )
				App.GlobalFontSize = val;
		}

		_orgFontSize = tbFontSize.Text; // In case the control is not loaded again

		// If command does not get executed try:
		//if( sender is Button btn )
		//{
		//	if( btn.Command.CanExecute( btn.CommandParameter ) )
		//	{
		//		btn.Command.Execute( btn.CommandParameter );
		//	}
		//}
	}
}
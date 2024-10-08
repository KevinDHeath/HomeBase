using System.Windows;

namespace Library.Wpf;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
	{
		if( App.Current.CheckSave( Title ) ) { e.Cancel = true; }
	}
}
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Navigation;

namespace MVVM.Wpf.Views;

/// <summary>Interaction logic for HomeView.xaml</summary>
public partial class HomeView : UserControl
{
	/// <summary>Initializes a new instance of the HomeView class.</summary>
	public HomeView()
	{
		InitializeComponent();
	}

	private void Hyperlink_RequestNavigate( object sender, RequestNavigateEventArgs e )
	{
		Process.Start( new ProcessStartInfo( e.Uri.AbsoluteUri )
		{
			UseShellExecute = true
		} );
	}
}
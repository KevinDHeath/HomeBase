using System.Windows;
using System.Windows.Controls;
using Common.Core.Models;
using Common.Wpf.Controls.Classes;

namespace MVVM.Wpf.Views;

/// <summary>Interaction logic for CompaniesView.xaml</summary>
public partial class CompaniesView : UserControl
{
	/// <summary>Initializes a new instance of the CompaniesView class.</summary>
	public CompaniesView()
	{
		InitializeComponent();
	}

	#region Event Handlers

	private void ExportData( object sender, RoutedEventArgs e )
	{
		if( string.IsNullOrWhiteSpace( tbPath.Text ) ) { return; }

		tbFile.Text = Win32Dialogs.ShowFileSave( tbPath.Text, Company.cDefaultFile, exists: false );
	}

	#endregion
}
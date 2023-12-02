using System;
using System.Windows;
using System.Windows.Controls;
using Common.Core.Models;
using Common.Wpf.Controls.Classes;

namespace MVVM.Wpf.Views;

/// <summary>Interaction logic for PeopleView.xaml</summary>
public partial class PeopleView : UserControl
{
	/// <summary>Initializes a new instance of the PeopleView class.</summary>
	public PeopleView()
	{
		InitializeComponent();
	}

	#region Event Handlers

	// Fix bug with event being fired twice
	// Maybe due to using UpdateSourceTrigger=PropertyChanged
	private DateTime? _lastPickerDate;

	private void SelectedDateChanged( object sender, SelectionChangedEventArgs e )
	{
		if( e.OriginalSource is DatePicker dp )
		{
			var pickerDate = dp.SelectedDate is null ? DateTime.MaxValue : dp.SelectedDate;
			if( _lastPickerDate != pickerDate )
			{
				_lastPickerDate = pickerDate;
			}
		}
	}

	private void ExportData( object sender, RoutedEventArgs e )
	{
		if( string.IsNullOrWhiteSpace( tbPath.Text ) ) { return; }

		tbFile.Text = Win32Dialogs.ShowFileSave( tbPath.Text, Person.cDefaultFile, exists: false );
	}

	#endregion
}
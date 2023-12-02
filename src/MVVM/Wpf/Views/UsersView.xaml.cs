using System;
using System.Windows;
using System.Windows.Controls;

namespace MVVM.Wpf.Views;

/// <summary>Interaction logic for UsersView.xaml</summary>
public partial class UsersView : UserControl
{
	/// <summary>Initializes a new instance of the UsersView class.</summary>
	public UsersView()
	{
		InitializeComponent();
	}

	#region Event Handlers

	private bool _listViewInit;

	private void UsersList_GotFocus( object sender, RoutedEventArgs e )
	{
		// Only do this once
		if( !_listViewInit )
		{
			_listViewInit = true;
			UsersList.Sort( UsersList.DefaultColumn );
		}
	}

	private void UsersList_ColumnHeaderClicked( object sender, RoutedEventArgs e )
	{
		if( e.OriginalSource is not null &&
			e.OriginalSource is GridViewColumnHeader header &&
			header.Role != GridViewColumnHeaderRole.Padding )
		{
			UsersList.Sort( header );
		}
	}

	private void OnFilterChanged( object sender, TextChangedEventArgs e )
	{
		UsersList.ApplyFilter();
	}

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

	#endregion
}
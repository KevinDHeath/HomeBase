using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM.Wpf.Views;

/// <summary>Interaction logic for LoginView.xaml</summary>
public partial class LoginView : UserControl
{
	/// <summary>Initializes a new instance of the LoginView class.</summary>
	public LoginView()
	{
		InitializeComponent();
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

	private bool _focusSet = false;

	private void FirstFocus( object sender, RoutedEventArgs e )
	{
		if( sender is TextBox tb && tb.IsEnabled )
		{
			_ = Keyboard.Focus( tb );
			_focusSet = true;
		}
		else if( sender is PasswordBox pb && !_focusSet )
		{
			_ = Keyboard.Focus( pb );
		}
	}
}
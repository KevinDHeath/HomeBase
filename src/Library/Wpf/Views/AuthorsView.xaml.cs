using System.Windows;
using System.Windows.Controls;
using Common.Wpf.Controls;

namespace Library.Wpf.Views;

public partial class AuthorsView : UserControl
{
	public AuthorsView()
	{
		InitializeComponent();
	}

	private void OnAuthorsFilterChanged( object sender, TextChangedEventArgs e )
	{
		AuthorsList.ApplyFilter();
	}

	private void AuthorsList_Loaded( object sender, RoutedEventArgs e )
	{
		if( sender is not null && sender is SortableListView lv && lv.HasItems )
		{
			AuthorsList.Reset();
		}
	}

	private void AuthorsList_ColumnHeaderClicked( object sender, RoutedEventArgs e )
	{
		if( e.OriginalSource is not null &&
			e.OriginalSource is GridViewColumnHeader header &&
			header.Role != GridViewColumnHeaderRole.Padding )
		{
			AuthorsList.Sort( header );
		}
	}

	private void OnItemsFilterChanged( object sender, TextChangedEventArgs e )
	{
		ItemsList.ApplyFilter();
	}

	private void ItemsList_Loaded( object sender, RoutedEventArgs e )
	{
		if( sender is not null && sender is SortableListView lv && lv.HasItems )
		{
			tbItemSearch.Text = string.Empty;
			ItemsList.Reset();
			ItemsList.ScrollIntoView( lv.Items[0] );
		}
	}

	private void ItemsList_ColumnHeaderClicked( object sender, RoutedEventArgs e )
	{
		if( e.OriginalSource is not null &&
			e.OriginalSource is GridViewColumnHeader header &&
			header.Role != GridViewColumnHeaderRole.Padding )
		{
			ItemsList.Sort( header );
		}
	}
}
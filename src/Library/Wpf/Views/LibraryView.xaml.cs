using System.Windows;
using System.Windows.Controls;
using Common.Wpf.Controls;

namespace Library.Wpf.Views;

public partial class LibraryView : UserControl
{
	public LibraryView()
	{
		InitializeComponent();
	}

	private void OnSectionsFilterChanged( object sender, TextChangedEventArgs e )
	{
		SectionsList.ApplyFilter();
	}

	private void SectionsList_Loaded( object sender, RoutedEventArgs e )
	{
		if( sender is not null && sender is SortableListView lv && lv.HasItems )
		{
			SectionsList.Reset();
		}
	}

	private void SectionsList_ColumnHeaderClicked( object sender, RoutedEventArgs e )
	{
		if( e.OriginalSource is not null &&
			e.OriginalSource is GridViewColumnHeader header &&
			header.Role != GridViewColumnHeaderRole.Padding )
		{
			SectionsList.Sort( header );
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
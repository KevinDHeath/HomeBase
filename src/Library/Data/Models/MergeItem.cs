using HtmlAgilityPack;

namespace Library.Data.Models;

/// <summary>Contains the details for a merge item.</summary>
public class MergeItem : ModelBase
{
	private readonly Item _item;
	private bool _canMerge;
	private bool _merge;
	private readonly FileInfo? _source;
	private readonly FileInfo? _target;
	private string? _error;

	/// <summary>Initializes a new instance of the MergeItem class.</summary>
	/// <param name="fileInfo">HTML file information.</param>
	/// <param name="itemsPath">Items path.</param>
	/// <param name="mergePath">Merge items path.</param>
	/// <param name="section">Section the item belongs in.</param>
	public MergeItem( FileInfo fileInfo, string? itemsPath, string? mergePath, Section section )
	{
		_item = ProcessHtml( fileInfo );
		_item.Initialize( section );

		_target = _item.GetFullPath( itemsPath, mergePath, section );
		if( _target is not null && _target.Exists )
		{
			_target = null;
			Error = "Target already exists.";
		}

		_item.Category = Categories.Merge;
		_source = _item.GetFullPath( itemsPath, mergePath, section );
		if( _source is not null && !_source.Exists )
		{
			_source = null;
			Error = "Source does not exists.";
		}

		CanMerge = _target is not null && _source is not null;
		Merge = CanMerge;
	}

	/// <summary>Indicates if the item can be merged.</summary>
	public bool CanMerge
	{
		get => _canMerge;
		set
		{
			_canMerge = value;
			OnPropertyChanged();
		}
	}

	/// <summary>Indicates if the item has been selected for merge.</summary>
	public bool Merge
	{
		get => _merge;
		set
		{
			_merge = value;
			OnPropertyChanged();
		}
	}

	/// <summary>Error message.</summary>
	public string? Error
	{
		get => _error;
		set
		{
			_error = value;
			OnPropertyChanged();
		}
	}

	/// <summary>Gets the item to be merged.</summary>
	public Item Item => _item;

	/// <summary>Perform the merge.</summary>
	/// <returns>True if the merge was completed.</returns>
	public bool PerformMerge()
	{
		// Check the file can be moved
		if( _source is null || _target is null ) { return false; }

		// Store the work directory as the source directory will change when the file is moved
		DirectoryInfo? source = _source.Directory;

		_source.MoveTo( _target.FullName );

		// Make sure the archive attribute is set
		if( ( _target.Attributes & FileAttributes.Archive ) != FileAttributes.Archive )
		{
			File.SetAttributes( _target.FullName,
				File.GetAttributes( _target.FullName ) | FileAttributes.Archive );
		}

		// Remove the original source folder if empty
		int? fileCount = source?.GetFiles().Length;
		if( fileCount is not null and 0 ) { source?.Delete(); }

		return true;
	}

	private static Item ProcessHtml( FileInfo fileInfo )
	{
		Item retValue = new()
		{
			Category = Categories.New,
			File = fileInfo.Name
		};

		HtmlDocument document = new();
		HtmlNode? node;
		using( var sr = new StreamReader( fileInfo.FullName ) )
		{
			document.Load( sr );
		}

		// Set the author
		HtmlNodeCollection? nodes = document.DocumentNode.SelectNodes( @"//small" );
		retValue.Author = nodes is not null && nodes.Count > 0 ? ReplaceText( nodes[0], @"by" ) : "Not specified";

		// Make sure the date is formatted correctly
		nodes = document.DocumentNode.SelectNodes( @"//p[@class='DateP']" );

		if( nodes is not null && nodes.Count > 0 )
		{
			node = nodes[0];
			string dateStr = ( null == node ) ? string.Empty : ReplaceText( node, @"Story submitted" );
			if( !string.IsNullOrWhiteSpace( dateStr ) )
			{
				bool ok = DateTime.TryParse( dateStr, out DateTime dt );
				if( ok ) dt.ToString( Item.cDateFormat );
			}
			retValue.Date = dateStr;
		}

		nodes = document.DocumentNode.SelectNodes( @"//title" );
		if( nodes is not null && nodes.Count > 0 )
		{
			node = nodes[0];
			retValue.Title = ( null == node ) ? string.Empty : node.InnerText;
		}

		return retValue;
	}

	private static string ReplaceText( HtmlNode node, string text )
	{
		return node.InnerText.Replace( text, string.Empty ).Trim();
	}
}
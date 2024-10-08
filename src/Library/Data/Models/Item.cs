namespace Library.Data.Models;

/// <summary>Contains the details for a library item.</summary>
public class Item : ModelBase, ICloneable
{
	private string? _author;
	private string? _date;
	private string _file = string.Empty;
	private string _title = string.Empty;
	private Item? _orgSnapshot;

	/// <summary>Format for item creation date.</summary>
	public const string cDateFormat = @"MMMM dd, yyyy";

	/// <summary>Used for serialization use only.</summary>
	[JsonPropertyName( "Category" )]
	[XmlAttribute( "style" )]
	[EditorBrowsable( EditorBrowsableState.Never )]
	public string? Status { get; set; }

	/// <summary>Gets or sets the author.</summary>
	public string? Author
	{
		get => _author;
		set
		{
			value = SetNullString( value );
			if( value != _author )
			{
				_author = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Gets or sets the creation date.</summary>
	public string? Date
	{
		get => _date;
		set
		{
			value = SetNullString( value );
			if( value != _date )
			{
				_date = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Gets or sets the filename.</summary>
	public string File
	{
		get => _file;
		set
		{
			if( value != _file )
			{
				_file = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Gets or sets the title.</summary>
	public string Title
	{
		get => _title;
		set
		{
			if( value != _title )
			{
				_title = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Gets or sets the status as an enumeration.</summary>
	[JsonIgnore()]
	[XmlIgnore()]
	public Categories Category
	{
		get => Enumerations.ConvertCategory( Status );
		set
		{
			if( value != Category )
			{
				Status = Enumerations.ConvertCategory( value ); ;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Determines whether the status should be serialized to XML.</summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public bool ShouldSerializeStatus() => Status?.Length > 0;

	/// <summary>Gets the section name.</summary>
	[JsonIgnore]
	[XmlIgnore]
	public string Section { get; private set; } = string.Empty;

	/// <summary>Gets a sortable version of the Date.</summary>
	[JsonIgnore]
	[XmlIgnore]
	public DateOnly? Created => DateOnly.TryParse( Date, out DateOnly date ) ? date : null;

	/// <inheritdoc/>
	public object Clone()
	{
		return MemberwiseClone();
	}

	/// <summary>Determines to changes should be tracked.</summary>
	/// <param name="mods">Object contains modifications.</param>
	/// <returns>True is changes should be tracked.</returns>
	public bool TrackChanges( Item? mods )
	{
		_orgSnapshot ??= (Item)Clone();
		return _orgSnapshot.HasChanges( mods );
	}

	/// <summary>Gets the full path for the item.</summary>
	/// <param name="filePath">Library path.</param>
	/// <param name="mergePath">Merge items path.</param>
	/// <param name="section">Section the item belongs in.</param>
	/// <returns>Null is returned if the path could not be determined.</returns>
	public FileInfo? GetFullPath( string? filePath, string? mergePath, Section? section )
	{
		if( filePath is null || mergePath is null || section is null ) { return null; }

		filePath = Category != Categories.Merge ?
			Path.Combine( filePath, section.Location ) :
			Path.Combine( mergePath, Path.GetFileName( section.Location ) );
		filePath = Path.Combine( filePath, File );
		return new FileInfo( filePath );
	}

	/// <summary>Indicates whether changes have been made.</summary>
	/// <param name="source">Source item to be compared with.</param>
	/// <returns>True if any changes have been made,</returns>
	public bool HasChanges( Item? source )
	{
		if( source is null ) { return false; }
		if( Author != source.Author ) { return true; }
		if( Date != source.Date ) { return true; }
		if( File != source.File ) { return true; }
		if( Title != source.Title ) { return true; }
		if( Category != source.Category ) { return true; }
		return false;
	}

	/// <summary>Applies changes to the supplied object.</summary>
	/// <param name="source">Source item to be updated.</param>
	/// <returns>True if the updates were applied.</returns>
	public bool ApplyChanges( Item? source )
	{
		if( source is null ) { return false; }
		source.Author = Author;
		source.Date = Date;
		source.File = File;
		source.Title = Title;
		source.Category = Category;
		return true;
	}

	internal void Initialize( Section section ) => Section = section.Name;
}
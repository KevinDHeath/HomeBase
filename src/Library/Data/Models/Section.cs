using System.Collections.ObjectModel;

namespace Library.Data.Models;

/// <summary>Contains the details for a library section.</summary>
public class Section : ModelBase, ICloneable
{
	private string _location = string.Empty;
	private string _name = string.Empty;
	private Section? _orgSnapshot;

	private const StringComparison cComp = StringComparison.OrdinalIgnoreCase;

	/// <summary>Gets or sets the location.</summary>
	[XmlAttribute( "folder" )]
	public string Location
	{
		get => _location;
		set
		{
			if( value != _location )
			{
				_location = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Gets or sets the name.</summary>
	[XmlAttribute( "name" )]
	public string Name
	{
		get => _name;
		set
		{
			if( value != _name )
			{
				_name = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>Gets or sets the collection of items.</summary>
	[XmlElement( ElementName = "Story" )]
	public List<Item> Items { get; set; } = [];

	/// <summary>Gets the count of items.</summary>
	public int Count => Items.Count;

	/// <summary>Gets or sets the status as an enumeration.</summary>
	[JsonIgnore()]
	[XmlIgnore()]
	public Categories Category { get; set; }

	/// <summary>Gets the collection of sections.</summary>
	[JsonIgnore()]
	[XmlIgnore()]
	public ObservableCollection<Item> ItemList { get; set; } = [];

	/// <inheritdoc/>
	public object Clone()
	{
		return MemberwiseClone();
	}

	/// <summary>Determines to changes should be tracked.</summary>
	/// <param name="mods">Object contains modifications.</param>
	/// <returns>True is changes should be tracked.</returns>
	public bool TrackChanges( Section? mods )
	{
		_orgSnapshot ??= (Section)Clone();
		return _orgSnapshot.HasChanges( mods );
	}

	/// <summary>Initializes the Section.</summary>
	/// <param name="authors">Collection of authors to be populated.</param>
	public void Initialize( SortedDictionary<string, ObservableCollection<Item>>? authors = null )
	{
		ItemList = [.. ItemList.OrderBy( x => x.Title )];
		foreach( Item item in ItemList )
		{
			string key = string.IsNullOrWhiteSpace( item.Author ) ? "Unknown" : item.Author.Trim();

			if( authors is not null ) // Add to authors dictionary
			{
				item.Initialize( this );
				if( !authors.TryGetValue( key, out ObservableCollection<Item>? value ) )
				{
					value = new ObservableCollection<Item>();
					authors.Add( key, value );
				}
				value.Add( item );
			}
		}
		SetCategory( this );
	}

	/// <summary>Indicates whether changes have been made.</summary>
	/// <param name="source">Source item to be compared with.</param>
	/// <returns>True if any changes have been made,</returns>
	public bool HasChanges( Section? source )
	{
		if( source is null ) { return false; }
		if( Location != source.Location ) { return true; }
		if( Name != source.Name ) { return true; }
		return false;
	}

	/// <summary>Applies changes to the supplied object.</summary>
	/// <param name="source">Source item to be updated.</param>
	/// <returns>True if the updates were applied.</returns>
	public bool ApplyChanges( Section? source )
	{
		if( source is null ) { return false; }
		source.Location = Location;
		source.Name = Name;
		return true;
	}

	/// <summary>Finds a section by name.</summary>
	/// <param name="sections">Section collection to search.</param>
	/// <param name="name">Section name.</param>
	/// <returns>Null is returned if the section is not found.</returns>
	public static Section? GetSectionByName( List<Section> sections, string? name )
	{
		return sections.FirstOrDefault( s => s.Name.Equals( name, cComp ) );
	}

	/// <summary>Finds a section by location.</summary>
	/// <param name="sections">Section collection to search.</param>
	/// <param name="location">Section location.</param>
	/// <returns>Null is returned if the section is not found.</returns>
	public static Section? GetSectionByLocation( List<Section> sections, string? location )
	{
		return sections.FirstOrDefault( s => s.Location.Equals( location, cComp ) );
	}

	/// <summary>Set section category.</summary>
	/// <param name="section">Section to use.</param>
	public static void SetCategory( Section? section )
	{
		if( section is null ) { return; }
		ObservableCollection<Item> items = section.ItemList;
		section.Category = Categories.Reviewed;
		if( items.Any( i => i.Category == Categories.Merge ) ) { section.Category = Categories.Merge; }
		else if( items.Any( i => i.Category == Categories.New ) ) { section.Category = Categories.New; }
		//else if( items.Any( i => i.Category == Categories.Popular ) ) { section.Category = Categories.Popular; }
	}
}
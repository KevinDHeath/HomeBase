global using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Library.Data;

/// <summary>This class uses XML data for a library.</summary>
[XmlRoot( ElementName = "Stories" )]
public sealed class StorageXml : DataStorage, IStorage
{
	/// <summary>Only used for the schema assignment during serialization.</summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	[XmlAttribute( "noNamespaceSchemaLocation", Namespace = XmlSchema.InstanceNamespace )]
	public string schema = @".\StorySchema.xsd";

	/// <summary>Initializes a new instance of the DataStorage class.</summary>
	[EditorBrowsable( EditorBrowsableState.Never )]
	public StorageXml() { }

	/// <summary>XML file extension.</summary>
	public const string cExt = ".xml";

	#region IStorage Implementation

	/// <summary>Loads data from a XML file.</summary>
	/// <inheritdoc/>
	public DataStorage Load( string? fileName = null )
	{
		DataStorage rtn = new();
		if( string.IsNullOrWhiteSpace( fileName ) ) { return rtn; }
		try
		{
			FileInfo fi = new( fileName );
			return !fi.Exists ? rtn : Deserialize( fi.FullName );
		}
		catch( Exception ex )
		{
			Trace.WriteLine( "XmlStorage.Load() failed!" );
			Trace.WriteLine( ex );
			return rtn;
		}
	}

	/// <summary>Saves data to a XML file.</summary>
	/// <inheritdoc/>
	public bool Save( DataStorage data, string? fileName )
	{
		if( string.IsNullOrWhiteSpace( fileName ) ) return false;
		Sections = data.Sections;
		try
		{
			XmlSerializer xmlSerializer = new( typeof( StorageXml ) );
			using StreamWriter writer = new( fileName );
			using XmlWriter xmlWriter = XmlWriter.Create( writer, GetWriterSettings( true ) );
			xmlSerializer.Serialize( xmlWriter, this );
			return true;
		}
		catch( Exception ex )
		{
			Trace.WriteLine( "XmlStorage.Save() failed!" );
			Trace.WriteLine( ex );
		}
		return false;
	}

	#endregion

	#region Private Methods

	private static DataStorage Deserialize( string fileName )
	{
		XmlSerializer ser = new( typeof( StorageXml ) );
		using StreamReader sr = new( fileName );
		object? obj = ser.Deserialize( sr );
		return obj is not null and DataStorage lib ? lib : new DataStorage();
	}

	private static XmlWriterSettings GetWriterSettings( bool indent )
	{
		return new XmlWriterSettings
		{
			Encoding = System.Text.Encoding.UTF8,
			Indent = indent,
			IndentChars = "\t"
		};
	}

	#endregion
}
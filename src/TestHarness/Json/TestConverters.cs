using System.Text.Json;
using System.Text.Json.Serialization;
using Json.Converters;

namespace TestHarness;

// Paste JSON or XML as classes
// https://learn.microsoft.com/en-us/visualstudio/ide/reference/paste-json-xml

internal class TestConverters
{
	internal static bool RunTest()
	{
		Console.WriteLine();
		Program.sLogger.Info( "Test of Json.Converters" );

		// Can test loading with TestJson.json or TestJson-null.json
		TestConverters? obj = Deserialize( @"Json\TestJson-null.json" );
		if( obj is null ) return false;

		// Test saving of loaded data
		//bool rtn = Serialize( obj, @"Json\TestJson-out.json" );
		//if( rtn ) { Console.WriteLine( File.ReadAllText( @"Json\TestJson-out.json" ) ); }
		//if( obj is null ) return false;

		return true;
	}

	#region Serialization

	/// <summary>Root element.</summary>
	public List<ITestClass>? Data { get; set; }

	internal static TestConverters? Deserialize( string fileName )
	{
		var fi = new FileInfo( fileName );
		if( fi.Exists )
		{
			var obj = Common.Core.Classes.JsonHelper.DeserializeFile<TestConverters>( fi.FullName, GetSerializerOptions() );
			if( obj is not null )
			{
				Program.sLogger.Log( $"File {fileName} loaded." );
				return obj;
			}
		}

		Program.sLogger.Error( $"Failed to load file {fi.Name}." );
		return null;
	}

	internal static bool Serialize( TestConverters? obj, string fileName )
	{
		if( null == obj ) return false;

		if( Common.Core.Classes.JsonHelper.Serialize( obj, fileName, GetSerializerOptions() ) )
		{
			Program.sLogger.Log( $"File {fileName} saved." );
			return true;
		}

		Program.sLogger.Error( $"Failed to save file {fileName}." );
		return false;
	}

	internal static JsonSerializerOptions GetSerializerOptions()
	{
		// https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=net-7.0
		// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-7-0

		JsonSerializerOptions rtn = Common.Core.Classes.JsonHelper.DefaultSerializerOptions();
		rtn.Converters.Add( new InterfaceFactory( typeof( TestClass ), typeof( ITestClass ) ) );
		rtn.Converters.Add( new BooleanNull() );
		rtn.Converters.Add( new BooleanString() );
		rtn.Converters.Add( new ByteNull() );
		rtn.Converters.Add( new ByteString() );
		rtn.Converters.Add( new DateOnlyNull() );
		rtn.Converters.Add( new DateOnlyString() );
		rtn.Converters.Add( new DateTimeNull() );
		rtn.Converters.Add( new DateTimeString() );
		rtn.Converters.Add( new DateTimeOffsetNull() );
		rtn.Converters.Add( new DateTimeOffsetString() );
		rtn.Converters.Add( new DecimalNull() );
		rtn.Converters.Add( new DecimalString() );
		rtn.Converters.Add( new DoubleNull() );
		rtn.Converters.Add( new DoubleString() );
		rtn.Converters.Add( new FloatNull() );
		rtn.Converters.Add( new FloatString() );
		rtn.Converters.Add( new GuidNull() );
		rtn.Converters.Add( new GuidString() );
		rtn.Converters.Add( new IntegerNull() );
		rtn.Converters.Add( new IntegerString() );
		rtn.Converters.Add( new LongNull() );
		rtn.Converters.Add( new LongString() );
		rtn.Converters.Add( new SByteNull() );
		rtn.Converters.Add( new SByteString() );
		rtn.Converters.Add( new ShortNull() );
		rtn.Converters.Add( new ShortString() );
		rtn.Converters.Add( new TimeOnlyNull() );
		rtn.Converters.Add( new TimeOnlyString() );
		rtn.Converters.Add( new TimeSpanNull() );
		rtn.Converters.Add( new TimeSpanString() );
		rtn.Converters.Add( new UIntegerNull() );
		rtn.Converters.Add( new UIntegerString() );
		rtn.Converters.Add( new ULongNull() );
		rtn.Converters.Add( new ULongString() );
		rtn.Converters.Add( new UShortNull() );
		rtn.Converters.Add( new UShortString() );

		rtn.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
		rtn.NumberHandling = JsonNumberHandling.AllowReadingFromString;
		rtn.WriteIndented = true;

		// May not need this
		rtn.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

		return rtn;
	}

	#endregion
}

public class TestClass : ITestClass
{
	#region Properties and Constructor

	public bool? Boolean { get; set; }

	public byte? Byte { get; set; }

	public DateOnly? DateOnly { get; set; }

	public DateTime? DateTime { get; set; }

	public DateTimeOffset? DateTimeOffset { get; set; }

	public decimal? Decimal { get; set; }

	public double? Double { get; set; }

	public float? Float { get; set; }

	public Guid? Guid { get; set; }

	public int? Integer { get; set; }

	public long? Long { get; set; }

	public sbyte? SByte { get; set; }

	public short? Short { get; set; }

	public string String { get; set; }

	public string? StringNull { get; set; }

	public TimeOnly? TimeOnly  { get; set; }

	public TimeSpan? TimeSpan { get; set; }

	public uint? UInt { get; set; }

	public ulong? ULong { get; set; }

	public ushort? UShort { get; set; }

	[JsonConstructor]
	public TestClass()
	{
		String = string.Empty;
	}

	#endregion
}
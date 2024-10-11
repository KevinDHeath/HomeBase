using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using HtmlAgilityPack;
using Library.Core.Stores;
using Library.Data;
using Library.Data.Models;

namespace TestHarness;

internal class LibraryTasks
{
	private static readonly LibraryStore data;

	static LibraryTasks()
	{
		IServiceCollection services = Library.Core.Services.RegisterServices.Build( "TestHarness.json" );
		ServiceProvider sp = services.BuildServiceProvider();
		data = sp.GetRequiredService<LibraryStore>();
	}

	internal static bool RunTests()
	{
		Console.WriteLine();

		return LibraryData( convert: true );
		//return ConvertData( null );
		//return CheckImages();
	}

	#region Test Library Data

	internal static bool LibraryData( bool convert )
	{
		int items = 0;
		foreach( var section in data.Sections ) { items += section.Items.Count; }
		_ = Program.sLogger.Info( "Testing library data..." );
		_ = Program.sLogger.Info( $"Items root: {data.Settings?.GetItemsPath()}" );
		_ = Program.sLogger.Info( $"Index file: {data.Settings?.GetMainFileName()}" );
		_ = Program.sLogger.Info( $"Merge root: {data.Settings?.GetMergePath()}" );
		_ = Program.sLogger.Info( $"Collection: {data.Settings?.GetCollectionName()}" );
		_ = Program.sLogger.Info( $"Backup....: {data.Settings?.Backup}" );
		_ = Program.sLogger.Info( $"{data.Sections.Count:N0} Sections" );
		_ = Program.sLogger.Info( $"{data.Authors.Count:N0} Authors" );
		_ = Program.sLogger.Info( $"{items:N0} Items" );
		_ = Program.sLogger.Info( $"{data.Merges.Count:N0} merges" );

		// Convert if requested
		string? mainFile = data.Settings?.GetMainFileName();
		if( convert && mainFile is not null ) { return ConvertData( mainFile ); }

		return true;
	}

	#endregion

	#region Test Convert Data

	private const string cTestFolder = @"C:\Temp\TestData";

	internal static bool ConvertData( string? mainFile )
	{
		// Load the test data from TestStories.xml or TestStories.json
		mainFile ??= Path.Combine( cTestFolder, "TestStories.xml" );
		_ = Program.sLogger.Info( $"Converting library file: {mainFile}" );

		string outFile = mainFile.Replace( "TestStories.", "TestStories-out." );
		IStorage? store; IStorage? storeOut;
		switch( Path.GetExtension( mainFile ) )
		{
			case StorageJson.cExt:
				store = new StorageJson();
				storeOut = new StorageXml();
				outFile = outFile.Replace( StorageJson.cExt, StorageXml.cExt );
				break;
			default:
				store = new StorageXml();
				storeOut = new StorageJson();
				outFile = outFile.Replace( StorageXml.cExt, StorageJson.cExt );
				break;
		}

		DataStorage library = store.Load( mainFile );
		if( library.Sections.Count > 0 )
		{
			_ = Program.sLogger.Info( $"Saving to library file.: {outFile}" );
			if( outFile.StartsWith( data.Settings.RootPath ) && data.Settings.Backup )
			{ _ = DataStorage.Backup( outFile ); }
			return storeOut.Save( library, outFile );
		}
		return false;
	}

	#endregion

	#region Check Picture Images

	private static StreamWriter? sw = null;

	internal static bool CheckImages()
	{
		Program.sLogger.Info( "Checking library images" );
		var rootDir = Path.Combine( data.Settings.RootPath, "Pictures" );

		// Get list of image directories
		var allLibs = new List<ImageLibrary>();
		foreach( var subDir in Directory.EnumerateDirectories( rootDir ) )
		{
			foreach( var imgDir in Directory.EnumerateDirectories( subDir ) )
			{
				var libs = ProcessDir( imgDir );
				if( libs.Count > 0 )
				{
					allLibs.AddRange( libs );
				}
			}
		}

		// Report missing images
		sw = new StreamWriter( Path.Combine( rootDir, "Test1.txt" ), append: false, encoding: Encoding.UTF8 );
		foreach( var lib in allLibs )
		{
			ReportMissing( lib );
		}
		sw.Flush();
		sw.Close();

		return true;
	}

	private static List<ImageLibrary> ProcessDir( string imgDir )
	{
		var retValue = new List<ImageLibrary>();

		var lib = new ImageLibrary( new DirectoryInfo( imgDir ) );
		if( lib.HtmFiles.Count > 0 || lib.ImgFiles.Count > 0 )
		{
			retValue.Add( lib );
		}

		foreach( var subDir in lib.Folder.EnumerateDirectories() )
		{
			retValue.Add( new ImageLibrary( subDir ) );
		}

		return retValue;
	}

	private static void ReportMissing( ImageLibrary lib )
	{
		if( null == sw || null == lib ) return;

		if( lib.ImgFiles.Count > 0 )
		{
			if( lib.HtmFiles.Count > 0 )
			{
				if( lib.HtmFiles.Count > 1 )
				{
					sw.WriteLine( lib.ImgFiles.Count + " " +
						lib.Folder.FullName + " - " +
						lib.HtmFiles.Count + " index files" );

					List<string> htmNames = [];
					foreach( var html in lib.HtmFiles )
					{
						htmNames.Add( html.Name );
					}
					sw.WriteLine( string.Join( ", ", [.. htmNames] ) );
				}
				else
				{
					sw.WriteLine( lib.ImgFiles.Count + " " +
						lib.HtmFiles[0].FullName );
				}

				foreach( var file in lib.ImgFiles.Values )
				{
					var imgHtml = FormatFilePath( file );
					sw.WriteLine( imgHtml );
				}
				sw.WriteLine( "<hr>" );
			}
			else
			{
				sw.WriteLine( "** Warning - no index file." + lib.Folder.FullName );
			}
			sw.WriteLine();
		}
	}

	private static string FormatFilePath( FileInfo imgFile )
	{
		var retValue = imgFile.DirectoryName;
		var imgSize = string.Empty;
		try
		{
			// System.Drawing.Common only supported on Windows
			// https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/system-drawing-common-windows-only
#pragma warning disable CA1416 // Validate platform compatibility
			var img = Image.FromFile( imgFile.FullName );
			imgSize = $" width=\"{img.Width}\" height=\"{img.Height}\"";
			img.Dispose();
#pragma warning restore CA1416 // Validate platform compatibility
		}
		catch
		{
			Console.WriteLine( "Could not load " + imgFile.FullName );
		}

		if( null == retValue ) return string.Empty;
		retValue = retValue.Replace( data.Settings.RootPath, string.Empty ).Replace( @"\", "/" ) + "/" + imgFile.Name;
		retValue = $"<img src=\"{imgFile.Name}\" alt=\"{retValue}\" title=\"{retValue}\"{imgSize}>";

		return retValue;
	}

	#endregion

	#region ImageLibrary Class

	internal class ImageLibrary
	{
		internal static readonly List<string> ext = [".txt", ".js"];

		internal DirectoryInfo Folder { get; private set; }

		internal List<FileInfo> HtmFiles { get; private set; }

		internal Dictionary<string, FileInfo> ImgFiles { get; private set; }

		internal ImageLibrary( DirectoryInfo dirInfo )
		{
			Folder = dirInfo;
			HtmFiles = ListHtmFiles( Folder );
			ImgFiles = ListImgFiles( Folder );
			RemoveImages();
		}

		internal void RemoveImages()
		{
			foreach( var file in HtmFiles )
			{
				var doc = new HtmlDocument();
				using( var sr = new StreamReader( file.FullName ) )
				{
					doc.Load( sr );
				}

				// Identify images that are included
				var nodes = doc.DocumentNode.SelectNodes( @"//img" );
				if( null != nodes )
				{
					foreach( var node in nodes )
					{
						var srcAttr = node.Attributes[@"src"];
						if( srcAttr != null )
						{
							ImgFiles.Remove( srcAttr.Value );
						}
					}
				}
			}
		}

		public override string ToString()
		{
			return Folder.FullName;
		}

		private static List<FileInfo> ListHtmFiles( DirectoryInfo di )
		{
			return new List<FileInfo>( di.EnumerateFiles( "*.htm*" ) );
		}

		private static Dictionary<string, FileInfo> ListImgFiles( DirectoryInfo di )
		{
			var retValue = new Dictionary<string, FileInfo>( StringComparer.OrdinalIgnoreCase );
			foreach( var item in di.EnumerateFiles() )
			{
				var fileExt = item.Extension.ToLower();
				if( !fileExt.StartsWith( ".htm" ) && !ext.Contains( fileExt ) )
				{
					retValue.Add( item.Name, item );
				}
			}

			return retValue;
		}
	}

	#endregion
}
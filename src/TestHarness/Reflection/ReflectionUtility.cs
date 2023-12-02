// How to Get a Collection Element Type Using Reflection in C#
// https://www.codeproject.com/Tips/5267157/How-to-Get-a-Collection-Element-Type-Using-Reflect

using System;
using System.Collections;
using System.Collections.Generic;

namespace TestHarness.Reflection;

public static class ReflectionUtility
{
	internal static void RunTest()
	{
		Console.WriteLine();

		var objType = typeof( List<string> );
		Console.WriteLine( "Is List<string> a list? " + IsList( objType ) );
		Console.WriteLine( "Element type: " + GetCollectionElementType( objType ) );

		Console.WriteLine();
		objType = typeof( List<int> );
		Console.WriteLine( "Is List<int> a list? " + IsList( objType ) );
		Console.WriteLine( "Element type: " + GetCollectionElementType( objType ) );

		Console.WriteLine();
		objType = typeof( Dictionary<string,List<int>> );
		Console.WriteLine( "Is Dictionary<string,List<int>> a dictionary? " + IsDictionary( objType ) );
		Console.WriteLine( "Element type: " + GetCollectionElementType( objType ) );

		Console.WriteLine();
		objType = typeof( Dictionary<string,PersonTest> );
		Console.WriteLine( "Is Dictionary<string,Person> a dictionary? " + IsDictionary( objType ) );
		Console.WriteLine( "Element type: " + GetCollectionElementType( objType ) );
	}

	/// <summary>
	/// Indicates whether or not the specified type is a list.
	/// </summary>
	/// <param name="type">The type to query</param>
	/// <returns>True if the type is a list, otherwise false</returns>
	private static bool IsList( Type type )
	{
		if( null == type ) throw new ArgumentNullException( nameof( type ) );

		if( typeof( IList ).IsAssignableFrom( type ) ) return true;

		foreach( var it in type.GetInterfaces() )
			if( it.IsGenericType && typeof( IList<> ) == it.GetGenericTypeDefinition() )
				return true;

		return false;
	}

	/// <summary>
	/// Indicates whether or not the specified type is a list.
	/// </summary>
	/// <param name="type">The type to query</param>
	/// <returns>True if the type is a list, otherwise false</returns>
	private static bool IsDictionary( Type type )
	{
		if( null == type ) throw new ArgumentNullException( nameof( type ) );

		if( typeof( IDictionary ).IsAssignableFrom( type ) ) return true;

		foreach( var it in type.GetInterfaces() )
			if( it.IsGenericType && typeof( IDictionary<,> ) == it.GetGenericTypeDefinition() )
				return true;

		return false;
	}

	/// <summary>
	/// Retrieves the collection element type from this type
	/// </summary>
	/// <param name="type">The type to query</param>
	/// <returns>The element type of the collection or null if the type was not a collection</returns>
	private static Type? GetCollectionElementType( Type type )
	{
		if( null == type )
			throw new ArgumentNullException( nameof( type ) );

		// first try the generic way
		// this is easy, just query the IEnumerable<T> interface for its generic parameter
		var etype = typeof( IEnumerable<> );
		foreach( var bt in type.GetInterfaces() )
			if( bt.IsGenericType && bt.GetGenericTypeDefinition() == etype )
				return bt.GetGenericArguments()[0];

		// now try the non-generic way
		// if it's a dictionary we always return DictionaryEntry
		if( typeof( IDictionary ).IsAssignableFrom( type ) )
			return typeof( DictionaryEntry );

		// if it's a list we look for an Item property with an int index parameter
		// where the property type is anything but object
		if( typeof( IList ).IsAssignableFrom( type ) )
		{
			foreach( var prop in type.GetProperties() )
			{
				if( "Item" == prop.Name && typeof( object ) != prop.PropertyType )
				{
					var ipa = prop.GetIndexParameters();
					if( 1 == ipa.Length && typeof( int ) == ipa[0].ParameterType )
					{
						return prop.PropertyType;
					}
				}
			}
		}

		// if it's a collection we look for an Add() method whose parameter is 
		// anything but object
		if( typeof( ICollection ).IsAssignableFrom( type ) )
		{
			foreach( var meth in type.GetMethods() )
			{
				if( "Add" == meth.Name )
				{
					var pa = meth.GetParameters();
					if( 1 == pa.Length && typeof( object ) != pa[0].ParameterType )
						return pa[0].ParameterType;
				}
			}
		}

		if( typeof( IEnumerable ).IsAssignableFrom( type ) )
			return typeof( object );

		return null;
	}
}
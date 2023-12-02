using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Core.Classes;
using Common.Core.Models;

namespace TestHarness.Reflection;

internal class TestReflection
{
	internal static bool TestOther()
	{
		//GenericTypes.RunTest(); { return; }
		//ReflectionUtility.RunTest(); { return; }
		return true;
	}

	internal static bool Test()
	{
		// Test List
		IList<PersonTest> list = new List<PersonTest>
		{
			CreatePerson(),
			CreatePerson()
		};

		// Test Dictionary
		IDictionary<int, IList<PersonTest>> dict = new Dictionary<int, IList<PersonTest>>
		{
			{ 1, list }
		};

		// Test Array
		//var arr = new int[] { 1, 3, 5, 7, 9 };
		//var arr = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

		if( ReflectionHelper.CreateDeepCopy( dict ) is IDictionary<int, IList<PersonTest>> ret )
		{
			var source = ret[1][0];
			var target = ReflectionHelper.CreateDeepCopy( source ) as PersonTest;

			if( target is not null )
			{
				target.MiddleName = "D";
				target.Address.ZipCode = "32746-4733";
				ReflectionHelper.ApplyChanges( target, source );
			}

			#region Show property info

			// Ignore static and non-public properties
			PropertyInfo[] props = source.GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public );

			foreach( var prop in props )
			{
				var name = prop.Name;

				var ptype = prop.PropertyType;
				string? typName, typNamespace;
				if( ptype.IsGenericType && ptype.GenericTypeArguments.Length > 0 )
				{
					typName = ptype.GenericTypeArguments[0].Name;
					typNamespace = ptype.GenericTypeArguments[0].Namespace;
				}
				else
				{
					typName = ptype.Name;
					typNamespace = ptype.Namespace;
				}

				Console.WriteLine( @$"Property: {name} Type: {typNamespace}.{typName} IsClass: {ptype.IsClass}" );

				// Show data
				var data = prop.GetValue( source );
				Console.WriteLine( $"Value: {data}" );
			}

			#endregion
		}
		return true;
	}

	#region Populate model data

	private static PersonTest CreatePerson()
	{
		return new PersonTest
		{
			Id = 1,
			FirstName = "Kevin",
			Address = CreateAddress(),
			BirthDate = new DateOnly( 1954, 5, 17 )
		};
	}

	private static Address CreateAddress()
	{
		return new Address
		{
			Street = "1731 Oak Springs PL",
			City = "Lake Mary",
			State = "Florida",
			Country = "USA"
		};
	}

	#endregion
}

#region Define an example base class.

public abstract class TestBase
{
	private uint? _id;
	public uint? Id
	{
		get => _id;
		set
		{
			if( value != _id )
			{
				_id = value;
			}
		}
	}

	private string? _firstName;
	public string FirstName
	{
		get => _firstName is not null ? _firstName : string.Empty;
		set
		{
			if( !value.Equals( _firstName ) )
			{
				_firstName = value;
			}
		}
	}

	private string? _middleName;
	public string? MiddleName
	{
		get => _middleName;
		set
		{
			if( value != _middleName )
			{
				_middleName = value;
			}
		}
	}

	private DateOnly? _birthDate;
	public DateOnly? BirthDate
	{
		get => _birthDate;
		set
		{
			if( value != _birthDate )
			{
				_birthDate = value;
			}
		}
	}

	public TestBase()
	{
		FirstName = string.Empty;
	}

	protected string GetFullName()
	{
		var ret = string.Empty;
		if( !string.IsNullOrWhiteSpace( FirstName ) ) ret += FirstName.Trim();
		if( !string.IsNullOrWhiteSpace( MiddleName ) ) ret += " " + MiddleName.Trim();
		return ret;
	}

	protected static int? CalculateAge( DateOnly? date )
	{
		if( !date.HasValue ) return null;
		var dateTime = date.Value.ToDateTime( TimeOnly.MinValue );
		return (int)( ( DateTime.Now - dateTime ).TotalDays / 365.242199 );
	}
}

#endregion

#region Define model classes

public class PersonTest : TestBase
{
	public Address Address { get; set; }

	public int? Age => CalculateAge( BirthDate );

	public string FullName => GetFullName();

	public PersonTest()
	{
		Address = new Address();
	}
}

#endregion
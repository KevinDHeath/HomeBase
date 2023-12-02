using System.Linq;
using Common.Core.Classes;
using Common.Data.Json;

namespace TestHarness;

internal class TestLINQ
{
	internal static bool RunTest()
	{
		AddressData data = new();

		// Min and Max Zip Codes by State
		//foreach( var (s, min, max) in
		//		from s in AddressData.GetZipCodes()
		//		let min = s.Min( z => z.ZipCode )
		//		let max = s.Max( z => z.ZipCode )
		//		select (s, min, max) )
		//{
		//	System.Console.WriteLine( $"{s.Key} Min: {min} Max: {max}" );
		//}

		var city = "Charleston";

		var test = from a in AddressFactory.ZipCodes
				   where a.City == city
				   orderby a.State
				   group a by a.State;
		var count = test.Count();

		var query =
			from state in AddressFactory.States
			join zip in AddressFactory.ZipCodes on state.Alpha equals zip.State
			where zip.City == city
			select new
			{
				State = state.Name,
				state.Capital,
				zip.City,
				zip.ZipCode
			};

		count = query.Count();

		return count > 0;
	}
}
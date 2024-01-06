using System.Linq;
using Common.Core.Classes;
using Common.Data.Json;

namespace TestHarness;

internal class TestLINQ
{
	internal static bool RunTest()
	{
		AddressData data = new();

		//Min and Max Postcodes by Province
		//foreach( var (s, min, max) in
		//		from s in AddressData.GetPostcodes()
		//		let min = s.Min( z => z.Code )
		//		let max = s.Max( z => z.Code )
		//		select (s, min, max) )
		//{
		//	System.Console.WriteLine( $"{s.Key} Min: {min} Max: {max}" );
		//}

		var city = "Charleston";

		var test = from a in AddressFactoryBase.Postcodes
				   where a.City == city
				   orderby a.Province
				   group a by a.Province;
		var count = test.Count();

		var query =
			from province in AddressFactoryBase.Provinces
			join postcode in AddressFactoryBase.Postcodes on province.Code equals postcode.Province
			where postcode.City == city
			select new
			{
				State = province.Name,
				postcode.City,
				postcode.Code
			};

		count = query.Count();

		return count > 0;
	}
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CandidateTesting.WernerMoecke.Convert;

namespace UnitTests
{
	[TestFixture]
	class AgileContentLogConverter
	{
		[Test]
		public void TestConvertLine()
		{
			string _input = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
			string _expected = "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT|";
			string _result = Log.ConvertLine(_input);

			Assert.AreEqual(_expected, _result);
		}

		[Test]
		public async Task TestConvertFileAsync()
		{
			Uri _input = new Uri("https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt");
			string _expected = 
				"\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT|" +
				"\"MINHA CDN\" POST 200 /myImages 319 101 MISS|" +
				"\"MINHA CDN\" GET 404 /not-found 143 199 MISS|" +
				"\"MINHA CDN\" GET 200 /robots.txt 245 312 INVALIDATE|";
			string _result = await Log.ConvertFile(_input);

			Assert.AreEqual(_expected, _result);
		}
	}
}

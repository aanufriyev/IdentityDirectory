using System;

namespace Klaims.Scim.Tests
{
    using IdentityDirectory.Scim.Query;
    using Xunit;

	public class ScimFilterParserTests
    {
		const string SimpleFilter = "title pr and userType eq \"Employee\"";
		const string PrecedenceFilter = "title pr and (userType eq \"Employee\" or userType eq \"ParttimeEmployee\")";
		
		[Fact]
		public void CanParseSimpleFilter()
		{
			var rootNode = FilterExpressionParser.Parse(SimpleFilter);
			Assert.NotNull(rootNode);
			Console.WriteLine(rootNode);
		}
		[Fact]
		public void CanParseFilterWithPrecedence()
		{
			var rootNode = FilterExpressionParser.Parse(PrecedenceFilter);
			Assert.NotNull(rootNode);
			Console.WriteLine(rootNode);
		}
	}
}

using System;

namespace Klaims.Scim.Tests
{
    using IdentityDirectory.Scim.Query;
    using Xunit;

	public class ScimFilterParserTests
    {
		const string SimpleFilter = "title pr and userType eq \"Employee\"";
        const string PathFilter = "name.familyName";
        const string PrecedenceFilter = "title pr and (userType eq \"Employee\" or userType eq \"ParttimeEmployee\")";
        const string CollectionFilter = "addresses[type eq \"work\"].streetAddress";

  //      [Fact]
		//public void CanParseSimpleFilter()
		//{
		//	var rootNode = FilterExpressionParser.Parse(SimpleFilter);
		//	Assert.NotNull(rootNode);
		//	Console.WriteLine(rootNode);
		//}
		//[Fact]
		//public void CanParseFilterWithPrecedence()
		//{
		//	var rootNode = FilterExpressionParser.Parse(PrecedenceFilter);
		//	Assert.NotNull(rootNode);
		//	Console.WriteLine(rootNode);
		//}
  //      [Fact]
  //      public void CanParsePathPrecedence()
  //      {
  //          var rootNode = FilterExpressionParser.Parse(PathFilter);
  //          Assert.NotNull(rootNode);
  //          Console.WriteLine(rootNode);
  //      }
        [Fact]
        public void CanParseCollectionFilter()
        {
            var rootNode = FilterExpressionParser.Parse(CollectionFilter);
            Assert.NotNull(rootNode);
            Console.WriteLine(rootNode);
            Console.WriteLine(rootNode.Operator);
            var expr = (ValueExpression) rootNode;
            Console.WriteLine(expr.Attribute);
            Console.WriteLine(expr.Value);
        }
    }
}

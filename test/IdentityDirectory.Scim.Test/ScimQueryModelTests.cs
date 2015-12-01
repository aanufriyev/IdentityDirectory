namespace Klaims.Scim.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IdentityDirectory.Scim.Query;
    using IdentityDirectory.Scim.Resources;
    using IdentityDirectory.Scim.Services;
    using Klaims.Framework.IdentityMangement.Models;
    using Moq;

    using Xunit;

    public class ScimQueryModelTests
    {
        private const string BasicFilter = "profileUrl pr and displayName co \"Employee\"";
        private const string PathFilter = "name.familyName co \"user3\"";
        private const string Path2Filter = "name.familyName co \"user\"";

        private readonly List<UserAccount> _testUsers = new List<UserAccount>
                                                    {
                                                        new UserAccount { ProfileUrl = "http://myprofile.com", DisplayName = "BestEmployee" },
                                                        new UserAccount { ProfileUrl = "http://myprofile.com", DisplayName = "SomeEmployee" },
                                                        new UserAccount { DisplayName = "BestEmployee" },
                                                    };

        private readonly List<ScimUser> _testResources = new List<ScimUser>
                                                    {
                                                        new ScimUser("2","user2","user2gn","user2fn"),
                                                        new ScimUser("3","user3","user3gn","user3fn")
                                                    };


        [Fact]
        public void CanConvertBasicFilter()
        {
            var mapperMoq = new Mock<IAttributeNameMapper>();
            mapperMoq.Setup(m => m.MapToInternal(It.IsAny<string>())).Returns((string s) => char.ToUpper(s[0]) + s.Substring(1));

            var filterNode = ScimExpressionParser.ParseExpression(BasicFilter);
            var converter = new DefaultFilterBinder();
            var predicate = converter.Bind<UserAccount>(filterNode, string.Empty, false, mapperMoq.Object);
            Assert.NotNull(predicate);
            Console.WriteLine(predicate);
            var usersCount = this._testUsers.AsQueryable().Count(predicate);
            Assert.Equal(2, usersCount);
        }

        //[Fact]
        //public void CanConvertPathFilter()
        //{
        //    var converter = new DefaultFilterBinder();
        //    var nameMapper = new DefaultAttributeNameMapper();
        //    var filterNode = FilterExpressionParser.Parse(PathFilter);
        //    var predicate = converter.Bind<ScimUser>(filterNode, string.Empty, false, nameMapper);
        //    Assert.NotNull(predicate);
        //    Console.WriteLine(predicate);
        //    var usersCount = this._testResources.AsQueryable().Count(predicate);
        //    Assert.Equal(1, usersCount);
        //}

        //[Fact]
        //public void CanConvertPath2Filter()
        //{
        //    var converter = new DefaultFilterBinder();
        //    var nameMapper = new DefaultAttributeNameMapper();
        //    var filterNode = FilterExpressionParser.Parse(Path2Filter);
        //    var predicate = converter.Bind<ScimUser>(filterNode, string.Empty, false, nameMapper);
        //    Assert.NotNull(predicate);
        //    Console.WriteLine(predicate);
        //    var usersCount = this._testResources.AsQueryable().Count(predicate);
        //    Assert.Equal(2, usersCount);
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LdapUserManager.Extensions;
using LdapUserManager.Factory;
using LdapUserManager.Fake;
using LdapUserManager.Proxy;
using LdapUserManager.Result;
using Moq;
using Novell.Directory.Ldap;
using Xunit;

namespace LdapUserManager.Tests
{
    public partial class LdapManagerTests
    {

        #region Success
        [Theory]
        [InlineData(10, new string[] { "someAttr1", "someAttr2" })]
        public void SuccessfullyFetchUser(int resultCount, string[] attributes)
        {
            //arrange
            var fakeResults = new FakeLdapEntry().WithAttribute(attributes).WithMany(resultCount).Cast<LdapEntry>().ToList();
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();

            mockProxy.Setup(x => x.Search(It.IsAny<string[]>())).Returns(fakeResults);
            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            var factory = mockFactory.Object;

            //act
            IList<LdapEntry> entries = new List<LdapEntry>();
            using (var connection = factory.OpenConnection(fakeConfig))
            {

                var manager = new LdapManager(mockProxy.Object);
                entries = manager.Fetch(attributes);
            }

            //assert
            Assert.NotNull(entries);
            Assert.NotEmpty(entries);
            Assert.Equal(entries.Count, resultCount);
            Assert.All(entries, (e) =>
            {
                foreach (var at in attributes)
                {
                    Assert.Equal(attributes.Length, e.getAttributeSet().Count);
                }
            });
        }

        [Theory]
        [InlineData(10)]
        public void SuccessfullyFetchUserWithDefaultAttributes(int resultCount)
        {
            //arrange
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeResults = new FakeLdapEntry().WithAttribute(LdapConfig.DefaultAttributes).WithMany(resultCount).Cast<LdapEntry>().ToList();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();

            mockProxy.Setup(x => x.Search(It.IsAny<string[]>())).Returns(fakeResults);
            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            var factory = mockFactory.Object;

            //act
            IList<LdapEntry> entries = new List<LdapEntry>();
            using (var connection = factory.OpenConnection(fakeConfig))
            {

                var manager = new LdapManager(mockProxy.Object);
                entries = manager.Fetch(LdapConfig.DefaultAttributes);
            }

            //assert
            Assert.NotNull(entries);
            Assert.NotEmpty(entries);
            Assert.Equal(resultCount, entries.Count);
            Assert.All(entries, (e) =>
            {
                foreach (var at in LdapConfig.DefaultAttributes)
                {
                    Assert.Equal(e.getAttributeSet().Count, LdapConfig.DefaultAttributes.Count());
                }
            });
        }
        [Theory]
        [InlineData(10, new string[] { "someAttr1", "someAttr2" })]
        public void SuccessfullyFetchUserwithCustomUserModel(int resultCount, string[] attributes)
        {
            //arrange
            var fakeResults = new FakeLdapEntry().WithAttribute(attributes).WithMany(resultCount).Cast<LdapEntry>().ToList();
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();

            mockProxy.Setup(x => x.Search(It.IsAny<string[]>())).Returns(fakeResults);
            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            var factory = mockFactory.Object;

            //act
            IList<FakeCustomUserModel> entries = new List<FakeCustomUserModel>();
            using (var connection = factory.OpenConnection(fakeConfig))
            {

                var manager = new LdapManager(mockProxy.Object);
                entries = manager.Fetch<FakeCustomUserModel>();
            }

            //assert
            Assert.NotNull(entries);
            Assert.NotEmpty(entries);
            Assert.Equal(resultCount, entries.Count);
        }


        [Fact]
        public void Success_Login_LdpaEntry()
        {
            var username = "validUsername";
            var password = "validPasword";
            var usernameAttribute = "UserNameAttribute";
            var attributes = new string[] { "att1", "att2" };
            string dn = "CN=validUsername,CN=Name,DC=domain,DC=,com";

            //arrange
            var fakeLoginEntry = new FakeLdapEntry(dn).WithAttribute(attributes);
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();

            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            mockProxy.Setup(x => x.FindUser(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fakeLoginEntry);
            mockProxy.Setup(x => x.Bind(dn, password)).Returns(true);
            var factory = mockFactory.Object;
            //act
            LoginResult<LdapEntry> result = null;
            using (var connection = factory.OpenConnection(fakeConfig))
            {
                var manager = new LdapManager(mockProxy.Object);
                result = manager.Login(username, password, usernameAttribute, attributes);
            }

            //assert
            Assert.True(result.IsAuthenticated);
            Assert.Null(result.Exception);
            Assert.Equal(dn, result.User.DN);
        }

        [Fact]
        public void Success_Login_CustomUser()
        {
            var username = "validUsername";
            var password = "validPasword";
            var stringValue = "value";
            var stringArrayValue = new string[] { "value1", "value2" };
            var byteArray = new sbyte[] { 1, 2, 3 };
            var byteArrayArray = new sbyte[][] { new sbyte[] { 1, 2, 3 }, new sbyte[] { 1, 2, 3 }, new sbyte[] { 1, 2, 3 } };

            var customModel = new FakeCustomUserModel();
            //arrange
            var fakeLoginEntry = new FakeLdapEntry()
                                        .WithAttribute("username", username)
                                        .WithAttribute("stringValue", stringValue)
                                        .WithAttribute("stringArrayValue", stringArrayValue)
                                        .WithAttribute("byteArray", byteArray);

            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();

            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            mockProxy.Setup(x => x.FindUser(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fakeLoginEntry);
            mockProxy.Setup(x => x.Bind(It.IsAny<string>(), password)).Returns(true);
            var factory = mockFactory.Object;
            //act
            LoginResult<FakeCustomUserModel> result = null;
            using (var connection = factory.OpenConnection(fakeConfig))
            {
                var manager = new LdapManager(mockProxy.Object);
                result = manager.Login<FakeCustomUserModel>(username, password);
            }

            //assert
            Assert.True(result.IsAuthenticated);
            Assert.Null(result.Exception);
            Assert.Equal(username, result.User.username);
            Assert.Equal(stringValue, result.User.stringValue);
            Assert.Equal(stringArrayValue, result.User.stringArrayValue);
            Assert.Equal(byteArray.Count(), result.User.byteArray.Count());
        }

        [Fact]
        public void Success_Login_LdpaEntry_UsernameWithAt()
        {
            var username = "valid@Username";
            var nameWithoutAt = "validUsername";
            var password = "validPasword";
            var usernameAttribute = "UserNameAttribute";
            string dn = "CN=validUsername,CN=Name,DC=domain,DC=,com";

            //arrange
            var fakeLoginEntry = new FakeLdapEntry(dn).WithAttribute(usernameAttribute, nameWithoutAt);
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();

            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            mockProxy.Setup(x => x.FindUser(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fakeLoginEntry);
            mockProxy.Setup(x => x.Bind(dn, password)).Returns(true);
            var factory = mockFactory.Object;
            //act
            LoginResult<LdapEntry> result = null;
            using (var connection = factory.OpenConnection(fakeConfig))
            {
                var manager = new LdapManager(mockProxy.Object);
                result = manager.Login(username, password, usernameAttribute, null);
            }

            //assert
            Assert.True(result.IsAuthenticated);
            Assert.Null(result.Exception);
            Assert.Equal(dn, result.User.DN);
            Assert.Equal(nameWithoutAt, result.User.getAttribute(usernameAttribute).StringValue);
        }
        #endregion
        #region Fails
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Fail_Login_UsernameNullOrEmpty(string username)
        {
            var password = "validPasword";
            var usernameAttribute = "UserNameAttribute";
            var attributes = new string[] { "att1", "att2" };
            //arrange
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();

            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            var factory = mockFactory.Object;
            //act
            LoginResult<LdapEntry> result = null;
            Action action;
            using (var connection = factory.OpenConnection(fakeConfig))
            {
                var manager = new LdapManager(mockProxy.Object);
                action = () => result = manager.Login(username, password, usernameAttribute, attributes);
            }
            var exception = Assert.Throws<LdapManagerException>(action);
            Assert.Equal("'username' field should not be empty", exception.Message);
            //assert
        }

        [Fact]
        public void Fail_Login_InvalidCredentials()
        {
            var username = "validUsername";
            var password = "invalidPassword";
            //arrange
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();
            var fakeLoginEntry = new FakeLdapEntry();
            var ldapException = new LdapException(It.IsAny<string>(), LdapException.INVALID_CREDENTIALS, It.IsAny<string>());

            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            mockProxy.Setup(x => x.FindUser(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fakeLoginEntry);
            mockProxy.Setup(x => x.Bind(It.IsAny<string>(), It.IsAny<string>())).Throws(ldapException);
            var factory = mockFactory.Object;
            //act
            LoginResult<FakeCustomUserModel> result = null;
            using (var connection = factory.OpenConnection(fakeConfig))
            {
                var manager = new LdapManager(mockProxy.Object);
                result = manager.Login<FakeCustomUserModel>(username, password);
            }

            //assert
            Assert.False(result.IsAuthenticated);
            Assert.NotNull(result.Exception);
            Assert.Equal(LdapException.INVALID_CREDENTIALS, result.Exception.ResultCode);
            //assert
        }


        [Fact]
        public void Fail_Login_Null()
        {
            var username = "validUsername";
            var password = "invalidPassword";
            //arrange
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();
            var ldapException = new LdapException(It.IsAny<string>(), LdapException.INVALID_CREDENTIALS, It.IsAny<string>());

            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            mockProxy.Setup(x => x.FindUser(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>())).Returns((LdapEntry)null);
            var factory = mockFactory.Object;
            //act
            LoginResult<FakeCustomUserModel> result = null;
            using (var connection = factory.OpenConnection(fakeConfig))
            {
                var manager = new LdapManager(mockProxy.Object);
                result = manager.Login<FakeCustomUserModel>(username, password);
            }

            //assert
            Assert.False(result.IsAuthenticated);
            Assert.NotNull(result.Exception);
            Assert.Equal(LdapException.NO_RESULTS_RETURNED, result.Exception.ResultCode);
            //assert
        }

        [Fact]
        public void Fail_Login_NotBound()
        {
            var username = "validUsername";
            var password = "invalidPassword";
            //arrange
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy = new Mock<ILdapProxyClient>();
            var fakeLoginEntry = new FakeLdapEntry();
            var ldapException = new LdapException(It.IsAny<string>(), LdapException.INVALID_CREDENTIALS, It.IsAny<string>());

            mockFactory.Setup(x => x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            mockProxy.Setup(x => x.FindUser(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>())).Returns(fakeLoginEntry);
            mockProxy.Setup(x => x.Bind(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var factory = mockFactory.Object;
            //act
            LoginResult<FakeCustomUserModel> result = null;
            using (var connection = factory.OpenConnection(fakeConfig))
            {
                var manager = new LdapManager(mockProxy.Object);
                result = manager.Login<FakeCustomUserModel>(username, password);
            }

            //assert
            Assert.False(result.IsAuthenticated);
            Assert.NotNull(result.Exception);
            Assert.Equal(LdapException.AUTH_UNKNOWN, result.Exception.ResultCode);
            //assert
        }

    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using LdapUserManager.Factory;
using LdapUserManager.Fake;
using LdapUserManager.Proxy;
using Moq;
using Novell.Directory.Ldap;
using Xunit;

namespace LdapUserManager.Tests
{
    public class LdapUserManagerTests
    {

        [Theory]
        [InlineData(10, new string[] { "someAttr1", "someAttr2" })]
        public void SuccessfullyFetchUser(int resultCount, string[] attributes)
        {
            //arrange
            var fakeResults = new FakeLdapEntry().WithAttribute(attributes).WithMany(resultCount).Cast<LdapEntry>().ToList();
            var fakeConfig = new FakeLdapConfig().WithDefaults();
            var fakeLdapManagerConnection = new FakeLdapManagerConnection();
            var mockFactory = new Mock<ILdapConnectionFactory>();
            var mockProxy  = new Mock<ILdapProxyClient>();
            
            mockProxy.Setup(x=>x.Search(It.IsAny<string[]>())).Returns(fakeResults);
            mockFactory.Setup(x=>x.OpenConnection(It.IsAny<LdapConfig>())).Returns(fakeLdapManagerConnection);
            var factory=mockFactory.Object;

            //act
            IList<LdapEntry> entries = new List<LdapEntry>();
            using(var connection =factory.OpenConnection(fakeConfig)){

                    var manager = new LdapManager(mockProxy.Object);
                     entries = manager.Fetch(attributes);
            }

            //assert
            Assert.NotNull(entries);
            Assert.NotEmpty(entries);
            Assert.Equal(entries.Count, resultCount);
            Assert.All(entries,(e)=>{
                foreach(var at in attributes){
                        Assert.Equal(attributes.Length,e.getAttributeSet().Count);
                }
            });
        }
    }
}

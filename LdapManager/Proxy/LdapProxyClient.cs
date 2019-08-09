using System.Collections.Generic;
using LdapUserManager.Connection;
using LdapUserManager.Extensions;
using Novell.Directory.Ldap;

namespace LdapUserManager.Proxy
{
    public class LdapProxyClient : ILdapProxyClient
    {
        private LdapManagerConnection _connection;
        public LdapProxyClient(ILdapManagerConnection connection)
        {
            _connection = (LdapManagerConnection)connection;
        }
        public bool Bind(string dn, string passwd)
        {
            _connection.Bind(dn, passwd);
            return _connection.Bound;
        }

        public LdapEntry FindUser(string[] attrs, string usernameAttribute, string username)
        {
            var config = _connection.GetConfig();
            var searchFilter = $"(&{string.Format(config.SearchFilter,config.UserObjectClass)}({usernameAttribute}={username}))";
            var searchResults = _connection.Search(
                                  @base: config.SearchBase,
                                  scope: LdapConnection.SCOPE_SUB,
                                  filter: searchFilter,
                                  attrs: attrs,
                                  typesOnly: false);
            return  searchResults.next();
        }
        public IList<LdapEntry> Search(string[] attrs)
        {
            var config = _connection.GetConfig();
            List<LdapEntry> entries = new List<LdapEntry>();
            var searchResults = _connection.Search(
                                  @base: config.SearchBase,
                                  scope: LdapConnection.SCOPE_SUB,
                                  filter: string.Format(config.SearchFilter,config.UserObjectClass),
                                  attrs: attrs,
                                  typesOnly: false
                                  );

            while (searchResults.hasMore())
            {
                try
                {
                    var entry = searchResults.next();
                    entries.Add(entry);
                }

                catch (LdapException)
                {
                    continue;
                }
            }
            return entries;
        }
    }
}
using System.Collections.Generic;
using LdapUserManager.Connection;
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

        public IList<LdapEntry> Search(string[] attrs)
        {
            List<LdapEntry> entries = new List<LdapEntry>();
            var config = _connection.GetConfig();
            var searchResults = _connection.Search(
                                  @base: config.SearchBase,
                                  scope: LdapConnection.SCOPE_SUB,
                                  filter: config.SearchFilter,
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
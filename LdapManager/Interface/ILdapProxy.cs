using System.Collections.Generic;
using Novell.Directory.Ldap;

namespace LdapUserManager.Proxy
{
    public interface ILdapProxyClient
    {

        IList<LdapEntry> Search(string[] attrs);
        LdapEntry FindUser(string[] attrs, string usernameAttribute, string username);
        bool Bind(string dn, string passwd);
    }
}
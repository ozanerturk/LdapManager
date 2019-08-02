using System.Collections.Generic;
using Novell.Directory.Ldap;

namespace LdapUserManager.Proxy
{
    public interface ILdapProxyClient
    {

        IList<LdapEntry> Search(string[] attrs);
        bool Bind(string dn, string passwd);
    }
}
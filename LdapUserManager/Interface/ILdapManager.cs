using System.Collections.Generic;
using Novell.Directory.Ldap;

namespace LdapUserManager
{
    public interface ILdapManager
    {
        IList<LdapEntry> Fetch(string[] ldapAttributes);
        IList<T> Fetch<T>() where T : class,new();
        LdapEntry Verify(string username, string password,string usernameAttribute,string[] ldapAttributes);
        T  Verify<T>(string username, string password) where T : class,new();
    }

}
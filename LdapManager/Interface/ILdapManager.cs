using System.Collections.Generic;
using LdapUserManager.Result;
using Novell.Directory.Ldap;

namespace LdapUserManager
{
    public interface ILdapManager
    {
        IList<LdapEntry> Fetch(string[] ldapAttributes);
        IList<T> Fetch<T>() where T : class,new();
        LoginResult<LdapEntry> Login(string username, string password,string usernameAttribute,string[] ldapAttributes);
        LoginResult<T>  Login<T>(string username, string password) where T : class,new();
    }

}
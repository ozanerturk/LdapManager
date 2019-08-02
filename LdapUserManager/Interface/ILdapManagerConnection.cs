using System;
using Novell.Directory.Ldap;

namespace LdapUserManager.Connection
{
    public interface ILdapManagerConnection :ILdapConnection
    {
         LdapConfig GetConfig();
    }
}
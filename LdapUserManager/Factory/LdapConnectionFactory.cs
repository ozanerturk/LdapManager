
using LdapUserManager.Connection;
using Novell.Directory.Ldap;

namespace LdapUserManager.Factory
{
    public class LdapConnectionFactory : ILdapConnectionFactory
    {
        public  ILdapManagerConnection OpenConnection(LdapConfig config)
        {
            var connection = new LdapManagerConnection(config);
            connection.Connect(config.Host, config.Port);
            connection.Bind(config.BindDn, config.BindPassword);
            return connection;
        }
  
    }
}
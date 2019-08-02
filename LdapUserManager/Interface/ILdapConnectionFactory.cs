using LdapUserManager.Connection;

namespace LdapUserManager.Factory
{
    public interface ILdapConnectionFactory
    {
          ILdapManagerConnection OpenConnection(LdapConfig config);
    }
}
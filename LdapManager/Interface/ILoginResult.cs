using Novell.Directory.Ldap;

namespace LdapUserManager.Result
{
    public interface ILoginResult<T> where T : class
    {
        T User { get; }
        bool IsAuthenticated { get; }
        LdapException Exception { get; }
    }
}
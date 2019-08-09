using Novell.Directory.Ldap;

namespace LdapUserManager.Result
{
    public class LoginResult<T> : ILoginResult<T> where T : class
    {
        public LoginResult(T user)
        {
            IsAuthenticated = true;
            User = user;
        }

        public LoginResult(LdapException ex){
            IsAuthenticated = false;
            Exception = ex;
        }

        public T User { get; }
        public bool IsAuthenticated { get;  }
        public LdapException Exception {get;} 
    }
}
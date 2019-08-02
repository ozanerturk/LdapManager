

using Novell.Directory.Ldap;

namespace LdapUserManager.Connection{

    public class LdapManagerConnection : LdapConnection,ILdapManagerConnection{
        
        private LdapConfig _config;
        protected LdapManagerConnection(){}
        public LdapManagerConnection(LdapConfig config)
        {
            this._config = config;
        }
        public LdapConfig GetConfig() => _config;
    }
}
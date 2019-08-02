namespace LdapUserManager.Fake{

   public class FakeLdapConfig :LdapConfig{
        public FakeLdapConfig WithDefaults(){
            this.BindDn = "default";
            this.BindPassword = "default";
            this.Host= "default";
            this.SearchBase = "default";
            this.SearchFilter = "default";
            this.UserObjectClass = "default";
            return this;
        }

    }
}
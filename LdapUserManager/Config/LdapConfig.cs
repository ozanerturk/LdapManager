public partial class LdapConfig
{
    public LdapConfig()
    {
        UserObjectClass = "user";
        SearchFilter = "(objectClass={0})";
    }

    public string Host { get; set; }
    public int Port = 389;
    public string BindDn { get; set; }
    public string BindPassword { get; set; }
    public string SearchBase { get; set; }
    public string UserObjectClass { get; set; }
    public string SearchFilter { get; set; }
    public static string[] DefaultAttributes => new string[] {
        "memberOf",
        "displayName",
        "sAMAccountName",
        "mail",
        "name",
        "sn",
        "phoneNumber",
        "title",
        "manager",
        "jpegPhoto" 
    };

}
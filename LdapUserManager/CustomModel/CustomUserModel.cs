using LdapUserManager;

public class CustomUserModel
{
    [LdapExtract("displayName")]
    public string DisplayName { get; set; }

    [LdapExtract("name")]
    public string Name { get; set; }

    [LdapExtract("sn")]
    public string Surname { get; set; }

    [LdapExtract("sAMAccountName",IsUsername:true)]
    public string Username { get; set; }

    [LdapExtract("mail")]
    public string Email { get; set; }

    [LdapExtract("memberOf")]
    public string MemberOf { get; set; }

    [LdapExtract("mobile")]
    public string PhoneNumber { get; set; }

    [LdapExtract("title")]
    public string Title { get; set; }

    [LdapExtract("manager")]
    public string Manager { get; set; }

    [LdapExtract("thumbnailPhoto")]
    public byte[] Image { get; set; }


    public override string ToString()
    {
        return $"DisplayName:{DisplayName},Name:{Name},Surname:{Surname},Title:{Title},Image:{(Image != null ? "yes" : "no")}";
    }
}
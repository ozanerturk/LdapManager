namespace LdapUserManager.Fake
{
    public class FakeCustomUserModel
    {

        [LdapExtract("stringValue")]
        public string stringValue { get; set; }

        [LdapExtract("stringArrayValue")]
        public string[] stringArrayValue { get; set; }

        [LdapExtract("byteArray")]
        public byte[] byteArray { get; set; }

        [LdapExtract("byteArrayArray")]//ex. image list
        public byte[][] byteArrayArray { get; set; }

        [LdapExtract("username", IsUsername: true)]
        public string username { get; set; }
    
    }

}

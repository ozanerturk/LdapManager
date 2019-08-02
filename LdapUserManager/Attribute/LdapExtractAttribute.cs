
using System;

namespace LdapUserManager
{
        public class LdapExtractAttribute : Attribute
        {
            public string attributeName;
            public bool IsUsername;
            public LdapExtractAttribute(string attributeName,bool IsUsername = false)
            {
                this.attributeName = attributeName;
                this.IsUsername = IsUsername;
            }
        }
}

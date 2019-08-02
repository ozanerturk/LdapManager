using System;
using System.Collections.Generic;
using Novell.Directory.Ldap;

namespace LdapUserManager.Fake
{

    public class FakeLdapEntry : LdapEntry
    {
            public string[] attributes {get;set;}
        public FakeLdapEntry()
        {
            this.attrs = new LdapAttributeSet();
        }

        public FakeLdapEntry WithAttribute(string [] attributes)
        {
            this.attributes=attributes;
            foreach(var at in attributes){
                    this.attrs.Add(new LdapAttribute(at, "value"));
            }
            return this;
        }

        public List<FakeLdapEntry> WithMany(int count)
        {
            var list = new List<FakeLdapEntry>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new FakeLdapEntry().WithAttribute(this.attributes));
            }
            return list;
        }
    }

}


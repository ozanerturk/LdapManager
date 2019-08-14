using System;
using System.Collections.Generic;
using Novell.Directory.Ldap;

namespace LdapUserManager.Fake
{

    public class FakeLdapEntry : LdapEntry
    {
        public string[] attributes { get; set; }
        public FakeLdapEntry(string dn) : base(dn)
        {
            this.attrs = new LdapAttributeSet();

        }
        public FakeLdapEntry()
        {

            this.attrs = new LdapAttributeSet();
        }

        public FakeLdapEntry WithAttribute(string[] attributes)
        {
            this.attributes = attributes;
            foreach (var at in attributes)
            {
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
         public FakeLdapEntry WithAttribute(string s, sbyte[][] v)
        {
            var attr = new  LdapAttribute(s);
            foreach(sbyte[] i in v){
                    attr.addValue(i);
            }
            this.attrs.Add(attr);
            return this;
        }
        public FakeLdapEntry WithAttribute(string s, sbyte[] v)
        {
            this.attrs.Add(new LdapAttribute(s, v));
            return this;
        }
        public FakeLdapEntry WithAttribute(string s, string v)
        {
            this.attrs.Add(new LdapAttribute(s, v));
            return this;
        }
        public FakeLdapEntry WithAttribute(string s, string[] v)
        {
            this.attrs.Add(new LdapAttribute(s, v));
            return this;
        }
  public FakeLdapEntry WithBound(string s, string[] v)
        {
            return this;
        }
     
    }

}


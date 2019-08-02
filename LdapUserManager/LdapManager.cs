using System;
using System.Collections.Generic;
using Novell.Directory.Ldap;
using System.Linq;
using LdapUserManager.Extensions;
using LdapUserManager.Proxy;
using LdapUserManager.Connection;

namespace LdapUserManager
{
    public class LdapManager : ILdapManager
    {
        private ILdapProxyClient _ldapProxy;
        public LdapManager(ILdapManagerConnection connection) : this(new LdapProxyClient(connection))
        {
        }
        public LdapManager(ILdapProxyClient ldapProxy)
        {
            this._ldapProxy = ldapProxy;
        }

        private T MapModel<T>(LdapEntry entry) where T : class, new()
        {
            T model = new T();
            var propertyInfos = typeof(T).GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var attribute = propertyInfo.GetCustomAttributes(typeof(LdapExtractAttribute), false).Cast<LdapExtractAttribute>().FirstOrDefault();
                if (attribute != null)
                {
                    var attributeName = attribute.attributeName;
                    var propertyType = propertyInfo.PropertyType;
                    if (propertyType == typeof(string))
                    {
                        var value = entry.getAttribute(attributeName)?.StringValue;
                        propertyInfo.SetValue(model, value);

                    }
                    else if (propertyType == typeof(string[]))
                    {
                        var value = entry.getAttribute(attributeName)?.StringValueArray;
                        propertyInfo.SetValue(model, value);
                    }
                    else if (propertyType == typeof(byte[]))
                    {
                        var value = (byte[])(Array)entry.getAttribute(attributeName)?.ByteValue;
                        propertyInfo.SetValue(model, value);

                    }
                    else if (propertyType == typeof(byte[][]))
                    {
                        var value = (byte[][])(Array)entry.getAttribute(attributeName)?.ByteValueArray;
                        propertyInfo.SetValue(model, value);
                    }

                }
            }
            return model;
        }

        public IList<T> Fetch<T>() where T : class, new()
        {
            List<T> models = new List<T>();

            string[] ldapAttributes = typeof(T).getExtractLdapAttributesFromCustomAttributes();

            var entries = this.Fetch(ldapAttributes.ToArray());

            foreach (var entry in entries)
            {
                T model = MapModel<T>(entry);
                models.Add(model);
            }

            return models;
        }
        public IList<LdapEntry> Fetch(string[] attributes)
        {
            attributes = attributes.ToList().Where(x => !string.IsNullOrEmpty(x)).ToArray();

            var entries = _ldapProxy.Search(attributes);

            return entries;
        }
        public T Verify<T>(string username, string password) where T : class, new()
        {
            var attributes = typeof(T).getExtractLdapAttributesFromCustomAttributes();
            var usernameLdapAttributeName = typeof(T).getUsernameAttribute();


            var entry = this.Verify(username, password, usernameLdapAttributeName, attributes);
            if (entry != null)
            {
                return MapModel<T>(entry);
            }
            else
            {
                return null;
            }

        }
        public LdapEntry Verify(string username, string password, string usernameAttribute, string[] attributes)
        {

            username = username.ToLower();

            if (username.Contains('@'))
            {
                username = username.Remove(username.IndexOf('@'));
            }

            var result = _ldapProxy.Search(attributes).FirstOrDefault();

            if (result != null)
            {
                var IsBound = _ldapProxy.Bind(result.DN, password);
                if (IsBound)
                {
                    return result;
                }
            }
            return null;

        }
    }
}
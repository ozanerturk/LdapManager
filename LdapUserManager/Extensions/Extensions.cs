using System;
using System.Linq;

namespace LdapUserManager.Extensions
{
    public static class ManagerExtensions
    {

        public static string[] getExtractLdapAttributesFromCustomAttributes(this Type type)
        {
            return type.GetProperties().Select(x =>
                                    {
                                        return x.GetCustomAttributes(typeof(LdapExtractAttribute), false)
                                                    .Cast<LdapExtractAttribute>()
                                                    .FirstOrDefault()?.attributeName ?? "";
                                    }).Where(x => !string.IsNullOrEmpty(x))
                                    .ToArray();
        }

        public static string getUsernameAttribute(this Type type)
        {
            var usernameAttribute = type.GetProperties()
                                        .Select(x =>
                                        {
                                            return x.GetCustomAttributes(typeof(LdapExtractAttribute), false)
                                                        .Cast<LdapExtractAttribute>()
                                                        .FirstOrDefault();
                                        })
                                        .Where(x => x.IsUsername)
                                        .FirstOrDefault();
            if(usernameAttribute == null){
                throw new LdapManagerException("You need to set one of the LdapExtractAttribute as username field");
            }
            return usernameAttribute.attributeName;
        }
    }

}
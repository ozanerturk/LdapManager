# Simple Ldap Manager 

Easily read ldap user enrties and authenticate them. 

[![Build Status](https://travis-ci.org/ozanerturk/LdapManager.svg?branch=master)](https://travis-ci.org/ozanerturk/LdapUserManager)
[![codecov](https://codecov.io/gh/ozanerturk/LdapManager/branch/master/graph/badge.svg)](https://codecov.io/gh/ozanerturk/LdapManager)
This is a simple library that helps to map and authenticate Ldap users easily. It is handling common connection case with provided configuration.





## Usage

Create your own custom usermodel class like below. [LdapExtract](LdapManager/Attribute/LdapExtractAttribute.cs) attribute indicates the attribute of LDAP entry
```csharp
public class CustomUserModel
{
    [LdapExtract("displayName")]
    public string DisplayName { get; set; }
    
    [LdapExtract("sAMAccountName",IsUsername:true)]
    public string Username { get; set; }

    [LdapExtract("name")]
    public string Name { get; set; }
    
    [LdapExtract("sn")]
    public string Surname { get; set; }

}
```

And open connection, create manager instance and fetch users based on the configuration

```csharp
  var config = new LdapConfig(){
                Host ="localhost",
                Port=389, //default
                BindDn="CN=admin,CN=Users,DC=example,DC=com",
                BindPassword="secret",
                SearchBase="CN=Users,DC=example,DC=com",
                UserObjectClass = "user",//default
                SearchFilter = "(objectClass={0})"//default
            };
            
ILdapConnectionFactory ldapConnectionFactory = new LdapConnectionFactory();

using(var connection = ldapConnectionFactory.OpenConnection(config)){

    ILdapManager manager = new LdapManager(connection);
    IList<CustomUserModel> users = manager.Fetch<CustomUserModel>();
    
}
```

or verify a user

```csharp
using(var connection = ldapConnectionFactory.OpenConnection(config)){

    ILdapManager manager = new LdapManager(connection);
    LoginResult users = manager.Login<CustomUserModel>("tbmm01","33301");
    
}
```



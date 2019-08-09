using System;
using System.Runtime.Serialization;

namespace LdapUserManager.Extensions
{
    [Serializable]
    internal class LdapManagerException : Exception
    {
        public LdapManagerException()
        {
        }

        public LdapManagerException(string message) : base(message)
        {
        }

        public LdapManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LdapManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
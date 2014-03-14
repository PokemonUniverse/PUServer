using System;
using NoNameLib;

namespace Server.Logic.Exceptions
{
    public class StartupException : NoNameLibException
    {
        public StartupException(Enum errorEnumValue) : base(errorEnumValue)
        {
        }

        public StartupException(Enum errorEnumValue, Exception ex) : base(errorEnumValue, ex)
        {
        }

        public StartupException(Enum errorEnumValue, string message, params object[] args) : base(errorEnumValue, message, args)
        {
        }

        public StartupException(Enum errorEnumValue, Exception ex, string message, params object[] args) : base(errorEnumValue, ex, message, args)
        {
        }
    }
}

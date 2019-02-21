using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MessageRespondAttribute : Attribute
    {
    }
}

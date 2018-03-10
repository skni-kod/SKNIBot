using System;

namespace SKNIBot.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandsGroupAttribute : Attribute
    {
        public string Group { get; }

        public CommandsGroupAttribute(string group)
        {
            Group = group;
        }
    }
}

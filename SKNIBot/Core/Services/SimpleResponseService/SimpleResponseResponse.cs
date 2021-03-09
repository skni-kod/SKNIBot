using SKNIBot.Core.Database.Models.StaticDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Services.SimpleResponseService
{
    public enum SimpleResponseResult { Ok, NoSuchCommand, CommandHasNoResponses }
    public class SimpleResponseResponse<T>
    {
        public SimpleResponseResult Result { get; }
        public T Responses { get; }
        public SimpleResponseResponse(SimpleResponseResult result)
        {
            Result = result;
        }

        public SimpleResponseResponse(T responses)
        {
            Result = SimpleResponseResult.Ok;
            Responses = responses;
        }
    }

    public class SimpleResponseElement
    {
        public string Content { get; }
        public SimpleResponseType Type { get; }
        public SimpleResponseElement(string content, SimpleResponseType type)
        {
            Content = content;
            Type = type;
        }
    }
}

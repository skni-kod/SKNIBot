using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models.StaticDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKNIBot.Core.Services.MessageResponseService
{
    class MessageResponseService
    {
        private Dictionary<string, string> MessageResponses;

        public MessageResponseService()
        {
            MessageResponses = new Dictionary<string, string>();

            using (var databaseContext = new StaticDBContext())
            {
                List<MessageResponse> responses = databaseContext.MessageResponses.Where(p => p.IsDeleted == false  ).ToList();
                foreach(var response in responses)
                {
                    MessageResponses.Add(response.Message, response.Response);
                }
            }
        }

        public List<string> GetResponses(string message)
        {
            List<string> responses = new List<string>();

            foreach(var response in MessageResponses)
            {
                if(message.Contains(response.Key, StringComparison.OrdinalIgnoreCase))
                {
                    responses.Add(response.Value);
                }
            }

            return responses;
        }
    }
}

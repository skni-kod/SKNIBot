using SKNIBot.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKNIBot.Core.Services.SimpleResponseService
{
    class SimpleResponseService
    {
        private Random _random;

        public SimpleResponseService()
        {
            _random = new Random();
        }
        public string GetAnswer(string commandName)
        {
            using (var databaseContext = new StaticDBContext())
            {

                var responses = databaseContext.SimpleResponses.Where(p => p.Command.Name == commandName);
                var randomIndex = _random.Next(responses.Count());

                var response = responses
                    .OrderBy(p => p.ID)
                    .Select(p => p.Content)
                    .Skip(randomIndex)
                    .First();
                return response;
            }
        }
    }
}

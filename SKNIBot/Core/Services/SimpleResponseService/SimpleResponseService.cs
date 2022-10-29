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
        public SimpleResponseResponse<SimpleResponseElement> GetAnswer(string commandName)
        {
            using (var databaseContext = new StaticDBContext())
            {
                var command = databaseContext.Commands.Where(p => p.Name == commandName).FirstOrDefault();
                if(command == null)
                {
                    return new SimpleResponseResponse<SimpleResponseElement>(SimpleResponseResult.NoSuchCommand);
                }

                var responses = databaseContext.SimpleResponses.Where(p => p.Command.Name == commandName && p.IsDeleted == false);
                var randomIndex = _random.Next(responses.Count());

                if(responses.Count() == 0)
                {
                    return new SimpleResponseResponse<SimpleResponseElement>(SimpleResponseResult.CommandHasNoResponses);
                }

                var response = responses
                    .OrderBy(p => p.ID)
                    .Skip(randomIndex)
                    .First();

                var list = new List<SimpleResponseElement>();
                return new SimpleResponseResponse<SimpleResponseElement>(new SimpleResponseElement(response.Content, response.Type ));
            }
        }

        public SimpleResponseResponse<List<SimpleResponseElement>> GetAnswers(string commandName)
        {
            using (var databaseContext = new StaticDBContext())
            {
                var command = databaseContext.Commands.Where(p => p.Name == commandName).FirstOrDefault();
                if (command == null)
                {
                    return new SimpleResponseResponse<List<SimpleResponseElement>>(SimpleResponseResult.NoSuchCommand);
                }

                var responses = databaseContext.SimpleResponses.Where(p => p.Command.Name == commandName && p.IsDeleted == false);
                if (responses.Count() == 0)
                {
                    return new SimpleResponseResponse<List<SimpleResponseElement>>(SimpleResponseResult.CommandHasNoResponses);
                }

                var list = new List<SimpleResponseElement>();
                foreach(var response in responses)
                {
                    list.Add(new SimpleResponseElement(response.Content, response.Type));
                }
                return new SimpleResponseResponse<List<SimpleResponseElement>>(list);
            }
        }
    }
}

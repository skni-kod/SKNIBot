using Microsoft.EntityFrameworkCore;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKNIBot.Core.Services.UserMessageStatsService
{
    class UserMessageStatsService
    {

        public bool UpdateGroupedMessageCount(IEnumerable<KeyValuePair<ulong, int>> data, ulong channelId,
            ulong serverId)
        {
            foreach (var (user, count) in data)
            {
                using (var databaseContext = new DynamicDBContext())
                {
                    Server dbServer = GetServerFromDatabase(databaseContext, serverId); 
                    UserMessageStat newStat = new UserMessageStat(count, channelId, user)
                    {
                        Server =  dbServer
                    };
                    
                    databaseContext.Add(newStat);
                    databaseContext.SaveChanges();
                }
            }
            return false;
        }

        public IEnumerable<KeyValuePair<ulong, int>> FetchGroupedMessageCount(ulong serverId, ulong channelId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, serverId);
                return dbServer.UserMessageStats.Where(p => p.ChannelID == channelId).Select(p => new KeyValuePair<ulong, int>(p.User, p.count)).ToList();
            }
        }
        private Server GetServerFromDatabase(DynamicDBContext databaseContext, ulong serverId)
         {
             Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).Include(p => p.UserMessageStats).FirstOrDefault();
 
             //If server is not present in database add it.
             if (dbServer == null)
             {
                 dbServer = new Server(serverId)
                 {
                     UserMessageStats = new List<UserMessageStat>()
                 };
                 databaseContext.Add(dbServer);
                 databaseContext.SaveChanges();
             }
             return dbServer;
         }
    }
}

using Microsoft.EntityFrameworkCore;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKNIBot.Core.Services.DateMessageStatsService
{
    class DateMessageStatsService
    {
        public bool UpdateGroupedMessageCount(IEnumerable<KeyValuePair<string, int>> data, ulong channelId,
            ulong serverId)
        {
            foreach (var (date, count) in data)
            {
                using (var databaseContext = new DynamicDBContext())
                {
                    Server dbServer = GetServerFromDatabase(databaseContext, serverId); 
                    DateMessageStat newStat = new DateMessageStat(count, channelId, date)
                    {
                        Server =  dbServer
                    };
                    
                    databaseContext.Add(newStat);
                    databaseContext.SaveChanges();
                }
            }
            return false;
        }

        public IEnumerable<KeyValuePair<string, int>> FetchGroupedMessageCount(ulong serverId, ulong channelId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, serverId);
                return dbServer.DateMessageStats.Where(p => p.ChannelID == channelId).Select(p => new KeyValuePair<string, int>(p.Date, p.count)).ToList();
            }
        }
        private Server GetServerFromDatabase(DynamicDBContext databaseContext, ulong serverId)
         {
             Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).Include(p => p.DateMessageStats).FirstOrDefault();
 
             //If server is not present in database add it.
             if (dbServer == null)
             {
                 dbServer = new Server(serverId)
                 {
                     DateMessageStats = new List<DateMessageStat>()
                 };
                 databaseContext.Add(dbServer);
                 databaseContext.SaveChanges();
             }
             return dbServer;
         }
    }
}

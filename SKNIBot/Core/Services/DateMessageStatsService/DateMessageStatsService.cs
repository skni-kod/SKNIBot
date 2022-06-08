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
            using (var databaseContext = new DynamicDBContext())
            {
                var statList = new List<DateMessageStat>();
                foreach (var (date, count) in data)
                {
                    {
                        Server dbServer = GetServerFromDatabase(databaseContext, serverId);
                        DateMessageStat newStat = new DateMessageStat(count, channelId.ToString(), date)
                        {
                            Server = dbServer
                        };

                        statList.Add(newStat);
                        
                    }
                }
                databaseContext.AddRange(statList);
                databaseContext.SaveChanges();
            }

            return false;
        }

        public IEnumerable<KeyValuePair<string, int>> FetchGroupedMessageCount(ulong serverId, ulong channelId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, serverId);
                return dbServer.DateMessageStats.Where(p => ulong.Parse(p.ChannelID) == channelId).Select(p => new KeyValuePair<string, int>(p.Date, p.count)).ToList();
            }
        }
        public void DeleteServerCache(ulong serverId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                var serverIdString = serverId.ToString();
                var toDelete = databaseContext.DateMessageStats.Where(p => databaseContext.Servers.FirstOrDefault(s => s.ID == p.ServerID).ServerID == serverIdString);
                databaseContext.DateMessageStats.RemoveRange(toDelete);
                databaseContext.SaveChanges();
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

        public HashSet<ulong> GetServerList()
        {
            using (var databaseContext = new DynamicDBContext())
            {
                var servers = databaseContext.Servers.Select(p => ulong.Parse(p.ServerID)).ToHashSet();
                return servers;
            }
        }
    }
}

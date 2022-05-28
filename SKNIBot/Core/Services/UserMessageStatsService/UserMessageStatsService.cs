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
            using (var databaseContext = new DynamicDBContext())
            {
                var statList = new List<UserMessageStat>();
                foreach (var (user, count) in data)
                {
                        Server dbServer = GetServerFromDatabase(databaseContext, serverId); 
                        UserMessageStat newStat = new UserMessageStat(count, channelId.ToString(), user.ToString())
                        {
                            Server =  dbServer
                        };
                        
                        statList.Add(newStat);
                }
                databaseContext.AddRange(statList);
                databaseContext.SaveChanges();
            }
            return false;
        }

        public IEnumerable<KeyValuePair<ulong, int>> FetchGroupedMessageCount(ulong serverId, ulong channelId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, serverId);
                return dbServer.UserMessageStats.Where(p => ulong.Parse(p.ChannelID) == channelId).Select(p => new KeyValuePair<ulong, int>(ulong.Parse(p.UserID), p.count)).ToList();
            }
        }

        public void DeleteServerCache(ulong serverId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                var serverIdString = serverId.ToString();
                var toDelete = databaseContext.UserMessageStats.Where(p => databaseContext.Servers.FirstOrDefault(s => s.ID == p.ServerID).ServerID == serverIdString);
                databaseContext.UserMessageStats.RemoveRange(toDelete);
                databaseContext.SaveChanges();
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

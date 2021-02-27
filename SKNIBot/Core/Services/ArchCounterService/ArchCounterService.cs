using Microsoft.EntityFrameworkCore;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKNIBot.Core.Services.ArchCounterService
{
    class ArchCounterService
    {
        public int GetCounter(ulong serverId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).Include(p => p.ServerVariable).FirstOrDefault();

                if (dbServer != null && dbServer.ServerVariable != null)
                {
                    return dbServer.ServerVariable.ArchCounter;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int IncrementCounter(ulong serverId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).Include(p => p.ServerVariable).FirstOrDefault();

                if (dbServer == null)
                {
                    dbServer = new Server(serverId);
                    databaseContext.Add(dbServer);
                }

                if (dbServer.ServerVariable == null)
                {
                    dbServer.ServerVariable = new ServerVariable();
                }
                dbServer.ServerVariable.ArchCounter++;
                databaseContext.SaveChanges();
                return dbServer.ServerVariable.ArchCounter;
            }
        }

        public int ResetCounter(ulong serverId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).Include(p => p.ServerVariable).FirstOrDefault();

                if (dbServer == null)
                {
                    dbServer = new Server(serverId);
                    databaseContext.Add(dbServer);
                }

                if (dbServer.ServerVariable == null)
                {
                    dbServer.ServerVariable = new ServerVariable();
                }
                dbServer.ServerVariable.ArchCounter = 0;
                databaseContext.SaveChanges();
                return dbServer.ServerVariable.ArchCounter;
            }
        }
    }
}

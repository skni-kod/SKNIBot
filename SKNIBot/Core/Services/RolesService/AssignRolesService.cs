using Microsoft.EntityFrameworkCore;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKNIBot.Core.Services.RolesService
{
    class AssignRolesService
    {
        public enum GiveRoleStatus { RoleGiven, AlreadyPossesed, RoleDoesntExist };

        public List<ulong> GetRoles(ulong serverId)
        {
            List<ulong> roles = new List<ulong>();
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, serverId);

                return dbServer.AssignRoles.Select(p => ulong.Parse(p.RoleID)).ToList();
            }
        }

        public bool IsRoleOnList(ulong roleId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                if (databaseContext.AssignRoles.FirstOrDefault(p => p.RoleID == roleId.ToString()) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void AddRoleToDatabase(ulong serverId, ulong roleId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                AssignRole assingRole = new AssignRole(roleId)
                {
                    Server = GetServerFromDatabase(databaseContext, serverId)
                };
                databaseContext.Add(assingRole);
                databaseContext.SaveChanges();
            }
        }

        public void RemoveRoleFromDatabase(ulong serverId, ulong roleId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, serverId);
                dbServer.AssignRoles.RemoveAll(p => p.RoleID == roleId.ToString());
                databaseContext.SaveChanges();
            }
        }

        private Server GetServerFromDatabase(DynamicDBContext databaseContext, ulong serverId)
        {
            Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).Include(p => p.AssignRoles).FirstOrDefault();

            //If server is not present in database add it.
            if (dbServer == null)
            {
                dbServer = new Server(serverId)
                {
                    AssignRoles = new List<AssignRole>()
                };
                databaseContext.Add(dbServer);
                databaseContext.SaveChanges();
            }
            return dbServer;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysTeamsRepository : ISysTeamsRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysTeamsRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<(int rowCount, IEnumerable<SysTeams> SystemTeamsList)> GetSystemTeamsList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemTeamss @sysID, @cultureID, @pageIndex, @pageSize;", new { sysID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemTeamsList = multi.Read<SysTeams>();

                return (rowCount, systemTeamsList);
            }
        }

        public async Task<SysTeams> GetSystemTeams(string sysID, string teamsChannelID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SysTeams>("EXEC dbo.sp_GetSystemTeams @sysID, @teamsChannelID;", new { sysID, teamsChannelID });
            }
        }

        public async Task EditSystemTeamsDetail(SysTeams systemTeams)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemTeams @SysID, @TeamsChannelID,
                                        @TeamsChannelNMZHTW, @TeamsChannelNMZHCN, @TeamsChannelNMENUS, @TeamsChannelNMTHTH, @TeamsChannelNMJAJP,@TeamsChannelNMKOKR,
                                        @TeamsChannelUrl, @IsDisable, @SortOrder, @UpdUserID;";

                await conn.ExecuteAsync(commandText, systemTeams);
            }
        }

        public async Task DeleteSystemTeamsDetail(string sysID, string teamsChannelID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemTeams @sysID, @teamsChannelID;", new { sysID, teamsChannelID });
            }
        }
    }
}
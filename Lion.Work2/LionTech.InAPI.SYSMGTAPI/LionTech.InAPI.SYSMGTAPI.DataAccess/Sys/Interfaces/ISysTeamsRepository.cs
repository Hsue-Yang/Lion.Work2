using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysTeamsRepository
    {
        Task<(int rowCount, IEnumerable<SysTeams> SystemTeamsList)> GetSystemTeamsList(string sysID, string cultureID, int pageIndex, int pageSize);

        Task<SysTeams> GetSystemTeams(string sysID, string teamsChannelID);

        Task EditSystemTeamsDetail(SysTeams systemTeams);

        Task DeleteSystemTeamsDetail(string sysID, string teamsChannelID);
    }
}
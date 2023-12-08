using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysTeamsService
    {
        Task<(int rowCount, IEnumerable<SysTeams> SystemTeamsList)> GetSystemTeamsList(string sysID, string cultureID, int pageIndex, int pageSize);

        Task<SysTeams> GetSystemTeams(string sysID, string teamsChannelID);

        Task EditSystemTeamsDetail(SysTeams systemTeams);

        Task DeleteSystemTeamsDetail(string sysID, string teamsChannelID);
    }
}
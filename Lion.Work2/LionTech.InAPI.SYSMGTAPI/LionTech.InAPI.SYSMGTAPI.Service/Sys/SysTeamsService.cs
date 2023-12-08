using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysTeamsService : ISysTeamsService
    {
        private readonly ISysTeamsRepository _sysTeamsRepository;

        public SysTeamsService(ISysTeamsRepository sysTeamsRepository)
        {
            _sysTeamsRepository = sysTeamsRepository;
        }

        public Task<(int rowCount, IEnumerable<SysTeams> SystemTeamsList)> GetSystemTeamsList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysTeamsRepository.GetSystemTeamsList(sysID, cultureID, pageIndex, pageSize);
        }

        public Task<SysTeams> GetSystemTeams(string sysID, string teamsChannelID)
        {
            return _sysTeamsRepository.GetSystemTeams(sysID, teamsChannelID);
        }

        public Task EditSystemTeamsDetail(SysTeams systemTeams)
        {
            return _sysTeamsRepository.EditSystemTeamsDetail(systemTeams);
        }

        public Task DeleteSystemTeamsDetail(string sysID, string teamsChannelID)
        {
            return _sysTeamsRepository.DeleteSystemTeamsDetail(sysID, teamsChannelID);
        }
    }
}
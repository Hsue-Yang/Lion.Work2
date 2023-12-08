using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysFunToolParaRepository : ISysFunToolParaRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysFunToolParaRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<SystemFunTool> GetSystemFunToolParaForms(string userID, string sysID, string funControllerID, string funActionName, string toolNo, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleAsync<SystemFunTool>(
                    @"EXEC dbo.sp_GetSystemFunToolParaForms
                        @userID
                      , @sysID
                      , @funControllerID
                      , @funActionName
                      , @toolNo
                      , @cultureID;",
                    new
                    {
                        userID,
                        sysID,
                        funControllerID,
                        funActionName,
                        toolNo,
                        cultureID
                    }
                );
            }
        }

        public async Task<(int count, IEnumerable<SystemFunTool>)> GetSystemFunToolParas(string userID, string sysID, string funControllerID, string funActionName, string toolNo, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync(
                    @"EXEC dbo.sp_GetSystemFunToolParas
                        @userID
                      , @sysID
                      , @funControllerID
                      , @funActionName
                      , @toolNo
                      , @pageIndex
                      , @pageSize;",
                    new
                    {
                        userID,
                        sysID,
                        funControllerID,
                        funActionName,
                        toolNo,
                        pageIndex,
                        pageSize
                    }
                );
                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemFunToolParaList = multi.Read<SystemFunTool>();

                return (rowCount, systemFunToolParaList);
            }
        }
    }
}

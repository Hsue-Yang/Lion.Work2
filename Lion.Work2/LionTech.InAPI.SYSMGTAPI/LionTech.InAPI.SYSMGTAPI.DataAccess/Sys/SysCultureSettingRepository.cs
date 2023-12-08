using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysCultureSettingRepository : ISysCultureSettingRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IConfiguration _configuration;

        public SysCultureSettingRepository(IConnectionStringProvider connectionStringProvider, IConfiguration configuration)
        {
            _connectionStringProvider = connectionStringProvider;
            _configuration = configuration;
        }

        public async Task<IEnumerable<SystemCulture>> GetSystemCultureIDs()
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemCulture>("EXEC dbo.sp_GetSystemCultureIDs");
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemCulture> systemCultures)> GetSystemCultures(string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemCultures @cultureID, @pageIndex, @pageSize;", new { cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemCultures = multi.Read<SystemCulture>();

                return (rowCount, systemCultures);
            }
        }

        public async Task<SystemCulture> GetSystemCultureDetail(string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemCulture>("EXEC dbo.sp_GetSystemCultureDetail @cultureID", new { cultureID });
            }
        }

        public async Task EditSystemCultureDetail(SystemCulture model)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemCultureDetail
                           @CultureID
                         , @CultureNM
                         , @DisplayNM
                         , @IsSerpUse
                         , @IsDisable
                         , @UpdUserID";
                await conn.ExecuteAsync(commandText, model);
            }
        }

        public async Task DeleteSystemCultureDetail(string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemCultureDetail @cultureID", new { cultureID });
            }
        }

        public async Task GenerateCultureJsonFile()
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var systemCultures = await conn.QueryAsync<SystemCulture>("EXEC dbo.sp_GetSystemCultureRelease");

                var systemCultureJson = new
                {
                    Cultures = systemCultures.Select(item => new
                    {
                        CultureId = item.CultureID,
                        CultureName = item.CultureNM,
                        DisplayName = item.DisplayNM
                    }).ToList()
                };

                await File.WriteAllTextAsync(_configuration["Settings:CultureJsonPath"],
                    JsonSerializer.Serialize(systemCultureJson, new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    }));
            }
        }
    }
}
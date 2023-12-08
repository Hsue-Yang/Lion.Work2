using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class TrustIPRepository : ITrustIPRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IMisConnectionStringProvider _misConnectionStringProvider;
        private readonly IMongoConnectionProvider _mongoConnectionProvider;

        public TrustIPRepository(IConnectionStringProvider connectionStringProvider, IMisConnectionStringProvider misConnectionStringProvider, IMongoConnectionProvider mongoConnectionProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _misConnectionStringProvider = misConnectionStringProvider;
            _mongoConnectionProvider = mongoConnectionProvider;
        }

        public async Task<(int rowCount, IEnumerable<TrustIP> TrustIPList)> GetTrustIPs(TrustIPPara para)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                string commandText = @"EXEC dbo.sp_GetTrustIPs
                                        @IPBegin,
                                        @IPEnd,
                                        @ComID,
                                        @TrustStatus,
                                        @TrustType,
                                        @SourceType,
                                        @Remark,
                                        @CultureID,
                                        @PageIndex,
                                        @PageSize; ";

                var multi = await conn.QueryMultipleAsync(commandText, para);

                var rowCount = multi.Read<int>().SingleOrDefault();
                var trustIPList = multi.Read<TrustIP>();

                return (rowCount, trustIPList);
            }
        }

        public async Task<TrustIP> GetTrustIPDetail(string IPBegin, string IPEnd, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<TrustIP>("EXEC dbo.sp_GetTrustIPDetail @IPBegin, @IPEnd, @cultureID ", new { IPBegin, IPEnd, cultureID });
            }
        }

        public async Task<bool> GetValidTrustIPRepeated(TrustIP trustIP)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                string commandText = @"EXEC dbo.sp_GetValidTrustIPRepeated
                                        @IPBeginOriginal,
                                        @IPEndOriginal,
                                        @IPBegin,
                                        @IPEnd,
                                        @ComID,
                                        @TrustStatus,
                                        @TrustType,
                                        @SourceType ";

                return await conn.ExecuteAsync(commandText, new
                {
                    trustIP.IPBeginOriginal,
                    trustIP.IPEndOriginal,
                    trustIP.IPBegin,
                    trustIP.IPEnd,
                    trustIP.ComID,
                    trustIP.TrustStatus,
                    trustIP.TrustType,
                    trustIP.SourceType
                }) > 0;
            }
        }

        public async Task<string> EditTrustIP(TrustIP trustIP)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditTrustIP
                                        @IPBeginOriginal,
                                        @IPEndOriginal,
                                        @IPBegin,
                                        @IPEnd,
                                        @ComID,
                                        @TrustStatus,
                                        @TrustType,
                                        @SourceType,
                                        @Remark,
                                        @SortOrder,
                                        @UpdUserID";

                return await conn.QuerySingleOrDefaultAsync<string>(commandText, new
                {
                    trustIP.IPBeginOriginal,
                    trustIP.IPEndOriginal,
                    trustIP.IPBegin,
                    trustIP.IPEnd,
                    trustIP.ComID,
                    trustIP.TrustStatus,
                    trustIP.TrustType,
                    trustIP.SourceType,
                    trustIP.Remark,
                    trustIP.SortOrder,
                    trustIP.UpdUserID
                });
            }
        }

        public async Task<string> EditASPMism98(TrustIP trustIP)
        {
            using (var conn = new SqlConnection(_misConnectionStringProvider.ConnStringErp))
            {
                string commandText = @"DECLARE @RESULT CHAR(1);
                                       SET @RESULT = 'N';
                                       BEGIN TRANSACTION
                                           BEGIN TRY
                                          
                                               DECLARE @mm98_intfrom NUMERIC; 
                                               DECLARE @mm98_intto NUMERIC;
                                               DECLARE @mm98_other CHAR(1);
                                          
                                               SET @mm98_intfrom=CAST(PARSENAME(@IPBegin,1) AS BIGINT) + 
                                                                   CAST(PARSENAME(@IPBegin,2) AS BIGINT) * Power(256,1)+ 
                                                                   CAST(PARSENAME(@IPBegin,3) AS BIGINT) * Power(256,2)+
                                                                   CAST(PARSENAME(@IPBegin,4) AS BIGINT) * Power(256,3);
                                               SET @mm98_intto=CAST(PARSENAME(@IPEnd,1) AS BIGINT) +
                                                               CAST(PARSENAME(@IPEnd,2) AS BIGINT) * Power(256,1)+
                                                               CAST(PARSENAME(@IPEnd,3) AS BIGINT) * Power(256,2)+
                                                               CAST(PARSENAME(@IPEnd,4) AS BIGINT) * Power(256,3);
                                               SET @mm98_other=(CASE WHEN @TrustStatus='N' THEN 'X'
                                                                       WHEN @TrustStatus='Y' AND @SourceType='C' THEN ''
                                                                       WHEN @TrustStatus='Y' AND @SourceType='N' THEN ''
                                                                       WHEN @TrustStatus='Y' AND @SourceType='P' THEN 'Z'
                                                                       ELSE @SourceType END);
                                          
                                               DELETE FROM mism98 WHERE mm98_from = @IPBeginOriginal AND mm98_to = @IPEndOriginal;
                                               INSERT INTO mism98 (mm98_from, mm98_to, mm98_intfrom, mm98_intto, mm98_comp, mm98_other, mm98_desc, mm98_mstfn, mm98_mdate)
                                                           VALUES (@IPBegin, @IPEnd, @mm98_intfrom, @mm98_intto, @ComID, @mm98_other, @Remark, @UpdUserNM, GETDATE());
                                          
                                               SET @RESULT = 'Y';
                                               COMMIT;
                                           END TRY
                                           BEGIN CATCH
                                               SET @RESULT = 'N';
                                               ROLLBACK TRANSACTION;
                                           END CATCH
                                          
                                       SELECT @RESULT; ";

                return await conn.QuerySingleOrDefaultAsync<string>(commandText, new 
                { 
                    trustIP.IPBeginOriginal, 
                    trustIP.IPEndOriginal,
                    trustIP.IPBegin, 
                    trustIP.IPEnd, 
                    trustIP.ComID, 
                    trustIP.TrustStatus, 
                    trustIP.SourceType, 
                    trustIP.Remark, 
                    trustIP.UpdUserNM 
                });
            }
        }

        public async Task<string> DeleteTrustIP(string IPBegin, string IPEnd)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<string>("EXEC dbo.sp_DeleteTrustIP @IPBegin, @IPEnd ", new { IPBegin, IPEnd });
            }
        }

        public async Task<string> DeleteASPMism98(string IPBegin, string IPEnd)
        {
            using (var conn = new SqlConnection(_misConnectionStringProvider.ConnStringErp))
            {
                string commandText = @"DECLARE @RESULT CHAR(1); 
                                       SET @RESULT = 'N';
                                       BEGIN TRANSACTION
                                           BEGIN TRY
                                          
                                               DELETE FROM mism98 WHERE mm98_from = @IPBegin AND mm98_to = @IPEnd;
                                       
                                               SET @RESULT = 'Y'; 
                                               COMMIT; 
                                           END TRY 
                                           BEGIN CATCH 
                                               SET @RESULT = 'N'; 
                                               ROLLBACK TRANSACTION; 
                                           END CATCH 
                                       SELECT @RESULT; ";

                return await conn.QuerySingleOrDefaultAsync<string>(commandText, new { IPBegin, IPEnd });
            }
        }

        public async Task InsertTrustIPMongoDB(RecordSysTrustIP para)
        {
            IMongoCollection<RecordSysTrustIP> col = _mongoConnectionProvider.LionGroupSERP.GetCollection<RecordSysTrustIP>(EnumMongoDocName.LOG_SYS_TRUST_IP.ToString());

            var builder = Builders<RecordSysTrustIP>.Filter;

            var dbFilter = builder.Eq(x => x.IPBegin, para.IPBegin) & builder.Eq(x => x.IPEnd, para.IPEnd);

            var findSysLogNo = await col.FindAsync(dbFilter);

            var sysLogNo = findSysLogNo.ToList().Select(x => { int i = 0; return int.TryParse(x.LogNo, out i) ? i : 0; }).OrderByDescending(o => o).FirstOrDefault();

            para.LogNo = (sysLogNo + 1).ToString().PadLeft(6, '0');

            await col.InsertOneAsync(para);
        }
    }
}
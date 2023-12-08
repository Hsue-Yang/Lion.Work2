using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Mis.Interfaces;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Mis
{
    public class MisRepository : IMisRepository
    {
        private readonly IMisConnectionStringProvider _misConnectionStringProvider;

        public MisRepository(IMisConnectionStringProvider misConnectionStringProvider)
        {
            _misConnectionStringProvider = misConnectionStringProvider;
        }

        public async Task<string> CheckIP(string stfn, int intIP)
        {
            string vIpChk = "O";

            string sqlstr = @"SELECT mm98_seq, mm98_other,
	                                 (CASE WHEN mm98_other = 'Z' THEN CHARINDEX(@stfn, mm98_desc) ELSE -1 END) special
                                FROM dbo.mism98
                               WHERE mm98_other IN ('', 'Z', 'X') AND @intIP BETWEEN mm98_intfrom AND mm98_intto";

            using (var conn = new SqlConnection(_misConnectionStringProvider.ConnStringErp))
            {
                var queryObject = await conn.QuerySingleOrDefaultAsync(sqlstr, new { stfn, intIP });
                if (queryObject != null)
                {
                    if (string.IsNullOrWhiteSpace(queryObject.mm98_other))
                        vIpChk = "I";
                    else
                    {
                        vIpChk = queryObject.mm98_other;

                        //若為X需再重判斷一次是否為此員專用
                        //q.special 可以理解為 mm98_desc like '%員編 %' ; q.special >0 則存在於 mm98_desc
                        if (queryObject.mm98_other == "X" && queryObject.special <= 0)
                        {
                            vIpChk = "O";
                        }
                    }
                }
                else
                {
                    vIpChk = "O";
                }
            }

            return vIpChk;
        }
    }
}

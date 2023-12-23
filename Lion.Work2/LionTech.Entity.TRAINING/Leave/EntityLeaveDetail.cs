using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.Entity.TRAINING.Leave
{
    public class EntityLeaveDetail : EntityLeave
    {
        public EntityLeaveDetail(string connectionString, string providerName) : base(connectionString, providerName) { }
        public class LeaveDetailPara
        {
            public LeaveDetailPara() { }

            public enum ParaField
            {
                ppm96_stfn,
                ppm96_begin,
                ppm96_end,
                ppm95_name,
                ppd95_name,
                ppm96_sign,
                ppd95_id,
                ppm95_id
            }
            public DBVarChar ppm96_stfn;
            public DBChar ppm96_begin;
            public DBChar ppm96_end;
            public DBNVarChar ppm95_name;
            public DBNVarChar ppd95_name;
            public DBInt pm96_sign;
            public DBVarChar ppd95_id;
            public DBVarChar ppm95_id;
        }

        public class LeaveDetail : DBTableRow
        {
            public enum DataField
            {
                ppm96_stfn, ppm96_begin, ppm96_end, ppm95_id, ppd95_id, ppm96_sign
            }
            public DBVarChar ppm96_stfn;
            public DBChar ppm96_begin;
            public DBChar ppm96_end;
            public DBVarChar ppm95_id;
            public DBVarChar ppd95_id;
            public DBInt ppm96_sign;
        }
        public enum EnumEditLeaveDetailResult
        {
            Success,
            Failure
        }

        public EnumEditLeaveDetailResult EditLeaveDataDetail(LeaveDetailPara para)
        {
            List<DBParameter> dBParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                "        DELETE FROM prapsppm96",
                "         WHERE ppm96_stfn = {ppm96_stfn}",
                "           AND ppm96_begin = CONVERT(DATETIME,{ppm96_begin},120)",
                "           AND ppm96_end = CONVERT(DATETIME,{ppm96_end},120)",
                "           AND ppm95_id = (SELECT ppm95_id FROM prapsppm95 WHERE ppm95_name = N'{ppm95_name}')",
                "           AND ppd95_id = (SELECT ppd95_id FROM prapsppd95 WHERE ppd95_name = N'{ppd95_name}')",
                "           AND ppm96_sign = {ppm96_sign};",

                "        INSERT INTO prapsppm96",
                "             ( ppm96_stfn,",
                "               ppm96_begin,",
                "               ppm96_end,",
                "               ppm95_id,",
                "               ppd95_id,",
                "               ppm96_sign",
                "             )",
                "        SELECT {ppm96_stfn},",
                "               CONVERT(DATETIME, {ppm96_begin}, 120),",
                "               CONVERT(DATETIME, {ppm96_end}, 120),",
                "               {ppm95_id},",
                "               {ppd95_id},",
                "               {ppm96_sign};",

                "        SET @RESULT = 'Y';",
                "        COMMIT;",
                "    END TRY",
                "    BEGIN CATCH",
                "        SET @RESULT = 'N';",
                "        SET @ERROR_LINE = ERROR_LINE();",
                "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "        ROLLBACK TRANSACTION;",
                "    END CATCH;",

                "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
        });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_stfn, Value = para.ppm96_stfn });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_begin, Value = para.ppm96_begin });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_end, Value = para.ppm96_end });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm95_name, Value = para.ppm95_name });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppd95_name, Value = para.ppd95_name });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_sign, Value = para.pm96_sign });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppd95_id, Value = para.ppd95_id });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm95_id, Value = para.ppm95_id });

            var result = GetEntityList<ExecuteResult>(commandText, dBParameters).SingleOrDefault();

            if (result != null && result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditLeaveDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}

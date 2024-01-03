using System;
using System.Collections.Generic;
using System.Linq;

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
                ppm96_id,
                ppm96_stfn,
                ppm96_begin,
                ppm96_end,
                ppm95_name,
                ppd95_name,
                ppm96_sign,
                ppd95_id,
                ppm95_id
            }

            public DBInt ppm96_id;
            public DBVarChar ppm96_stfn;
            public DBDateTime ppm96_begin;
            public DBDateTime ppm96_end;
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
                ppm96_id, ppm96_stfn, ppm96_begin, ppm96_end, ppm95_id, ppd95_id, ppm96_sign
            }

            public DBInt ppm96_id;
            public DBVarChar ppm96_stfn;
            public DBDateTime ppm96_begin;
            public DBDateTime ppm96_end;
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
                "        DELETE FROM prapsppm96 ",
                "         WHERE ppm96_id = {ppm96_id} ",
                "        INSERT INTO prapsppm96",
                "             ( ppm96_stfn",
                "             , ppm96_begin",
                "             , ppm96_end",
                "             , ppm95_id",
                "             , ppd95_id",
                "             , ppm96_sign",
                "             )",
                "        SELECT {ppm96_stfn}",
                "             , {ppm96_begin}",
                "             , {ppm96_end}",
                "             , {ppm95_id}",
                "             , {ppd95_id}",
                "             , {ppm96_sign};",

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
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_id, Value = para.ppm96_id });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_stfn, Value = para.ppm96_stfn });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_begin, Value = para.ppm96_begin });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_end, Value = para.ppm96_end });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm95_id, Value = para.ppm95_id });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppd95_id, Value = para.ppd95_id });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_sign, Value = para.pm96_sign });

            var result = GetEntityList<ExecuteResult>(commandText, dBParameters).SingleOrDefault();

            if (result != null && result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditLeaveDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public EnumEditLeaveDetailResult DeleteLeaveDataDetail(LeaveDetailPara para)
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
               "         WHERE ppm96_id = {ppm96_id}",

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
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_id, Value = para.ppm96_id });

            var result = GetEntityList<ExecuteResult>(commandText, dBParameters).SingleOrDefault();

            if (result != null && result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditLeaveDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }


        public LeaveDetail GetLeaveDataDetail(LeaveDetailPara para)
        {
            List<DBParameter> dBParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine, new Object[]
            {
                "SELECT ppm96_id",
                "     , ppm96_stfn",
                "     , ppm96_begin",
                "     , ppm96_end",
                "     , ppm95_id",
                "     , ppd95_id",
                "     , ppm96_sign ",
                "  FROM prapsppm96 ",
                " WHERE ppm96_id = {ppm96_id}"
            });
            dBParameters.Add(new DBParameter { Name = LeaveDetailPara.ParaField.ppm96_id.ToString(), Value = para.ppm96_id });

            return GetEntityList<LeaveDetail>(commandText, dBParameters).FirstOrDefault();
        }
    }
}
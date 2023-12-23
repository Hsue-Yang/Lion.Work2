using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.TRAINING.Leave
{
    public class EntityLeaveDetail : EntityLeave
    {
        public EntityLeaveDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
        public class LeavePara
        {
            public enum ParaField
            {
                ppm96_id, ppm96_stfn, ppm96_begin, ppm96_end, ppm95_id, ppd95_id, ppm96_sign
            }
            public DBInt ppm96_id;
            public DBNVarChar ppm96_stfn;
            public DBDateTime ppm96_begin;
            public DBDateTime ppm96_end;
            public DBChar ppm95_id;
            public DBChar ppd95_id;
            public DBInt ppm96_sign;
        }

        public class Leave : DBTableRow
        {
            public DBInt ppm96_id;
            public DBNVarChar ppm96_stfn;
            public DBDateTime ppm96_begin;
            public DBDateTime ppm96_end;
            public DBNVarChar ppm95_id;
            public DBNVarChar ppd95_id;
            public DBInt ppm96_sign;
            public DBNVarChar ppm95_name;
            public DBNVarChar ppd95_name;
        }

        public enum EnumEditLeaveDetailResult
        {
            Success, Failure
        }

        public EnumEditLeaveDetailResult EditLeaveParaDetail(LeavePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "BEGIN TRY",
                    "DELETE FROM prapsppm96 ",
                    "WHERE ppm96_id = {ppm96_id} ",
                    "INSERT INTO prapsppm96 (ppm96_stfn, ppm96_begin, ppm96_end, ppm95_id, ppd95_id, ppm96_sign) ",
                    "VALUES( {ppm96_stfn}, {ppm96_begin}, {ppm96_end}, {ppm95_id}, {ppd95_id}, {ppm96_sign});",
                    "SET @RESULT = 'Y'; ",
                    "COMMIT; ",
                    "END TRY ",
                    "BEGIN CATCH",
                    "SET @RESULT = 'N';",
                    "SET @ERROR_LINE = ERROR_LINE();",
                    "SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "ROLLBACK TRANSACTION;",
                    "END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                });
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm96_id, Value = para.ppm96_id });
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm96_stfn, Value = para.ppm96_stfn });
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm96_begin, Value = para.ppm96_begin });
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm96_end, Value = para.ppm96_end });
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm95_id, Value = para.ppm95_id });
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppd95_id, Value = para.ppd95_id });
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm96_sign, Value = para.ppm96_sign });
            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();
            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditLeaveDetailResult.Success;
            }
            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public EnumEditLeaveDetailResult DeleteLeaveParaDetail(LeavePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "BEGIN TRY",
                    "DELETE FROM prapsppm96 ",
                    "WHERE ppm96_id = {ppm96_id} ",
                    "SET @RESULT = 'Y';",
                    "COMMIT;",
                    "END TRY",
                    "BEGIN CATCH",
                    "SET @RESULT = 'N';",
                    "SET @ERROR_LINE = ERROR_LINE();",
                    "SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "ROLLBACK TRANSACTION;",
                    "END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                });
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm96_id, Value = para.ppm96_id });
            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();
            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditLeaveDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public List<Leave> SelectLeaveListMenuChild(LeavePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT",
                "D.ppd95_id,",
                "D.ppd95_name",
                "FROM",
                "[dbo].[prapsppd95] D",
                "INNER JOIN",
                "[dbo].[prapsppm95] M ON D.ppm95_id = M.ppm95_id",
                "WHERE",
                "M.ppm95_id = {ppm95_id};"
            }));
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm95_id.ToString(), Value = para.ppm95_id });
            return GetEntityList<Leave>(commandText.ToString(), dbParameters);
        }

        public List<Leave> SelectLeaveListMenu()
        {
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT",
                "ppm95_id,",
                "ppm95_name",
                "FROM",
                "[dbo].[prapsppm95]"
            }));
            return GetEntityList<Leave>(commandText.ToString(), null);
        }

        public Leave SelectLeaveListDetail(LeavePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT",
                "p96.ppm96_id,",
                "p96.ppm96_stfn,",
                "p96.ppm96_begin,",
                "p96.ppm96_end,",
                "p95.ppm95_id,",
                "p95d.ppd95_id,",
                "ppm96_sign",
                "FROM",
                "[dbo].[prapsppm96] p96",
                "JOIN",
                "[dbo].[prapsppm95] p95",
                "ON",
                "p96.ppm95_id = p95.ppm95_id",
                "LEFT JOIN",
                "[dbo].[prapsppd95] p95d",
                "ON",
                "p96.ppd95_id = p95d.ppd95_id",
                "WHERE",
                "p96.ppm96_id = {ppm96_id};"
            }));
            dbParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm96_id.ToString(), Value = para.ppm96_id });
            return GetEntityList<Leave>(commandText.ToString(), dbParameters).SingleOrDefault(); ;
        }
    }
}
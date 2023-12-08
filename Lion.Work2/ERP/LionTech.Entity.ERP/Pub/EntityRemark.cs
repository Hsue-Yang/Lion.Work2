using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityRemark : EntityPub
    {
        public EntityRemark(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢備註 -
        public class RemarkPara : DBCulture
        {
            public RemarkPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                WF_NO,
                SYS_NM,
                WF_FLOW,
                WF_NODE,
                WF_SIG,
                WF_DOC,
                CULTURE_ID
            }

            public DBChar WFNo;
        }

        public class RemarkResult : DBTableRow
        {
            public DBChar RemarkNo;
            public DBNVarChar SysNM;
            public DBNVarChar WFFlowNM;
            public DBNVarChar WFNodeNM;

            public DBVarChar NodeResultID;
            public DBNVarChar NodeResultNM;
            public DBNVarChar NodeNewUserNM;
            public DBNVarChar BackWFNodeNM;
            
            public DBChar SigStep;
            public DBNVarChar WFSigSeqNM;

            public DBVarChar SigResultID;
            public DBNVarChar SigResultNM;
            
            public DBNVarChar WFDocSeqNM;
            public DBNVarChar RemarkUserNM;
            public DBChar RemarkDate;
            public DBNVarChar Remark;
        }

        /// <summary>
        /// 查詢備註
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<RemarkResult> SelectRemarkList(RemarkPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT R.REMARK_NO AS RemarkNo ",
                        "     , dbo.FN_GET_NMID(R.SYS_ID, M.{SYS_NM}) AS SysNM ",
                        "     , dbo.FN_GET_NMID(R.WF_FLOW_ID, WF.{WF_FLOW}) + '-' + R.WF_FLOW_VER AS WFFlowNM ",
                        "     , dbo.FN_GET_NMID(R.WF_NODE_ID, N.{WF_NODE}) + '-' + R.NODE_NO AS WFNodeNM ",
                        "     , R.NODE_RESULT_ID AS NodeResultID ",
                        "     , CASE WHEN R.NODE_RESULT_ID IS NULL THEN NULL ELSE dbo.FN_GET_CM_NM({WorkFlowResultType}, R.NODE_RESULT_ID, {CULTURE_ID}) END AS NodeResultNM ",
                        "     , dbo.FN_GET_USER_NM(NODE_NEW_USER_ID) AS NodeNewUserNM",
                        "     , CASE WHEN R.BACK_WF_NODE_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(R.BACK_WF_NODE_ID,(SELECT RN.{WF_NODE} FROM SYS_SYSTEM_WF_NODE RN WHERE R.SYS_ID = RN.SYS_ID AND R.WF_FLOW_ID = RN.WF_FLOW_ID AND R.WF_FLOW_VER = RN.WF_FLOW_VER AND R.BACK_WF_NODE_ID = RN.WF_NODE_ID)) END AS BackWFNodeNM ",
                        "     , R.SIG_STEP AS SigStep ",
                        "     , CASE WHEN R.WF_SIG_SEQ IS NULL THEN NULL ELSE dbo.FN_GET_NMID(R.WF_SIG_SEQ, S.{WF_SIG}) END AS WFSigSeqNM ",
                        "     , R.SIG_RESULT_ID AS SigResultID ",
                        "     , CASE WHEN R.SIG_RESULT_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(R.SIG_RESULT_ID, dbo.FN_GET_CM_NM({SignatureResultType}, R.SIG_RESULT_ID, {CULTURE_ID})) END AS SigResultNM  ",
                        "     , CASE WHEN R.WF_DOC_SEQ IS NULL THEN NULL ELSE dbo.FN_GET_NMID(R.WF_DOC_SEQ, D.{WF_DOC}) END AS WFDocSeqNM ",
                        "     , dbo.FN_GET_USER_NM(R.REMARK_USER_ID) AS RemarkUserNM ",
                        "     , R.REMARK_DATE AS RemarkDate ",
                        "     , R.REMARK AS Remark ",
                        "  FROM WF_REMARK R ",
                        "  JOIN SYS_SYSTEM_MAIN M ",
                        "    ON R.SYS_ID = M.SYS_ID ",
                        "  JOIN SYS_SYSTEM_WF_FLOW WF ",
                        "    ON R.SYS_ID = WF.SYS_ID ",
                        "   AND R.WF_FLOW_ID = WF.WF_FLOW_ID ",
                        "   AND R.WF_FLOW_VER= WF.WF_FLOW_VER ",
                        "  JOIN SYS_SYSTEM_WF_NODE N ",
                        "    ON R.SYS_ID = N.SYS_ID ",
                        "   AND R.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "   AND R.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "   AND R.WF_NODE_ID = N.WF_NODE_ID ",
                        "  LEFT JOIN SYS_SYSTEM_WF_DOC D ",
                        "    ON R.SYS_ID = D.SYS_ID ",
                        "   AND R.WF_FLOW_ID = D.WF_FLOW_ID ",
                        "   AND R.WF_FLOW_VER = D.WF_FLOW_VER ",
                        "   AND R.WF_NODE_ID = D.WF_NODE_ID ",
                        "   AND R.WF_DOC_SEQ = D.WF_DOC_SEQ ",
                        "  LEFT JOIN (SELECT * ",
                        "              FROM WF_SIG ",
                        "             WHERE WF_NO = {WF_NO} ",
                        "        ) AS S ",
                        "    ON R.NODE_NO = S.NODE_NO ",
                        "   AND R.WF_SIG_SEQ = S.WF_SIG_SEQ ",
                        " WHERE R.WF_NO = {WF_NO} ",
                        "   AND (R.DOC_IS_DELETE IS NULL OR R.DOC_IS_DELETE = 'N') ",
                        " ORDER BY R.REMARK_NO DESC ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RemarkPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = RemarkPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(RemarkPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = RemarkPara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(RemarkPara.ParaField.WF_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = RemarkPara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(RemarkPara.ParaField.WF_NODE.ToString())) });
            dbParameters.Add(new DBParameter { Name = RemarkPara.ParaField.WF_SIG.ToString(), Value = para.GetCultureFieldNM(new DBObject(RemarkPara.ParaField.WF_SIG.ToString())) });
            dbParameters.Add(new DBParameter { Name = RemarkPara.ParaField.WF_DOC.ToString(), Value = para.GetCultureFieldNM(new DBObject(RemarkPara.ParaField.WF_DOC.ToString())) });
            dbParameters.Add(new DBParameter { Name = RemarkPara.ParaField.CULTURE_ID.ToString(), Value = new DBChar(para.CultureID) });
            dbParameters.Add(new DBParameter { Name = EnumCMCodeKind.WorkFlowResultType, Value = new DBVarChar(Common.GetEnumDesc(EnumCMCodeKind.WorkFlowResultType)) });
            dbParameters.Add(new DBParameter { Name = EnumCMCodeKind.SignatureResultType, Value = new DBVarChar(Common.GetEnumDesc(EnumCMCodeKind.SignatureResultType)) });
            return GetEntityList<RemarkResult>(commandText, dbParameters);
        }
        #endregion
        
        #region - 新增備註 -
        public class InsertRemarkPara
        {
            public enum ParaField
            {
                WF_NO,
                NODE_NO,
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                SIG_STEP,
                WF_SIG_SEQ,
                REMARK_USER_ID,
                REMARK,
                UPD_USER_ID
            }

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar SigStep;
            public DBChar WFSigSeq;
            public DBVarChar RemarkUserID;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
        }

        public enum EnumInsertRemarkResult
        {
            Success,
            Failure
        }

        /// <summary>
        /// 新增備註
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnumInsertRemarkResult InsertRemark(InsertRemarkPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "DECLARE @REMARK_NO CHAR(3); ",

                        "SELECT @REMARK_NO = RIGHT('00' + CAST(ISNULL(CAST(MAX(REMARK_NO) AS INT), 0) + 1 AS VARCHAR), 3) ",
                        "  FROM WF_REMARK ",
                        " WHERE WF_NO = {WF_NO};",

                        "BEGIN TRANSACTION",
                        "    BEGIN TRY",

                        "        INSERT INTO WF_REMARK (",
                        "               WF_NO",
                        "             , NODE_NO",
                        "             , REMARK_NO",
                        "             , SYS_ID",
                        "             , WF_FLOW_ID",
                        "             , WF_FLOW_VER",
                        "             , WF_NODE_ID",
                        "             , NODE_RESULT_ID",
                        "             , NODE_NEW_USER_ID",
                        "             , BACK_WF_NODE_ID",
                        "             , SIG_STEP",
                        "             , WF_SIG_SEQ",
                        "             , SIG_DATE",
                        "             , SIG_RESULT_ID",
                        "             , DOC_NO",
                        "             , WF_DOC_SEQ",
                        "             , DOC_DATE",
                        "             , DOC_IS_DELETE",
                        "             , REMARK_USER_ID",
                        "             , REMARK_DATE",
                        "             , REMARK",
                        "             , UPD_USER_ID",
                        "             , UPD_DT",
                        "        ) VALUES (",
                        "              {WF_NO}",
                        "            , {NODE_NO}",
                        "            , @REMARK_NO",
                        "            , {SYS_ID}",
                        "            , {WF_FLOW_ID}",
                        "            , {WF_FLOW_VER}",
                        "            , {WF_NODE_ID}",
                        "            , NULL",
                        "            , NULL",
                        "            , NULL",
                        "            , {SIG_STEP}",
                        "            , {WF_SIG_SEQ}",
                        "            , NULL",
                        "            , NULL",
                        "            , NULL",
                        "            , NULL",
                        "            , NULL",
                        "            , NULL",
                        "            , {REMARK_USER_ID}",
                        "            , dbo.FN_GET_SYSDATE(GETDATE()) + dbo.FN_GET_SYSTIME(GETDATE())",
                        "            , {REMARK}",
                        "            , {UPD_USER_ID}",
                        "            , GETDATE()",
                        "        );",

                        "        SET @RESULT = 'Y'; ",
                        "        COMMIT; ",
                        "    END TRY ",
                        "    BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION; ",
                        "    END CATCH; ",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;",
                        Environment.NewLine
                    });

            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.NODE_NO, Value = para.NodeNo });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.SIG_STEP, Value = para.SigStep });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.WF_SIG_SEQ, Value = para.WFSigSeq });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.REMARK_USER_ID, Value = para.RemarkUserID });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = InsertRemarkPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumInsertRemarkResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityDocument : EntityPub
    {
        public EntityDocument(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class DocumentRemarkPara : DBCulture
        {
            public DocumentRemarkPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                WF_NO, SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID, IS_DELETE, WF_DOC, WF_NODE
            }

            public DBChar WFNo;
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar IsDelete;
        }

        public class DownloadDocumentPara
        {
            public enum ParaField
            {
                WF_NO, DOC_NO
            }

            public DBChar WFNo;
            public DBChar DocNo;
        }

        public class DocumentRemark : DBTableRow
        {
            public DBNVarChar WFDocSeqNM;
            public DBNVarChar WFNodeNM;
            public DBChar IsReq;
            public DBChar DocNo;
            public DBChar DocDate;
            public DBNVarChar DocFileNM;
            public DBVarChar DocUserID;
            public DBChar RemarkNo;
            public DBChar WFDocSeq;
            public DBNVarChar Remark;
            public DBNVarChar RemarkUserNM;
            public DBChar RemarkDate;
        }

        public List<DocumentRemark> SelectDocumentRemarkList(DocumentRemarkPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @SYS_ID VARCHAR(12) = {SYS_ID};",
                        "DECLARE @WF_FLOW_ID VARCHAR(50) = {WF_FLOW_ID};",
                        "DECLARE @WF_FLOW_VER CHAR(3) = {WF_FLOW_VER};",
                        "SELECT dbo.FN_GET_NMID(R.WF_DOC_SEQ, SW.{WF_DOC}) AS WFDocSeqNM",
                        "     , dbo.FN_GET_NMID(R.WF_NODE_ID, WN.{WF_NODE}) AS WFNodeNM",
                        "     , SW.IS_REQ AS IsReq",
                        "     , R.REMARK_NO AS RemarkNo ",
                        "     , R.WF_DOC_SEQ AS WFDocSeq ",
                        "     , R.DOC_NO AS DocNo ",
                        "     , R.DOC_DATE AS DocDate ",
                        "     , R.REMARK AS Remark ",
                        "     , R.REMARK_DATE AS RemarkDate ",
                        "     , dbo.FN_GET_USER_NM(R.REMARK_USER_ID) AS RemarkUserNM ",
                        "     , D.DOC_FILE_NAME AS DocFileNM ",
                        "     , D.DOC_USER_ID AS DocUserID ",
                        "  FROM WF_REMARK R ",
                        "  LEFT JOIN ( ",
                        "        SELECT DOC_NO ",
                        "             , DOC_FILE_NAME ",
                        "             , DOC_LOCAL_PATH ",
                        "             , DOC_USER_ID ",
                        "          FROM WF_DOC ",
                        "         WHERE WF_NO = {WF_NO} ",
                        "       ) D ON R.DOC_NO = D.DOC_NO ",
                        "  JOIN SYS_SYSTEM_WF_DOC SW",
                        "    ON SW.WF_DOC_SEQ = R.WF_DOC_SEQ",
                        "   AND SW.SYS_ID = @SYS_ID ",
                        "   AND SW.WF_FLOW_ID = @WF_FLOW_ID ",
                        "   AND SW.WF_FLOW_VER = @WF_FLOW_VER",
                        "JOIN SYS_SYSTEM_WF_NODE WN",
                        "  ON WN.WF_NODE_ID = R.WF_NODE_ID",
                        " AND WN.SYS_ID = @SYS_ID ",
                        " AND WN.WF_FLOW_ID = @WF_FLOW_ID ",
                        " AND WN.WF_FLOW_VER = @WF_FLOW_VER",
                        " WHERE R.WF_NO = {WF_NO} ",
                        "   AND R.SYS_ID = @SYS_ID ",
                        "   AND R.WF_FLOW_ID = @WF_FLOW_ID ",
                        "   AND R.WF_FLOW_VER = @WF_FLOW_VER ",
                        "   AND R.DOC_NO IS NOT NULL ",
                        "   AND (R.DOC_IS_DELETE IS NULL OR R.DOC_IS_DELETE <> {IS_DELETE}) ",
                        " ORDER BY R.DOC_DATE, R.REMARK_DATE ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = DocumentRemarkPara.ParaField.WF_DOC.ToString(), Value = para.GetCultureFieldNM(new DBObject(DocumentRemarkPara.ParaField.WF_DOC.ToString())) });
            dbParameters.Add(new DBParameter { Name = DocumentRemarkPara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(DocumentRemarkPara.ParaField.WF_NODE.ToString())) });
            dbParameters.Add(new DBParameter { Name = DocumentRemarkPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = DocumentRemarkPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = DocumentRemarkPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = DocumentRemarkPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = DocumentRemarkPara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = DocumentRemarkPara.ParaField.IS_DELETE.ToString(), Value = para.IsDelete });
            return GetEntityList<DocumentRemark>(commandText, dbParameters);
        }

        public class SysSystemWorkFlowDocumentPara : DBCulture
        {
            public SysSystemWorkFlowDocumentPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                WF_NO, SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID, IS_DELETE, WF_DOC
            }

            public DBChar WFNo;
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar IsDelete;
        }

        public class SysSystemWorkFlowDocument : DBTableRow
        {
            public DBChar WFDocSeq;
            public DBNVarChar WFDocSeqNM;
            public DBChar IsReq;
            public DBNVarChar DocUserNM;
            public DBChar DocDate;
        }

        public List<SysSystemWorkFlowDocument> SelectUploadDocumentList(SysSystemWorkFlowDocumentPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT S.WF_DOC_SEQ AS WFDocSeq ",
                        "     , dbo.FN_GET_NMID(S.WF_DOC_SEQ, S.{WF_DOC}) AS WFDocSeqNM ",
                        "     , S.IS_REQ AS IsReq ",
                        "     , dbo.FN_GET_USER_NM(D.DOC_USER_ID) AS DocUserNM ",
                        "     , D.DOC_DATE AS DocDate ",
                        "     , CASE WHEN D.DOC_NO IS NULL  ",
                        "            THEN dbo.FN_GET_NMID(S.WF_DOC_SEQ, S.{WF_DOC}) ",
                        "            ELSE dbo.FN_GET_NMID(S.WF_DOC_SEQ, D.{WF_DOC}) ",
                        "       END WF_DOC_SEQ_NM ",
                        "  FROM SYS_SYSTEM_WF_DOC S ",
                        "  LEFT JOIN (SELECT WF_DOC_SEQ ",
                        "                  , MAX(DOC_NO) AS DOC_NO ",
                        "               FROM WF_DOC ",
                        "              WHERE WF_NO = {WF_NO} ",
                        "                AND WF_NODE_ID = {WF_NODE_ID} ",
                        "                AND (IS_DELETE IS NULL OR IS_DELETE <> {IS_DELETE}) ",
                        "              GROUP BY WF_DOC_SEQ ",
                        "            ) W ON W.WF_DOC_SEQ = S.WF_DOC_SEQ ",
                        "  LEFT JOIN (SELECT DOC_NO ",
                        "                  , DOC_USER_ID, DOC_DATE, {WF_DOC} ",
                        "               FROM WF_DOC ",
                        "              WHERE WF_NO = {WF_NO} ",
                        "                AND WF_NODE_ID = {WF_NODE_ID} ",
                        "            ) D  ON W.DOC_NO = D.DOC_NO ",
                        " WHERE S.SYS_ID = {SYS_ID} ",
                        "   AND S.WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND S.WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND S.WF_NODE_ID = {WF_NODE_ID} ",
                        Environment.NewLine
                    });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowDocumentPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowDocumentPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowDocumentPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowDocumentPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowDocumentPara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowDocumentPara.ParaField.IS_DELETE.ToString(), Value = para.IsDelete });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowDocumentPara.ParaField.WF_DOC.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemWorkFlowDocumentPara.ParaField.WF_DOC.ToString())) });
            return GetEntityList<SysSystemWorkFlowDocument>(commandText, dbParameters);
        }

        public class DownloadResult : DBTableRow
        {
            public DBNVarChar DocPath;
            public DBNVarChar DocFileNM;
            public DBVarChar DocEncodeName;
        }

        public DownloadResult SelectDocumentPath(DownloadDocumentPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT DOC_PATH AS DocPath ",
                        "     , DOC_FILE_NAME AS DocFileNM ",
                        "     , DOC_ENCODE_NAME AS DocEncodeName ",
                        "  FROM WF_DOC ",
                        " WHERE WF_NO = {WF_NO} ",
                        "   AND DOC_NO = {DOC_NO} ",
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = DownloadDocumentPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = DownloadDocumentPara.ParaField.DOC_NO.ToString(), Value = para.DocNo });
            return GetEntityList<DownloadResult>(commandText, dbParameters).SingleOrDefault();
        }
    }
}

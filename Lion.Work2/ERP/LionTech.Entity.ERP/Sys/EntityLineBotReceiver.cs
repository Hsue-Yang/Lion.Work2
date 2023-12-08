using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityLineBotReceiver : EntitySys
    {
        public EntityLineBotReceiver(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得LineBot好友清單 -
        public class LineBotReceiverPara : DBCulture
        {
            public LineBotReceiverPara(string cultureID) : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                QUERY_LINE_RECEIVER_NM,
                LINE_RECEIVER_NM
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBNVarChar LineReceiverNM;
        }

        public class LineBotReceiver : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBVarChar LineReceiverID;
            public DBNVarChar LineReceiverNM;
            public DBVarChar SourceType;
            public DBChar IsDisable;
            public DBVarChar ReceiverID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<LineBotReceiver> SelectLineBotReceiverList(LineBotReceiverPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT SYS_ID AS SysID",
                "     , LINE_ID AS LineID",
                "     , LINE_RECEIVER_ID AS LineReceiverID",
                "     , {LINE_RECEIVER_NM} AS LineReceiverNM",
                "     , SOURCE_TYPE AS SourceType",
                "     , IS_DISABLE AS IsDisable",
                "     , RECEIVER_ID AS ReceiverID",
                "     , dbo.FN_GET_USER_NM(UPD_USER_ID) AS UpdUserNM",
                "     , UPD_DT AS UpdDT",
                "  FROM SYS_SYSTEM_LINE_RECEIVER",
                " WHERE SYS_ID = {SYS_ID}",
                "   AND LINE_ID = {LINE_ID}"
            }));

            if (para.LineReceiverNM.IsNull() == false)
            {
                commandText.AppendLine(" AND (LINE_RECEIVER_ID LIKE '%' + {QUERY_LINE_RECEIVER_NM} + '%' OR {LINE_RECEIVER_NM} LIKE '%' + {QUERY_LINE_RECEIVER_NM} + '%') ");
            }

            dbParameters.Add(new DBParameter { Name = LineBotReceiverPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverPara.ParaField.QUERY_LINE_RECEIVER_NM, Value = para.LineReceiverNM });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverPara.ParaField.LINE_RECEIVER_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(LineBotReceiverPara.ParaField.LINE_RECEIVER_NM.ToString())) });

            return GetEntityList<LineBotReceiver>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 取得Line名稱 -
        public class LineNMIDPara : DBCulture
        {
            public LineNMIDPara(string cultureID) : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                LINE_NM
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
        }

        public DBNVarChar SelectLineNMID(LineNMIDPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT dbo.FN_GET_NMID(LINE_ID, {LINE_NM})",
                "  FROM SYS_SYSTEM_LINE",
                " WHERE SYS_ID = {SYS_ID}",
                "   AND LINE_ID = {LINE_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = LineNMIDPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineNMIDPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineNMIDPara.ParaField.LINE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(LineNMIDPara.ParaField.LINE_NM.ToString())) });

            return new DBNVarChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion
    }
}
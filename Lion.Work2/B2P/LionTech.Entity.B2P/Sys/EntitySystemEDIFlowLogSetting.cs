using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemEDIFlowLogSetting : EntitySys
    {
        public EntitySystemEDIFlowLogSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class StatusIDPara : DBCulture
        {
            public StatusIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class StatusID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return this.CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return this.CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<StatusID> SelectStatusIDList(StatusIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0002' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = StatusIDPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(StatusIDPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<StatusID> statusIDList = new List<StatusID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StatusID statusID = new StatusID()
                    {
                        CodeID = new DBVarChar(dataRow[StatusID.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[StatusID.DataField.CODE_NM.ToString()])
                    };
                    statusIDList.Add(statusID);
                }
                return statusIDList;
            }
            return null;
        }

        public class SystemEditEDIFlowLogSettingPara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID, EDI_NO,
                DATA_DATE, STATUS_ID, IS_AUTOMATIC, IS_DELETED, UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBChar EDINO;

            public DBChar DataDate;
            public DBVarChar StatusID;
            public DBChar IsAutomatic;
            public DBChar IsDeleted;
            public DBVarChar UpdUserID;
        }

        public enum EnumEditSystemEDIFlowLogSettingDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemEDIFlowLogSettingDetailResult EditSystemEDIFlowLogSetting(SystemEditEDIFlowLogSettingPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @TODAY_YMD CHAR(8); ", Environment.NewLine,
                "DECLARE @EDI_NO CHAR(12); ", Environment.NewLine,
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,

                "SET @TODAY_YMD = dbo.FN_GET_SYSDATE(NULL); ", Environment.NewLine,

                "SELECT @EDI_NO=@TODAY_YMD+RIGHT('000'+CAST(ISNULL(CAST(SUBSTRING(MAX(EDI_NO),9,4) AS INT),0)+1 AS VARCHAR),4) ", Environment.NewLine,
                "FROM EDI_FLOW ", Environment.NewLine,
                "WHERE EDI_NO>@TODAY_YMD + '0000'; ", Environment.NewLine,

                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        INSERT INTO EDI_FLOW (SYS_ID, EDI_FLOW_ID, EDI_NO, DATA_DATE, STATUS_ID, IS_AUTOMATIC, IS_DELETED, UPD_USER_ID, UPD_DT)", Environment.NewLine,
                "          VALUES ( ", Environment.NewLine,
                "            {SYS_ID} ", Environment.NewLine,
                "          , {EDI_FLOW_ID} ", Environment.NewLine,
                "          , @EDI_NO ", Environment.NewLine,
                "          , {DATA_DATE}, {STATUS_ID}, {IS_AUTOMATIC}, {IS_DELETED} ", Environment.NewLine,
                "          , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,

                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "        SET @RESULT = 'N'; ", Environment.NewLine,
                "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEditEDIFlowLogSettingPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEditEDIFlowLogSettingPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEditEDIFlowLogSettingPara.Field.DATA_DATE, Value = para.DataDate });
            dbParameters.Add(new DBParameter { Name = SystemEditEDIFlowLogSettingPara.Field.STATUS_ID, Value = para.StatusID });
            dbParameters.Add(new DBParameter { Name = SystemEditEDIFlowLogSettingPara.Field.IS_AUTOMATIC, Value = para.IsAutomatic });
            dbParameters.Add(new DBParameter { Name = SystemEditEDIFlowLogSettingPara.Field.IS_DELETED, Value = para.IsDeleted });
            dbParameters.Add(new DBParameter { Name = SystemEditEDIFlowLogSettingPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemEDIFlowLogSettingDetailResult.Success : EnumEditSystemEDIFlowLogSettingDetailResult.Failure;
        }
    }
}
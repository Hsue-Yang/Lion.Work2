using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemEDIPara : EntitySys
    {
        public EntitySystemEDIPara(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region EDIParaTypePara
        public class EDIParaTypePara : DBCulture
        {
            public EDIParaTypePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class EDIParaType : DBTableRow, ISelectItem
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

        public List<EDIParaType> SelectEDIParaTypeList(EDIParaTypePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0008' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIParaTypePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(EDIParaTypePara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<EDIParaType> ediparaTypeList = new List<EDIParaType>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EDIParaType ediparaType = new EDIParaType()
                    {
                        CodeID = new DBVarChar(dataRow[EDIParaType.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[EDIParaType.DataField.CODE_NM.ToString()])
                    };
                    ediparaTypeList.Add(ediparaType);
                }
                return ediparaTypeList;
            }
            return null;
        }
        #endregion

        public class SystemEDIParaPara : DBCulture
        {
            public SystemEDIParaPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW, EDI_JOB_ID, EDI_JOB,
                EDI_JOB_PARA_ID, EDI_JOB_PARA_TYPE, EDI_JOB_PARA_VALUE,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBVarChar EDIParaID;
            public DBVarChar EDIParaType;
            public DBNVarChar EDIParaValue;
            public DBVarChar UpdUserID;
        }

        public class EDIParaParaValue : ValueListRow
        {
            public enum ValueField
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB_ID, EDI_JOB_PARA_ID, SORT_ORDER
            }

            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIJobID { get; set; }
            public string EDIParaID { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }

            public DBVarChar GetSysID()
            {
                return new DBVarChar(SysID);
            }
            public DBVarChar GetEDIFlowID()
            {
                return new DBVarChar(EDIFlowID);
            }
            public DBVarChar GetEDIJobID()
            {
                return new DBVarChar(EDIJobID);
            }
            public DBVarChar GetEDIParaID()
            {
                return new DBVarChar(EDIParaID);
            }
            public DBVarChar GetBeforeSortOrder()
            {
                return new DBVarChar(BeforeSortOrder);
            }
            public DBVarChar GetAfterSortOrder()
            {
                return new DBVarChar(AfterSortOrder);
            }
        }

        public class SystemEDIPara : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW_NM, EDI_JOB_ID, EDI_JOB_NM,
                EDI_JOB_PARA_ID, EDI_JOB_PARA_TYPE, EDI_JOB_PARA_VALUE,
                SORT_ORDER, UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;
            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobNM;

            public DBVarChar EDIParaID;
            public DBVarChar EDIParaType;
            public DBNVarChar EDIParaValue;
            public DBVarChar SortOrder;

            public DBVarChar UpdUserID;
            public DBDateTime UpdDt;
        }

        public enum EnumEDIParaSettingResult
        {
            Success, Failure
        }

        public EnumEDIParaSettingResult EditEDIParaSetting(SystemEDIParaPara para, List<EDIParaParaValue> EDIParaParaValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();
            foreach (EDIParaParaValue EDIParaParaValue in EDIParaParaValueList)
            {
                //判斷SORT_ORDER有變才更新
                if (EDIParaParaValue.AfterSortOrder != EDIParaParaValue.BeforeSortOrder)
                {
                    string insertCommand = string.Concat(new object[]
                    {
                        "        UPDATE SYS_SYSTEM_EDI_PARA SET ", Environment.NewLine,
                        "        SORT_ORDER={SORT_ORDER},UPD_USER_ID={UPD_USER_ID},UPD_DT=GETDATE() ", Environment.NewLine,
                        "        WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID} AND EDI_JOB_PARA_ID={EDI_JOB_PARA_ID}; ", Environment.NewLine,
                    });

                    dbParameters.Add(new DBParameter { Name = EDIParaParaValue.ValueField.SYS_ID, Value = para.SysID });
                    dbParameters.Add(new DBParameter { Name = EDIParaParaValue.ValueField.EDI_FLOW_ID, Value = para.EDIFlowID });
                    dbParameters.Add(new DBParameter { Name = EDIParaParaValue.ValueField.EDI_JOB_ID, Value = para.EDIJobID });
                    dbParameters.Add(new DBParameter { Name = EDIParaParaValue.ValueField.EDI_JOB_PARA_ID, Value = EDIParaParaValue.GetEDIParaID() });
                    dbParameters.Add(new DBParameter { Name = EDIParaParaValue.ValueField.SORT_ORDER, Value = EDIParaParaValue.GetAfterSortOrder() });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                    dbParameters.Clear();
                }
            }

            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                commandTextStringBuilder.ToString(), Environment.NewLine,
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

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEDIParaSettingResult.Success : EnumEDIParaSettingResult.Failure;
        }

        public class SystemEDIParaDetailPara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB_ID,
                EDI_JOB_PARA_ID, EDI_JOB_PARA_TYPE, EDI_JOB_PARA_VALUE, SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBVarChar EDIParaID;
            public DBVarChar EDIParaType;
            public DBNVarChar EDIParaValue;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
        }

        public class SystemEDIParaDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB_ID,
                EDI_JOB_PARA_ID, EDI_JOB_PARA_TYPE, EDI_JOB_PARA_VALUE,
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBVarChar EDIParaID;
            public DBVarChar EDIParaType;
            public DBNVarChar EDIParaValue;
        }

        public List<SystemEDIPara> SelectSystemEDIParaList(SystemEDIParaPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , F.EDI_FLOW_ID, dbo.FN_GET_NMID(F.EDI_FLOW_ID, F.{EDI_FLOW}) AS EDI_FLOW_NM ", Environment.NewLine,
                "     , J.EDI_JOB_ID, dbo.FN_GET_NMID(J.EDI_JOB_ID, J.{EDI_JOB}) AS EDI_JOB_NM ", Environment.NewLine,
                "     , P.EDI_JOB_PARA_ID, P.EDI_JOB_PARA_TYPE, P.EDI_JOB_PARA_VALUE, P.SORT_ORDER ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(P.UPD_USER_ID) AS UPD_USER_NM, P.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_PARA P ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON P.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EDI_FLOW F ON P.SYS_ID=F.SYS_ID AND P.EDI_FLOW_ID=F.EDI_FLOW_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EDI_JOB J ON P.SYS_ID=J.SYS_ID AND P.EDI_FLOW_ID=J.EDI_FLOW_ID AND P.EDI_JOB_ID=J.EDI_JOB_ID ", Environment.NewLine,
                "WHERE P.SYS_ID={SYS_ID} AND P.EDI_FLOW_ID={EDI_FLOW_ID} AND P.EDI_JOB_ID={EDI_JOB_ID} ", Environment.NewLine,
                "ORDER BY P.SORT_ORDER "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIParaPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaPara.ParaField.EDI_JOB_ID.ToString(), Value = para.EDIJobID });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIParaPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIParaPara.ParaField.EDI_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaPara.ParaField.EDI_JOB.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIParaPara.ParaField.EDI_JOB.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEDIPara> SystemEDIParaList = new List<SystemEDIPara>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemEDIPara systemEDIPara = new SystemEDIPara()
                    {
                        SysID = new DBVarChar(dataRow[SystemEDIPara.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemEDIPara.DataField.SYS_NM.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[SystemEDIPara.DataField.EDI_FLOW_ID.ToString()]),
                        EDIFlowNM = new DBNVarChar(dataRow[SystemEDIPara.DataField.EDI_FLOW_NM.ToString()]),
                        EDIJobID = new DBVarChar(dataRow[SystemEDIPara.DataField.EDI_JOB_ID.ToString()]),
                        EDIJobNM = new DBNVarChar(dataRow[SystemEDIPara.DataField.EDI_JOB_NM.ToString()]),
                        EDIParaID = new DBVarChar(dataRow[SystemEDIPara.DataField.EDI_JOB_PARA_ID.ToString()]),
                        EDIParaType = new DBVarChar(dataRow[SystemEDIPara.DataField.EDI_JOB_PARA_TYPE.ToString()]),
                        EDIParaValue = new DBNVarChar(dataRow[SystemEDIPara.DataField.EDI_JOB_PARA_VALUE.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemEDIPara.DataField.SORT_ORDER.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[SystemEDIPara.DataField.UPD_USER_NM.ToString()]),
                        UpdDt = new DBDateTime(dataRow[SystemEDIPara.DataField.UPD_DT.ToString()])
                    };
                    SystemEDIParaList.Add(systemEDIPara);
                }
                return SystemEDIParaList;
            }
            return null;
        }

        public enum EnumEditSystemEDIParaDetailResult //新增
        {
            Success, Failure
        }

        public EnumEditSystemEDIParaDetailResult EditSystemEDIParaDetail(SystemEDIParaDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_EDI_PARA ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID} AND EDI_JOB_PARA_ID={EDI_JOB_PARA_ID}; ", Environment.NewLine,

                "        INSERT INTO SYS_SYSTEM_EDI_PARA VALUES ( ", Environment.NewLine,
                "            {SYS_ID} ", Environment.NewLine,
                "          , {EDI_FLOW_ID} ", Environment.NewLine,
                "          , {EDI_JOB_ID} ", Environment.NewLine,
                "          , {EDI_JOB_PARA_ID}, {EDI_JOB_PARA_TYPE}, {EDI_JOB_PARA_VALUE}, {SORT_ORDER} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_ID, Value = para.EDIJobID });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_ID, Value = para.EDIParaID });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_TYPE, Value = para.EDIParaType });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_VALUE, Value = para.EDIParaValue });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemEDIParaDetailResult.Success : EnumEditSystemEDIParaDetailResult.Failure;
        }

        public enum EnumDeleteSystemEDIParaDetailResult //刪除
        {
            Success, Failure, DataExist
        }

        public EnumDeleteSystemEDIParaDetailResult DeleteSystemEDIParaDetail(SystemEDIParaDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1) = 'N'; ", Environment.NewLine,
                    "BEGIN ", Environment.NewLine,
                    "    DELETE FROM SYS_SYSTEM_EDI_PARA ", Environment.NewLine,
                    "    WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID} AND EDI_JOB_PARA_ID={EDI_JOB_PARA_ID}; ", Environment.NewLine,
                    "    SET @RESULT = 'Y'; ", Environment.NewLine,
                    "END; ", Environment.NewLine,
                    "SELECT @RESULT; ", Environment.NewLine
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
                dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_ID, Value = para.EDIJobID });
                dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_ID, Value = para.EDIParaID });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemEDIParaDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemEDIParaDetailResult.DataExist;
                }
            }
            catch
            {
                return EnumDeleteSystemEDIParaDetailResult.Failure;
            }
        }
    }
}
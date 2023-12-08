using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemEDIJobDetail : EntitySys
    {
        public EntitySystemEDIJobDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum SortorderField
        {
            [Description("00")]
            Right,
            [Description("000")]
            Left,
            [Description("000001")]
            First
        }

        public class SysSystemEDIConIDPara : DBCulture
        {
            public SysSystemEDIConIDPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EDI_FLOW_ID, EDI_CON
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
        }

        public class SysSystemEDIConID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                EDI_CON_ID, EDI_CON_NM
            }

            public DBVarChar EDIConID;
            public DBNVarChar EDIConNM;

            public string ItemText()
            {
                return this.EDIConNM.StringValue();
            }

            public string ItemValue()
            {
                return this.EDIConID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemEDIConID> SelectSystemEDIConIDList(SysSystemEDIConIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT DISTINCT EDI_CON_ID, dbo.FN_GET_NMID(EDI_CON_ID, {EDI_CON}) AS EDI_CON_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_CON ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemEDIConIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemEDIConIDPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SysSystemEDIConIDPara.ParaField.EDI_CON.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemEDIConIDPara.ParaField.EDI_CON.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemEDIConID> systemEDIConIDList = new List<SysSystemEDIConID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemEDIConID systemEDIConID = new SysSystemEDIConID()
                    {
                        EDIConID = new DBVarChar(dataRow[SysSystemEDIConID.DataField.EDI_CON_ID.ToString()]),
                        EDIConNM = new DBNVarChar(dataRow[SysSystemEDIConID.DataField.EDI_CON_NM.ToString()])
                    };
                    systemEDIConIDList.Add(systemEDIConID);
                }
                return systemEDIConIDList;
            }
            return null;
        }

        public class SysSystemDepEDIJobIDPara : DBCulture
        {
            public SysSystemDepEDIJobIDPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
        }

        public class SysSystemDepEDIJobID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                EDI_JOB_ID, EDI_JOB_NM
            }

            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobNM;

            public string ItemText()
            {
                return this.EDIJobNM.StringValue();
            }

            public string ItemValue()
            {
                return this.EDIJobID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemDepEDIJobID> SelectSystemDepEDIJobIDList(SysSystemDepEDIJobIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT DISTINCT EDI_JOB_ID, dbo.FN_GET_NMID(EDI_JOB_ID, {EDI_JOB}) AS EDI_JOB_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemDepEDIJobIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemDepEDIJobIDPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SysSystemDepEDIJobIDPara.ParaField.EDI_JOB.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemDepEDIJobIDPara.ParaField.EDI_JOB.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemDepEDIJobID> systemDepEDIJobIDList = new List<SysSystemDepEDIJobID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemDepEDIJobID systemDepEDIJobID = new SysSystemDepEDIJobID()
                    {
                        EDIJobID = new DBVarChar(dataRow[SysSystemDepEDIJobID.DataField.EDI_JOB_ID.ToString()]),
                        EDIJobNM = new DBNVarChar(dataRow[SysSystemDepEDIJobID.DataField.EDI_JOB_NM.ToString()])
                    };
                    systemDepEDIJobIDList.Add(systemDepEDIJobID);
                }
                return systemDepEDIJobIDList;
            }
            return null;
        }

        public class EDIFileEncodingPara : DBCulture
        {
            public EDIFileEncodingPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class EDIFileEncoding : DBTableRow, ISelectItem
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

        public List<EDIFileEncoding> SelectEDIFileEncodingList(EDIFileEncodingPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0007' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIFileEncodingPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(EDIFileEncodingPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<EDIFileEncoding> ediFileEncodingList = new List<EDIFileEncoding>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EDIFileEncoding ediFileEncoding = new EDIFileEncoding()
                    {
                        CodeID = new DBVarChar(dataRow[EDIFileEncoding.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[EDIFileEncoding.DataField.CODE_NM.ToString()])
                    };
                    ediFileEncodingList.Add(ediFileEncoding);
                }
                return ediFileEncodingList;
            }
            return null;
        }

        public class SystemEDIJobDetailPara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB_ID,
                EDI_JOB_ZH_TW, EDI_JOB_ZH_CN, EDI_JOB_EN_US,EDI_JOB_TH_TH,EDI_JOB_JA_JP,
                EDI_JOB_TYPE, EDI_CON_ID, OBJECT_NAME, DEP_EDI_JOB_ID,
                IS_USE_RES, IGNORE_WARNING, IS_DISABLE, FILE_SOURCE, FILE_ENCODING, URL_PATH,
                SORT_ORDER, UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobZHTW;
            public DBNVarChar EDIJobZHCN;
            public DBNVarChar EDIJobENUS;
            public DBNVarChar EDIJobTHTH;
            public DBNVarChar EDIJobJAJP;
            public DBVarChar EDIJobType;
            public DBVarChar EDIConID;
            public DBVarChar ObjectName;
            public DBVarChar DepEDIJobID;
            public DBChar IsUseRes;
			public DBChar IgnoreWarning;
            public DBChar IsDisable;
            public DBNVarChar FileSource;
            public DBVarChar EDIFileEncoding;
            public DBNVarChar URLPath;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
        }

        public class SystemEDIJobDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB_ID,
                EDI_JOB_ZH_TW, EDI_JOB_ZH_CN, EDI_JOB_EN_US,EDI_JOB_TH_TH,EDI_JOB_JA_JP,
                EDI_JOB_TYPE, EDI_CON_ID, OBJECT_NAME, DEP_EDI_JOB_ID,
                IS_USE_RES, IGNORE_WARNING, IS_DISABLE, FILE_SOURCE, FILE_ENCODING, URL_PATH,
                SORT_ORDER, NEW_SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobZHTW;
            public DBNVarChar EDIJobZHCN;
            public DBNVarChar EDIJobENUS;
            public DBNVarChar EDIJobTHTH;
            public DBNVarChar EDIJobJAJP;
            public DBVarChar EDIJobType;
            public DBVarChar EDIConID;
            public DBVarChar ObjectName;
            public DBVarChar DepEDIJobID;
            public DBChar IsUseRes;
			public DBChar IgnoreWarning;
            public DBChar IsDisable;
            public DBNVarChar FileSource;
            public DBVarChar EDIFileEncoding;
            public DBNVarChar URLPath;

            public DBVarChar SortOrder;
        }

        public SystemEDIJobDetail SelectSystemEDIJobDetail(SystemEDIJobDetailPara para) 
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, EDI_FLOW_ID, EDI_JOB_ID ", Environment.NewLine, 
                "     , EDI_JOB_ZH_TW, EDI_JOB_ZH_CN, EDI_JOB_EN_US,EDI_JOB_TH_TH,EDI_JOB_JA_JP ", Environment.NewLine,
                "     , EDI_JOB_TYPE, EDI_CON_ID, OBJECT_NAME, DEP_EDI_JOB_ID ", Environment.NewLine,
                "     , IS_USE_RES, IGNORE_WARNING, IS_DISABLE, FILE_SOURCE, FILE_ENCODING, URL_PATH ", Environment.NewLine,
                "     , SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_ID, Value = para.EDIJobID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemEDIJobDetail systemEDIJob = new SystemEDIJobDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemEDIJobDetail.DataField.SYS_ID.ToString()]),
                    EDIJobZHTW = new DBNVarChar(dataRow[SystemEDIJobDetail.DataField.EDI_JOB_ZH_TW.ToString()]),
                    EDIJobZHCN = new DBNVarChar(dataRow[SystemEDIJobDetail.DataField.EDI_JOB_ZH_CN.ToString()]),
                    EDIJobENUS = new DBNVarChar(dataRow[SystemEDIJobDetail.DataField.EDI_JOB_EN_US.ToString()]),
                    EDIJobTHTH = new DBNVarChar(dataRow[SystemEDIJobDetail.DataField.EDI_JOB_TH_TH.ToString()]),
                    EDIJobJAJP = new DBNVarChar(dataRow[SystemEDIJobDetail.DataField.EDI_JOB_JA_JP.ToString()]),
                    EDIJobType = new DBVarChar(dataRow[SystemEDIJobDetail.DataField.EDI_JOB_TYPE.ToString()]),
                    EDIConID = new DBVarChar(dataRow[SystemEDIJobDetail.DataField.EDI_CON_ID.ToString()]),
                    ObjectName = new DBVarChar(dataRow[SystemEDIJobDetail.DataField.OBJECT_NAME.ToString()]),
                    DepEDIJobID = new DBVarChar(dataRow[SystemEDIJobDetail.DataField.DEP_EDI_JOB_ID.ToString()]),
                    IsUseRes = new DBChar(dataRow[SystemEDIJobDetail.DataField.IS_USE_RES.ToString()]),
					IgnoreWarning = new DBChar(dataRow[SystemEDIJobDetail.DataField.IGNORE_WARNING.ToString()]),
                    IsDisable = new DBChar(dataRow[SystemEDIJobDetail.DataField.IS_DISABLE.ToString()]),
                    FileSource = new DBNVarChar(dataRow[SystemEDIJobDetail.DataField.FILE_SOURCE.ToString()]),
                    EDIFileEncoding = new DBVarChar(dataRow[SystemEDIJobDetail.DataField.FILE_ENCODING.ToString()]),
                    URLPath = new DBNVarChar(dataRow[SystemEDIJobDetail.DataField.URL_PATH.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemEDIJobDetail.DataField.SORT_ORDER.ToString()])
                };
                return systemEDIJob;
            }
            return null;
        }

        public enum EnumEditSystemEDIJobDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemEDIJobDetailResult EditSystemEDIJobDetail(SystemEDIJobDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID}; ", Environment.NewLine,

                "        INSERT INTO SYS_SYSTEM_EDI_JOB VALUES ( ", Environment.NewLine,
                "            {SYS_ID} ", Environment.NewLine,
                "          , {EDI_FLOW_ID} ", Environment.NewLine,
                "          , {EDI_JOB_ID} ", Environment.NewLine,
                "          , {EDI_JOB_ZH_TW}, {EDI_JOB_ZH_CN}, {EDI_JOB_EN_US}, {EDI_JOB_TH_TH}, {EDI_JOB_JA_JP} ", Environment.NewLine,
                "          , {EDI_JOB_TYPE}, {EDI_CON_ID}, {OBJECT_NAME}, {DEP_EDI_JOB_ID} ", Environment.NewLine,
                "          , {IS_USE_RES}, {FILE_SOURCE}, {FILE_ENCODING}, {URL_PATH}, {IGNORE_WARNING}, {IS_DISABLE} ", Environment.NewLine,
                "          , {SORT_ORDER} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_ID, Value = para.EDIJobID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_ZH_TW, Value = para.EDIJobZHTW });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_ZH_CN, Value = para.EDIJobZHCN });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_EN_US, Value = para.EDIJobENUS });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_TH_TH, Value = para.EDIJobTHTH });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_JA_JP, Value = para.EDIJobJAJP });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_TYPE, Value = para.EDIJobType });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_CON_ID, Value = para.EDIConID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.OBJECT_NAME, Value = para.ObjectName });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.DEP_EDI_JOB_ID, Value = para.DepEDIJobID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.IS_USE_RES, Value = para.IsUseRes });
			dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.IGNORE_WARNING, Value = para.IgnoreWarning });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.FILE_SOURCE, Value = para.FileSource });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.FILE_ENCODING, Value = para.EDIFileEncoding });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.URL_PATH, Value = para.URLPath });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemEDIJobDetailResult.Success : EnumEditSystemEDIJobDetailResult.Failure;
        }

        public enum EnumDeleteSystemEDIJobDetailResult
        {
            Success, Failure, DataExist
        }

        public string GetJobNewSortOrder(SystemEDIJobDetailPara para) 
        {
            string NewSortOrder = Common.GetEnumDesc(SortorderField.First);
            string commandText = string.Concat(new object[]
            {
                "SELECT TOP 1 SORT_ORDER AS NEW_SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER DESC; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                NewSortOrder = dataRow[SystemEDIJobDetail.DataField.NEW_SORT_ORDER.ToString()].ToString().Replace("0", null);
                if (!string.IsNullOrWhiteSpace(NewSortOrder))
                {
                    NewSortOrder = Convert.ToString(Convert.ToInt32(NewSortOrder) + 1);
                    NewSortOrder = Common.GetEnumDesc(SortorderField.Left) + NewSortOrder + Common.GetEnumDesc(SortorderField.Right);
                    NewSortOrder = NewSortOrder.Substring(NewSortOrder.Length - 6, 6);
                }
            }
            return NewSortOrder;
        }

        public EnumDeleteSystemEDIJobDetailResult DeleteSystemEDIJobDetail(SystemEDIJobDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1) = 'N'; ", Environment.NewLine,
                    "IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_EDI_PARA WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID}) ", Environment.NewLine,
                    "BEGIN ", Environment.NewLine,
                    "    DELETE FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                    "    WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID}; ", Environment.NewLine,
                    "    SET @RESULT = 'Y'; ", Environment.NewLine,
                    "END; ", Environment.NewLine,
                    "SELECT @RESULT; ", Environment.NewLine
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_ID, Value = para.EDIJobID });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemEDIJobDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemEDIJobDetailResult.DataExist;
                }
            }
            catch
            {
                return EnumDeleteSystemEDIJobDetailResult.Failure;
            }
        }
    }
}
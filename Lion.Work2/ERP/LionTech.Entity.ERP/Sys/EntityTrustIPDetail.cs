using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityTrustIPDetail : EntitySys
    {
        public EntityTrustIPDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class TrustIPDetailPara : DBCulture
        {
            public TrustIPDetailPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                IP_BEGIN_ORIGINAL, IP_END_ORIGINAL,
                IP_BEGIN, IP_END,
                COM_ID,
                TRUST_STATUS,
                TRUST_TYPE, TRUST_TYPE_NM,
                SOURCE_TYPE, SOURCE_TYPE_NM,
                REMARK, SORT_ORDER,
                UPD_USER_ID, UPD_USER_NM,

                CODE_NM
            }

            public DBVarChar IPBeginOriginal;
            public DBVarChar IPEndOriginal;
            public DBVarChar IPBegin;
            public DBVarChar IPEnd;
            public DBVarChar ComID;
            public DBChar TrustStatus;
            public DBChar TrustType;
            public DBChar SourceType;
            public DBNVarChar Remark;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
        }

        public class TrustIPDetail : DBTableRow
        {
            public enum DataField
            {
                IP_BEGIN, IP_END,
                COM_ID, COM_NM,
                TRUST_STATUS, TRUST_TYPE, TRUST_TYPE_NM, SOURCE_TYPE, SOURCE_TYPE_NM,
                REMARK, SORT_ORDER
            }

            public DBVarChar IPBegin;
            public DBVarChar IPEnd;
            public DBVarChar ComID;
            public DBNVarChar ComNM;
            public DBChar TrustStatus;
            public DBChar TrustType;
            public DBNVarChar TrustTypeNM;
            public DBChar SourceType;
            public DBNVarChar SourceTypeNM;
            public DBNVarChar Remark;
            public DBVarChar SortOrder;
        }

        public TrustIPDetail SelectTrustIPDetail(TrustIPDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT T.IP_BEGIN, T.IP_END ", Environment.NewLine,
                "     , T.COM_ID, C.COM_NM ", Environment.NewLine,
                "     , T.TRUST_STATUS ", Environment.NewLine,
                "     , T.TRUST_TYPE, A.{TRUST_TYPE} AS TRUST_TYPE_NM ", Environment.NewLine,
                "     , T.SOURCE_TYPE, B.{SOURCE_TYPE} AS SOURCE_TYPE_NM ", Environment.NewLine,
                "     , T.REMARK, T.SORT_ORDER ", Environment.NewLine,
                "FROM SYS_TRUST_IP T ", Environment.NewLine,
                "LEFT JOIN RAW_CM_ORG_COM C ON T.COM_ID=C.COM_ID ", Environment.NewLine,
                "LEFT JOIN CM_CODE A ON T.TRUST_TYPE=A.CODE_ID AND A.CODE_KIND='0031' ", Environment.NewLine,
                "LEFT JOIN CM_CODE B ON T.SOURCE_TYPE=B.CODE_ID AND B.CODE_KIND='0032' ", Environment.NewLine,
                "WHERE T.IP_BEGIN={IP_BEGIN} AND T.IP_END={IP_END} ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN, Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END, Value = para.IPEnd });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.TRUST_TYPE.ToString(), Value = para.GetCultureFieldNM(new DBObject(TrustIPDetailPara.ParaField.CODE_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.SOURCE_TYPE.ToString(), Value = para.GetCultureFieldNM(new DBObject(TrustIPDetailPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                TrustIPDetail trustIPDetail = new TrustIPDetail()
                {
                    IPBegin = new DBVarChar(dataRow[TrustIPDetail.DataField.IP_BEGIN.ToString()]),
                    IPEnd = new DBVarChar(dataRow[TrustIPDetail.DataField.IP_END.ToString()]),
                    ComID = new DBVarChar(dataRow[TrustIPDetail.DataField.COM_ID.ToString()]),
                    ComNM = new DBNVarChar(dataRow[TrustIPDetail.DataField.COM_NM.ToString()]),
                    TrustStatus = new DBChar(dataRow[TrustIPDetail.DataField.TRUST_STATUS.ToString()]),
                    TrustType = new DBChar(dataRow[TrustIPDetail.DataField.TRUST_TYPE.ToString()]),
                    TrustTypeNM = new DBNVarChar(dataRow[TrustIPDetail.DataField.TRUST_TYPE_NM.ToString()]),
                    SourceType = new DBChar(dataRow[TrustIPDetail.DataField.SOURCE_TYPE.ToString()]),
                    SourceTypeNM = new DBNVarChar(dataRow[TrustIPDetail.DataField.SOURCE_TYPE_NM.ToString()]),
                    Remark = new DBNVarChar(dataRow[TrustIPDetail.DataField.REMARK.ToString()]),
                    SortOrder = new DBVarChar(dataRow[TrustIPDetail.DataField.SORT_ORDER.ToString()])
                };
                return trustIPDetail;
            }
            return null;
        }

        public enum EnumValidTrustIPRepeatedResult
        {
            Success, Repeated
        }

        public EnumValidTrustIPRepeatedResult ValidTrustIPRepeatedResult(TrustIPDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @IP_CNT INT; ", Environment.NewLine,

                "SELECT @IP_CNT=COUNT(1) ", Environment.NewLine,
                "FROM SYS_TRUST_IP ", Environment.NewLine,
                "WHERE dbo.FN_GET_TRSUT_BYIP(IP_BEGIN, IP_END, {IP_BEGIN})='Y' ", Environment.NewLine,
                "  AND dbo.FN_GET_TRSUT_BYIP(IP_BEGIN, IP_END, {IP_END})='Y' ", Environment.NewLine,
                "  AND COM_ID={COM_ID} ", Environment.NewLine,
                "  AND TRUST_STATUS={TRUST_STATUS} ", Environment.NewLine,
                "  AND TRUST_TYPE={TRUST_TYPE} ", Environment.NewLine,
                "  AND SOURCE_TYPE={SOURCE_TYPE} ", Environment.NewLine,
            });

            if (para.IPBeginOriginal.GetValue() != para.IPBegin.GetValue() ||
                para.IPEndOriginal.GetValue() != para.IPEnd.GetValue())
            {
                commandText = string.Concat(commandText, "  AND IP_BEGIN<>{IP_BEGIN_ORIGINAL} AND IP_END<>{IP_END_ORIGINAL} ", Environment.NewLine);
            }

            commandText = string.Concat(commandText, "SELECT @IP_CNT; ", Environment.NewLine);

            List <DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN_ORIGINAL, Value = para.IPBeginOriginal });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END_ORIGINAL, Value = para.IPEndOriginal });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN, Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END, Value = para.IPEnd });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.COM_ID, Value = para.ComID });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.TRUST_STATUS, Value = para.TrustStatus });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.TRUST_TYPE, Value = para.TrustType });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.SOURCE_TYPE, Value = para.SourceType });

            DBInt result = new DBInt(base.ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() > 0)
            {
                return EnumValidTrustIPRepeatedResult.Repeated;
            }
            return EnumValidTrustIPRepeatedResult.Success;
        }

        public enum EnumEditTrustIPDetailResult
        {
            Success, Failure
        }

        public EnumEditTrustIPDetailResult EditTrustIPDetail(TrustIPDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_TRUST_IP ", Environment.NewLine,
                "        WHERE IP_BEGIN={IP_BEGIN_ORIGINAL} AND IP_END={IP_END_ORIGINAL}; ", Environment.NewLine,

                "        INSERT INTO SYS_TRUST_IP VALUES ( ", Environment.NewLine,
                "            {IP_BEGIN}, {IP_END}, {COM_ID} ", Environment.NewLine,
                "          , {TRUST_STATUS}, {TRUST_TYPE}, {SOURCE_TYPE}, {REMARK}, {SORT_ORDER} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN_ORIGINAL, Value = para.IPBeginOriginal });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END_ORIGINAL, Value = para.IPEndOriginal });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN, Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END, Value = para.IPEnd });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.COM_ID, Value = para.ComID });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.TRUST_STATUS, Value = para.TrustStatus });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.TRUST_TYPE, Value = para.TrustType });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.SOURCE_TYPE, Value = para.SourceType });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditTrustIPDetailResult.Success : EnumEditTrustIPDetailResult.Failure;
        }

        public enum EnumSyncEditASPMism98Result
        {
            Success, Failure
        }

        public EnumSyncEditASPMism98Result SyncEditASPMism98(TrustIPDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DECLARE @mm98_intfrom NUMERIC; ", Environment.NewLine,
                "        DECLARE @mm98_intto NUMERIC; ", Environment.NewLine,
                "        DECLARE @mm98_other CHAR(1); ", Environment.NewLine,
                
                "        SET @mm98_intfrom=CAST(PARSENAME({IP_BEGIN},1) AS BIGINT) + ", Environment.NewLine,
                "                          CAST(PARSENAME({IP_BEGIN},2) AS BIGINT) * Power(256,1)+ ", Environment.NewLine,
                "                          CAST(PARSENAME({IP_BEGIN},3) AS BIGINT) * Power(256,2)+ ", Environment.NewLine,
                "                          CAST(PARSENAME({IP_BEGIN},4) AS BIGINT) * Power(256,3); ", Environment.NewLine,
                "        SET @mm98_intto=CAST(PARSENAME({IP_END},1) AS BIGINT) + ", Environment.NewLine,
                "                        CAST(PARSENAME({IP_END},2) AS BIGINT) * Power(256,1)+ ", Environment.NewLine,
                "                        CAST(PARSENAME({IP_END},3) AS BIGINT) * Power(256,2)+ ", Environment.NewLine,
                "                        CAST(PARSENAME({IP_END},4) AS BIGINT) * Power(256,3); ", Environment.NewLine,
                "        SET @mm98_other=(CASE WHEN {TRUST_STATUS}='N' THEN 'X' ", Environment.NewLine,
                "                              WHEN {TRUST_STATUS}='Y' AND {SOURCE_TYPE}='C' THEN '' ", Environment.NewLine,
                "                              WHEN {TRUST_STATUS}='Y' AND {SOURCE_TYPE}='N' THEN '' ", Environment.NewLine,
                "                              WHEN {TRUST_STATUS}='Y' AND {SOURCE_TYPE}='P' THEN 'Z' ", Environment.NewLine,
                "                              ELSE {SOURCE_TYPE} END); ", Environment.NewLine,

                "        DELETE FROM mism98 WHERE mm98_from={IP_BEGIN_ORIGINAL} AND mm98_to={IP_END_ORIGINAL}; ", Environment.NewLine,
                "        INSERT INTO mism98 (mm98_from, mm98_to, mm98_intfrom, mm98_intto, mm98_comp, mm98_other, mm98_desc, mm98_mstfn, mm98_mdate) ", Environment.NewLine,
                "                    VALUES ({IP_BEGIN}, {IP_END}, @mm98_intfrom, @mm98_intto, {COM_ID}, @mm98_other, {REMARK}, {UPD_USER_NM}, GETDATE()); ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN_ORIGINAL, Value = para.IPBeginOriginal });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END_ORIGINAL, Value = para.IPEndOriginal });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN, Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END, Value = para.IPEnd });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.COM_ID, Value = para.ComID });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.TRUST_STATUS, Value = para.TrustStatus });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.SOURCE_TYPE, Value = para.SourceType });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumSyncEditASPMism98Result.Success : EnumSyncEditASPMism98Result.Failure;
        }

        public enum EnumDeleteTrustIPDetailResult
        {
            Success, Failure
        }

        public EnumDeleteTrustIPDetailResult DeleteTrustIPDetail(TrustIPDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_TRUST_IP ", Environment.NewLine,
                "        WHERE IP_BEGIN={IP_BEGIN} AND IP_END={IP_END}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN, Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END, Value = para.IPEnd });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteTrustIPDetailResult.Success : EnumDeleteTrustIPDetailResult.Failure;
        }

        public enum EnumSyncDeleteASPMism98Result
        {
            Success, Failure
        }

        public EnumSyncDeleteASPMism98Result SyncDeleteASPMism98(TrustIPDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                
                "        DELETE FROM mism98 WHERE mm98_from={IP_BEGIN} AND mm98_to={IP_END}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_BEGIN, Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = TrustIPDetailPara.ParaField.IP_END, Value = para.IPEnd });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumSyncDeleteASPMism98Result.Success : EnumSyncDeleteASPMism98Result.Failure;
        }
    }
}
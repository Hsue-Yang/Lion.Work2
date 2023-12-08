using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Subscriber
{
    public class EntityERPAPIspfm00 : EntitySubscriber
    {
        public EntityERPAPIspfm00(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RawCMOrgUnitPara
        {
            public enum ParaField
            {
                UNIT_ID, UNIT_NM, IS_CEASED, UPD_USER_ID, UPD_EDI_EVENT_NO
            }

            public DBVarChar UnitID;
            public DBNVarChar UnitNM;
            public DBChar IsCeased;
            public DBChar UpdEDIEventNo;
        }

        public enum EnumEditRawCMOrgUnitResult
        {
            Success, Failure
        }

        public EnumEditRawCMOrgUnitResult EditRawCMOrgUnit(RawCMOrgUnitPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM RAW_CM_ORG_UNIT WHERE UNIT_ID={UNIT_ID}; ", Environment.NewLine,
                "        INSERT INTO RAW_CM_ORG_UNIT VALUES ( ", Environment.NewLine,
                "            {UNIT_ID}, {UNIT_NM}, {IS_CEASED}, {UPD_USER_ID}, GETDATE(), {UPD_EDI_EVENT_NO} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = RawCMOrgUnitPara.ParaField.UNIT_ID.ToString(), Value = para.UnitID });
            dbParameters.Add(new DBParameter { Name = RawCMOrgUnitPara.ParaField.UNIT_NM.ToString(), Value = para.UnitNM });
            dbParameters.Add(new DBParameter { Name = RawCMOrgUnitPara.ParaField.IS_CEASED.ToString(), Value = para.IsCeased });
            dbParameters.Add(new DBParameter { Name = RawCMOrgUnitPara.ParaField.UPD_USER_ID.ToString(), Value = base.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RawCMOrgUnitPara.ParaField.UPD_EDI_EVENT_NO.ToString(), Value = para.UpdEDIEventNo });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditRawCMOrgUnitResult.Success : EnumEditRawCMOrgUnitResult.Failure;
        }

        public void DeleteRawCMOrgUnit(RawCMOrgUnitPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DELETE FROM RAW_CM_ORG_UNIT WHERE UNIT_ID={UNIT_ID}; ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMOrgUnitPara.ParaField.UNIT_ID.ToString(), Value = para.UnitID });

            base.ExecuteNonQuery(commandText, dbParameters);
        }
    }
}
// 新增日期：2018-10-25
// 新增人員：方道筌
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.Subscriber
{
    public class EntityERPAPOptbm00 : EntitySubscriber
    {
        public EntityERPAPOptbm00(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RawCMCountryPara
        {
            public enum ParaField
            {
                COUNTRY_ID, IS_DISABLE,
                COUNTRY_NM_DISPLAY, COUNTY_NM_ZH_TW, COUNTY_NM_EN_US,
                UPD_USER_ID
            }

            public DBChar CountryID;
            public DBChar IsDisable;
            public DBNVarChar CountryNmDisplay;
            public DBNVarChar CountryNmZhTw;
            public DBVarChar CountryNmEnUs;
        }

        public enum EnumEditRawCMCountryResult
        {
            Success, Failure
        }

        public EnumEditRawCMCountryResult EditRawCMCountry(RawCMCountryPara para)
        {
            string commandText = string.Join(Environment.NewLine,new object[]
            {
                "DECLARE @RESULT CHAR(1); ",
                "SET @RESULT = 'N'; ",
                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",
                "        DELETE FROM RAW_CM_COUNTRY ",
                "         WHERE COUNTRY_ID={COUNTRY_ID}; ",

                "        INSERT INTO RAW_CM_COUNTRY ",
                "        VALUES ( {COUNTRY_ID}",
                "               , {IS_DISABLE}",
                "               , {COUNTRY_NM_DISPLAY}",
                "               , {COUNTY_NM_ZH_TW}",
                "               , {COUNTY_NM_EN_US}",
                "               , {UPD_USER_ID}, GETDATE() ",
                "               ); ",

                "           SET @RESULT = 'Y'; ",
                "        COMMIT; ",
                "    END TRY ", 
                "    BEGIN CATCH ",
                "        SET @RESULT = 'N'; ",
                "        ROLLBACK TRANSACTION; ",
                "    END CATCH; ",
                "SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMCountryPara.ParaField.COUNTRY_ID.ToString(), Value = para.CountryID });
            dbParameters.Add(new DBParameter { Name = RawCMCountryPara.ParaField.COUNTRY_NM_DISPLAY.ToString(), Value = para.CountryNmDisplay });
            dbParameters.Add(new DBParameter { Name = RawCMCountryPara.ParaField.COUNTY_NM_EN_US.ToString(), Value = para.CountryNmEnUs });
            dbParameters.Add(new DBParameter { Name = RawCMCountryPara.ParaField.COUNTY_NM_ZH_TW.ToString(), Value = para.CountryNmZhTw });
            dbParameters.Add(new DBParameter { Name = RawCMCountryPara.ParaField.IS_DISABLE.ToString(), Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = RawCMCountryPara.ParaField.UPD_USER_ID.ToString(), Value = UpdUserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditRawCMCountryResult.Success : EnumEditRawCMCountryResult.Failure;
        }
        public enum EnumDeleteRawCMCountryResult
        {
            Success, Failure
        }

        public EnumDeleteRawCMCountryResult DeleteRawCMCountry(RawCMCountryPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                " DECLARE @RESULT CHAR(1) = 'N';",
                " DECLARE @ERROR_LINE INT;",
                " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                " BEGIN TRANSACTION",
                " BEGIN TRY",
                "     DELETE FROM RAW_CM_COUNTRY ",
                "      WHERE COUNTRY_ID={COUNTRY_ID}; ",
                "        SET @RESULT = 'Y';",
                "     COMMIT;",
                " END TRY",
                " BEGIN CATCH",
                "        SET @RESULT = 'N';",
                "        SET @ERROR_LINE = ERROR_LINE();",
                "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "   ROLLBACK TRANSACTION;",
                " END CATCH;",
                " SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMCountryPara.ParaField.COUNTRY_ID.ToString(), Value = para.CountryID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteRawCMCountryResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}
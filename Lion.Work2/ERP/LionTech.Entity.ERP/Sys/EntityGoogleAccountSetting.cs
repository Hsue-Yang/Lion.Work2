using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityGoogleAccountSetting : EntitySys
    {
        public EntityGoogleAccountSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class GoogleAccountSettingValue : ValueListRow
        {
            public enum ValueField
            {
                Picklist, UserID, GoogleAccount, IsGAccEnable
            }

            public string PickList { get; set; }
            public string UserID { get; set; }
            public string GoogleAccount { get; set; }
            public string IsGAccEnable { get; set; }

            public DBChar GetPickList()
            {
                return new DBChar(PickList);
            }
            public DBVarChar GetUserID()
            {
                return new DBVarChar(UserID);
            }
            public DBVarChar GetGoogleAccount()
            {
                return new DBVarChar(GoogleAccount);
            }
            public DBChar GetIsGAccEnable()
            {                 
                if (string.IsNullOrWhiteSpace(IsGAccEnable))
                {
                    IsGAccEnable = EnumYN.N.ToString();
                }
                return new DBChar(IsGAccEnable);
            }
            
        }

        public class GoogleAccountSettingPara : DBCulture
        {
            public GoogleAccountSettingPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                 PICK_LIST,
                 USER_ID, ISONLY_GACC_ENABLE,
                 USER_GOOGLE_ACCOUNT, IS_GACC_ENABLE, UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBChar IsOnlyGAccEnable;
            public DBVarChar UpdUserID;
        }

        public class GoogleAccountSetting : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM, COM_NM, UNIT_NM, IS_LEFT,
                USER_GOOGLE_ACCOUNT, IS_GACC_ENABLE, UPD_USER_NM, UPD_DT,
                BG_COLOR
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBNVarChar ComNM;
            public DBNVarChar UnitNM;
            public DBChar IsLeft;

            public DBVarChar GoogleAccount;
            public DBChar IsGAccEnable;

            public DBVarChar UpdUser;
            public DBDateTime UpdDT;

            public DBInt BgColor;
        }


        public List<GoogleAccountSetting> SelectGoogleAccountSettingList(GoogleAccountSettingPara para)
        {
            #region - commandWhere -
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.UserID.GetValue()))
            {
                commandWhere = commandWhere + (string.IsNullOrWhiteSpace(commandWhere) ? "WHERE " : "  AND ");
                commandWhere = string.Concat(new object[] { commandWhere, "D.USER_ID={USER_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.IsOnlyGAccEnable.GetValue()))
            {
                commandWhere = commandWhere + (string.IsNullOrWhiteSpace(commandWhere) ? "WHERE " : "  AND ");
                commandWhere = string.Concat(new object[] { commandWhere, "D.IS_GACC_ENABLE = 'Y' AND D.USER_GOOGLE_ACCOUNT IS NOT NULL ", Environment.NewLine });
            }
            #endregion

            string commandText = string.Concat(new object[]
            {
                "SELECT  M.USER_ID ", Environment.NewLine,
                "     , dbo.FN_GET_IDNM(M.USER_ID, U.USER_NM ) AS USER_NM ", Environment.NewLine,
                "     , dbo.FN_GET_IDNM(U.USER_COM_ID, C.COM_NM) AS COM_NM ", Environment.NewLine,
                "     , dbo.FN_GET_IDNM(U.USER_UNIT_ID, O.UNIT_NM )AS UNIT_NM ", Environment.NewLine,
                "     , U.IS_LEFT ", Environment.NewLine,
                "     , D.USER_GOOGLE_ACCOUNT ", Environment.NewLine,
                "     , D.IS_GACC_ENABLE ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(D.UPD_USER_ID) AS UPD_USER_NM ", Environment.NewLine,
                "     , D.UPD_DT ", Environment.NewLine,
                "     , (CASE WHEN U.IS_LEFT = 'Y' AND U.USER_COM_ID IN ('!', 'CZ') ", Environment.NewLine,
                "                                  AND D.USER_GOOGLE_ACCOUNT IS NOT NULL AND D.IS_GACC_ENABLE ='Y' THEN 1 ", Environment.NewLine,
                "             WHEN	U.IS_LEFT = 'N' AND U.USER_COM_ID NOT IN ('!', 'CZ') ", Environment.NewLine,
                "                                  AND D.USER_GOOGLE_ACCOUNT IS NOT NULL AND D.IS_GACC_ENABLE ='Y' THEN -1 ", Environment.NewLine,
                "             ELSE 0 END) AS BG_COLOR ", Environment.NewLine,
                "FROM SYS_USER_MAIN M ", Environment.NewLine,
                "LEFT JOIN RAW_CM_USER U ON M.USER_ID = U.USER_ID ", Environment.NewLine,
                "LEFT JOIN RAW_CM_ORG_COM C ON U.USER_COM_ID = C.COM_ID ", Environment.NewLine,
                "LEFT JOIN RAW_CM_ORG_UNIT O ON U.USER_UNIT_ID = O.UNIT_ID ", Environment.NewLine,
                "LEFT JOIN SYS_USER_DETAIL D ON U.USER_ID = D.USER_ID ", Environment.NewLine,
                commandWhere,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = GoogleAccountSettingPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = GoogleAccountSettingPara.ParaField.ISONLY_GACC_ENABLE.ToString(), Value = para.IsOnlyGAccEnable });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<GoogleAccountSetting> GoogleAccountSettingList = new List<GoogleAccountSetting>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GoogleAccountSetting googleAccountSetting = new GoogleAccountSetting()
                    {
                        UserID = new DBVarChar(dataRow[GoogleAccountSetting.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[GoogleAccountSetting.DataField.USER_NM.ToString()]),
                        ComNM = new DBNVarChar(dataRow[GoogleAccountSetting.DataField.COM_NM.ToString()]),
                        UnitNM = new DBNVarChar(dataRow[GoogleAccountSetting.DataField.UNIT_NM.ToString()]),
                        IsLeft = new DBChar(dataRow[GoogleAccountSetting.DataField.IS_LEFT.ToString()]),

                        GoogleAccount = new DBVarChar(dataRow[GoogleAccountSetting.DataField.USER_GOOGLE_ACCOUNT.ToString()]),
                        IsGAccEnable = new DBChar(dataRow[GoogleAccountSetting.DataField.IS_GACC_ENABLE.ToString()]),

                        UpdUser = new DBVarChar(dataRow[GoogleAccountSetting.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[GoogleAccountSetting.DataField.UPD_DT.ToString()]),

                        BgColor = new DBInt(dataRow[GoogleAccountSetting.DataField.BG_COLOR.ToString()]),
                    };
                    GoogleAccountSettingList.Add(googleAccountSetting);
                }
                return GoogleAccountSettingList;
            }
            return null;
        }

        public enum EnumEditGoogleAccountSettingResult
        {
            Success, Failure
        }

        public EnumEditGoogleAccountSettingResult EditGoogleAccountSetting(GoogleAccountSettingPara para, List<GoogleAccountSettingValue> googleAccountValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();
           
            foreach (GoogleAccountSettingValue googleAccountValue in googleAccountValueList)
            {
                if (!string.IsNullOrWhiteSpace(googleAccountValue.PickList) && googleAccountValue.PickList == EnumYN.Y.ToString())
                {
                    string updateCommand = string.Concat(new object[]
                    {
                        "UPDATE SYS_USER_DETAIL ", Environment.NewLine,
                        "SET USER_GOOGLE_ACCOUNT = {USER_GOOGLE_ACCOUNT} ", Environment.NewLine,
                        "    , IS_GACC_ENABLE = {IS_GACC_ENABLE} ", Environment.NewLine,
                        "    , UPD_USER_ID = {UPD_USER_ID} ", Environment.NewLine,
                        "    , UPD_DT = GETDATE() ", Environment.NewLine,
                        "WHERE USER_ID = {USER_ID}; ", Environment.NewLine,
                    });
                    dbParameters.Add(new DBParameter { Name = GoogleAccountSettingPara.ParaField.USER_ID, Value = googleAccountValue.GetUserID() });
                    dbParameters.Add(new DBParameter { Name = GoogleAccountSettingPara.ParaField.USER_GOOGLE_ACCOUNT, Value = googleAccountValue.GetGoogleAccount() });
                    dbParameters.Add(new DBParameter { Name = GoogleAccountSettingPara.ParaField.IS_GACC_ENABLE, Value = googleAccountValue.GetIsGAccEnable() });
                    dbParameters.Add(new DBParameter { Name = GoogleAccountSettingPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, updateCommand, dbParameters));
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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditGoogleAccountSettingResult.Success : EnumEditGoogleAccountSettingResult.Failure;
        }
    }
}
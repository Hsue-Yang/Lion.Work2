using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserBasicInfoDetail : EntitySys
    {
        public EntityUserBasicInfoDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserBasicInfoDetailPara : DBCulture
        {
            public UserBasicInfoDetailPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID,
                CODE_NM
            }

            public DBVarChar UserID;
        }

        public class UserBasicInfoDetail : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                COM_ID, COM_NM,
                UNIT_ID, UNIT_NM,
                RESTRICT_TYPE, RESTRICT_TYPE_NM,
                ERROR_TIMES, IS_LOCK,
                IS_DISABLE, IS_LEFT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar ComID;
            public DBNVarChar ComNM;
            public DBVarChar UnitID;
            public DBNVarChar UnitNM;
            public DBVarChar RestrictType;
            public DBNVarChar RestrictTypeNM;
            public DBInt ErrorTimes;
            public DBChar IsLock;
            public DBChar IsDisable;
            public DBChar IsLeft;
        }

        public UserBasicInfoDetail SelectUserBasicInfoDetail(UserBasicInfoDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT R.USER_ID, R.USER_NM ", Environment.NewLine,
                "     , R.USER_COM_ID AS COM_ID, O.COM_NM ", Environment.NewLine,
                "     , R.USER_UNIT_ID AS UNIT_ID, U.UNIT_NM ", Environment.NewLine,
                "     , M.RESTRICT_TYPE, C.{CODE_NM} AS RESTRICT_TYPE_NM ", Environment.NewLine,
                "     , M.ERROR_TIMES, M.IS_LOCK ", Environment.NewLine,
                "     , M.IS_DISABLE, R.IS_LEFT ", Environment.NewLine,
                "FROM RAW_CM_USER R ", Environment.NewLine,
                "LEFT JOIN SYS_USER_MAIN M ON R.USER_ID=M.USER_ID ", Environment.NewLine,
                "LEFT JOIN RAW_CM_ORG_COM O ON R.USER_COM_ID=O.COM_ID ", Environment.NewLine,
                "LEFT JOIN RAW_CM_ORG_UNIT U ON R.USER_UNIT_ID=U.UNIT_ID ", Environment.NewLine,
                "LEFT JOIN CM_CODE C ON M.RESTRICT_TYPE=C.CODE_ID AND C.CODE_KIND='0033' ", Environment.NewLine,
                "WHERE R.USER_ID={USER_ID} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserBasicInfoDetailPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserBasicInfoDetailPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserBasicInfoDetailPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserBasicInfoDetail userBasicInfoDetail = new UserBasicInfoDetail()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.USER_ID.ToString()]),
                    UserNM = new DBNVarChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.USER_NM.ToString()]),
                    ComID = new DBVarChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.COM_ID.ToString()]),
                    ComNM = new DBNVarChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.COM_NM.ToString()]),

                    UnitID = new DBVarChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.UNIT_ID.ToString()]),
                    UnitNM = new DBNVarChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.UNIT_NM.ToString()]),
                    RestrictType = new DBVarChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.RESTRICT_TYPE.ToString()]),
                    RestrictTypeNM = new DBNVarChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.RESTRICT_TYPE_NM.ToString()]),

                    ErrorTimes = new DBInt(dataTable.Rows[0][UserBasicInfoDetail.DataField.ERROR_TIMES.ToString()]),
                    IsLock = new DBChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.IS_LOCK.ToString()]),
                    IsDisable = new DBChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.IS_DISABLE.ToString()]),
                    IsLeft = new DBChar(dataTable.Rows[0][UserBasicInfoDetail.DataField.IS_LEFT.ToString()])
                };
                return userBasicInfoDetail;
            }
            return null;
        }
    }
}
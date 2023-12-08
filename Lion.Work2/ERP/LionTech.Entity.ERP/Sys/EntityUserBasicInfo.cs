using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserBasicInfo : EntitySys
    {
        public EntityUserBasicInfo(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserBasicInfoPara : DBCulture
        {
            public UserBasicInfoPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID, USER_NM,
                CONNECT_DT_BEGIN, CONNECT_DT_END,
                CODE_NM
            }

            public DBVarChar UserID;
            public DBObject UserNM;
            public DBChar IsDisable;
            public DBChar IsLeft;
            public DBChar ConnectDTBegin;
            public DBChar ConnectDTEnd;
        }

        public class UserBasicInfo : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                COM_ID, COM_NM,
                UNIT_ID, UNIT_NM,
                RESTRICT_TYPE, RESTRICT_TYPE_NM,
                IS_LOCK, IS_DISABLE, IS_LEFT,
                LAST_CONNECT_DT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar ComID;
            public DBNVarChar ComNM;
            public DBVarChar UnitID;
            public DBNVarChar UnitNM;
            public DBVarChar RestrictType;
            public DBNVarChar RestrictTypeNM;
            public DBChar IsLock;
            public DBChar IsDisable;
            public DBChar IsLeft;
            public DBDateTime LastConnectDT;
        }

        public List<UserBasicInfo> SelectUserBasicInfoList(UserBasicInfoPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.UserID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere,
                    (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " R.USER_ID={USER_ID} ", Environment.NewLine
                });
            }

            if (!string.IsNullOrWhiteSpace(para.UserNM.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere,
                    (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " R.USER_NM LIKE N'%{USER_NM}%' ", Environment.NewLine
                });
            }

            if (!string.IsNullOrWhiteSpace(para.IsDisable.GetValue()) && para.IsDisable.GetValue() == EnumYN.Y.ToString())
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere,
                    (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " M.IS_DISABLE='Y' ", Environment.NewLine
                });
            }

            if (!string.IsNullOrWhiteSpace(para.IsLeft.GetValue()) && para.IsLeft.GetValue() == EnumYN.Y.ToString())
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere,
                    (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " R.IS_LEFT='Y' ", Environment.NewLine
                });
            }

            if (!string.IsNullOrWhiteSpace(para.ConnectDTBegin.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere,
                    (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " Z.LAST_CONNECT_DT>={CONNECT_DT_BEGIN} ", Environment.NewLine
                });
            }

            if (!string.IsNullOrWhiteSpace(para.ConnectDTEnd.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere,
                    (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " Z.LAST_CONNECT_DT<={CONNECT_DT_END} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT R.USER_ID, dbo.FN_GET_IDNM(R.USER_ID, R.USER_NM) AS USER_NM ", Environment.NewLine,
                "     , R.USER_COM_ID AS COM_ID, O.COM_NM ", Environment.NewLine,
                "     , R.USER_UNIT_ID AS UNIT_ID, U.UNIT_NM ", Environment.NewLine,
                "     , M.RESTRICT_TYPE, C.{CODE_NM} AS RESTRICT_TYPE_NM ", Environment.NewLine,
                "     , M.IS_LOCK, M.IS_DISABLE, R.IS_LEFT ", Environment.NewLine,
                "     , Z.LAST_CONNECT_DT ", Environment.NewLine,
                "FROM RAW_CM_USER R ", Environment.NewLine,
                "LEFT JOIN SYS_USER_MAIN M ON R.USER_ID=M.USER_ID ", Environment.NewLine,
                "LEFT JOIN RAW_CM_ORG_COM O ON R.USER_COM_ID=O.COM_ID ", Environment.NewLine,
                "LEFT JOIN RAW_CM_ORG_UNIT U ON R.USER_UNIT_ID=U.UNIT_ID ", Environment.NewLine,
                "LEFT JOIN CM_CODE C ON M.RESTRICT_TYPE=C.CODE_ID AND C.CODE_KIND='0033' ", Environment.NewLine,
                "LEFT JOIN( ", Environment.NewLine,
                "    SELECT USER_ID, MAX(LAST_CONNECT_DT) AS LAST_CONNECT_DT ", Environment.NewLine,
                "    FROM SYS_USER_CONNECT ", Environment.NewLine,
                "    GROUP BY USER_ID ", Environment.NewLine,
                ") Z ON R.USER_ID=Z.USER_ID ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY R.USER_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserBasicInfoPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserBasicInfoPara.ParaField.USER_NM.ToString(), Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = UserBasicInfoPara.ParaField.CONNECT_DT_BEGIN.ToString(), Value = para.ConnectDTBegin });
            dbParameters.Add(new DBParameter { Name = UserBasicInfoPara.ParaField.CONNECT_DT_END.ToString(), Value = para.ConnectDTEnd });
            dbParameters.Add(new DBParameter { Name = UserBasicInfoPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserBasicInfoPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserBasicInfo> userBasicInfoList = new List<UserBasicInfo>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserBasicInfo userBasicInfo = new UserBasicInfo()
                    {
                        UserID = new DBVarChar(dataRow[UserBasicInfo.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[UserBasicInfo.DataField.USER_NM.ToString()]),
                        ComID = new DBVarChar(dataRow[UserBasicInfo.DataField.COM_ID.ToString()]),
                        ComNM = new DBNVarChar(dataRow[UserBasicInfo.DataField.COM_NM.ToString()]),
                        UnitID = new DBVarChar(dataRow[UserBasicInfo.DataField.UNIT_ID.ToString()]),
                        UnitNM = new DBNVarChar(dataRow[UserBasicInfo.DataField.UNIT_NM.ToString()]),
                        RestrictType = new DBVarChar(dataRow[UserBasicInfo.DataField.RESTRICT_TYPE.ToString()]),
                        RestrictTypeNM = new DBNVarChar(dataRow[UserBasicInfo.DataField.RESTRICT_TYPE_NM.ToString()]),
                        IsLock = new DBChar(dataRow[UserBasicInfo.DataField.IS_LOCK.ToString()]),
                        IsDisable = new DBChar(dataRow[UserBasicInfo.DataField.IS_DISABLE.ToString()]),
                        IsLeft = new DBChar(dataRow[UserBasicInfo.DataField.IS_LEFT.ToString()]),
                        LastConnectDT = new DBDateTime(dataRow[UserBasicInfo.DataField.LAST_CONNECT_DT.ToString()])
                    };
                    userBasicInfoList.Add(userBasicInfo);
                }
                return userBasicInfoList;
            }
            return null;
        }
    }
}
// 新增日期：2018-01-09
// 新增人員：廖先駿
// 新增內容：元素權限
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemFunElm : EntitySys
    {
        public EntitySystemFunElm(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得元素權限清單 -
        public class SystemFunElmPara : DBCulture
        {
            public SystemFunElmPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                ELM_ID,
                ELM_NAME,
                SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                IS_DISABLE,
                ELM_NM
            }

            public DBVarChar ElmID;
            public DBNVarChar ElmName;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
            public DBChar IsDisable;
        }

        public class SystemFunElm : DBTableRow
        {
            public DBVarChar ElmID;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
            public DBNVarChar ElmNM;
            public DBTinyInt DefaultDisplaySts;
            public DBChar IsDisable;
            public DBVarChar UpdUserIDNM;
            public DBDateTime UpdDT;
        }

        public List<SystemFunElm> SelectSystemFunElmList(SystemFunElmPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandWhere = new List<string>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT ELM_ID AS ElmID",
                "     , SYS_ID AS SysID",
                "     , FUN_CONTROLLER_ID AS FunControllerID",
                "     , FUN_ACTION_NAME AS FunActionNM",
                "     , {ELM_NM} AS ElmNM",
                "     , DEFAULT_DISPLAY_STS AS DefaultDisplaySts",
                "     , IS_DISABLE AS IsDisable",
                "     , dbo.FN_GET_USER_NM(UPD_USER_ID) AS UpdUserIDNM",
                "     , UPD_DT AS UpdDT",
                "  FROM SYS_SYSTEM_FUN_ELM",
                " WHERE SYS_ID = {SYS_ID}"
            }));

            if (para.IsDisable.GetValue() == EnumYN.N.ToString())
            {
                commandWhere.Add("IS_DISABLE = {IS_DISABLE}");
                dbParameters.Add(new DBParameter { Name = SystemFunElmPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            }

            if (para.ElmName.IsNull() == false)
            {
                commandWhere.Add("{ELM_NM} LIKE '%' + {ELM_NAME} + '%'");
                dbParameters.Add(new DBParameter { Name = SystemFunElmPara.ParaField.ELM_NAME, Value = para.ElmName });
            }

            if (para.ElmID.IsNull() == false)
            {
                commandWhere.Add("ELM_ID LIKE '%' + {ELM_ID} + '%'");
                dbParameters.Add(new DBParameter { Name = SystemFunElmPara.ParaField.ELM_ID, Value = para.ElmID });
            }

            if (para.FunControllerID.IsNull() == false)
            {
                commandWhere.Add("FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}");
                dbParameters.Add(new DBParameter { Name = SystemFunElmPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            }

            if (para.FunActionNM.IsNull() == false)
            {
                commandWhere.Add("FUN_ACTION_NAME = {FUN_ACTION_NAME}");
                dbParameters.Add(new DBParameter { Name = SystemFunElmPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            }

            if (commandWhere.Any())
            {
                commandText.AppendLine($" AND {string.Join(" AND ", commandWhere)}");
            }

            dbParameters.Add(new DBParameter { Name = SystemFunElmPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = SystemFunElmPara.ParaField.ELM_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunElmPara.ParaField.ELM_NM.ToString())) });

            return GetEntityList<SystemFunElm>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}
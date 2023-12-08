using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.SystemSetting
{
    public class EntitySystemFun : EntitySystemSetting
    {
        public EntitySystemFun(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemFunPara
        {
            public enum ParaField
            {
                SYS_ID
            }

            public DBVarChar SysID;
        }

        public class SystemFun : DBTableRow
        {
            public DBVarChar ControllerID;
            public DBVarChar ActionName;
            public DBNVarChar ActionNMzhTW;
            public DBNVarChar ActionNMzhCN;
            public DBNVarChar ActionNMenUS;
            public DBNVarChar ActionNMthTH;
            public DBNVarChar ActionNMjaJP;
            public DBNVarChar ActionNMkoKR;
        }

        public List<SystemFun> SelectSystemFunList(SystemFunPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT F.FUN_CONTROLLER_ID AS ControllerID",
                "     , F.FUN_ACTION_NAME AS ActionName",
                "     , F.FUN_NM_ZH_TW AS ActionNMzhTW",
                "     , F.FUN_NM_ZH_CN AS ActionNMzhCN",
                "     , F.FUN_NM_EN_US AS ActionNMenUS",
                "     , F.FUN_NM_TH_TH AS ActionNMthTH",
                "     , F.FUN_NM_JA_JP AS ActionNMjaJP",
                "     , F.FUN_NM_KO_KR AS ActionNMkoKR",
                "  FROM SYS_SYSTEM_FUN F",
                "WHERE F.SYS_ID={SYS_ID} "
            });

            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            return GetEntityList<SystemFun>(commandText, dbParameters);
        }
    }
}

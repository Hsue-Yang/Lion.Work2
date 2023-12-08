using System;
using System.ComponentModel;
using LionTech.Entity;
using LionTech.Entity.B2P;

namespace B2PAP.Models
{
    public class _BaseAPModel : _BaseModel
    {
        public enum EnumTabController
        {
            Pub, Sys, Dev, Help
        }

        public enum EnumTabAction
        {
            [Description("SystemEDIFlow")]
            SysSystemEDIFlow,
            [Description("SystemEDIJob")]
            SysSystemEDIJob,
            [Description("SystemEDICon")]
            SysSystemEDICon,
            [Description("SystemEDIPara")]
            SysSystemEDIPara,
            [Description("SystemEDIFlowLog")]
            SysSystemEDIFlowLog,
            [Description("SystemEDIJobLog")]
            SysSystemEDIJobLog,
            [Description("SystemEDIFlowLogSetting")]
            SysSystemEDIFlowLogSetting,

            [Description("SystemEventGroup")]
            SysSystemEventGroup,
            [Description("SystemEvent")]
            SysSystemEvent,
            [Description("SystemEventEDI")]
            SysSystemEventEDI,

            [Description("SystemEventTargetEDI")]
            SysSystemEventTargetEDI,
            [Description("SystemEventTargetSend")]
            SysSystemEventTargetSend,

            [Description("SystemSetting")]
            SysSystemSetting,
            [Description("SystemRole")]
            SysSystemRole,
            [Description("SystemPurview")]
            SysSystemPurview,
            [Description("SystemFunMenu")]
            SysSystemFunMenu,
            [Description("SystemFunGroup")]
            SysSystemFunGroup,
            [Description("SystemFun")]
            SysSystemFun,
            [Description("SystemFunDetail")]
            SysSystemFunDetail,
            [Description("SystemFunAssign")]
            SysSystemFunAssign,

            [Description("UserRoleFun")]
            SysUserRoleFun,
            [Description("UserRoleFunDetail")]
            SysUserRoleFunDeatil,
            [Description("UserFunction")]
            SysUserFunction,
            [Description("UserSystem")]
            SysUserSystem
        }

        public enum EnumCookieKey
        {
            
        }

        #region - Log -
        public bool GetRecordUserSystemRoleResult(string userID, string updUserID, string sysID, string ipAddress)
        {
            try
            {
                Entity_BaseAP.RecordUserSystemRolePara para = new Entity_BaseAP.RecordUserSystemRolePara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID)),
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (new Entity_BaseAP(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .RecordUserSystemRole(para) == Entity_BaseAP.EnumRecordUserSystemRoleResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetRecordUserFunctionResult(string userID, string updUserID, string sysID, string ipAddress)
        {
            try
            {
                Entity_BaseAP.RecordUserFunctionPara para = new Entity_BaseAP.RecordUserFunctionPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID)),
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (new Entity_BaseAP(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .RecordUserFunction(para) == Entity_BaseAP.EnumRecordUserFunctionResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
        #endregion
    }
}
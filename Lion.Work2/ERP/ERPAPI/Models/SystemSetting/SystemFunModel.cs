using LionTech.APIService.SystemSetting;
using LionTech.Entity;
using LionTech.Entity.ERP.SystemSetting;
using System.Collections.Generic;
using System.Linq;

namespace ERPAPI.Models.SystemSetting
{
    public class SystemFunModel : SystemSettingModel
    {
        #region - Constructor -
        public SystemFunModel()
        {
            _entity = new EntitySystemFun(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        #endregion

        #region - Private -
        private readonly EntitySystemFun _entity;
        #endregion

        public List<SystemFun> GetSystemFunList()
        {
            EntitySystemFun.SystemFunPara para = new EntitySystemFun.SystemFunPara
            {
                SysID = new DBVarChar(ClientSysID)
            };
            return (from s in _entity.SelectSystemFunList(para)
                    select new SystemFun
                    {
                        ControllerID = s.ControllerID.GetValue(),
                        ActionName = s.ActionName.GetValue(),
                        ActionNMzhTW = s.ActionNMzhTW.GetValue(),
                        ActionNMzhCN = s.ActionNMzhCN.GetValue(),
                        ActionNMenUS = s.ActionNMenUS.GetValue(),
                        ActionNMthTH = s.ActionNMthTH.GetValue(),
                        ActionNMjaJP = s.ActionNMjaJP.GetValue(),
                        ActionNMkoKR = s.ActionNMkoKR.GetValue()
                    }).ToList();
        }
    }
}
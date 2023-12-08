using System.Collections.Generic;
using System.Web.Script.Serialization;
using LionTech.Entity.ERP.EDIService;

namespace LionTech.EDIService.ERPExternal
{
    public partial class SUBS
    {
        public class ERPAPSysUserSystemRoleEditModel
        {
            public List<string> TargetSysIDList { get; set; }

            public string UserID;
            public List<string> RoleIDList;
        }

        private static string GetRequestEventParaERPAPSysUserSystemRoleEdit(EntityERPExternal.SystemEventTarget systemEventTarget, string eventPara)
        {
            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
            ERPAPSysUserSystemRoleEditModel eventParaModel = jsonConvert.Deserialize<ERPAPSysUserSystemRoleEditModel>(eventPara);

            ERPAPSysUserSystemRoleEditModel returnModel = new ERPAPSysUserSystemRoleEditModel();
            if (eventParaModel.TargetSysIDList != null)
            {
                returnModel.TargetSysIDList = new List<string>(eventParaModel.TargetSysIDList);
            }
            returnModel.UserID = eventParaModel.UserID;

            if (eventParaModel.RoleIDList != null)
            {
                returnModel.RoleIDList = new List<string>();

                foreach (string roleID in eventParaModel.RoleIDList)
                {
                    string[] roleIDArray = roleID.Split('|');
                    if (roleIDArray.Length == 2 && roleIDArray[0] == systemEventTarget.TargetSysID.GetValue())
                    {
                        returnModel.RoleIDList.Add(roleIDArray[1]);
                    }
                }
            }

            return jsonConvert.Serialize(returnModel);
        }
    }
}

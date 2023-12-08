using System;
using System.Linq;
using System.Web.Script.Serialization;
using LionTech.Entity;
using LionTech.Entity.ERP.Pub;
using LionTech.Utility;

namespace ERPAP.Models.Pub
{
    public class BulletinModel : PubModel
    {
        public string SystemID { get; set; }

        public string IsFirstLogin { get; set; }

        public string TargetPathListJsonString { get; set; }

        public string TargetPath { get; set; }
        
        public BulletinModel()
        {
            this.IsFirstLogin = EnumYN.N.ToString();
        }

        public bool ValidDailyFirstLogin(string userID)
        {
            try
            {
                EntityBulletin.UserLoginInfoPara para = new EntityBulletin.UserLoginInfoPara()
                {
                    UserID = new DBVarChar(userID)
                };

                EntityBulletin.UserLoginInfo entity = new EntityBulletin(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserLoginInfo(para);

                if ((!entity.IsDailyFirst.IsNull() && entity.IsDailyFirst.GetValue() == EnumYN.Y.ToString()) &&
                    (!entity.LastLoginDate.IsNull() && entity.LastLoginDate.GetValue() == Common.GetDateString()))
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
    }
}
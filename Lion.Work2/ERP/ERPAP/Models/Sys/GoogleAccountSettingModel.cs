using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class GoogleAccountSettingModel : SysModel
    {
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string QueryUserID { get; set; }

        public string IsOnlyGAccEnable { get; set; }


        public GoogleAccountSettingModel()
        {

        }

        public void FormReset()
        {
         
        }

        List<EntityGoogleAccountSetting.GoogleAccountSetting> _entityGoogleAccountSettingList;
        public List<EntityGoogleAccountSetting.GoogleAccountSetting> EntityGoogleAccountSettingList { get { return _entityGoogleAccountSettingList; } }
        

        public bool GetGoogleAccountSettingList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntityGoogleAccountSetting.GoogleAccountSettingPara para = new EntityGoogleAccountSetting.GoogleAccountSettingPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                    IsOnlyGAccEnable = new DBChar((string.IsNullOrWhiteSpace(this.IsOnlyGAccEnable) ? null : this.IsOnlyGAccEnable)),
                };

                _entityGoogleAccountSettingList = new EntityGoogleAccountSetting(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectGoogleAccountSettingList(para);

                _entityGoogleAccountSettingList = base.GetEntitysByPage(_entityGoogleAccountSettingList, pageSize);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetEditGoogleAccountSettingResult(string userID, EnumCultureID cultureID, List<EntityGoogleAccountSetting.GoogleAccountSettingValue> googleAccountValueList)
        {
            try
            {
                EntityGoogleAccountSetting.GoogleAccountSettingPara para = new EntityGoogleAccountSetting.GoogleAccountSettingPara(cultureID.ToString())
                {
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntityGoogleAccountSetting(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditGoogleAccountSetting(para, googleAccountValueList) == EntityGoogleAccountSetting.EnumEditGoogleAccountSettingResult.Success)
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
using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Pub;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Pub
{
    public class MenuSettingModel : PubModel
    {
        public enum MenuID
        {
            Menu1 = 1,
            Menu2 = 2,
            Menu3 = 3,
        }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=PubMenuSetting.TabText_MenuSetting,
                ImageURL=string.Empty
            }
        };

        public Dictionary<string, string> MenuIDList = new Dictionary<string, string>()
        {
            {((int)MenuID.Menu1).ToString(), MenuID.Menu1.ToString()},
            {((int)MenuID.Menu2).ToString(), MenuID.Menu2.ToString()},
            {((int)MenuID.Menu3).ToString(), MenuID.Menu3.ToString()}
        };

        public MenuSettingModel()
        {
        }

        List<EntityMenuSetting.MenuSetting> _entityMenuSettingList;
        public List<EntityMenuSetting.MenuSetting> EntityMenuSettingList { get { return _entityMenuSettingList; } }

        public bool GetMenuSettingList(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntityMenuSetting.MenuSettingPara para = new EntityMenuSetting.MenuSettingPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(userID)
                };

                _entityMenuSettingList = new EntityMenuSetting(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectMenuSettingList(para);

                if (_entityMenuSettingList != null)
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

        public bool GetEditMenuSettingResult(string userID, EnumCultureID cultureID, List<EntityMenuSetting.MenuSettingValue> menuSettingValueList)
        {
            try
            {
                EntityMenuSetting.MenuSettingPara para = new EntityMenuSetting.MenuSettingPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntityMenuSetting(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditMenuSetting(para, menuSettingValueList) == EntityMenuSetting.EnumEditMenuSettingResult.Success)
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
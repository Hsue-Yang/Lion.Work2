using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class UserSettingModel : SysModel
    {
        public enum Field
        {
            QueryUserID, QueryUserNM
        }

        private string _QueryUserID;

        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string QueryUserID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_QueryUserID))
                {
                    return _QueryUserID;
                }
                return _QueryUserID.ToUpper();
            }
            set
            {
                _QueryUserID = value;
            }
        }

        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string QueryUserNM { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysUserSetting.TabText_UserSetting,
                ImageURL=string.Empty
            }
        };

        public UserSettingModel()
        {
        }

        public void FormReset()
        {
            this.QueryUserID = string.Empty;
            this.QueryUserNM = string.Empty;
        }

        List<EntityUserSetting.UserSetting> _entityUserSettingList;
        public List<EntityUserSetting.UserSetting> EntityUserSettingList { get { return _entityUserSettingList; } }

        public bool GetUserSettingList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntityUserSetting.UserSettingPara para = new EntityUserSetting.UserSettingPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    QueryUserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                    QueryUserNM = new DBObject((string.IsNullOrWhiteSpace(this.QueryUserNM) ? null : this.QueryUserNM))
                };

                _entityUserSettingList = new EntityUserSetting(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserSettingList(para);

                if (_entityUserSettingList != null)
                {
                    _entityUserSettingList = base.GetEntitysByPage(_entityUserSettingList, pageSize);
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}
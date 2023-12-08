using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;

namespace B2PAP.Models.Sys
{
    public class UserSystemModel : SysModel
    {
        public enum Field
        {
            QueryUserID, QueryUserNM
        }

        [StringLength(4, MinimumLength = 4)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string QueryUserID { get; set; }

        [InputType(EnumInputType.TextBox)]
        public string QueryUserNM { get; set; }

        public UserSystemModel()
        {
        }

        public void FormReset(string userID)
        {
            this.QueryUserID = userID;
            this.QueryUserNM = string.Empty;
        }

        List<EntityUserSystem.UserSystem> _entityUserSystemList;
        public List<EntityUserSystem.UserSystem> EntityUserSystemList { get { return _entityUserSystemList; } }

        public bool GetUserSystemList(int pageSize)
        {
            try
            {
                EntityUserSystem.UserSystemPara para = new EntityUserSystem.UserSystemPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                    UserNM = new DBObject((string.IsNullOrWhiteSpace(this.QueryUserNM) ? null : this.QueryUserNM))
                };

                _entityUserSystemList = new EntityUserSystem(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserSystemList(para);

                if (_entityUserSystemList != null)
                {
                    _entityUserSystemList = base.GetEntitysByPage(_entityUserSystemList, pageSize);
                }
                else
                {
                    _entityUserSystemList = new List<EntityUserSystem.UserSystem>();
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
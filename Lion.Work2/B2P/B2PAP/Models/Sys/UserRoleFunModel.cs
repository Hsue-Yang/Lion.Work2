using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;

namespace B2PAP.Models.Sys
{
    public class UserRoleFunModel : SysModel
    {
        public enum Field
        {
            QueryUserID, QueryUserNM
        }

        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string QueryUserID { get; set; }

        [InputType(EnumInputType.TextBox)]
        public string QueryUserNM { get; set; }

        public UserRoleFunModel()
        {
        }

        public void FormReset(string userID)
        {
            this.QueryUserID = userID;
            this.QueryUserNM = string.Empty;
        }

        List<EntityUserRoleFun.UserRoleFun> _entityUserRoleFunList;
        public List<EntityUserRoleFun.UserRoleFun> EntityUserRoleFunList { get { return _entityUserRoleFunList; } }

        public bool GetUserRoleFunList(int pageSize)
        {
            try
            {
                EntityUserRoleFun.UserRoleFunPara para = new EntityUserRoleFun.UserRoleFunPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                    UserNM = new DBObject((string.IsNullOrWhiteSpace(this.QueryUserNM) ? null : this.QueryUserNM))
                };

                _entityUserRoleFunList = new EntityUserRoleFun(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserRoleFunList(para);

                if (_entityUserRoleFunList != null)
                {
                    _entityUserRoleFunList = base.GetEntitysByPage(_entityUserRoleFunList, pageSize);
                }
                else
                {
                    _entityUserRoleFunList = new List<EntityUserRoleFun.UserRoleFun>();
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
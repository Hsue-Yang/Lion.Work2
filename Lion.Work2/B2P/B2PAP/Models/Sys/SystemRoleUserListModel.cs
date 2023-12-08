using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemRoleUserListModel : SysModel
    {
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string RoleID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string RoleNM { get; set; }

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

        [InputType(EnumInputType.TextBox)]
        public string QueryUserNM { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemRoleUserList.TabText_SystemRoleUserList,
                ImageURL=string.Empty
            }
        };

        public SystemRoleUserListModel()
        {

        }

        List<EntitySystemRoleUserList.SystemRoleUserList> _entitySystemRoleUserList;
        public List<EntitySystemRoleUserList.SystemRoleUserList> EntitySystemRoleUserList { get { return _entitySystemRoleUserList; } }

        public bool GetSystemRoleUserList(int pageSize)
        {
            try
            {
                EntitySystemRoleUserList.SystemRoleUserListPara para = new EntitySystemRoleUserList.SystemRoleUserListPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    RoleID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleID) ? null : this.RoleID)),
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                    UserNM = new DBObject((string.IsNullOrWhiteSpace(this.QueryUserNM) ? null : this.QueryUserNM))
                };

                _entitySystemRoleUserList = new EntitySystemRoleUserList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemRoleUserList(para);

                if (_entitySystemRoleUserList != null)
                {
                    _entitySystemRoleUserList = base.GetEntitysByPage(_entitySystemRoleUserList, pageSize);
                }
                else
                {
                    _entitySystemRoleUserList = new List<EntitySystemRoleUserList.SystemRoleUserList>();
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemUserListModel : SysModel
    {
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

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
                TabText=SysSystemUserList.TabText_SystemUserList,
                ImageURL=string.Empty
            }
        };

        public SystemUserListModel()
        {

        }

        List<EntitySystemUserList.SystemUserList> _entitySystemUserList;
        public List<EntitySystemUserList.SystemUserList> EntitySystemUserList { get { return _entitySystemUserList; } }

        public bool GetSystemUserList(int pageSize)
        {
            try
            {
                EntitySystemUserList.SystemUserListPara para = new EntitySystemUserList.SystemUserListPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryUserID) ? null : this.QueryUserID)),
                    UserNM = new DBObject((string.IsNullOrWhiteSpace(this.QueryUserNM) ? null : this.QueryUserNM))
                };

                _entitySystemUserList = new EntitySystemUserList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemUserList(para);

                if (_entitySystemUserList != null)
                {
                    _entitySystemUserList = base.GetEntitysByPage(_entitySystemUserList, pageSize);
                }
                else
                {
                    _entitySystemUserList = new List<EntitySystemUserList.SystemUserList>();
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
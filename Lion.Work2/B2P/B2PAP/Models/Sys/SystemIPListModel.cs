using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemIPListModel : SysModel
    {
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

        [StringLength(15)]
        [InputType(EnumInputType.TextBox)]
        public string IPAddress { get; set; }

        [Required]
        public string IsAPServer { get; set; }

        public string IsAPIServer { get; set; }

        public string IsDBServer { get; set; }

        public string IsFileServer { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string FolderPath { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() {
            new TabStripHelper.Tab {
                ControllerName=string.Empty,
                ActionName=string.Empty,                
                TabText=SysSystemIPList.TabText_SystemIPList,
                ImageURL=string.Empty
            }
        };

        public SystemIPListModel()
        {

        }

        public void FormReset()
        {
            this.IPAddress = string.Empty;
            this.IsAPServer = string.Empty;
            this.IsAPIServer = string.Empty;
            this.IsDBServer = string.Empty;
            this.IsFileServer = string.Empty;
            this.FolderPath = string.Empty;
            this.Remark = string.Empty;
        }

        List<EntitySystemIPList.SystemIPList> _entitySystemIPList;
        public List<EntitySystemIPList.SystemIPList> EntitySystemIPList { get { return _entitySystemIPList; } }

        public bool GetSystemIPList(int pageSize)
        {
            try
            {
                EntitySystemIPList.SystemIPListPara para = new EntitySystemIPList.SystemIPListPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID))
                };

                _entitySystemIPList = new EntitySystemIPList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemIPList(para);

                if (_entitySystemIPList != null)
                {
                    _entitySystemIPList = base.GetEntitysByPage(_entitySystemIPList, pageSize);
                }
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool InsertSystemIP(string userID)
        {
            try
            {
                EntitySystemIPList.SystemIPListPara para = new EntitySystemIPList.SystemIPListPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(this.IPAddress) ? null : this.IPAddress)),
                    IsAPServer=new DBChar(string.IsNullOrWhiteSpace(this.IsAPServer) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    IsAPIServer = new DBChar(string.IsNullOrWhiteSpace(this.IsAPIServer) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    IsDBServer = new DBChar(string.IsNullOrWhiteSpace(this.IsDBServer) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    IsFileServer = new DBChar(string.IsNullOrWhiteSpace(this.IsFileServer) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    FolderPath = new DBNVarChar((string.IsNullOrWhiteSpace(this.FolderPath) ? null : this.FolderPath)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemIPList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .InsertSystemIP(para) == LionTech.Entity.B2P.Sys.EntitySystemIPList.EnumInsertSystemIPResult.Success)
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

        public bool UpdateSystemIP(string userID)
        {
            try
            {
                EntitySystemIPList.SystemIPListPara para = new EntitySystemIPList.SystemIPListPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(this.IPAddress) ? null : this.IPAddress)),
                    IsAPServer=new DBChar(string.IsNullOrWhiteSpace(this.IsAPServer) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    IsAPIServer = new DBChar(string.IsNullOrWhiteSpace(this.IsAPIServer) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    IsDBServer = new DBChar(string.IsNullOrWhiteSpace(this.IsDBServer) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    IsFileServer = new DBChar(string.IsNullOrWhiteSpace(this.IsFileServer) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    FolderPath = new DBNVarChar((string.IsNullOrWhiteSpace(this.FolderPath) ? null : this.FolderPath)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemIPList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .UpdateSystemIP(para) == LionTech.Entity.B2P.Sys.EntitySystemIPList.EnumUpdateSystemIPResult.Success)
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

        public bool DeleteSystemIP()
        {
            try
            {
                EntitySystemIPList.SystemIPListPara para = new EntitySystemIPList.SystemIPListPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(this.IPAddress) ? null : this.IPAddress))
                };

                if (new EntitySystemIPList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .DeleteSystemIP(para) == LionTech.Entity.B2P.Sys.EntitySystemIPList.EnumDeleteSystemIPResult.Success)
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
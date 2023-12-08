using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemSettingDetailModel : SysModel
    {
        public enum DefaultRoleID
        {
            IT
        }

        [Required]
        [StringLength(6, MinimumLength = 4)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string SysID { get; set; }

        private string _UserID;

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string UserID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_UserID))
                {
                    return _UserID;
                }
                return _UserID.ToUpper();
            }
            set
            {
                _UserID = value;
            }
        }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMJAJP { get; set; }

        [Required]
        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string SysIndexPath { get; set; }

        public string SysIconPath { get; set; }

        public string SysKey { get; set; }

        public string ENSysID { get; set; }

        public string IsOutsourcing { get; set; }

        public string IsDisable { get; set; }

        [Required]
        [StringLength(6, MinimumLength=6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemSettingDetail.TabText_SystemSettingDetail,
                ImageURL=string.Empty
            }
        };

        public SystemSettingDetailModel()
        {

        }

        public void FormReset()
        {
            this.UserID = string.Empty;
            this.SysNMZHTW = string.Empty;
            this.SysNMZHCN = string.Empty;
            this.SysNMENUS = string.Empty;
            this.SysNMTHTH = string.Empty;
            this.SysNMJAJP = string.Empty;
            this.SysIndexPath = string.Empty;
            this.SysIconPath = string.Empty;
            this.SysKey = string.Empty;
            this.ENSysID = string.Empty;
            this.IsOutsourcing = EnumYN.N.ToString();
            this.IsDisable = EnumYN.N.ToString();
            this.SortOrder = string.Empty;
        }

        EntitySystemSettingDetail.SystemSettingDetail _entitySystemSettingDetail;
        public EntitySystemSettingDetail.SystemSettingDetail EntitySystemSettingDetail { get { return _entitySystemSettingDetail; } }

        public bool GetSystemSettingDetail()
        {
            try
            {
                EntitySystemSettingDetail.SystemSettingDetailPara para = new EntitySystemSettingDetail.SystemSettingDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID))
                };

                _entitySystemSettingDetail = new EntitySystemSettingDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemSettingDetail(para);

                if (_entitySystemSettingDetail != null)
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

        public bool GetUserSystemRole()
        {
            try
            {
                EntitySystemSettingDetail.UserSystemRolePara para = new EntitySystemSettingDetail.UserSystemRolePara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                if (new EntitySystemSettingDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserSystemRole(para) == LionTech.Entity.B2P.Sys.EntitySystemSettingDetail.EnumSelectUserSystemRoleResult.Success)
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

        public bool GetEditSystemSettingDetailResult(EnumActionType actionType, string userID)
        {
            try
            {
                EntitySystemSettingDetail.SystemSettingDetailPara para = new EntitySystemSettingDetail.SystemSettingDetailPara()
                {
                    FirstEdit = new DBChar(actionType == EnumActionType.Add ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    UserID = (string.IsNullOrWhiteSpace(this.UserID) ? null : new DBVarChar(this.UserID)),

                    RoleID = new DBVarChar(DefaultRoleID.IT.ToString()),
                    RoleNMZHTW = new DBNVarChar(DefaultRoleID.IT.ToString()),
                    RoleNMZHCN = new DBNVarChar(DefaultRoleID.IT.ToString()),
                    RoleNMENUS = new DBNVarChar(DefaultRoleID.IT.ToString()),
                    RoleNMTHTH = new DBNVarChar(DefaultRoleID.IT.ToString()),
                    RoleNMJAJP = new DBNVarChar(DefaultRoleID.IT.ToString()),

                    SysID = (string.IsNullOrWhiteSpace(this.SysID) ? null : new DBVarChar(this.SysID)),
                    SysMANUserID = (string.IsNullOrWhiteSpace(this.UserID) ? null : new DBVarChar(this.UserID)),
                    SysNMZHTW = (string.IsNullOrWhiteSpace(this.SysNMZHTW) ? null : new DBNVarChar(this.SysNMZHTW)),
                    SysNMZHCN = (string.IsNullOrWhiteSpace(this.SysNMZHCN) ? null : new DBNVarChar(this.SysNMZHCN)),
                    SysNMENUS = (string.IsNullOrWhiteSpace(this.SysNMENUS) ? null : new DBNVarChar(this.SysNMENUS)),
                    SysNMTHTH = (string.IsNullOrWhiteSpace(this.SysNMTHTH) ? null : new DBNVarChar(this.SysNMTHTH)),
                    SysNMJAJP = (string.IsNullOrWhiteSpace(this.SysNMJAJP) ? null : new DBNVarChar(this.SysNMJAJP)),
                    SysIndexPath = (string.IsNullOrWhiteSpace(this.SysIndexPath) ? null : new DBNVarChar(this.SysIndexPath)),
                    SysIconPath = new DBNVarChar(null),
                    SysKey = new DBChar(LionTech.Utility.Validator.GetEncodeString(RandomString.Generate(31))),
                    ENSysID = new DBChar(Token.Encrypt(string.IsNullOrWhiteSpace(this.SysID) ? string.Empty : this.SysID)),
                    IsOutsourcing = new DBChar(string.IsNullOrWhiteSpace(this.IsOutsourcing) ? EnumYN.N.ToString() : this.IsOutsourcing),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(this.IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    SortOrder = (string.IsNullOrWhiteSpace(this.SortOrder) ? null : new DBVarChar(this.SortOrder)),
                    UpdUserID = (string.IsNullOrWhiteSpace(userID) ? null : new DBVarChar(userID))
                };

                if (new EntitySystemSettingDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditSystemSettingDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemSettingDetail.EnumEditSystemSettingDetailResult.Success)
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

        public EntitySystemSettingDetail.EnumDeleteSystemSettingDetailResult GetDeleteSystemSettingDetailResult()
        {
            EntitySystemSettingDetail.SystemSettingDetailPara para = new EntitySystemSettingDetail.SystemSettingDetailPara()
            {
                SysID = (string.IsNullOrWhiteSpace(this.SysID) ? null : new DBVarChar(this.SysID))
            };

            var result = new EntitySystemSettingDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemSettingDetail(para);

            return result;
        }
    }
}
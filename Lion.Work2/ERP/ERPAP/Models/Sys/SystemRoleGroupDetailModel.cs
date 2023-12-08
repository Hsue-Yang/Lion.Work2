using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemRoleGroupDetailModel : SysModel
    {
        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMZhTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMZhCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMEnUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMThTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMJaJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleGroupNMKoKR { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }
        
        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemRoleGroupDetail.TabText_SystemRoleGroupDetail,
                ImageURL=string.Empty
            }
        };

        public string SystemRoleConditionFilterJsonString { get; private set; }

        public string SystemRoleConditionRulesJsonString { get; private set; }

        public SystemRoleGroupDetailModel()
        {

        }

        public void FormReset()
        {
            this.RoleGroupNMZhTW = string.Empty;
            this.RoleGroupNMZhCN = string.Empty;
            this.RoleGroupNMEnUS = string.Empty;
            this.RoleGroupNMThTH = string.Empty;
            this.RoleGroupNMJaJP = string.Empty;
            this.RoleGroupNMKoKR = string.Empty;
            this.SortOrder = string.Empty;
            this.Remark = string.Empty;
        }

        EntitySystemRoleGroupDetail.SystemRoleGroupDetail _entitySystemRoleGroupDetail;
        public EntitySystemRoleGroupDetail.SystemRoleGroupDetail EntitySystemRoleGroupDetail { get { return _entitySystemRoleGroupDetail; } }

        public bool GetSystemRoleGroupDetail()
        {
            try
            {
                EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara para = new EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                };

                _entitySystemRoleGroupDetail = new EntitySystemRoleGroupDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSystemRoleGroupDetail(para);

                if (_entitySystemRoleGroupDetail != null)
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

        public bool GetEditSystemRoleGroupDetailResult(EnumActionType actionType, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara para = new EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                    RoleGroupNMZhTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMZhTW) ? null : this.RoleGroupNMZhTW)),
                    RoleGroupNMZhCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMZhCN) ? null : this.RoleGroupNMZhCN)),
                    RoleGroupNMEnUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMEnUS) ? null : this.RoleGroupNMEnUS)),
                    RoleGroupNMThTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMThTH) ? null : this.RoleGroupNMThTH)),
                    RoleGroupNMJaJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMJaJP) ? null : this.RoleGroupNMJaJP)),
                    RoleGroupNMKoKR = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMKoKR) ? null : this.RoleGroupNMKoKR)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID))
                };

                if (new EntitySystemRoleGroupDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditSystemRoleGroupDetail(para) == LionTech.Entity.ERP.Sys.EntitySystemRoleGroupDetail.EnumEditSystemRoleGroupDetailResult.Success)
                {
                    Mongo_BaseAP.EnumModifyType modifyType = Mongo_BaseAP.EnumModifyType.U;
                    if (actionType == EnumActionType.Add)
                    {
                        modifyType = Mongo_BaseAP.EnumModifyType.I;
                    }

                    this.GetRecordSysSystemRoleGroupResult(modifyType, updUserID, ipAddress, cultureID);

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public EntitySystemRoleGroupDetail.EnumDeleteSystemRoleGroupDetailResult GetDeleteSystemRoleGroupDetailResult(string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara para = new EntitySystemRoleGroupDetail.SystemRoleGroupDetailPara()
            {
                RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
            };

            EntitySystemRoleGroupDetail.EnumDeleteSystemRoleGroupDetailResult result = new EntitySystemRoleGroupDetail(this.ConnectionStringSERP, this.ProviderNameSERP)
                .DeleteSystemRoleGroupDetail(para);

            if (result == LionTech.Entity.ERP.Sys.EntitySystemRoleGroupDetail.EnumDeleteSystemRoleGroupDetailResult.Success)
            {
                this.GetRecordSysSystemRoleGroupResult(Mongo_BaseAP.EnumModifyType.D, updUserID, ipAddress, cultureID);
            }

            return result;
        }

        private bool GetRecordSysSystemRoleGroupResult(Mongo_BaseAP.EnumModifyType modifyType, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara para = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectBasicInfo(para);

                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                List<Entity_BaseAP.CMCode> modifyTypeList = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectCMCodeList(codePara);

                Mongo_BaseAP.RecordSysSystemRoleGroupPara recordPara = new Mongo_BaseAP.RecordSysSystemRoleGroupPara()
                {
                    ModifyType = new DBChar(modifyType.ToString()),
                    ModifyTypeNM = new DBNVarChar(null),
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                    RoleGroupNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMZhTW) ? null : this.RoleGroupNMZhTW)),
                    RoleGroupNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMZhCN) ? null : this.RoleGroupNMZhCN)),
                    RoleGroupNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMEnUS) ? null : this.RoleGroupNMEnUS)),
                    RoleGroupNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMThTH) ? null : this.RoleGroupNMThTH)),
                    RoleGroupNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMJaJP) ? null : this.RoleGroupNMJaJP)),
                    RoleGroupNMKOKR = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMKoKR) ? null : this.RoleGroupNMKoKR)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    APINo = new DBChar(null),
                    UpdUserID = entityBasicInfo.UpdUserID,
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = entityBasicInfo.ExecSysID,
                    ExecSysNM = entityBasicInfo.ExecSysNM,
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (modifyTypeList != null && modifyTypeList.Count > 0 &&
                    !string.IsNullOrWhiteSpace(modifyType.ToString()))
                {
                    recordPara.ModifyTypeNM = (modifyTypeList.Find(e => e.CodeID.GetValue() == modifyType.ToString())).CodeNM;
                }

                // if (new Mongo_BaseAP(this.ConnectionStringMSERP, this.ProviderNameMSERP)
                //     .RecordSysSystemRoleGroup(recordPara) == Mongo_BaseAP.EnumRecordSysSystemRoleGroupResult.Success)
                // {
                //     return true;
                // }
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        #region Event
        public EntityEventPara.SysRoleGroupEdit GetEventParaSysRoleGroupEditEntity()
        {
            try
            {
                EntityEventPara.SysRoleGroupEdit entityEventParaRoleGroupEdit = new EntityEventPara.SysRoleGroupEdit()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                    RoleGroupNMzhTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMZhTW) ? null : this.RoleGroupNMZhTW)),
                    RoleGroupNMzhCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMZhCN) ? null : this.RoleGroupNMZhCN)),
                    RoleGroupNMenUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMEnUS) ? null : this.RoleGroupNMEnUS)),
                    RoleGroupNMthTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMThTH) ? null : this.RoleGroupNMThTH)),
                    RoleGroupNMjaJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMJaJP) ? null : this.RoleGroupNMJaJP)),
                    RoleGroupNMkoKR = new DBNVarChar((string.IsNullOrWhiteSpace(this.RoleGroupNMKoKR) ? null : this.RoleGroupNMKoKR)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark))
                };

                return entityEventParaRoleGroupEdit;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }

        public EntityEventPara.SysRoleGroupDelete GetEventParaSysRoleGroupDeleteEntity()
        {
            try
            {
                EntityEventPara.SysRoleGroupDelete entityEventParaRoleGroupDelete = new EntityEventPara.SysRoleGroupDelete()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID))
                };

                return entityEventParaRoleGroupDelete;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }
        #endregion
    }
}
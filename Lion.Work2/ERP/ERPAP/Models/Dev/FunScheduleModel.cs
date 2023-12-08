using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Dev;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Dev
{
    public class FunScheduleModel : DevModel
    {
        public string SysID { get; set; }
        
        public string FunControllerID { get; set; }
        
        public string FunActionName { get; set; }

        [Required]
        public string DevPhase { get; set; }

        public string IsFun { get; set; }

        private string _DevOwner;

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string DevOwner
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DevOwner))
                {
                    return _DevOwner;
                }
                return _DevOwner.ToUpper();
            }
            set
            {
                _DevOwner = value;
            }
        }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string PreBeginDate { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string PreEndDate { get; set; }

        [StringLength(3)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string PreWorkHours { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string ActBeginDate { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string ActEndDate { get; set; }

        [StringLength(3)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string ActWorkHours { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=DevFunSchedule.TabText_FunSchedule,
                ImageURL=string.Empty
            }
        };

        public FunScheduleModel()
        {

        }

        public void FormReset()
        {
            this.DevOwner = string.Empty;
            this.PreBeginDate = string.Empty;
            this.PreEndDate = string.Empty;
            this.PreWorkHours = string.Empty;
            this.ActBeginDate = string.Empty;
            this.ActEndDate = string.Empty;
            this.ActWorkHours = string.Empty;
        }

        EntityFunSchedule.SystemFun _entitySystemFun;
        public EntityFunSchedule.SystemFun EntitySystemFun { get { return _entitySystemFun; } }

        public bool GetSystemFun(EnumCultureID cultureID)
        {
            try
            {
                EntityFunSchedule.SystemFunPara para = new EntityFunSchedule.SystemFunPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                    DevPhase = new DBVarChar((string.IsNullOrWhiteSpace(this.DevPhase) ? null : this.DevPhase))
                };

                if (this.IsFun == EnumYN.Y.ToString())
                {
                    _entitySystemFun = new EntityFunSchedule(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectSystemFun(para);
                }
                else
                {
                    _entitySystemFun = new EntityFunSchedule(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectSystemEventTarget(para);
                }

                if (_entitySystemFun != null)
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

        List<EntityFunSchedule.FunSchedule> _entityFunScheduleList;
        public List<EntityFunSchedule.FunSchedule> EntityFunScheduleList { get { return _entityFunScheduleList; } }

        public bool GetFunScheduleList(EnumCultureID cultureID)
        {
            try
            {
                EntityFunSchedule.FunSchedulePara para = new EntityFunSchedule.FunSchedulePara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName))
                };

                _entityFunScheduleList = new EntityFunSchedule(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectFunScheduleList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetEditFunScheduleResult(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntityFunSchedule.FunSchedulePara para = new EntityFunSchedule.FunSchedulePara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                    IsFun = new DBChar((string.IsNullOrWhiteSpace(this.IsFun) ? null : this.IsFun)),
                    DevPhase = new DBVarChar((string.IsNullOrWhiteSpace(this.DevPhase) ? null : this.DevPhase)),
                    DevOwner = new DBVarChar((string.IsNullOrWhiteSpace(this.DevOwner) ? null : this.DevOwner)),
                    PreBeginDate = new DBChar((string.IsNullOrWhiteSpace(this.PreBeginDate) ? null : this.PreBeginDate)),
                    PreEndDate = new DBChar((string.IsNullOrWhiteSpace(this.PreEndDate) ? null : this.PreEndDate)),
                    PreWorkHours = new DBNumeric((string.IsNullOrWhiteSpace(this.PreWorkHours) ? null : this.PreWorkHours)),
                    ActBeginDate = new DBChar((string.IsNullOrWhiteSpace(this.ActBeginDate) ? null : this.ActBeginDate)),
                    ActEndDate = new DBChar((string.IsNullOrWhiteSpace(this.ActEndDate) ? null : this.ActEndDate)),
                    ActWorkHours = new DBNumeric((string.IsNullOrWhiteSpace(this.ActWorkHours) ? null : this.ActWorkHours)),
                    Remark = null,
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntityFunSchedule(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditFunSchedule(para) == EntityFunSchedule.EnumEditFunScheduleResult.Success)
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
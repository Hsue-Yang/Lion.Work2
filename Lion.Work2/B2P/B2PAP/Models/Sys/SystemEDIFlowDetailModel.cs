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
    public class SystemEDIFlowDetailModel : SysModel
    {
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowJAJP { get; set; }

        [Required]
        public string SCHFrequency { get; set; }

        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string SCHStartDate { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 9)]
        [InputType(EnumInputType.TextBox)]
        public string SCHStartTime { get; set; }

        [StringLength(9, MinimumLength = 9)]
        [InputType(EnumInputType.TextBox)]
        public string SCHEndTime { get; set; }

        [Required]
        public string SCHIntervalTime { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SCHDataDelay { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSCmd { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSDat { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSSrc { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSRes { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSBad { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSLog { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSFlowXml { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSFlowCmd { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSZipDat { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSException { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSSummary { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public bool HasSysID { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemEDIFlowDetail.TabText_SystemEDIFlowDetail,
                ImageURL=string.Empty
            }
        };

        public SystemEDIFlowDetailModel()
        {

        }

        public void FormReset()
        {
            this.EDIFlowZHTW = string.Empty;
            this.EDIFlowZHCN = string.Empty;
            this.EDIFlowENUS = string.Empty;
            this.EDIFlowTHTH = string.Empty;
            this.EDIFlowJAJP = string.Empty;
            this.SCHStartDate = string.Empty;
            this.SCHStartTime = string.Empty;
            this.SCHDataDelay = string.Empty;
            this.PATHSCmd = string.Empty;
            this.PATHSDat = string.Empty;
            this.PATHSSrc = string.Empty;
            this.PATHSRes = string.Empty;
            this.PATHSBad = string.Empty;
            this.PATHSLog = string.Empty;
            this.PATHSFlowXml = string.Empty;
            this.PATHSFlowCmd = string.Empty;
            this.PATHSZipDat = string.Empty;
            this.PATHSException = string.Empty;
            this.PATHSSummary = string.Empty;
            this.SortOrder = string.Empty;
            this.HasSysID = false;
        }

        public bool SetHasSysID()
        {
            foreach (EntitySys.SysUserSystemSysID systemSysIDList in EntitySysUserSystemSysIDList)
            {
                if (this.SysID == systemSysIDList.SysID.GetValue())
                {
                    this.HasSysID = true;
                    break;
                }
            }

            return this.HasSysID;
        }

        List<EntitySystemEDIFlowDetail.SCHFrequecny> _entitySCHFrequecnyList = new List<EntitySystemEDIFlowDetail.SCHFrequecny>();
        public List<EntitySystemEDIFlowDetail.SCHFrequecny> EntitySCHFrequecnyList { get { return _entitySCHFrequecnyList; } }

        public bool GetSCHFrequecnyList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIFlowDetail.SCHFrequecnyPara para =
                    new EntitySystemEDIFlowDetail.SCHFrequecnyPara(cultureID.ToString());

                _entitySCHFrequecnyList = new EntitySystemEDIFlowDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSCHFrequecnyList(para);

                if (_entitySCHFrequecnyList != null)
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

        List<EntitySystemEDIFlowDetail.SCHIntervalTime> _entitySCHIntervalTimeList = new List<EntitySystemEDIFlowDetail.SCHIntervalTime>();
        public List<EntitySystemEDIFlowDetail.SCHIntervalTime> EntitySCHIntervalTimeList { get { return _entitySCHIntervalTimeList; } }

        public bool GetSCHIntervalTimeList(EnumCultureID cultrueID)
        {
            try
            {
                EntitySystemEDIFlowDetail.SCHIntervalTimePara para =
                    new EntitySystemEDIFlowDetail.SCHIntervalTimePara(cultrueID.ToString());

                _entitySCHIntervalTimeList = new EntitySystemEDIFlowDetail(this.ConnectionStringB2P, this.ProviderNameB2P).SelectSCHIntervalTimeList(para);

                if (_entitySCHIntervalTimeList != null)
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

        EntitySystemEDIFlowDetail.SystemEDIFlowDetail _entitySystemEDIFlowDetail;
        public EntitySystemEDIFlowDetail.SystemEDIFlowDetail EntitySystemEDIFlowDetail { get { return _entitySystemEDIFlowDetail; } }
        public EntitySystemEDIFlowDetail.DBResultField DBResultField = new EntitySystemEDIFlowDetail.DBResultField();
        public bool GetSystemEDIFlowDetail()
        {
            try
            {
                EntitySystemEDIFlowDetail.SystemEDIFlowDetailPara para = new EntitySystemEDIFlowDetail.SystemEDIFlowDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
                };

                _entitySystemEDIFlowDetail = new EntitySystemEDIFlowDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIFlowDetail(para);

                if (_entitySystemEDIFlowDetail != null)
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

        public bool GetEditSystemEDIFlowDetailResult(string userID)
        {
            try
            {
                EntitySystemEDIFlowDetail.SystemEDIFlowDetailPara para = new EntitySystemEDIFlowDetail.SystemEDIFlowDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
                    EDIFlowZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIFlowZHTW) ? null : this.EDIFlowZHTW)),
                    EDIFlowZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIFlowZHCN) ? null : this.EDIFlowZHCN)),
                    EDIFlowENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIFlowENUS) ? null : this.EDIFlowENUS)),
                    EDIFlowTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIFlowTHTH) ? null : this.EDIFlowTHTH)),
                    EDIFlowJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIFlowJAJP) ? null : this.EDIFlowJAJP)),
                    SCHFrequency = new DBVarChar((string.IsNullOrWhiteSpace(this.SCHFrequency) ? null : this.SCHFrequency)),
                    SCHStartDate = new DBChar((string.IsNullOrWhiteSpace(this.SCHStartDate) ? null : this.SCHStartDate)),
                    SCHStartTime = new DBChar((string.IsNullOrWhiteSpace(this.SCHStartTime) ? null : this.SCHStartTime)),
                    SCHIntervalTime = new DBInt((string.IsNullOrWhiteSpace(this.SCHIntervalTime) ? null : this.SCHIntervalTime)),
                    SCHEndTime = new DBChar((string.IsNullOrWhiteSpace(this.SCHEndTime) ? null : this.SCHEndTime)),
                    SCHDataDelay = new DBInt((string.IsNullOrWhiteSpace(this.SCHDataDelay) ? null : this.SCHDataDelay)),
                    PATHSCmd = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSCmd) ? null : this.PATHSCmd)),
                    PATHSDat = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSDat) ? null : this.PATHSDat)),
                    PATHSSrc = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSSrc) ? null : this.PATHSSrc)),
                    PATHSRes = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSRes) ? null : this.PATHSRes)),
                    PATHSBad = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSBad) ? null : this.PATHSBad)),
                    PATHSLog = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSLog) ? null : this.PATHSLog)),
                    PATHSFlowXml = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSFlowXml) ? null : this.PATHSFlowXml)),
                    PATHSFlowCmd = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSFlowCmd) ? null : this.PATHSFlowCmd)),
                    PATHSZipDat = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSZipDat) ? null : this.PATHSZipDat)),
                    PATHSException = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSException) ? null : this.PATHSException)),
                    PATHSSummary = new DBNVarChar((string.IsNullOrWhiteSpace(this.PATHSSummary) ? null : this.PATHSSummary)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemEDIFlowDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemEDIFlowDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemEDIFlowDetail.EnumEditSystemEDIFlowDetailResult.Success)
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

        public bool GetSortOrder()
        {
            try
            {
                EntitySystemEDIFlowDetail.SystemEDIFlowDetailPara para = new EntitySystemEDIFlowDetail.SystemEDIFlowDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID))
                };

                this.SortOrder = new EntitySystemEDIFlowDetail(this.ConnectionStringB2P, this.ProviderNameB2P).GetFlowNewSortOrder(para);

                if (this.SortOrder != null)
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

        public EntitySystemEDIFlowDetail.EnumDeleteSystemEDIFlowDetailResult GetDeleteSystemEDIFlowDetailResult()
        {
            EntitySystemEDIFlowDetail.SystemEDIFlowDetailPara para = new EntitySystemEDIFlowDetail.SystemEDIFlowDetailPara()
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
            };

            var result = new EntitySystemEDIFlowDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemEDIFlowDetail(para);

            return result;
        }
    }
}
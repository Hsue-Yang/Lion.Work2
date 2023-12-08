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
    public class SystemEDIJobDetailModel : SysModel
    {
        [Required]
        public string SysID { get; set; }

        [Required]
        public string EDIFlowID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobJAJP { get; set; }

        [Required]
        public string EDIJobType { get; set; }

        public string EDIConID { get; set; }

        [StringLength(100)]
        [InputType(EnumInputType.TextBox)]
        public string ObjectName { get; set; }

        public string DepEDIJobID { get; set; }

        public string IsUseRes { get; set; }

        public string IsDisable { get; set; }

        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string FileSource { get; set; }
        public string GetFileSource { get; set; }

        public string FileEncoding { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string URLPath { get; set; }

        [StringLength(6,MinimumLength=6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public bool HasSysID { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemEDIJobDetail.TabText_SystemEDIJobDetail,
                ImageURL=string.Empty
            }
        };

        public SystemEDIJobDetailModel()
        {

        }

        public void FormReset()
        {
            this.EDIJobZHTW = string.Empty;
            this.EDIJobZHCN = string.Empty;
            this.EDIJobENUS = string.Empty;
            this.EDIJobTHTH = string.Empty;
            this.EDIJobJAJP = string.Empty;
            this.EDIConID = string.Empty;
            this.ObjectName = string.Empty;
            this.DepEDIJobID = string.Empty;
            this.IsUseRes = EnumYN.Y.ToString();
            this.IsDisable = EnumYN.N.ToString();
            this.FileSource = string.Empty;
            this.SortOrder = string.Empty;
            this.URLPath = string.Empty;
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

        List<EntitySystemEDIJobDetail.EDIJobType> _entityEDIJobTypeList = new List<EntitySystemEDIJobDetail.EDIJobType>();
        public List<EntitySystemEDIJobDetail.EDIJobType> EntityEDIJobTypeList { get { return _entityEDIJobTypeList; } }

        public bool GetEDIJobTypeList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIJobDetail.EDIJobTypePara para =
                    new EntitySystemEDIJobDetail.EDIJobTypePara(cultureID.ToString());

                _entityEDIJobTypeList = new EntitySystemEDIJobDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectEDIJobTypeList(para);

                if (_entityEDIJobTypeList != null)
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

        List<EntitySystemEDIJobDetail.SysSystemEDIConID> _entitySysSystemEDIConList = new List<EntitySystemEDIJobDetail.SysSystemEDIConID>();
        public List<EntitySystemEDIJobDetail.SysSystemEDIConID> EntitySysSystemEDIConList { get { return _entitySysSystemEDIConList; } }

        public bool GetSysSystemEDIConList(string SysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIJobDetail.SysSystemEDIConIDPara para = new EntitySystemEDIJobDetail.SysSystemEDIConIDPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID))
                };

                _entitySysSystemEDIConList = new EntitySystemEDIJobDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIConIDList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySystemEDIJobDetail.SysSystemDepEDIJobID> _entitySysSystemDepEDIJobList = new List<EntitySystemEDIJobDetail.SysSystemDepEDIJobID>();
        public List<EntitySystemEDIJobDetail.SysSystemDepEDIJobID> EntitySysSystemDepEDIJobList { get { return _entitySysSystemDepEDIJobList; } }

        public bool GetSysSystemDepEDIJobList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIJobDetail.SysSystemDepEDIJobIDPara para = new EntitySystemEDIJobDetail.SysSystemDepEDIJobIDPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID))
                };

                _entitySysSystemDepEDIJobList = new EntitySystemEDIJobDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemDepEDIJobIDList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySystemEDIJobDetail.EDIFileEncoding> _entityEDIFileEncodingList = new List<EntitySystemEDIJobDetail.EDIFileEncoding>();
        public List<EntitySystemEDIJobDetail.EDIFileEncoding> EntityEDIFileEncodingList { get { return _entityEDIFileEncodingList; } }

        public bool GetEDIFileEncodingList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIJobDetail.EDIFileEncodingPara para =
                    new EntitySystemEDIJobDetail.EDIFileEncodingPara(cultureID.ToString());

                _entityEDIFileEncodingList = new EntitySystemEDIJobDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectEDIFileEncodingList(para);

                if (_entityEDIFileEncodingList != null)
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

        EntitySystemEDIJobDetail.SystemEDIJobDetail _entitySystemEDIJobDetail;
        public EntitySystemEDIJobDetail.SystemEDIJobDetail EntitySystemEDIJobDetail { get { return _entitySystemEDIJobDetail; } }

        public bool GetSystemEDIJobDetail()
        {
            try
            {
                EntitySystemEDIJobDetail.SystemEDIJobDetailPara para = new EntitySystemEDIJobDetail.SystemEDIJobDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
                    EDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIJobID) ? null : this.EDIJobID)),
                };

                _entitySystemEDIJobDetail = new EntitySystemEDIJobDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIJobDetail(para);

                if (_entitySystemEDIJobDetail != null)
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
                EntitySystemEDIJobDetail.SystemEDIJobDetailPara para = new EntitySystemEDIJobDetail.SystemEDIJobDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID))
                };

                this.SortOrder = new EntitySystemEDIJobDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .GetJobNewSortOrder(para);

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

        public bool GetEditSystemEDIJobDetailResult(string userID)
        {
            try
            {
                EntitySystemEDIJobDetail.SystemEDIJobDetailPara para = new EntitySystemEDIJobDetail.SystemEDIJobDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
                    EDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIJobID) ? null : this.EDIJobID)),
                    EDIJobZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIJobZHTW) ? null : this.EDIJobZHTW)),
                    EDIJobZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIJobZHCN) ? null : this.EDIJobZHCN)),
                    EDIJobENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIJobENUS) ? null : this.EDIJobENUS)),
                    EDIJobTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIJobTHTH) ? null : this.EDIJobTHTH)),
                    EDIJobJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIJobJAJP) ? null : this.EDIJobJAJP)),
                    EDIJobType = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIJobType) ? null : this.EDIJobType)),
                    EDIConID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIConID) ? null : this.EDIConID)),
                    ObjectName = new DBVarChar((string.IsNullOrWhiteSpace(this.ObjectName) ? null : this.ObjectName)),
                    DepEDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.DepEDIJobID) ? null : this.DepEDIJobID)),
                    IsUseRes = new DBChar((string.IsNullOrWhiteSpace(this.IsUseRes) ? null : EnumYN.Y.ToString())),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(this.IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    FileSource = new DBNVarChar((string.IsNullOrWhiteSpace(this.FileSource) ? null : this.FileSource)),
                    EDIFileEncoding = new DBVarChar((string.IsNullOrWhiteSpace(this.FileEncoding) ? null : this.FileEncoding)),
                    URLPath = new DBNVarChar((string.IsNullOrWhiteSpace(this.URLPath) ? null : this.URLPath)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemEDIJobDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemEDIJobDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemEDIJobDetail.EnumEditSystemEDIJobDetailResult.Success)
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

        public EntitySystemEDIJobDetail.EnumDeleteSystemEDIJobDetailResult GetDeleteSystemEDIJobDetailResult()
        {
            EntitySystemEDIJobDetail.SystemEDIJobDetailPara para = new EntitySystemEDIJobDetail.SystemEDIJobDetailPara()
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIFlowID) ? null : this.EDIFlowID)),
                EDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIJobID) ? null : this.EDIJobID))
            };

            var result = new EntitySystemEDIJobDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemEDIJobDetail(para);

            return result;
        }
    }
}
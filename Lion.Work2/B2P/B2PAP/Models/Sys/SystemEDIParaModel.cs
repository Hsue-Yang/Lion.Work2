using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemEDIParaModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID, QueryEDIJobID
        }

        [Required]
        public string QuerySysID { get; set; }

        [Required]
        public string QueryEDIFlowID { get; set; }

        [Required]
        public string QueryEDIJobID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string EDIParaID { get; set; }

        [Required]
        public string EDIParaType { get; set; }

        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string EDIParaValue { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public bool HasSysID { get; set; }

        public List<TabStripHelper.Tab> SysEDIParaTabList = new List<TabStripHelper.Tab>(); //TabText

        public void GetSysEDIParaTabList(EnumTabAction actionNM)
        {
            SysEDIParaTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDIPara ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDIPara ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIPara)),
                    TabText=SysSystemEDIPara.TabText_SystemEDIPara,
                    ImageURL=string.Empty
                }
            };
        }

        public SystemEDIParaModel()
        {

        }

        public void FormReset()
        {
            this.EDIParaType = string.Empty;
            this.EDIParaValue = string.Empty;
            this.HasSysID = false;
        }

        public bool SetHasSysID()
        {
            foreach (EntitySys.SysUserSystemSysID systemSysIDList in EntitySysUserSystemSysIDList)
            {
                if (this.QuerySysID == systemSysIDList.SysID.GetValue())
                {
                    this.HasSysID = true;
                    break;
                }
            }

            return this.HasSysID;
        }

        List<EntitySystemEDIPara.EDIParaType> _entityEDIParaTypeList = new List<EntitySystemEDIPara.EDIParaType>();
        public List<EntitySystemEDIPara.EDIParaType> EntityEDIParaTypeList { get { return _entityEDIParaTypeList; } }

        public bool GetEDIParaTypeList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIPara.EDIParaTypePara para =
                    new EntitySystemEDIPara.EDIParaTypePara(cultureID.ToString());

                _entityEDIParaTypeList = new EntitySystemEDIPara(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectEDIParaTypeList(para);

                if (_entityEDIParaTypeList != null)
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

        public bool GetEditSystemEDIParaDetailResult(string userID)
        {
            try
            {
                EntitySystemEDIPara.SystemEDIParaDetailPara para = new EntitySystemEDIPara.SystemEDIParaDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID)),
                    EDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIJobID) ? null : this.QueryEDIJobID)),
                    EDIParaID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIParaID) ? null : this.EDIParaID)),
                    EDIParaType = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIParaType) ? null : this.EDIParaType)),
                    EDIParaValue = new DBNVarChar((string.IsNullOrWhiteSpace(this.EDIParaValue) ? null : this.EDIParaValue)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemEDIPara(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemEDIParaDetail(para) == LionTech.Entity.B2P.Sys.EntitySystemEDIPara.EnumEditSystemEDIParaDetailResult.Success)
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

        public EntitySystemEDIPara.EnumDeleteSystemEDIParaDetailResult GetDeleteSystemEDIParaDetailResult()
        {
            EntitySystemEDIPara.SystemEDIParaDetailPara para = new EntitySystemEDIPara.SystemEDIParaDetailPara()
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID)),
                EDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIJobID) ? null : this.QueryEDIJobID)),
                EDIParaID = new DBVarChar((string.IsNullOrWhiteSpace(this.EDIParaID) ? null : this.EDIParaID)),
            };

            var result = new EntitySystemEDIPara(this.ConnectionStringB2P, this.ProviderNameB2P)
                .DeleteSystemEDIParaDetail(para);

            return result;
        }

        List<EntitySystemEDIPara.SystemEDIPara> _entitySystemEDIParaList;
        public List<EntitySystemEDIPara.SystemEDIPara> EntitySystemEDIParaList { get { return _entitySystemEDIParaList; } }

        public bool GetSystemEDIParaList( EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIPara.SystemEDIParaPara para = new EntitySystemEDIPara.SystemEDIParaPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID)),
                    EDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIJobID) ? null : this.QueryEDIJobID))
                };

                _entitySystemEDIParaList = new EntitySystemEDIPara(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIParaList(para);

                

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetEDIParaSettingResult(string userID, EnumCultureID cultureID, List<EntitySystemEDIPara.EDIParaParaValue> EDIParaParaValueList)
        {
            try
            {
                EntitySystemEDIPara.SystemEDIParaPara para = new EntitySystemEDIPara.SystemEDIParaPara(cultureID.ToString())
                {
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID)),
                    EDIJobID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIJobID) ? null : this.QueryEDIJobID))
                };

                if (new EntitySystemEDIPara(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditEDIParaSetting(para, EDIParaParaValueList) == EntitySystemEDIPara.EnumEDIParaSettingResult.Success)
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
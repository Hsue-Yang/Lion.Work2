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
    public class SystemEDIFlowLogSettingModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID
        }

        public enum ParaEnum
        {
            W
        }

        public bool SaveType;
        [Required]
        public string QuerySysID { get; set; }

        [Required]
        public string QueryEDIFlowID { get; set; }

        [StringLength(12, MinimumLength = 12)]
        public string EDINO { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        [Required]
        public string DataDate { get; set; }

        [Required]
        public string StatusID { get; set; }

        [Required]
        public string IsAutomatic { get; set; }

        [Required]
        public string IsDeleted { get; set; }

        public List<TabStripHelper.Tab> SysEDIFlowLogSettingTabList = new List<TabStripHelper.Tab>(); //TabText

        public void GetSysEDIFlowLogSettingTabList(EnumTabAction actionNM)
        {
            SysEDIFlowLogSettingTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDIFlowLogSetting ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDIFlowLogSetting ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIPara)),
                    TabText=SysSystemEDIFlowLogSetting.TabText_SystemEDIFlowLogSetting,
                    ImageURL=string.Empty
                }
            };
        }

        List<EntitySystemEDIFlowLogSetting.StatusID> _entityStatusIDList = new List<EntitySystemEDIFlowLogSetting.StatusID>();
        public List<EntitySystemEDIFlowLogSetting.StatusID> EntityStatusIDList { get { return _entityStatusIDList; } }

        public bool GetStatusIDList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIFlowLogSetting.StatusIDPara para =
                    new EntitySystemEDIFlowLogSetting.StatusIDPara(cultureID.ToString());

                _entityStatusIDList = new EntitySystemEDIFlowLogSetting(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectStatusIDList(para);

                if (_entityStatusIDList != null)
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

        public SystemEDIFlowLogSettingModel()
        {

        }

        public void FormReset()
        {
            this.DataDate = DateTime.Now.ToString("yyyyMMdd");
        }

        public bool GetEditEDIFlowLogSettingResult(string userID)
        {
            try
            {
                EntitySystemEDIFlowLogSetting.SystemEditEDIFlowLogSettingPara para = new EntitySystemEDIFlowLogSetting.SystemEditEDIFlowLogSettingPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID)),
                    DataDate = new DBChar((string.IsNullOrWhiteSpace(this.DataDate) ? null : this.DataDate)),
                    StatusID = new DBVarChar(ParaEnum.W.ToString()),
                    IsAutomatic = new DBChar(EnumYN.N.ToString()),
                    IsDeleted = new DBChar(EnumYN.N.ToString()),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemEDIFlowLogSetting(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditSystemEDIFlowLogSetting(para) == LionTech.Entity.B2P.Sys.EntitySystemEDIFlowLogSetting.EnumEditSystemEDIFlowLogSettingDetailResult.Success)
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
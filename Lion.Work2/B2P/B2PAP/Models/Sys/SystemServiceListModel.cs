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
    public class SystemServiceListModel : SysModel
    {
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

        public string ServiceID { get; set; }
        
        public string Remark { get; set; }

        public string UpdUserID { get; set; }

        public List<TabStripHelper.Tab> SystemServiceList = new List<TabStripHelper.Tab>() {
            new TabStripHelper.Tab {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemServiceList.TabText_SystemServiceList,
                ImageURL=string.Empty
            }
        };

        public SystemServiceListModel()
        {

        }

        public void FormReset()
        {
            this.ServiceID = string.Empty;
            this.Remark = string.Empty;
        }

        List<Entity_BaseAP.CMCode> _entityBaseSystemServiceList;
        public List<Entity_BaseAP.CMCode> EntityBaseSystemServiceList { get { return _entityBaseSystemServiceList; } }
        
        public bool GetBaseSystemServiceList(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara()
                {
                    CodeKind = new DBVarChar("0001"),
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                _entityBaseSystemServiceList = new Entity_BaseAP(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectCMCodeList(codePara);

                if (_entityBaseSystemServiceList != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _entityBaseSystemServiceList = null;
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySystemServiceList.SystemServiceList> _entitySystemServiceList;
        public List<EntitySystemServiceList.SystemServiceList> EntitySystemServiceList { get { return _entitySystemServiceList; } }

        public bool GetSystmeServiceList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemServiceList.SystemServiceListPara para = new EntitySystemServiceList.SystemServiceListPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID))
                };

                _entitySystemServiceList = new EntitySystemServiceList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemServiceList(para);

                if (_entitySystemServiceList != null)
                {
                    _entitySystemServiceList = base.GetEntitysByPage(_entitySystemServiceList, pageSize);
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool EditSystmeService(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemServiceList.SystemServiceListPara para = new EntitySystemServiceList.SystemServiceListPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    ServiceID = new DBVarChar((string.IsNullOrWhiteSpace(this.ServiceID) ? null : this.ServiceID)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (new EntitySystemServiceList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditSystemService(para) == LionTech.Entity.B2P.Sys.EntitySystemServiceList.EnumEditSystemServiceResult.Success)
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

        public bool DeleteSystmeService(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemServiceList.SystemServiceListPara para = new EntitySystemServiceList.SystemServiceListPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    ServiceID = new DBVarChar((string.IsNullOrWhiteSpace(this.ServiceID) ? null : this.ServiceID))
                };

                if (new EntitySystemServiceList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .DeleteSystemService(para) == LionTech.Entity.B2P.Sys.EntitySystemServiceList.EnumDeleteSystemServiceResult.Success)
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
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
    public class FunIssueModel : DevModel
    {
        public string SysID { get; set; }

        public string FunControllerID { get; set; }
        
        public string FunActionName { get; set; }
        
        public string DevPhase { get; set; }
        
        public string IsFun { get; set; }

        [Required]
        [StringLength(300)]
        public string Remark { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=DevFunIssue.TabText_FunIssue,
                ImageURL=string.Empty
            }
        };

        public FunIssueModel()
        {

        }

        public void FormReset()
        {
            this.Remark = string.Empty;
        }

        EntityFunIssue.SystemFun _entitySystemFun;
        public EntityFunIssue.SystemFun EntitySystemFun { get { return _entitySystemFun; } }

        public bool GetSystemFun(EnumCultureID cultureID)
        {
            try
            {
                EntityFunIssue.SystemFunPara para = new EntityFunIssue.SystemFunPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                    DevPhase = new DBVarChar((string.IsNullOrWhiteSpace(this.DevPhase) ? null : this.DevPhase))
                };

                if (this.IsFun == EnumYN.Y.ToString())
                {
                    _entitySystemFun = new EntityFunIssue(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectSystemFun(para);
                }
                else
                {
                    _entitySystemFun = new EntityFunIssue(this.ConnectionStringSERP, this.ProviderNameSERP)
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

        List<EntityFunIssue.FunIssue> _entityFunIssueList;
        public List<EntityFunIssue.FunIssue> EntityFunIssueList { get { return _entityFunIssueList; } }

        public bool GetFunIssueList(EnumCultureID cultureID)
        {
            try
            {
                EntityFunIssue.FunIssuePara para = new EntityFunIssue.FunIssuePara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                    DevPhase = new DBVarChar((string.IsNullOrWhiteSpace(this.DevPhase) ? null : this.DevPhase))
                };

                _entityFunIssueList = new EntityFunIssue(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectFunIssueList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetAddFunIssueResult(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntityFunIssue.FunIssuePara para = new EntityFunIssue.FunIssuePara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                    DevPhase = new DBVarChar((string.IsNullOrWhiteSpace(this.DevPhase) ? null : this.DevPhase)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                new EntityFunIssue(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .AddFunIssue(para);

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
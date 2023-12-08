using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SysModel : _BaseAPModel
    {
        //for FileData's FilePath
        public enum EnumFilePathKeyWord
        {
            FileData, LionTech, EDIService
        }

        #region - Tab -
        public List<TabStripHelper.Tab> SysEDITabList = new List<TabStripHelper.Tab>();

        public void GetSysEDITabList(EnumTabAction actionNM)
        {
            SysEDITabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDIFlow ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDIFlow ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIFlow)),
                    TabText=SysResource.TabText_SystemEDIFlow,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDICon ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDICon ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDICon)),
                    TabText=SysResource.TabText_SystemEDICon,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDIJob ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDIJob ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIJob)),
                    TabText=SysResource.TabText_SystemEDIJob,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDIFlowLog ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDIFlowLog ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIFlowLog)),
                    TabText=SysResource.TabText_SystemEDIFlowLog,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDIJobLog ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDIJobLog ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIJobLog)),
                    TabText=SysResource.TabText_SystemEDIJobLog,
                    ImageURL=string.Empty
                }
            };
        }
        
        public List<TabStripHelper.Tab> SysEventTabList = new List<TabStripHelper.Tab>();

        public void GetSysEventTabList(EnumTabAction actionNM)
        {
            SysEventTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEventGroup ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEventGroup ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEventGroup)),
                    TabText=SysResource.TabText_SystemEventGroup,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEvent ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEvent ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEvent)),
                    TabText=SysResource.TabText_SystemEvent,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEventEDI ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEventEDI ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEventEDI)),
                    TabText=SysResource.TabText_SystemEventEDI,
                    ImageURL=string.Empty
                }
            };
        }

        public List<TabStripHelper.Tab> SysEventTargetTabList = new List<TabStripHelper.Tab>();

        public void GetSysEventTargetTabList(EnumTabAction actionNM)
        {
            SysEventTargetTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEventTargetEDI ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEventTargetEDI ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEventTargetEDI)),
                    TabText=SysResource.TabText_SystemEventTargetEDI,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEventTargetSend ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEventTargetSend ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEventTargetSend)),
                    TabText=SysResource.TabText_SystemEventTargetSend,
                    ImageURL=string.Empty
                }
            };
        }

        public List<TabStripHelper.Tab> SysSystemTabList = new List<TabStripHelper.Tab>();

        public void GetSysSystemTabList(EnumTabAction actionNM)
        {
            SysSystemTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemSetting ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemSetting ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemSetting)),
                    TabText=SysResource.TabText_SystemSetting,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemRole ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemRole ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemRole)),
                    TabText=SysResource.TabText_SystemRole,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemPurview ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemPurview ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemPurview)),
                    TabText=SysResource.TabText_SystemPurview,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemFunMenu ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemFunMenu ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunMenu)),
                    TabText=SysResource.TabText_SystemFunMenu,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemFunGroup ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemFunGroup ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunGroup)),
                    TabText=SysResource.TabText_SystemFunGroup,
                    ImageURL=string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemFun ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemFun ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFun)),
                    TabText=SysResource.TabText_SystemFun,
                    ImageURL=string.Empty
                }
            };
        }

        public List<TabStripHelper.Tab> SysSystemFunTabList = new List<TabStripHelper.Tab>();

        public void GetSysSystemFunTabList(EnumTabAction actionNM)
        {
            SysSystemFunTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemFunDetail ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemFunDetail ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunDetail)),
                    TabText=SysResource.TabText_SystemFunDetail,
                    ImageURL=string.Empty
                }
            };

            if (base.ExecAction == EnumActionType.Update)
            {
                SysSystemFunTabList.Add(new TabStripHelper.Tab
                {
                    ControllerName = (actionNM == EnumTabAction.SysSystemFunAssign ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName = (actionNM == EnumTabAction.SysSystemFunAssign ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunAssign)),
                    TabText = SysResource.TabText_SystemFunAssign,
                    ImageURL = string.Empty
                });
            }
        }

        public List<TabStripHelper.Tab> SysUserSystemTabList = new List<TabStripHelper.Tab>();

        public void GetSysUserSystemTabList(EnumTabAction actionNM)
        {
            SysUserSystemTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysUserRoleFun ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysUserRoleFun ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserRoleFun)),
                    TabText=SysResource.TabText_UserRoleFun,
                    ImageURL=string.Empty
                }
            };
        }

        public List<TabStripHelper.Tab> SysUserRoleFunctionTabList = new List<TabStripHelper.Tab>();

        public void GetSysUserRoleFunctionTabList(EnumTabAction actionNM)
        {
            SysUserRoleFunctionTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName = (actionNM == EnumTabAction.SysUserRoleFunDeatil ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName = (actionNM == EnumTabAction.SysUserRoleFunDeatil ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserRoleFunDeatil)),
                    TabText = SysResource.TabText_UserRoleFunDetail,
                    ImageURL = string.Empty
                },
                new TabStripHelper.Tab
                {
                    ControllerName = (actionNM == EnumTabAction.SysUserFunction ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName = (actionNM == EnumTabAction.SysUserFunction ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserFunction)),
                    TabText = SysResource.TabText_UserFunction,
                    ImageURL = string.Empty
                }
            };
        }

        #endregion

        List<EntitySys.SysUserSystemSysID> _entitySysUserSystemSysIDList = new List<EntitySys.SysUserSystemSysID>();
        public List<EntitySys.SysUserSystemSysID> EntitySysUserSystemSysIDList { get { return _entitySysUserSystemSysIDList; } }

        public bool GetSysUserSystemSysIDList(string userID, bool excludeOutsourcing, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysUserSystemSysIDPara para = new EntitySys.SysUserSystemSysIDPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                _entitySysUserSystemSysIDList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysUserSystemSysIDList(para, excludeOutsourcing);

                if (_entitySysUserSystemSysIDList != null)
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

        List<EntitySys.SystemSysID> _entitySysSystemSysIDList = new List<EntitySys.SystemSysID>();
        public List<EntitySys.SystemSysID> EntitySysSystemSysIDList { get { return _entitySysSystemSysIDList; } }

        public bool GetSysSystemSysIDList(bool excludeOutsourcing, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SystemSysIDPara para = new EntitySys.SystemSysIDPara(cultureID.ToString())
                {
                };

                _entitySysSystemSysIDList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemSysIDList(para, excludeOutsourcing);

                if (_entitySysSystemSysIDList != null)
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

        List<EntitySys.SystemSubsysID> _entitySysSystemSubsysIDList = new List<EntitySys.SystemSubsysID>();
        public List<EntitySys.SystemSubsysID> EntitySysSystemSubsysIDList { get { return _entitySysSystemSubsysIDList; } }

        public bool GetSysSystemSubsysIDList(string sysID, bool hasDefaultValue, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SystemSubsysIDPara para = new EntitySys.SystemSubsysIDPara(cultureID.ToString())
                {
                    ParentSysID = new DBVarChar(sysID)
                };

                List<EntitySys.SystemSubsysID> entityList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemSubsysIDList(para);

                EnumSystemID systemID;
                if (Enum.TryParse(sysID, true, out systemID))
                {
                    if (hasDefaultValue)
                    {
                        _entitySysSystemSubsysIDList.Add(new EntitySys.SystemSubsysID
                        {
                            SysID = new DBVarChar(systemID.ToString()),
                            SysNM = new DBNVarChar(string.Format("{0} ({1})", SysResource.Combobox_DefaultSysID, systemID.ToString()))
                        });
                    }

                    if (entityList != null)
                    {
                        foreach (EntitySys.SystemSubsysID item in entityList)
                        {
                            _entitySysSystemSubsysIDList.Add(item);
                        }
                    }
                    else
                    {
                        if (_entitySysSystemSubsysIDList == null)
                        {
                            _entitySysSystemSubsysIDList = new List<EntitySys.SystemSubsysID>();
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemRoleID> _entitySysSystemRoleIDList = new List<EntitySys.SysSystemRoleID>();
        public List<EntitySys.SysSystemRoleID> EntitySysSystemRoleIDList { get { return _entitySysSystemRoleIDList; } }

        public bool GetSysSystemRoleIDList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemRoleIDPara para = new EntitySys.SysSystemRoleIDPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemRoleIDList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemRoleIDList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemFunControllerID> _entitySysSystemFunControllerIDList = new List<EntitySys.SysSystemFunControllerID>();
        public List<EntitySys.SysSystemFunControllerID> EntitySysSystemFunControllerIDList { get { return _entitySysSystemFunControllerIDList; } }

        public bool GetSysSystemFunControllerIDList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemFunControllerIDPara para = new EntitySys.SysSystemFunControllerIDPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemFunControllerIDList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemFunControllerIDList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemFunName> _entitySysSystemFunNameList = new List<EntitySys.SysSystemFunName>();
        public List<EntitySys.SysSystemFunName> EntitySysSystemFunNameList { get { return _entitySysSystemFunNameList; } }

        public bool GetSysSystemFunNameList(string sysID, string funControllerID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemFunNamePara para = new EntitySys.SysSystemFunNamePara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID))
                };

                _entitySysSystemFunNameList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemFunNameList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemPurviewID> _entitySysSystemPurviewIDList = new List<EntitySys.SysSystemPurviewID>();
        public List<EntitySys.SysSystemPurviewID> EntitySysSystemPurviewIDList { get { return _entitySysSystemPurviewIDList; } }

        public bool GetSysSystemPurviewIDList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemPurviewIDPara para = new EntitySys.SysSystemPurviewIDPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemPurviewIDList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemPurviewIDList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemFunMenu> _entitySysSystemFunMenuList = new List<EntitySys.SysSystemFunMenu>();
        public List<EntitySys.SysSystemFunMenu> EntitySysSystemFunMenuList { get { return _entitySysSystemFunMenuList; } }

        public bool GetSysSystemFunMenuList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemFunMenuPara para = new EntitySys.SysSystemFunMenuPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemFunMenuList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemFunMenuList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public Dictionary<string, string> SysSystemFunMenuXAxisList = new Dictionary<string, string>()
        {
            {"1", "1"}, {"2", "2"}, {"3", "3"}, {"4", "4"}, {"5", "5"}
        };

        public Dictionary<string, string> SysSystemFunMenuYAxisList = new Dictionary<string, string>()
        {
            {"1", "1"}, {"2", "2"}, {"3", "3"}, {"4", "4"}, {"5", "5"},
            {"6", "6"}, {"7", "7"}, {"8", "8"}, {"9", "9"}, {"10", "10"}
        };

        List<EntitySys.SysSystemFunType> _entitySysSystemFunTypeList = new List<EntitySys.SysSystemFunType>();
        public List<EntitySys.SysSystemFunType> EntitySysSystemFunTypeList { get { return _entitySysSystemFunTypeList; } }

        public bool GetSysSystemFunTypeList(EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemFunTypePara para = new EntitySys.SysSystemFunTypePara(cultureID.ToString());

                _entitySysSystemFunTypeList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemFunTypeList(para);

                if (_entitySysSystemFunTypeList != null)
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

        List<EntitySys.SysSystemEDIFlow> _entitySysSystemEDIFlowList = new List<EntitySys.SysSystemEDIFlow>();
        public List<EntitySys.SysSystemEDIFlow> EntitySysSystemEDIFlowList { get { return _entitySysSystemEDIFlowList; } }

        public bool GetSysSystemEDIFlowList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemEDIFlowPara para = new EntitySys.SysSystemEDIFlowPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemEDIFlowList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIFlowList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemEDIJob> _entitySysSystemEDIJobList = new List<EntitySys.SysSystemEDIJob>();
        public List<EntitySys.SysSystemEDIJob> EntitySysSystemEDIJobList { get { return _entitySysSystemEDIJobList; } }

        public bool GetSysSystemEDIJobList(string SysID, string EDIFlowID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemEDIJobPara para = new EntitySys.SysSystemEDIJobPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(EDIFlowID) ? null : EDIFlowID))
                };

                _entitySysSystemEDIJobList = new EntitySystemEDIPara(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIJobList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemEventGroup> _entitySysSystemEventGroupList = new List<EntitySys.SysSystemEventGroup>();
        public List<EntitySys.SysSystemEventGroup> EntitySysSystemEventGroupList { get { return _entitySysSystemEventGroupList; } }

        public bool GetSysSystemEventGroupList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemEventGroupPara para = new EntitySys.SysSystemEventGroupPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemEventGroupList = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEventGroupList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        EntitySys.SystemEvent _entitySystemEvent;
        public EntitySys.SystemEvent EntitySystemEvent { get { return _entitySystemEvent; } }

        public bool GetSystemEvent(string sysID, string eventGroupID, string eventID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SystemEventPara para = new EntitySys.SystemEventPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID)),
                    EventGroupID = new DBVarChar((string.IsNullOrWhiteSpace(eventGroupID) ? null : eventGroupID)),
                    EventID = new DBVarChar((string.IsNullOrWhiteSpace(eventID) ? null : eventID))
                };

                _entitySystemEvent = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEvent(para);

                if (_entitySystemEvent != null)
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

        EntitySys.SystemUserDetail _entitySystemUserDetail;
        public EntitySys.SystemUserDetail EntitySystemUserDetail { get { return _entitySystemUserDetail; } }

        public bool GetSystemUserDetail(string userID)
        {
            try
            {
                EntitySys.SystemUserDetailPara para = new EntitySys.SystemUserDetailPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                _entitySystemUserDetail = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemUserDetail(para);

                if (_entitySystemUserDetail != null)
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
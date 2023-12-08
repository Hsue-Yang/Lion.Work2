using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using LionTech.Utility;

namespace LionTech.Entity.B2P
{
    public static class Utility
    {
        public static EnumCultureID GetCultureID(string cultureID)
        {
            if (string.IsNullOrWhiteSpace(cultureID))
            {
                return EnumCultureID.zh_TW;
            }
            else
            {
                if (Enum.IsDefined(typeof(EnumCultureID), cultureID))
                {
                    return (EnumCultureID)Enum.Parse(typeof(EnumCultureID), cultureID);
                }
                else
                {
                    throw new EntityException(EnumEntityMessage.EnumValueError, new string[] { "EnumCultureID", cultureID });
                }
            }
        }

        public static EnumSystemID GetSystemID(string userSystemID)
        {
            if (Enum.IsDefined(typeof(EnumSystemID), userSystemID))
            {
                return (EnumSystemID)Enum.Parse(typeof(EnumSystemID), userSystemID);
            }
            else
            {
                throw new EntityException(EnumEntityMessage.EnumValueError, new string[] { "EnumSystemID", userSystemID });
            }
        }

        public static EnumAPISystemID GetAPISystemID(string apiSystemID)
        {
            if (Enum.IsDefined(typeof(EnumAPISystemID), apiSystemID))
            {
                return (EnumAPISystemID)Enum.Parse(typeof(EnumAPISystemID), apiSystemID);
            }
            else
            {
                throw new EntityException(EnumEntityMessage.EnumValueError, new string[] { "EnumAPISystemID", apiSystemID });
            }
        }

        public static EnumEDISystemID GetEnumEDISystemID(string SysID)
        {
            if (Enum.IsDefined(typeof(EnumEDISystemID), SysID))
            {
                return (EnumEDISystemID)Enum.Parse(typeof(EnumEDISystemID), SysID);
            }
            else
            {
                throw new EntityException(EnumEntityMessage.EnumValueError, new string[] { "EnumEDISystemID", SysID });
            }
        }

        public static Entity_BaseAP.EnumGenerateUserMenuXMLResult GenerateUserMenuXML(List<Entity_BaseAP.UserMenuFun> userFunMenuList, string userID, string filePath)
        {
            if (string.IsNullOrWhiteSpace(userID))
                return Entity_BaseAP.EnumGenerateUserMenuXMLResult.Failure;

            Entity_BaseAP.UserMenu userMenu = new Entity_BaseAP.UserMenu();
            userMenu.Attribute = new DBXmlNode();
            userMenu.Attribute.IsNode = false;
            userMenu.Attribute.AddAttribute(Entity_BaseAP.UserMenu.Field.MenuUserID.ToString(), userID);

            userMenu.MenuDatas = new List<Entity_BaseAP.MenuData>();

            if (userFunMenuList != null && userFunMenuList.Any())
            {
                string menuID = string.Empty;
                userFunMenuList.ForEach(menuDataRow =>
                {
                    if (menuDataRow.MenuID.StringValue() == menuID)
                        return;
                    menuID = menuDataRow.MenuID.StringValue();

                    Entity_BaseAP.MenuData menuData = new Entity_BaseAP.MenuData();
                    menuData.Attribute = new DBXmlNode();
                    menuData.Attribute.IsNode = false;
                    menuData.Attribute.AddAttribute(Entity_BaseAP.MenuData.Field.MenuID.ToString(), menuDataRow.MenuID.StringValue());

                    List<Entity_BaseAP.UserMenuFun> menuContents = userFunMenuList.Where(menuDatas => menuDatas.MenuID.StringValue() == menuID).ToList();
                    if (menuContents != null && menuContents.Any())
                    {
                        string menuGroupID = string.Empty;
                        menuData.MenuContents = new List<Entity_BaseAP.MenuContent>();
                        menuContents.ForEach(menuContentRow =>
                        {
                            if (menuContentRow.FunMenu.StringValue() == menuGroupID)
                                return;
                            menuGroupID = menuContentRow.FunMenu.StringValue();

                            Entity_BaseAP.MenuContent menuContent = new Entity_BaseAP.MenuContent();
                            menuContent.MenuItemHeader = new DBXmlNode() { Value = new DBVarChar(menuContentRow.FunMenuNM.GetValue()) };

                            List<Entity_BaseAP.UserMenuFun> funItems = userFunMenuList.Where(menuItems => menuItems.FunMenu.StringValue() == menuGroupID).ToList();
                            if (funItems != null && funItems.Any())
                            {
                                menuContent.MenuItems = new List<Entity_BaseAP.MenuItem>();
                                funItems.ForEach(menuItemRow =>
                                {
                                    Entity_BaseAP.MenuItem menuItem = new Entity_BaseAP.MenuItem();
                                    menuItem.Attribute = new DBXmlNode();
                                    menuItem.Attribute.AddAttribute(Entity_BaseAP.MenuItem.Field.xAxis.ToString(), menuItemRow.FunMenuXAxis.StringValue());
                                    menuItem.Attribute.AddAttribute(Entity_BaseAP.MenuItem.Field.href.ToString(),
                                                            string.Concat(new object[] { Common.GetEnumDesc(GetSystemID(menuItemRow.SysID.StringValue())),
                                                                                            "/", menuItemRow.FunControllerID.StringValue(), 
                                                                                            "/", menuItemRow.FunActionName.StringValue() }));
                                    menuItem.Attribute.Value = new DBVarChar(menuItemRow.FunNM.GetValue());
                                    menuItem.Attribute.IsNode = false;

                                    menuContent.MenuItems.Add(menuItem);
                                });
                            }
                            menuData.MenuContents.Add(menuContent);
                        });
                    }
                    userMenu.MenuDatas.Add(menuData);
                });
            }
            else
            {
                return Entity_BaseAP.EnumGenerateUserMenuXMLResult.MeunIsEmpty;
            }

            XMLFileEntity xmlFileEntity = new XMLFileEntity(filePath);
            xmlFileEntity.SetContent(userMenu);
            xmlFileEntity.Save();

            return Entity_BaseAP.EnumGenerateUserMenuXMLResult.Success;
        }

        //生成EDIXML
        public static Entity_BaseAP.EnumGenerateEDIXMLResult GenerateEDIXML(List<Entity_BaseAP.SystemEDIFlowDetail> SysEDIFlowList, List<Entity_BaseAP.SystemEDIJobDetail> SysEDIJobList, List<Entity_BaseAP.SystemEDIConnectionDetail> SysEDIConList, List<Entity_BaseAP.SystemEDIParaDetail> SysEDIParaList, List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> SysEDIFlowFixedTimeList, string filePath)
        {

            //root
            Entity_BaseAP.root root = new Entity_BaseAP.root();
            root.Attribute = new DBXmlNode();
            root.flows = new List<Entity_BaseAP.flow>();
            root.Attribute.IsNode = false;
            if (SysEDIFlowList != null && SysEDIFlowList.Any())
            {
                string FlowID = "";
                SysEDIFlowList.ForEach(FlowDataRow =>
                {
                    if (FlowDataRow.EDIFlowID.StringValue() == FlowID)
                        return;
                    FlowID = FlowDataRow.EDIFlowID.StringValue();
                    Entity_BaseAP.flow FlowData = new Entity_BaseAP.flow();
                    FlowData.Attribute = new DBXmlNode();

                    //當為FALSE時Attribute才會在同一節點,否則,Attribute會自成一個子節點
                    FlowData.Attribute.IsNode = false;
                    FlowData.Attribute.AddAttribute(Entity_BaseAP.flow.Field.id.ToString(), FlowDataRow.EDIFlowID.StringValue());
                    FlowData.Attribute.AddAttribute(Entity_BaseAP.flow.Field.description.ToString(), FlowDataRow.EDIFlowNM.StringValue());

                    //Schedules
                    #region
                    List<Entity_BaseAP.SystemEDIFlowDetail> schedules = SysEDIFlowList.Where(Flows => Flows.EDIFlowID.StringValue() == FlowID).ToList();
                    FlowData.schedule = new Entity_BaseAP.schedule();
                    if (schedules != null && schedules.Any())
                    {
                        Entity_BaseAP.schedule schedule = new Entity_BaseAP.schedule();
                        schedule.frequency = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHFrequency.GetValue()) };
                        schedule.startDate = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHStartDate.GetValue()) };
                        schedule.startTime = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHStartTime.GetValue()) };
                        schedule.fixedTimes = new List<Entity_BaseAP.fixedTime>();
                        //fixedtimes
                        #region
                        if (SysEDIFlowFixedTimeList != null)
                        {
                            List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> FixedTimes = SysEDIFlowFixedTimeList.Where(Flows => (Flows.EDIFlowID.StringValue() == FlowID)).ToList();
                            if (FixedTimes != null && FixedTimes.Any())
                            {
                                string ExecuteTime = "";
                                FixedTimes.ForEach(FixedTimesDataRow =>
                                {
                                    if (FixedTimesDataRow.ExecuteTime.StringValue() == ExecuteTime)
                                        return;
                                    ExecuteTime = FixedTimesDataRow.ExecuteTime.StringValue();
                                    Entity_BaseAP.fixedTime FixedTimeData = new Entity_BaseAP.fixedTime();
                                    FixedTimeData.Attribute = new DBXmlNode();
                                    FixedTimeData.Attribute.IsNode = false;
                                    FixedTimeData.Attribute.AddAttribute(Entity_BaseAP.fixedTime.Field.value.ToString(), FixedTimesDataRow.ExecuteTime.StringValue());
                                    schedule.fixedTimes.Add(FixedTimeData);
                                });
                            }
                        }
                        #endregion
                        schedule.dataDelay = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHDataDelay.GetValue()) };
                        FlowData.schedule = schedule;
                    }
                    #endregion
                    //Paths
                    #region
                    List<Entity_BaseAP.SystemEDIFlowDetail> Paths = SysEDIFlowList.Where(Flows => Flows.EDIFlowID.StringValue() == FlowID).ToList();
                    FlowData.paths = new List<Entity_BaseAP.path>();
                    if (Paths != null && Paths.Any())
                    {
                        Entity_BaseAP.path PathData = new Entity_BaseAP.path();
                        PathData.Attribute = new DBXmlNode();
                        PathData.Attribute.IsNode = false;
                        Entity_BaseAP.SystemEDIFlowDetail FlowDetail = new Entity_BaseAP.SystemEDIFlowDetail();
                        int i;
                        for (i = 0; i < 11; i++)
                        {
                            PathData = new Entity_BaseAP.path();
                            PathData.Attribute = new DBXmlNode();
                            PathData.Attribute.IsNode = false;
                            PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.id.ToString(), Enum.GetName(typeof(Entity_BaseAP.PathType.Id), i).ToString());

                            switch (i)
                            {
                                case 0:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSCmd.StringValue());
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 0).ToString());
                                    break;
                                case 1:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSDat.StringValue());
                                    //PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 1).ToString());
                                    break;
                                case 2:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSSrc.StringValue());
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 1).ToString());
                                    break;
                                case 3:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSRes.StringValue());
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 1).ToString());
                                    break;
                                case 4:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSBad.StringValue());
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 2).ToString());
                                    break;
                                case 5:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSLog.StringValue());
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 3).ToString());
                                    break;
                                case 6:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSFlowXml.StringValue());
                                    //PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 4).ToString());
                                    break;
                                case 7:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSFlowCmd.StringValue());
                                    //PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 5).ToString());
                                    break;
                                case 8:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSZipDat.StringValue());
                                    //PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 5).ToString());
                                    break;
                                case 9:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSException.StringValue());
                                    //PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 6).ToString());
                                    break;
                                case 10:
                                    PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.value.ToString(), FlowDataRow.PATHSSummary.StringValue());
                                    //PathData.Attribute.AddAttribute(Entity_BaseAP.path.Field.exName.ToString(), "." + Enum.GetName(typeof(Entity_BaseAP.PathType.Extension), 3).ToString());
                                    break;
                            }
                            FlowData.paths.Add(PathData);
                        }

                    }
                    #endregion

                    //Connections
                    #region
                    List<Entity_BaseAP.SystemEDIConnectionDetail> Connections = SysEDIConList.Where(Flows => Flows.EDIFlowID.StringValue() == FlowID).ToList();
                    FlowData.connections = new List<Entity_BaseAP.connection>();
                    if (Connections != null && Connections.Any())
                    {
                        string ConnectionID = "";
                        Connections.ForEach(ConDataRow =>
                        {
                            if (ConDataRow.EDIConID.StringValue() == ConnectionID)
                                return;
                            ConnectionID = ConDataRow.EDIConID.StringValue();
                            Entity_BaseAP.connection ConData = new Entity_BaseAP.connection();
                            ConData.Attribute = new DBXmlNode();
                            ConData.Attribute.IsNode = false;
                            ConData.Attribute.AddAttribute(Entity_BaseAP.connection.Field.id.ToString(), ConDataRow.EDIConID.StringValue());
                            ConData.Attribute.AddAttribute(Entity_BaseAP.connection.Field.providerName.ToString(), ConDataRow.ProviderName.StringValue());
                            ConData.Attribute.AddAttribute(Entity_BaseAP.connection.Field.value.ToString(), ConDataRow.ConValue.StringValue());
                            FlowData.connections.Add(ConData);
                        });
                    }
                    #endregion

                    //Jobs
                    #region
                    List<Entity_BaseAP.SystemEDIJobDetail> Jobs = SysEDIJobList.Where(Flows => Flows.EDIFlowID.StringValue() == FlowID).ToList();
                    FlowData.jobs = new List<Entity_BaseAP.job>();
                    if (Jobs != null && Jobs.Any())
                    {
                        string JOBID = "";
                        Jobs.ForEach(JobDataRow =>
                        {
                            JobDataRow.URLPath = new DBNVarChar(JobDataRow.URLPath.StringValue().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;"));
                            if (JobDataRow.EDIJobID.StringValue() == JOBID)
                                return;
                            JOBID = JobDataRow.EDIJobID.StringValue();
                            Entity_BaseAP.job JobData = new Entity_BaseAP.job();
                            JobData.Attribute = new DBXmlNode();
                            JobData.Attribute.IsNode = false;
                            JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.id.ToString(), JobDataRow.EDIJobID.StringValue());
                            JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.fileSource.ToString(), JobDataRow.FileSource.StringValue());
                            JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.fileEncoding.ToString(), JobDataRow.FileEncoding.StringValue());
                            JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.urlPath.ToString(), JobDataRow.URLPath.StringValue());
                            JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.description.ToString(), JobDataRow.EDIJobNM.StringValue());
                            JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.useRES.ToString(), JobDataRow.IsUseRes.StringValue());
                            JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.isDisable.ToString(), JobDataRow.IsDisable.StringValue());
                            JobData.type = new DBXmlNode() { Value = new DBVarChar(JobDataRow.EDIJobType.GetValue()) };
                            JobData.connectionID = new DBXmlNode() { Value = new DBVarChar(JobDataRow.EDIConID.GetValue()) };
                            JobData.objectName = new DBXmlNode() { Value = new DBVarChar(JobDataRow.ObjectName.GetValue()) };
                            JobData.dependOnJobID = new DBXmlNode() { Value = new DBVarChar(JobDataRow.DepEDIJobID.GetValue()) };

                            //parameters
                            #region
                            JobData.parameters = new List<Entity_BaseAP.parameter>();
                            string a = FlowID;
                            if (SysEDIParaList != null)
                            {
                                List<Entity_BaseAP.SystemEDIParaDetail> Paras = SysEDIParaList.Where(Flows => (Flows.EDIFlowID.StringValue() == FlowID && Flows.EDIJobID.StringValue() == JOBID)).ToList();

                                if (Paras != null && Paras.Any())
                                {
                                    string ParaID = "";
                                    Paras.ForEach(ParasDataRow =>
                                    {
                                        if (ParasDataRow.EDIJobParaID.StringValue() == ParaID)
                                            return;
                                        ParaID = ParasDataRow.EDIJobParaID.StringValue();
                                        Entity_BaseAP.parameter ParaData = new Entity_BaseAP.parameter();
                                        ParaData.Attribute = new DBXmlNode();
                                        ParaData.Attribute.IsNode = false;
                                        ParaData.Attribute.AddAttribute(Entity_BaseAP.parameter.Field.id.ToString(), ParasDataRow.EDIJobParaID.StringValue());
                                        ParaData.Attribute.AddAttribute(Entity_BaseAP.parameter.Field.type.ToString(), ParasDataRow.EDIJobParaType.StringValue());
                                        ParaData.Attribute.AddAttribute(Entity_BaseAP.parameter.Field.value.ToString(), ParasDataRow.EDIJobParaValue.StringValue());
                                        JobData.parameters.Add(ParaData);
                                    });
                                }
                            }
                            #endregion

                            FlowData.jobs.Add(JobData);
                        });
                    }
                    #endregion
                    root.flows.Add(FlowData);
                });
            }
            else
            {
                return Entity_BaseAP.EnumGenerateEDIXMLResult.EDIIsEmpty;
            }
            XMLFileEntity xmlFileEntity = new XMLFileEntity(filePath);

            xmlFileEntity.SetContent(root);
            //現行XML最上層ROOT下沒有<flows>節點,與底層XML產出元件有所衝突,故用XMLDOCUMENT將該節點剃除
            XmlNode node = xmlFileEntity.Content.SelectSingleNode("root");
            foreach (XmlNode xn in node.ChildNodes)
            {
                XmlNode parentNode = xn.ParentNode;

                //將該節點的所有子節點加至父節點
                for (int i = xn.ChildNodes.Count - 1; i >= 0; i--)
                {
                    parentNode.PrependChild(xn.ChildNodes.Item(i).Clone());
                }
                //最後將該節點刪除
                parentNode.RemoveChild(xn);
            }

            xmlFileEntity.Save();

            return Entity_BaseAP.EnumGenerateEDIXMLResult.Success;
        }
    }
}
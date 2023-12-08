using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using LionTech.Utility;

namespace LionTech.Entity.ERP
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

        public static List<string> TransParaToList(string value)
        {
            List<string> result = new List<string>();

            if (value != null)
            {
                string[] dataArray = value.Split(',');
                if (dataArray.Length > 0)
                {
                    foreach (string data in dataArray)
                    {
                        if (!string.IsNullOrWhiteSpace(data))
                            result.Add(data);
                    }
                }
            }

            return result;
        }

        public static string TransListToString(List<string> valueList)
        {
            string result = string.Empty;

            if (valueList != null && valueList.Count > 0)
            {
                foreach (string value in valueList)
                {
                    result += value + ",";
                }
                result = result.TrimEnd(',');
            }

            return result;
        }

        #region - 轉型為 DBType List -
        /// <summary>
        /// 轉型為 DBType List
        /// </summary>
        /// <typeparam name="TSource">DBType Object</typeparam>
        /// <param name="query">轉型集合</param>
        /// <returns>DBType List</returns>
        public static List<TSource> ToDBTypeList<TSource>(IEnumerable<object> query)
            where TSource : IDBType
        {
            if (query == null)
            {
                return new List<TSource>();
            }

            var enumerable = query.ToArray();

            if (enumerable.Any())
            {
                var objectType = typeof(TSource);
                Type[] parameterTypes = { typeof(object).MakeByRefType() };
                return (from item in enumerable
                        let objConstructorInfo = objectType.GetConstructor(parameterTypes)
                        where objConstructorInfo != null
                        select (TSource)objConstructorInfo.Invoke(new [] { item })).ToList();
            }
            return new List<TSource>();
        }
        #endregion

        #region - 生成使用者MenuXML -
        public static XmlDocument StringToXmlDocument(string xmlStr)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlStr);
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.InsertBefore(xmlDeclaration, xml.FirstChild);
            return xml;
        }

        public static Entity_BaseAP.UserMenu GetUserMenu(List<Entity_BaseAP.UserMenuFun> userFunMenuList, string userID, bool IsDevEnv = false)
        {
            if (string.IsNullOrWhiteSpace(userID))
                return null;

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
                                    if (IsDevEnv)
                                    {
                                        string domainUrl = APDomainName.GetDomainNameUrlByDeveloping(GetSystemID(menuItemRow.SysID.StringValue()));
                                        menuItem.Attribute.AddAttribute(Entity_BaseAP.MenuItem.Field.href.ToString(),
                                                                string.Concat(new object[] { domainUrl,
                                                                                            "/", menuItemRow.FunControllerID.StringValue(),
                                                                                            "/", menuItemRow.FunActionName.StringValue() }));
                                    }
                                    else
                                    {
                                        menuItem.Attribute.AddAttribute(Entity_BaseAP.MenuItem.Field.href.ToString(),
                                                                string.Concat(new object[] { Common.GetEnumDesc(GetSystemID(menuItemRow.SysID.StringValue())),
                                                                                            "/", menuItemRow.FunControllerID.StringValue(),
                                                                                            "/", menuItemRow.FunActionName.StringValue() }));
                                    }
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
                return userMenu;
            }
            else
            {
                return null;
            }
        }
                
        public static Entity_BaseAP.EnumGenerateUserMenuXMLResult GenerateUserMenuXML(List<Entity_BaseAP.UserMenuFun> userFunMenuList, string userID, string filePath, bool IsDevEnv = false)
        {
            if (string.IsNullOrWhiteSpace(userID))
                return Entity_BaseAP.EnumGenerateUserMenuXMLResult.Failure;

            Entity_BaseAP.UserMenu userMenu;

            if (userFunMenuList != null && userFunMenuList.Any())
            {
                userMenu = GetUserMenu(userFunMenuList, userID, IsDevEnv);
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
        #endregion

        #region - 生成EDIXML -
        public static Entity_BaseAP.EnumGenerateEDIXMLResult GenerateEDIXML(
            List<Entity_BaseAP.SystemEDIFlowDetail> SysEDIFlowList,
            List<Entity_BaseAP.SystemEDIJobDetail> SysEDIJobList,
            List<Entity_BaseAP.SystemEDIConnectionDetail> SysEDIConList,
            List<Entity_BaseAP.SystemEDIParaDetail> SysEDIParaList,
            List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> SysEDIFlowFixedTimeList,
            string filePath)
        {
            //root
            Entity_BaseAP.root root = new Entity_BaseAP.root();
            root.Attribute=new DBXmlNode();
            root.flows = new List<Entity_BaseAP.flow>();
            root.Attribute.IsNode = false;
            if (SysEDIFlowList != null && SysEDIFlowList.Any())
            {
                string flowId = "";
                SysEDIFlowList.ForEach(FlowDataRow =>
                {
                    if (FlowDataRow.EDIFlowID.StringValue() == flowId)
                        return;
                    flowId = FlowDataRow.EDIFlowID.StringValue();
                    Entity_BaseAP.flow FlowData = new Entity_BaseAP.flow();
                    FlowData.Attribute = new DBXmlNode();

                    //當為FALSE時Attribute才會在同一節點,否則,Attribute會自成一個子節點
                    FlowData.Attribute.IsNode = false;
                    FlowData.Attribute.AddAttribute(Entity_BaseAP.flow.Field.id.ToString(), FlowDataRow.EDIFlowID.StringValue());
                    FlowData.Attribute.AddAttribute(Entity_BaseAP.flow.Field.description.ToString(), FlowDataRow.EDIFlowNM.StringValue());

                    //Schedules
                    #region
                    List<Entity_BaseAP.SystemEDIFlowDetail> schedules = SysEDIFlowList.Where(w => w.EDIFlowID.GetValue() == flowId).ToList();
                    FlowData.schedule = new Entity_BaseAP.schedule();
                    if (schedules.Any())
                    {
                        Entity_BaseAP.schedule schedule = new Entity_BaseAP.schedule();
                        schedule.frequency = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHFrequency.GetValue()) };
                        schedule.startDate = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHStartDate.GetValue()) };
                        schedule.startTime = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHStartTime.GetValue()) };
                        schedule.keepLogDay = new DBXmlNode() {Value = new DBVarChar(FlowDataRow.SCHKeepLogDay.GetValue())};
                        if (FlowDataRow.SCHIntervalNum.IsNull())
                        {
                            schedule.numDelay = new DBXmlNode() { Value = new DBVarChar(1) };
                        }
                        else
                        {
                            schedule.numDelay = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHIntervalNum.GetValue()) };
                        }
                        schedule.fixedTimes = new List<Entity_BaseAP.fixedTime>();
                        schedule.fixedWeeklys = new List<Entity_BaseAP.fixedWeekly>();
                        schedule.fixedDays = new List<Entity_BaseAP.fixedDay>();
                        //fixedtimes
                        #region - fixedTimes -
                        if (SysEDIFlowFixedTimeList != null)
                        {
                            string ExecuteTime = "";
                            SysEDIFlowFixedTimeList
                                .Where(w => w.EDIFlowID.GetValue() == flowId)
                                .ToList()
                                .ForEach(row =>
                                {
                                    if (row.ExecuteTime.StringValue() == ExecuteTime)
                                        return;
                                    ExecuteTime = row.ExecuteTime.StringValue();
                                    Entity_BaseAP.fixedTime FixedTimeData = new Entity_BaseAP.fixedTime();
                                    FixedTimeData.Attribute = new DBXmlNode();
                                    FixedTimeData.Attribute.IsNode = false;
                                    FixedTimeData.Attribute.AddAttribute(Entity_BaseAP.fixedCycle.Field.value.ToString(), row.ExecuteTime.StringValue());
                                    schedule.fixedTimes.Add(FixedTimeData);
                                });
                        }
                        #endregion
                        #region - fixedWeeklys -
                        if (FlowDataRow.SCHWeeks.GetValue() > 0)
                        {
                            var weekDictionary = Enumerable.Range(0, 7).ToDictionary(k => (int)Math.Pow(2, k), v => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), v.ToString(), true));
                            for (var index = 0; Math.Pow(2, index) <= FlowDataRow.SCHWeeks.GetValue(); index++)
                            {
                                int weekValue = (int)Math.Pow(2, index);
                                if ((FlowDataRow.SCHWeeks.GetValue() & weekValue) == weekValue)
                                {
                                    Entity_BaseAP.fixedWeekly weekTimeData = new Entity_BaseAP.fixedWeekly();
                                    weekTimeData.Attribute = new DBXmlNode();
                                    weekTimeData.Attribute.IsNode = false;
                                    weekTimeData.Attribute.AddAttribute(Entity_BaseAP.fixedCycle.Field.value.ToString(), weekDictionary[weekValue].ToString());
                                    schedule.fixedWeeklys.Add(weekTimeData);
                                }
                            }
                        }
                        #endregion
                        #region - fixedDays -
                        if (FlowDataRow.SCHDaysStr.IsNull() == false)
                        {
                            schedule.fixedDays.AddRange
                                (
                                    FlowDataRow
                                        .SCHDaysStr
                                        .GetValue()
                                        .Split(',')
                                        .OrderBy(w => w)
                                        .Select(day =>
                                        {
                                            Entity_BaseAP.fixedDay monthTimeData = new Entity_BaseAP.fixedDay
                                            {
                                                Attribute = new DBXmlNode { IsNode = false }
                                            };
                                            monthTimeData.Attribute.AddAttribute(Entity_BaseAP.fixedCycle.Field.value.ToString(), day);
                                            return monthTimeData;
                                        })
                                );
                        }
                        #endregion
                        schedule.dataDelay = new DBXmlNode() { Value = new DBVarChar(FlowDataRow.SCHDataDelay.GetValue()) };
                        FlowData.schedule = schedule;
                    }
                    #endregion
                    //Paths
                    #region
                    List<Entity_BaseAP.SystemEDIFlowDetail> Paths = SysEDIFlowList.Where(Flows => Flows.EDIFlowID.StringValue() == flowId).ToList();
                    FlowData.paths = new List<Entity_BaseAP.path>();
                    if (Paths != null && Paths.Any())
                    {
                        Entity_BaseAP.path PathData = new Entity_BaseAP.path();
                        PathData.Attribute = new DBXmlNode();
                        PathData.Attribute.IsNode = false;
                        
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
                    List<Entity_BaseAP.SystemEDIConnectionDetail> Connections = SysEDIConList.Where(Flows => Flows.EDIFlowID.StringValue() == flowId).ToList();
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
                    List<Entity_BaseAP.SystemEDIJobDetail> Jobs = SysEDIJobList.Where(Flows => Flows.EDIFlowID.StringValue() == flowId).ToList();
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
							JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.ignoreWarning.ToString(), JobDataRow.IgnoreWarning.StringValue());
                            JobData.Attribute.AddAttribute(Entity_BaseAP.job.Field.isDisable.ToString(), JobDataRow.IsDisable.StringValue());
                            JobData.type = new DBXmlNode() { Value = new DBVarChar(JobDataRow.EDIJobType.GetValue()) };
                            JobData.connectionID = new DBXmlNode() { Value = new DBVarChar(JobDataRow.EDIConID.GetValue()) };
                            JobData.objectName = new DBXmlNode() { Value = new DBVarChar(JobDataRow.ObjectName.GetValue()) };
                            JobData.dependOnJobID = new DBXmlNode() { Value = new DBVarChar(JobDataRow.DepEDIJobID.GetValue()) };

                            //parameters
                            #region
                            JobData.parameters = new List<Entity_BaseAP.parameter>();
                            if (SysEDIParaList != null)
                            {
                                List<Entity_BaseAP.SystemEDIParaDetail> Paras = SysEDIParaList.Where(Flows => (Flows.EDIFlowID.StringValue() == flowId && Flows.EDIJobID.StringValue() == JOBID)).ToList();
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
        #endregion

        #region - 取得員工編號新舊碼清單 -

        /// <summary>
        /// 取得員工編號新舊碼清單
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static List<DBVarChar> GetUserIDList(DBVarChar value)
        {
            List<DBVarChar> userIDList = new List<DBVarChar>();
            if (string.IsNullOrWhiteSpace(value.GetValue()) == false)
            {
                string userID = value.GetValue();
                List<string> prevCodes = new List<string> { "00", "ZZ" };

                userIDList.Add(value);
                
                if (prevCodes.Contains(userID.Substring(0, 2)))
                {
                    userIDList.Add(new DBVarChar(userID.Remove(0, 2)));
                }
            }
            return userIDList;
        }

        /// <summary>
        /// 取得員工編號新舊碼清單
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> GetUserIDList(string value)
        {
            List<string> userIDList = new List<string>();
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                string userID = value;
                List<string> prevCodes = new List<string> { "00", "ZZ" };

                userIDList.Add(value);

                if (prevCodes.Contains(userID.Substring(0, 2)))
                {
                    userIDList.Add(userID.Remove(0, 2));
                }
            }
            return userIDList;
        }

        /// <summary>
        /// 取得員工編號新舊碼清單
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static List<DBVarChar> GetUserIDList(List<DBVarChar> value)
        {
            List<DBVarChar> userIDInfoList = new List<DBVarChar>();

            if (value != null &&
                value.Any())
            {
                List<string> prevCodes = new List<string> {"00", "ZZ"};
                userIDInfoList.AddRange(value);

                userIDInfoList
                    .AddRange(
                        from id in value
                        where prevCodes.Contains(id.GetValue().Substring(0, 2))
                        select new DBVarChar(id.GetValue().Remove(0, 2))
                    );
            }

            return userIDInfoList;
        }
        #endregion
    }
}
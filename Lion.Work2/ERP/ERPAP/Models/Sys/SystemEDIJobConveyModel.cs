using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using LionTech.EDI;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemEDIJobConveyModel : SysModel
    {
        #region - Definitions -
        public enum EnumJobConveyCase
        {
            UnJobData,
            EditJobFailure,
            EditJobSuccess
        }

        public class SystemEDICon
        {
            public string EDIConID { get; set; }
            public string ConValue { get; set; }
            public string ProviderName { get; set; }
        }

        public class EdiJobSettingPara
        {
            public List<SystemEDIJobDetail> EDIJobDetailEditList = new List<SystemEDIJobDetail>();
            public List<SystemEDIParaValue> EDIJobParaEditList = new List<SystemEDIParaValue>();
        }
        #endregion

        #region - Constructor -
        public SystemEDIJobConveyModel()
        {
            _entity = new EntitySystemEDIJobConvey(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string QuerySysID { get; set; }

        public string QueryEDIFlowID { get; set; }

        [File(4, "xml")]
        public HttpPostedFileBase UploadEdiJobFile { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemEDIJobConvey.TabText_SystemEDIJobConvey,
                ImageURL=string.Empty
            }
        };
        #endregion

        #region - Private -
        private const string JobNodeXPath = @"//flow/jobs";
        private const string FlowAttrID = "id";
        private readonly EntitySystemEDIJobConvey _entity;
        #endregion

        #region - 編輯工作設定 -
        /// <summary>
        /// 編輯工作設定
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<EnumJobConveyCase> GetEDIJobSettingResult(string userID, EnumCultureID cultureID)
        {
            Flow flow = new Flow
            {
                id = string.Empty,
                description = string.Empty,
                schedule = new Schedule(),
                paths = new Paths(),
                connections = new Connections(),
                jobs = new Jobs(),
                ediNo = string.Empty,
                ediDate = string.Empty,
                ediTime = string.Empty,
                dataDate = string.Empty,
                result = EnumFlowResult.None
            };

            string content = _GetEdiJobFileContent();

            try
            {
                #region GetSystemEDIConData

                string apiUrlCon = API.SystemEDIJob.QuerySystemEDIConByIdsProviderCons(userID, QuerySysID, QueryEDIFlowID);
                string responseCon = await Common.HttpWebRequestGetResponseStringAsync(apiUrlCon, AppSettings.APITimeOut);

                var _systemEDIConList = Common.GetJsonDeserializeObject<List<SystemEDICon>>(responseCon);

                if (_systemEDIConList.Count > 0)
                {
                    foreach (var i in _systemEDIConList)
                    {
                        Connection connection = new Connection()
                        {
                            id = i.EDIConID,
                            providerName = i.ProviderName.ToString(),
                            value = i.ConValue.ToString()
                        };
                        flow.connections.Add(connection);
                    }
                }
                #endregion

                #region JobMaxSortOrder
                string apiUrlSort = API.SystemEDIJob.QueryJobMaxSortOrder(userID, QuerySysID, QueryEDIFlowID);
                string responseSort = await Common.HttpWebRequestGetResponseStringAsync(apiUrlSort, AppSettings.APITimeOut);

                if (string.IsNullOrWhiteSpace(responseSort))
                    return EnumJobConveyCase.UnJobData;

                var sortOrderStr = (Convert.ToInt32(responseSort) + 100).ToString().PadLeft(6, '0');
                int jobSortOrder = Convert.ToInt32(sortOrderStr.Substring(sortOrderStr.Length - 6, 6));

                flow.jobs = Job.FromString(content, flow);

                if (flow.jobs.Count == 0)
                {
                    return EnumJobConveyCase.UnJobData;
                }
                #endregion

                #region EdiJobSetting
                EdiJobSettingPara ediJobSettingPara = new EdiJobSettingPara();

                foreach (var job in flow.jobs)
                {
                    int paraCount = 0;

                    var detail = new SystemEDIJobDetail()
                    {
                        SysID = QuerySysID,
                        EDIFlowID = QueryEDIFlowID,
                        EDIJobID = job.id,
                        EDIJobZHTW = job.description,
                        EDIJobZHCN = job.description,
                        EDIJobENUS = job.description,
                        EDIJobTHTH = job.description,
                        EDIJobJAJP = job.description,
                        EDIJobKOKR = job.description,
                        EDIJobType = job.type.ToString(),
                        EDIConID = job.connectionID,
                        ObjectName = job.objectName,
                        DepEDIJobID = job.dependOnJobID,
                        IsUseRes = job.useRES.ToString(),
                        IgnoreWarning = job.ignoreWarning.ToString(),
                        IsDisable = job.isDisable.ToString(),
                        FileSource = job.fileSource,
                        FileEncoding = job.fileEncoding.ToString(),
                        URLPath = job.urlPath,
                        SortOrder = (jobSortOrder).ToString().PadLeft(6, '0'),
                        UpdUserID = userID
                    };
                    ediJobSettingPara.EDIJobDetailEditList.Add(detail);

                    foreach (var para in job.parameters)
                    {
                        var paraVal = new SystemEDIParaValue()
                        {
                            SysID = QuerySysID,
                            SortOrder = (paraCount * 100).ToString().PadLeft(6, '0'),
                            EDIFlowID = QueryEDIFlowID,
                            EDIJobID = job.id,
                            EDIJobParaID = para.id,
                            EDIJobParaType = string.IsNullOrWhiteSpace(para.type.ToString()) ? null : para.type.ToString(),
                            EDIJobParaValue = para.value,
                            UpdUserID = userID
                        };
                        ediJobSettingPara.EDIJobParaEditList.Add(paraVal);
                        paraCount++;
                    }
                    jobSortOrder += 100;
                }

                var paraJsonStr = Common.GetJsonSerializeObject(ediJobSettingPara);

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                     {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemEDIJob.EditSystemEDIJobImport(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                #endregion

                return EnumJobConveyCase.EditJobSuccess;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return EnumJobConveyCase.EditJobFailure;
        }
        #endregion

        #region - 取得工作檔 -
        /// <summary>
        /// 取得工作檔XML內容
        /// </summary>
        /// <returns></returns>
        private string _GetEdiJobFileContent()
        {
            var ediJobContent = string.Empty;

            try
            {
                using (StreamReader strReader = new StreamReader(UploadEdiJobFile.InputStream, Encoding.GetEncoding("big5")))
                {
                    ediJobContent = strReader.ReadToEnd();
                    XmlDocument ediXml = new XmlDocument();
                    ediXml.LoadXml(ediJobContent);

                    ediJobContent =
                        (from XmlNode n in ediXml.SelectNodes(JobNodeXPath)
                         where n.ParentNode.Attributes.GetNamedItem(FlowAttrID).Value == QueryEDIFlowID
                               && n.HasChildNodes
                         select n.OuterXml).FirstOrDefault();

                    ediJobContent = ediJobContent.Replace("&LT;", "<");
                    ediJobContent = ediJobContent.Replace("&RT;", ">");
                }

                return ediJobContent;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return ediJobContent;
        }
        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class SystemEDIFlowModel : SysModel
    {
        #region - Definition -
        public enum Field
        {
            QuerySysID,
            QuerySCHFrequency
        }

        public enum EnumDirTreeOption
        {
            RENAME,
            CREATE_FILE,
            REMOVE
        }

        public class DirTree
        {
            public bool IsMultiple { get; set; }
            public bool IsCheckCallback { get; set; }
            public DirTheme Themes { get; set; }
            public List<string> Plugins { get; set; }
            public List<DirData> Data { get; set; }
        }

        public class DirTheme
        {
            public string Variant { get; set; }
        }

        public class DirData
        {
            public string ID { get; set; }
            public string Text { get; set; }
            public string Old { get; set; }
            public string Icon { get; set; }
            public string Parent { get; set; }
        }


        public class SystemEDIFlow
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIFlowNM { get; set; }
            public string SCHFrequency { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }
        }

        public class EDIFlowValue
        {
            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string SCHFrequency { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }
        }

        public class SystemEDIFlowSort
        {
            public string SysID { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
            public string EDIFlowID { get; set; }
        }

        #endregion

        [Required]
        public string QuerySysID { get; set; }

        public string QuerySCHFrequency { get; set; }

        public string ServiceStatus { get; set; }

        public SystemEDIFlowModel()
        {
            _entity = new EntitySystemEDIFlow(ConnectionStringSERP, ProviderNameSERP);
        }

        public void FormReset()
        {
            QuerySysID = string.Empty;
            QuerySCHFrequency = string.Empty;
        }

        private List<SystemEDIFlow> _entitySystemEDIFlowList;

        public List<SystemEDIFlow> EntitySystemEDIFlowList
        {
            get { return _entitySystemEDIFlowList; }
        }

        public string DirTreeOption { get; set; }
        public string DirTreeJsonStr { get; set; }
        public string DirTreeSelectNodeJsonStr { get; set; }
        public bool HasNoEDIService { get; set; }

        public async Task<bool> GetSystemEDIFlowList(string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID))
                {
                    _entitySystemEDIFlowList = new List<SystemEDIFlow>();
                    return true;
                }

                string apiUrl = API.SystemEDIFlow.QuerySystemEDIFlows(userID, QuerySysID, QuerySCHFrequency, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemEDIFlowList = (List<SystemEDIFlow>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    _entitySystemEDIFlowList = responseObj.systemEDIFlowList;
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        //Output XML
        private List<Entity_BaseAP.SystemEDIFlowDetail> _entitySystemEDIFlowDetail;

        public List<Entity_BaseAP.SystemEDIFlowDetail> EntitySystemEDIFlowDetail
        {
            get { return _entitySystemEDIFlowDetail; }
        }

        private List<Entity_BaseAP.SystemEDIJobDetail> _entitySystemEDIJobDetail;

        public List<Entity_BaseAP.SystemEDIJobDetail> EntitySystemEDIJobDetail
        {
            get { return _entitySystemEDIJobDetail; }
        }

        private List<Entity_BaseAP.SystemEDIConnectionDetail> _entitySystemEDIConDetail;

        public List<Entity_BaseAP.SystemEDIConnectionDetail> EntitySystemEDIConDetail
        {
            get { return _entitySystemEDIConDetail; }
        }

        private List<Entity_BaseAP.SystemEDIParaDetail> _entitySystemEDIParDetail;

        public List<Entity_BaseAP.SystemEDIParaDetail> EntitySystemEDIParDetail
        {
            get { return _entitySystemEDIParDetail; }
        }

        private List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> _entitySystemEDIFlowExecuteTimeDetail;

        public List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> EntitySystemEDIFlowExecuteTimeDetail
        {
            get { return _entitySystemEDIFlowExecuteTimeDetail; }
        }

        private string _fileDataPath;
        private readonly EntitySystemEDIFlow _entity;

        public string _FileDataPath
        {
            get { return _fileDataPath; }
        }

        public async Task<bool> SaveEDIXML(string Sys_ID, EnumCultureID cultureID, string UserID)
        {
            try
            {
                EntitySystemEDIFlowOutputXML.SystemEDIXMLPara para = new EntitySystemEDIFlowOutputXML.SystemEDIXMLPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(Sys_ID) ? null : Sys_ID)),

                };

                EntitySys.SysSystemSysIDPara Pathpara = new EntitySys.SysSystemSysIDPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(Sys_ID) ? null : Sys_ID)),

                };
                _entitySystemEDIFlowDetail = new EntitySystemEDIFlowOutputXML(ConnectionStringSERP, ProviderNameSERP).SysEDIFlowList(para);
                _entitySystemEDIJobDetail = new EntitySystemEDIFlowOutputXML(ConnectionStringSERP, ProviderNameSERP).SysEDIJobList(para);
                _entitySystemEDIConDetail = new EntitySystemEDIFlowOutputXML(ConnectionStringSERP, ProviderNameSERP).SysEDIConnectionList(para);
                _entitySystemEDIParDetail = new EntitySystemEDIFlowOutputXML(ConnectionStringSERP, ProviderNameSERP).SysEDIParaList(para);
                _entitySystemEDIFlowExecuteTimeDetail = new EntitySystemEDIFlowOutputXML(ConnectionStringSERP, ProviderNameSERP).SysEDIFlowFixedTimeList(para);
                if (_entitySystemEDIFlowDetail != null &&
                    _entitySystemEDIJobDetail != null &&
                    _entitySystemEDIConDetail != null)
                {
                string apiUrl = API.SystemSetting.QuerySystemFilePath(Sys_ID, UserID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    FilePath = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                _fileDataPath = responseObj.FilePath;

                if (Common.IsInKubernetes())
                {
                    _fileDataPath = responseObj.FilePath.Replace(@"\\", @"C:\");
                }
                    string fullFilePath =
                        Path.Combine(
                            new string[]
                            {
                                _fileDataPath,
                                EnumFilePathKeyWord.FileData.ToString(),
                                Common.GetEnumDesc(Utility.GetEnumEDISystemID(Sys_ID)),
                                Common.GetEnumDesc(Utility.GetEnumEDISystemID(Sys_ID)) + "." + UserID + "." + Common.GetDateTimeString() +
                                Common.GetEnumDesc(EntitySystemEDIFlowOutputXML.EnumEDIXMLPathFile.xml)
                            });

                    if (Utility.GenerateEDIXML(_entitySystemEDIFlowDetail,
                        _entitySystemEDIJobDetail, _entitySystemEDIConDetail, _entitySystemEDIParDetail, _entitySystemEDIFlowExecuteTimeDetail
                        , fullFilePath) == Entity_BaseAP.EnumGenerateEDIXMLResult.Success)
                    {
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        internal async Task<string> GetSystemEDIIPAddress(string userID,string sysID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID))
                {
                    return null;
                }
                string apiUrl = API.SystemEDIFlow.QuerySystemEDIIPAddress(userID, sysID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                if (response != null)
                {
                   return response;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }

        #region - 取得檔案目錄樹狀結構Json資料 -
        /// <summary>
        /// 取得檔案目錄樹狀結構Json資料
        /// </summary>
        /// <returns></returns>
        public bool GetDirFileTreeJsonString()
        {
            try
            {
                var data = _GetDirFileTreeNodeData(_fileDataPath, new List<DirData>(), "#");

                DirTreeJsonStr = Common.GetJsonSerializeObject(new DirTree
                {
                    IsMultiple = true,
                    IsCheckCallback = true,
                    Plugins = new List<string> { "contextmenu", "types" },
                    Themes = new DirTheme { Variant = "large" },
                    Data = data
                });

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 更新檔案目錄資料 -
        /// <summary>
        /// 更新檔案目錄資料
        /// </summary>
        /// <returns></returns>
        public string UpdateDirFileData()
        {
            bool result = true;
            string errMsg = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(DirTreeJsonStr) == false)
                {
                    var dirTreeOption = (EnumDirTreeOption)Enum.Parse(typeof(EnumDirTreeOption), DirTreeOption);
                    var targetNode = Common.GetJsonDeserializeObject<DirData>(DirTreeSelectNodeJsonStr);

                    var filePath = $@"{_fileDataPath}\{targetNode.Text}";

                    switch (dirTreeOption)
                    {
                        case EnumDirTreeOption.RENAME:
                            var sourcePath = $@"{_fileDataPath}\{targetNode.Old}";
                            File.Move(sourcePath, filePath);
                            break;
                        case EnumDirTreeOption.CREATE_FILE:
                            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                fileStream.Write(new byte[] { }, 0, 0);
                            }
                            break;
                        case EnumDirTreeOption.REMOVE:
                            FileInfo file = new FileInfo(filePath);
                            file.Delete();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
                result = false;
                errMsg = ex.Message;
            }

            return Common.GetJsonSerializeObject(new
            {
                result,
                errMsg
            });
        }
        #endregion

        public async Task<bool> GetEDIFlowSettingResult(string userID, List<EDIFlowValue> EDIFlowValueList)
        {
            try
            {
                List<SystemEDIFlowSort> ediFlowValueList = new List<SystemEDIFlowSort>();
                foreach (var ediFlowValue in EDIFlowValueList)
                {
                    if (ediFlowValue.AfterSortOrder != ediFlowValue.BeforeSortOrder)
                    {
                        ediFlowValueList.Add(new SystemEDIFlowSort
                        {
                            SysID = string.IsNullOrWhiteSpace(QuerySysID) ? null : QuerySysID,
                            SortOrder = string.IsNullOrWhiteSpace(ediFlowValue.AfterSortOrder) ? null : ediFlowValue.AfterSortOrder,
                            UpdUserID = userID,
                            EDIFlowID = string.IsNullOrWhiteSpace(ediFlowValue.EDIFlowID) ? null : ediFlowValue.EDIFlowID
                        });
                    }
                }

                if (ediFlowValueList.Any())
                {
                    Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                    var paraJsonStr = Common.GetJsonSerializeObject(ediFlowValueList);
                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                    string apiUrl = API.SystemEDIFlow.EditSystemEDIFlowSortOrder(QuerySysID);
                    await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                }
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        #region - 取得EDI服務檔案目錄 -
        /// <summary>
        /// 取得EDI服務檔案目錄
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task GetEDIServiceFilePath(string userID)
        {
            string apiUrl = API.SystemSetting.QuerySystemFilePath(QuerySysID, userID);
            string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

            var responseObj = new
            {
                FilePath = (string)null
            };

            responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
            _fileDataPath = responseObj.FilePath;
            if (Common.IsInKubernetes())
            {
                _fileDataPath = responseObj.FilePath.Replace(@"\\", @"C:\");
            }

            _fileDataPath = Path.Combine(
                new[]
                {
                    _fileDataPath,
                    EnumFilePathKeyWord.FileData.ToString(),
                    Common.GetEnumDesc(Utility.GetEnumEDISystemID(QuerySysID))
                });
        }
        #endregion

        #region - 產生檔案目錄樹狀結構節點資料 -
        /// <summary>
        /// 產生檔案目錄樹狀結構節點資料
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private List<DirData> _GetDirFileTreeNodeData(string path, List<DirData> data, string parent)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                var file = directory.GetFiles();

                foreach (var row in data)
                {
                    if (row.Icon != "jstree-file")
                    {
                        row.Icon = string.Empty;
                    }
                }

                var rdm = new Random();

                data.AddRange(file.Select(n => new DirData
                {
                    ID = $"{rdm.Next(1, 1000).ToString()}|{n.Name}",
                    Text = n.Name,
                    Icon = "jstree-file",
                    Parent = parent
                }));

                return data;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return null;
        }
        #endregion
    }
}
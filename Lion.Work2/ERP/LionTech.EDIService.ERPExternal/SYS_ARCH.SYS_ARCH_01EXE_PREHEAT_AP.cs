using System;
using System.Collections.Generic;
using System.Net;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;
using LionTech.Utility;

namespace LionTech.EDIService.ERPExternal
{
    public partial class SYS_ARCH
    {
        public static EnumJobResult SYS_ARCH_01EXE_PREHEAT_AP(Flow flow, Job job)
        {
            Connection connection = flow.connections[job.connectionID];
            LogPath = flow.paths[EnumPathID.LOG.ToString()].value;
            ExceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            List<EntityERPExternal.SystemMain> systemMainList = GetSysSystemMainList(flow, job, connection);
            if (systemMainList != null && systemMainList.Count > 0)
            {
                string requestURL = string.Empty;
                string logFileName = string.Empty;

                foreach (EntityERPExternal.SystemMain systemMain in systemMainList)
                {
                    logFileName = string.Format(FileNameFormat, systemMain.ParentSysID.GetValue(), systemMain.SysID.GetValue());

                    try
                    {
                        EnumSystemID systemID;
                        if (Enum.TryParse<EnumSystemID>(systemMain.SysID.GetValue(), out systemID))
                        {
                            requestURL = Common.GetEnumDesc(systemID);
                        }

                        if (systemMain.IsMaster.GetValue() == EnumYN.Y.ToString())
                        {
                            requestURL = string.Format(UrlFormat, requestURL, string.Empty);
                        }
                        else
                        {
                            requestURL = string.Format(UrlFormat, requestURL, systemMain.SysID.GetValue());
                        }

                        if (string.IsNullOrWhiteSpace(requestURL)) continue;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
                        request.Method = "HEAD";
                        request.AllowAutoRedirect = false;

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        response.Close();

                        if (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.OK)
                        {
                            FileLog.Write(string.Concat(new object[] { LogPath, logFileName }), Common.GetEnumDesc(EnumRequestStatus.ResponseSuccess));
                        }
                        else
                        {
                            FileLog.Write(string.Concat(new object[] { LogPath, logFileName }), Common.GetEnumDesc(EnumRequestStatus.ResponseAbnormal));
                        }

                        WriteFileLog(logFileName, systemMain.SysID.GetValue(), response.StatusCode.ToString(), requestURL);
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse response = (HttpWebResponse)ex.Response;

                        string responseStatus = string.Empty;
                        if (response != null)
                        {
                            responseStatus = response.StatusCode.ToString();
                        }

                        FileLog.Write(string.Concat(new object[] { LogPath, logFileName }), Common.GetEnumDesc(EnumRequestStatus.RequestError));
                        WriteFileLog(logFileName, systemMain.SysID.GetValue(), responseStatus, requestURL);

                        WriteFileException(systemMain.SysID.GetValue(), requestURL, ex);
                    }
                }

                foreach (EntityERPExternal.SystemMain systemMain in systemMainList)
                {
                    logFileName = string.Format(FileNameFormat, systemMain.ParentSysID.GetValue(), systemMain.SysID.GetValue());

                    try
                    {
                        if (systemMain.HasAPI.GetValue() == EnumYN.Y.ToString())
                        {
                            EnumAPISystemID apiSystemID;
                            if (Enum.TryParse<EnumAPISystemID>(systemMain.SysID.GetValue(), out apiSystemID))
                            {
                                requestURL = Common.GetEnumDesc(apiSystemID);
                            }

                            if (string.IsNullOrWhiteSpace(requestURL)) continue;

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
                            request.Method = "HEAD";
                            request.AllowAutoRedirect = false;

                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            response.Close();

                            if (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.OK)
                            {
                                FileLog.Write(string.Concat(new object[] { LogPath, logFileName }), Common.GetEnumDesc(EnumRequestStatus.ResponseSuccess));
                            }
                            else
                            {
                                FileLog.Write(string.Concat(new object[] { LogPath, logFileName }), Common.GetEnumDesc(EnumRequestStatus.ResponseAbnormal));
                            }

                            WriteFileLog(logFileName, systemMain.SysID.GetValue(), response.StatusCode.ToString(), requestURL);
                        }
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse response = (HttpWebResponse)ex.Response;

                        string responseStatus = string.Empty;
                        if (response != null)
                        {
                            responseStatus = response.StatusCode.ToString();
                        }

                        FileLog.Write(string.Concat(new object[] { LogPath, logFileName }), Common.GetEnumDesc(EnumRequestStatus.RequestError));
                        WriteFileLog(logFileName, systemMain.SysID.GetValue(), responseStatus, requestURL);

                        WriteFileException(systemMain.SysID.GetValue(), requestURL, ex);
                    }
                }
            }

            return EnumJobResult.Success;
        }

        private static List<EntityERPExternal.SystemMain> GetSysSystemMainList(Flow flow, Job job, Connection connection)
        {
            return new EntityERPExternal(connection.value, connection.providerName)
                .SelectSystemMainList();
        }

        private static void WriteFileLog(string logFileName, string sysID, string statusCode, string requestURL)
        {
            FileLog.Write(string.Concat(new object[] { LogPath, logFileName }), string.Format("SystemID : {0}, Result : {1}", sysID, statusCode));
            FileLog.Write(string.Concat(new object[] { LogPath, logFileName }), string.Format("RequestURL : {0}", requestURL));
        }

        private static void WriteFileException(string sysID, string requestURL, WebException ex)
        {
            FileLog.Write(ExceptionPath, string.Format("SystemID : {0}", sysID));
            FileLog.Write(ExceptionPath, string.Format("RequestURL : {0}", requestURL));
            FileLog.Write(ExceptionPath, ex);
        }
    }
}
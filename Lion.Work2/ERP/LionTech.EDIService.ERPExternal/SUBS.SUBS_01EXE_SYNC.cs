using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using LionTech.APIService.Message;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;
using LionTech.Utility;

namespace LionTech.EDIService.ERPExternal
{
    public partial class SUBS
    {
        public class EventParaModel
        {
            public List<string> TargetSysIDList { get; set; }
        }
        public class MessageContent
        {
            public string GroupID { get; set; }
            public string EventID { get; set; }
            public string UserID { get; set; }
        }

        private static string UpdUserID = "EDIService.ERPExternal";

        public static EnumJobResult SUBS_01EXE_SYNC(Flow flow, Job job)
        {
            Connection connection = flow.connections[job.connectionID];
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;
            string summaryPath = flow.paths[EnumPathID.Summary.ToString()].value;

            try
            {
                string filePath = string.Empty;
                if (string.IsNullOrWhiteSpace(job.parameters["FilePath"].value))
                {
                    throw new EDIException(EnumEDIMessage.JobParameterIsNull, new string[] { Job.GetID(flow, job), "FilePath" });
                }
                else
                {
                    filePath = job.parameters["FilePath"].value;
                }

                int timeOut = 5000;
                if (string.IsNullOrWhiteSpace(job.parameters["TimeOut"].value))
                {
                    throw new EDIException(EnumEDIMessage.JobParameterIsNull, new string[] { Job.GetID(flow, job), "TimeOut" });
                }
                else
                {
                    timeOut = int.Parse(job.parameters["TimeOut"].value);
                }

                string errLogPath = string.Empty;
                if (string.IsNullOrWhiteSpace(job.parameters["ErrLogPath"].value))
                {
                    throw new EDIException(EnumEDIMessage.JobParameterIsNull, new string[] { Job.GetID(flow, job), "ErrLogPath" });
                }
                else
                {
                    errLogPath = job.parameters["ErrLogPath"].value;
                }

                List<EntityERPExternal.SystemEventTarget> entitySystemEventTargetList = GetSystemEventTargetList(flow, job, connection);
                if (entitySystemEventTargetList != null)
                {
                    string execEDIEventNo = string.Empty;
                    string eventParaFilePath = string.Empty;
                    string eventPara = string.Empty;
                    bool isSync = false;

                    for (int i = 0; i < entitySystemEventTargetList.Count; i++)
                    {
                        EntityERPExternal.SystemEventTarget systemEventTarget = entitySystemEventTargetList[i];

                        if (i > 0 && systemEventTarget.EDIEventNo.GetValue() != entitySystemEventTargetList[i - 1].EDIEventNo.GetValue())
                        {
                            ExecUpdateEDIEventEndData(flow, job, connection, entitySystemEventTargetList[i - 1], true);
                        }
                        if (i == 0 || systemEventTarget.EDIEventNo.GetValue() != entitySystemEventTargetList[i - 1].EDIEventNo.GetValue())
                        {
                            ExecUpdateEDIEventBeginData(flow, job, connection, systemEventTarget);

                            execEDIEventNo = systemEventTarget.ExecEDIEventNo.GetValue();
                            eventParaFilePath = string.Format(filePath, new string[] { execEDIEventNo.Substring(0, 8), execEDIEventNo });

                            while (true)
                            {
                                try
                                {
                                    eventPara = Common.FileReadStream(eventParaFilePath);
                                    if (string.IsNullOrWhiteSpace(eventPara) == false)
                                    {
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }

                        if (systemEventTarget.TargetSysID.IsNull() == false)
                        {
                            try
                            {
                                isSync = false;

                                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                                EventParaModel eventParaModel = jsonConvert.Deserialize<EventParaModel>(eventPara);
                                if (eventParaModel.TargetSysIDList == null || eventParaModel.TargetSysIDList.IndexOf(systemEventTarget.TargetSysID.GetValue()) > -1)
                                {
                                    ExecInsertNewEDIEventTarget(flow, job, connection, systemEventTarget);
                                    isSync = true;

                                    string requestEventPara = GetRequestEventPara(systemEventTarget, eventPara);

                                    string requestUriString = string.Concat(new object[]
                                    {
                                        systemEventTarget.TargetPath.GetValue(), "?ClientSysID={0}&EDIEventNo={1}&EventPara={2}"
                                    });

                                    requestUriString = string.Format(requestUriString, new string[]
                                    {
                                        EnumSystemID.ERPAP.ToString(), systemEventTarget.EDIEventNo.GetValue(), HttpUtility.UrlEncode(requestEventPara)
                                    });

                                    string responseString;
                                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                                    httpWebRequest.ContentType = "application/json; charset=utf-8";
                                    httpWebRequest.Headers.Set(HttpRequestHeader.Pragma, "no-cache");
                                    httpWebRequest.Timeout = timeOut;
                                    httpWebRequest.Method = WebRequestMethods.Http.Get;

                                    using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                                    {
                                        using (Stream responseStream = httpWebResponse.GetResponseStream())
                                        {
                                            responseString = httpWebResponse.Headers["liontech-req-id"];

                                            if (string.IsNullOrWhiteSpace(responseString))
                                            {
                                                using (StreamReader streamReader = new StreamReader(responseStream))
                                                {
                                                    responseString = streamReader.ReadToEnd();
                                                }
                                            }
                                        }
                                    }

                                    if (string.IsNullOrWhiteSpace(responseString) == false && responseString.Length == 16)
                                    {
                                        FileLog.Write(summaryPath, systemEventTarget.ToString());
                                        FileLog.Write(summaryPath, eventPara);
                                        ExecUpdateEDIEventTargetEndData(flow, job, connection, systemEventTarget, true, responseString);
                                    }
                                    else
                                    {
                                        throw new ERPExternalException(EnumERPExternalMessage.HttpWebRequestGetResponseStringError, new string[]
                                        {
                                            (string.IsNullOrWhiteSpace(responseString) ? string.Empty : responseString)
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                SubEventFailSendMessage(job, systemEventTarget, flow.ediDate, flow.ediNo, connection);

                                FileLog.Write(exceptionPath, systemEventTarget.ToString());
                                FileLog.Write(exceptionPath, eventPara);
                                FileLog.Write(exceptionPath, ex);
                                if (isSync) ExecUpdateEDIEventTargetEndData(flow, job, connection, systemEventTarget, false, null);

                                FileLog.Write(errLogPath, systemEventTarget.ToString());
                                FileLog.Write(errLogPath, eventPara);
                                FileLog.Write(errLogPath, ex);
                            }
                        }
                    }
                    if (entitySystemEventTargetList.Count > 0)
                    {
                        ExecUpdateEDIEventEndData(flow, job, connection, entitySystemEventTargetList[entitySystemEventTargetList.Count - 1], true);
                    }
                }

                return EnumJobResult.Success;
            }
            catch (Exception ex)
            {
                FileLog.Write(exceptionPath, ex);
            }

            return EnumJobResult.Failure;
        }

        private static string GetRequestEventPara(EntityERPExternal.SystemEventTarget systemEventTarget, string eventPara)
        {
            //if (systemEventTarget.SysID.GetValue() == "ERPAP" && 
            //    systemEventTarget.EventGroupID.GetValue() == "SysUserSystemRole" && systemEventTarget.EventID.GetValue() == "Edit")
            //{
            //    return GetRequestEventParaERPAPSysUserSystemRoleEdit(systemEventTarget, eventPara);
            //}
            return eventPara;
        }

        private static List<EntityERPExternal.SystemEventTarget> GetSystemEventTargetList(Flow flow, Job job, Connection connection)
        {
            EntityERPExternal.SystemEventTargetPara para = new EntityERPExternal.SystemEventTargetPara()
            {
                InsertEDINo = new DBChar((string.IsNullOrWhiteSpace(flow.ediNo) ? null : flow.ediNo))
            };

            return new EntityERPExternal(connection.value, connection.providerName)
                .SelectSystemEventTargetList(para);
        }

        private static void ExecUpdateEDIEventBeginData(Flow flow, Job job, Connection connection, EntityERPExternal.SystemEventTarget systemEventTarget)
        {
            EntityERPExternal.EDIEventPara para = new EntityERPExternal.EDIEventPara()
            {
                UpdUserID = new DBVarChar(UpdUserID),
                EDIEventNo = new DBChar((string.IsNullOrWhiteSpace(systemEventTarget.EDIEventNo.GetValue()) ? null : systemEventTarget.EDIEventNo.GetValue()))
            };

            new EntityERPExternal(connection.value, connection.providerName)
                .UpdateEDIEventBeginData(para);
        }

        private static void ExecUpdateEDIEventEndData(Flow flow, Job job, Connection connection, EntityERPExternal.SystemEventTarget systemEventTarget, bool result)
        {
            EntityERPExternal.EDIEventPara para = new EntityERPExternal.EDIEventPara()
            {
                Result = result,
                UpdUserID = new DBVarChar(UpdUserID),
                EDIEventNo = new DBChar((string.IsNullOrWhiteSpace(systemEventTarget.EDIEventNo.GetValue()) ? null : systemEventTarget.EDIEventNo.GetValue()))
            };

            new EntityERPExternal(connection.value, connection.providerName)
                .UpdateEDIEventEndData(para);
        }

        private static void ExecInsertNewEDIEventTarget(Flow flow, Job job, Connection connection, EntityERPExternal.SystemEventTarget systemEventTarget)
        {
            EntityERPExternal.EDIEventTargetPara para = new EntityERPExternal.EDIEventTargetPara()
            {
                EDIEventNo = new DBChar((string.IsNullOrWhiteSpace(systemEventTarget.EDIEventNo.GetValue()) ? null : systemEventTarget.EDIEventNo.GetValue())),
                TargetSysID = new DBVarChar((string.IsNullOrWhiteSpace(systemEventTarget.TargetSysID.GetValue()) ? null : systemEventTarget.TargetSysID.GetValue())),
                UpdUserID = new DBVarChar(UpdUserID)
            };

            new EntityERPExternal(connection.value, connection.providerName)
                .InsertNewEDIEventTarget(para);
        }

        private static void ExecUpdateEDIEventTargetEndData(Flow flow, Job job, Connection connection, EntityERPExternal.SystemEventTarget systemEventTarget, bool result, string returnAPINo)
        {
            EntityERPExternal.EDIEventTargetPara para = new EntityERPExternal.EDIEventTargetPara()
            {
                Result = result,
                ReturnAPINo = new DBChar(returnAPINo),
                UpdUserID = new DBVarChar(UpdUserID),
                EDIEventNo = new DBChar((string.IsNullOrWhiteSpace(systemEventTarget.EDIEventNo.GetValue()) ? null : systemEventTarget.EDIEventNo.GetValue())),
                TargetSysID = new DBVarChar((string.IsNullOrWhiteSpace(systemEventTarget.TargetSysID.GetValue()) ? null : systemEventTarget.TargetSysID.GetValue()))
            };

            new EntityERPExternal(connection.value, connection.providerName)
                .UpdateEDIEventTargetEndData(para);
        }

        /// <summary>
        /// 事件訂閱失敗，發送ERP留言給相關部門IT人員
        /// </summary>
        /// <param name="systemEventTarget">訂閱者</param>
        /// <param name="connection">連線字串</param>
        private static void SubEventFailSendMessage(Job job, EntityERPExternal.SystemEventTarget systemEventTarget, string ediDate, string ediNo, Connection connection)
        {
            try
            {
                var sourceSysID = systemEventTarget.SysID.GetValue();
                var targetSysID = systemEventTarget.TargetSysID.GetValue();
                var eventGroupID = systemEventTarget.EventGroupID.GetValue();
                var eventID = systemEventTarget.EventID.GetValue();

                var failMsgRole = job.parameters["FailMsgRole"].value;
                var sendMsgUserCount = Convert.ToInt32(job.parameters["SendMsgUserCount"].value);

                var messageContents = GetFailSendMessageContent(targetSysID, eventGroupID, eventID, failMsgRole, sendMsgUserCount, connection);

                var client = UserMessageClient.Create(new Uri(ConfigurationManager.AppSettings["LionTech:APIServiceDomain"]));

                foreach (var content in messageContents)
                {
                    var contant = string.Format(job.parameters["FailMsg"].value, sourceSysID, content.GroupID, content.EventID, targetSysID, ediNo);
                    client.ClientSysID = EnumSystemID.ERPAP.ToString();
                    client.ClientUserID = "系統";

                    Message message = new Message
                    {
                        MSG_HDATE = ediDate,
                        MSG_STFN = content.UserID,
                        MSG_SYS = true,
                        MSG_MESSAGE = contant,
                        MSG_TIMEOUT = false,
                    };
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                FileLog.Write(job.parameters["ErrLogPath"].value, ex);
            }        
        }

        /// <summary>
        /// 取得訂閱失敗，留言內容
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="eventGroupID"></param>
        /// <param name="eventID"></param>
        /// <param name="failMsgRole"></param>
        /// <param name="sendMsgUserCount"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static List<MessageContent> GetFailSendMessageContent(string sysID, string eventGroupID, string eventID, string failMsgRole, int sendMsgUserCount, Connection connection)
        {
            List<MessageContent> result = new List<MessageContent>();

            //依造EDI參數設定檔，取得IT人員USERID
            List<EntityERPExternal.SystemRoleData> sysITUserIdList = new EntityERPExternal(connection.value, connection.providerName)
                .SelectSystemRoleUserIdBySysId(sysID, failMsgRole, sendMsgUserCount);

            foreach (var sysITuserID in sysITUserIdList)
            {
                MessageContent msg = new MessageContent
                {
                    GroupID = eventGroupID,
                    EventID = eventID,
                    UserID = sysITuserID.USER_ID.GetValue()
                };
                result.Add(msg);
            }
            return result;
        }
    }
}
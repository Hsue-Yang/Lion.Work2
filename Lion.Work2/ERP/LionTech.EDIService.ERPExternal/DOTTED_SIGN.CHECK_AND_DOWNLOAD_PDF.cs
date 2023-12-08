using Dapper;
using LionTech.EDI;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;

namespace LionTech.EDIService.ERPExternal
{
    public class Task
    {
        public int TaskID { get; set; }
        public bool IsPDFDownload { get; set; }
        public bool IsTrackDownload { get; set; }
    }

    public class DOTTED_SIGN
    {
        private static string exceptionPath;
        private static string summaryPath;
        private static string clientUserID = "EDIService.ERPExternal";
        private static JavaScriptSerializer jsonConvert = new JavaScriptSerializer();

        public static EnumJobResult CHECK_AND_DOWNLOAD_PDF(Flow flow, Job job)
        {
            exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;
            summaryPath = flow.paths[EnumPathID.Summary.ToString()].value;
            string connStr = flow.connections[job.connectionID].value;
            connStr = Security.Decrypt(connStr);

            try
            {
                List<Task> tasks = null;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    tasks = conn.Query<Task>("EXEC dbo.SP_EDI_DOTTED_SIGN_CheckTaskIsFinisned").AsList();
                }

                FileLog.Write(summaryPath, jsonConvert.Serialize(new { TaskIDs = tasks.Select(x => x.TaskID).ToList() }));

                GetTaskDownloadFile(tasks);
                return EnumJobResult.Success;
            }
            catch (Exception ex)
            {
                FileLog.Write(exceptionPath, ex);

                string writtenStream = ex.Message;
                if (ex.StackTrace != null)
                {
                    writtenStream = string.Concat(new object[] { writtenStream, Common.GetNewLineString(), ex.StackTrace });
                }

                Utility.SendMessageToTeams(job.objectName, writtenStream, exceptionPath);
            }
            // 回傳成功，不讓EDI失敗，log會存紀錄
            return EnumJobResult.Success;
        }

        private enum EnumUrl
        {
            [Description("{0}/v1/DottedSign/Task/{1}/Status?ClientUserID={2}&ClientSysID={3}")]
            GetTaskStatus,
            [Description("{0}/v1/DottedSign/Task/{1}/File?ClientUserID={2}&ClientSysID={3}&FileType={4}")]
            GetTaskFile
        }

        private enum EnumAppSettingKey
        {
            APITimeOut
        }

        private static int GetAPITimeOut()
        {
            return int.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.APITimeOut.ToString()]);
        }

        private class TaskCompleted
        {
            public int TaskID { get; set; }
            public string Status { get; set; }
            public bool IsDownload { get; set; }
        }

        private static void GetTaskDownloadFile(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.IsPDFDownload == false && task.IsTrackDownload == false)
                {
                    Thread.Sleep(200);
                    string apiTaskStatusUrl = string.Format(Common.GetEnumDesc(EnumUrl.GetTaskStatus),
                        Common.GetEnumDesc(EnumAPISystemID.ESIGNAP), task.TaskID, clientUserID, EnumSystemID.ERPAP);

                    var responseTaskStatus = Common.HttpWebRequestGetResponseString(apiTaskStatusUrl, GetAPITimeOut());

                    var responseObjTaskStatus = Common.GetJsonDeserializeAnonymousType(responseTaskStatus, new { status = string.Empty });

                    if (responseObjTaskStatus.status == "completed")
                    {
                        GetTaskPDFFile(task.TaskID);
                    }
                }
                else if (task.IsPDFDownload == false)
                {
                    GetTaskPDFFile(task.TaskID, "sign");
                }
                else if (task.IsTrackDownload == false)
                {
                    GetTaskPDFFile(task.TaskID, "trail");
                }
            }
        }

        private static void GetTaskPDFFile(int taskID, string fileType = null)
        {
            string apiTaskFileUrl = string.Format(Common.GetEnumDesc(EnumUrl.GetTaskFile),
                      Common.GetEnumDesc(EnumAPISystemID.ESIGNAP), taskID, clientUserID, EnumSystemID.ERPAP, fileType);

            var responseTaskFile = Common.HttpWebRequestGetResponseString(apiTaskFileUrl, GetAPITimeOut());
            var responseObjTaskFile = false;
            responseObjTaskFile = Common.GetJsonDeserializeAnonymousType(responseTaskFile, responseObjTaskFile);

            TaskCompleted taskCompleted = new TaskCompleted()
            {
                TaskID = taskID,
                Status = "completed",
                IsDownload = responseObjTaskFile
            };
            FileLog.Write(summaryPath, jsonConvert.Serialize(taskCompleted));
        }
    }
}
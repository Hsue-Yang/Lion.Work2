using LionTech.EDI;
using LionTech.Log;
using LionTech.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace LionTech.EDIService.ERPExternal
{
    public partial class EDI_CHECK
    {
        #region - 發送卡檔警告訊息  -
        public static EnumJobResult SEND_STUCK_ALERT_MESSAGE(Flow flow, Job job)
        {
            var notifyContent = job.parameters["NotifyContent"].value;

            if (string.IsNullOrWhiteSpace(notifyContent))
            {
                throw new EDIException(EnumEDIMessage.JobParameterIsNull, new string[] { Job.GetID(flow, job), "NotifyContent" });
            }

            var ediServiceFilerRootPath = job.parameters["EDIServiceFilerRootPath"].value;

            if (string.IsNullOrWhiteSpace(ediServiceFilerRootPath))
            {
                throw new EDIException(EnumEDIMessage.JobParameterIsNull, new string[] { Job.GetID(flow, job), "EDIServiceFilerRootPath" });
            }


            if (string.IsNullOrWhiteSpace(job.parameters["NotifuCheckTime"].value))
            {
                throw new EDIException(EnumEDIMessage.JobParameterIsNull, new string[] { Job.GetID(flow, job), "NotifuCheckTime" });
            }

            var notifuCheckTime = Convert.ToInt32(job.parameters["NotifuCheckTime"].value);

            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            try
            {
                List<string> sysIDs = GetEDISystemIDs(job.parameters);

                foreach (var sysID in sysIDs)
                {
                    var ediSystemID = Common.GetEnumDesc(Utility.GetEnumEDISystemID(sysID));
                    var ediServiceFilePath = System.IO.Path.Combine(
                                              new[]
                                              {
                                              ediServiceFilerRootPath,
                                              ediSystemID
                                              });
                    var runFilePaths = Directory.GetFiles(ediServiceFilePath, "*.run");

                    foreach (var filePath in runFilePaths)
                    {
                        var time = DateTime.Now - File.GetLastWriteTime(filePath);
                        if (File.Exists(filePath) && time.TotalMinutes > notifuCheckTime)
                        {
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                            string message = string.Format(notifyContent
                                , ediSystemID, fileName, (int)time.TotalHours, time.Minutes);
                            Utility.SendMessageToTeams(ediSystemID, message, exceptionPath);
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.Write(exceptionPath, ex);
                return EnumJobResult.Failure;
            }

            return EnumJobResult.Success;
        }
        #endregion

        #region - 工作檔參數取得應用系統 -
        private static List<string> GetEDISystemIDs(Parameters parameters)
        {
            List<string> result = new List<string>();

            foreach (var para in parameters)
            {
                if (para.id.Split('_')[0].Equals("EDISystemID"))
                {
                    result.Add(para.value);
                }
            }
            return result;
        }
        #endregion
    }
}
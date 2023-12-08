using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemEDIFlowScheduleListModel : SysModel
    {
        #region - Definition -
        public enum SCHFrequencyField
        {
            Daily,
            FixedTime,
            Monthly,
            Weekly
        }
        public class SystemEDIFlowSchedule
        {
            public int id { get; set; }
            public string EDIFlowNM { get; set; }
            public string SCHFrequency { get; set; }
            public string SCHStartTime { get; set; }
            public string start_date { get; set; }
            public Nullable<int> SCHIntervalNum { get; set; }
            public Nullable<int> SCHIntervalTime { get; set; }
            public string SCHEndTime { get; set; }
            public string end_date { get; set; }
            public string ExecuteTime { get; set; }
            public string parent { get; set; }
            public string Remark { get; set; }
            public string duration { get; set; }
        }

        #endregion

        public string QuerySysID { get; set; }

        public List<SystemEDIFlowSchedule> SystemEDIFlowScheduleList { get; private set; }

        public string SystemEDIFlowScheduleJson { get; private set; }

        public async Task<bool> GetSystemEDIFlowScheduleList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEDIFlow.QuerySystemEDIFlowScheduleList(userID, QuerySysID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemEDIFlowScheduleList = (List<SystemEDIFlowSchedule>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    SystemEDIFlowScheduleList = responseObj.systemEDIFlowScheduleList;

                    Content(SystemEDIFlowScheduleList);

                    foreach (var item in SystemEDIFlowScheduleList)
                    {
                        RemarkContent(item);
                    }

                    SystemEDIFlowScheduleJson = Common.GetJsonSerializeObject(new {
                        data = SystemEDIFlowScheduleList,
                        links = new List<string>()
                    });
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        //id、parent、ExecuteTime、duration、start_date、end_date
        private void Content(List<SystemEDIFlowSchedule> systemEDIFlowScheduleList)
        {
            var initscheduleLength = systemEDIFlowScheduleList.Count();
            var finalscheduleLength = initscheduleLength + 1;
            var index = 1;
            var subSystemEDIFlowScheduleList = new List<SystemEDIFlowSchedule>();

            foreach (var item in systemEDIFlowScheduleList)
            {
                item.id = index;
                var frequency = Common.GetEnumField<SCHFrequencyField>(item.SCHFrequency);
                
                if ((frequency == SCHFrequencyField.Daily) || (frequency == SCHFrequencyField.Weekly) || (frequency == SCHFrequencyField.Monthly))
                {
                    var executeTimeList = item.ExecuteTime.Split(',').Select(s => s.Substring(0, 4).Insert(2, ":")).ToList();

                    if (executeTimeList.Count() > 1)
                    {
                        item.start_date = executeTimeList.Min();
                        item.end_date = executeTimeList.Max();
                        item.ExecuteTime = string.Join("、", executeTimeList);

                        foreach (var time in executeTimeList)
                        {
                            var subSystemEDIFlowSchedule = new SystemEDIFlowSchedule();
                            subSystemEDIFlowSchedule.id = finalscheduleLength;
                            subSystemEDIFlowSchedule.parent = item.id.ToString();
                            subSystemEDIFlowSchedule.EDIFlowNM = item.EDIFlowNM;
                            subSystemEDIFlowSchedule.SCHFrequency = item.SCHFrequency;
                            subSystemEDIFlowSchedule.SCHIntervalNum = item.SCHIntervalNum;
                            subSystemEDIFlowSchedule.ExecuteTime = time;
                            subSystemEDIFlowSchedule.start_date = time;
                            subSystemEDIFlowSchedule.duration = "0.15";
                            subSystemEDIFlowScheduleList.Add(subSystemEDIFlowSchedule);
                            finalscheduleLength++;
                        }
                    }
                    else
                    {
                        item.ExecuteTime = executeTimeList.Min();
                        item.start_date = item.ExecuteTime;
                        item.duration = "0.15";
                    }
                }
                else if (frequency == SCHFrequencyField.FixedTime)
                {
                    item.start_date = item.SCHStartTime.Substring(0, 4).Insert(2, ":");
                    item.end_date = (item.SCHEndTime ?? string.Empty).Substring(0, 4).Insert(2, ":");
                }
                index++;
            }
            systemEDIFlowScheduleList.AddRange(subSystemEDIFlowScheduleList);
        }
        //Remark
        private void RemarkContent(SystemEDIFlowSchedule systemEDIFlowSchedule)
        {
            var frequency = Common.GetEnumField<SCHFrequencyField>(systemEDIFlowSchedule.SCHFrequency);
            
            if ((frequency == SCHFrequencyField.Daily) || (frequency == SCHFrequencyField.Weekly) || (frequency == SCHFrequencyField.Monthly))
            {
                string remarkContentFormat = null;
                
                if (frequency == SCHFrequencyField.Daily)
                {
                    remarkContentFormat = SysSystemEDIFlowScheduleList.JsMsg_RemarkContentDaily;
                }
                else if (frequency == SCHFrequencyField.Weekly)
                {
                    remarkContentFormat = SysSystemEDIFlowScheduleList.JsMsg_RemarkContentWeekly;
                }
                else if (frequency == SCHFrequencyField.Monthly)
                {
                    remarkContentFormat = SysSystemEDIFlowScheduleList.JsMsg_RemarkContentMonthly;
                }

                systemEDIFlowSchedule.Remark = string.Format(remarkContentFormat, systemEDIFlowSchedule.SCHIntervalNum, systemEDIFlowSchedule.ExecuteTime);
            }
            else if (frequency == SCHFrequencyField.FixedTime)
            {
                systemEDIFlowSchedule.Remark = string.Format(SysSystemEDIFlowScheduleList.JsMsg_RemarkContentFixedTime, systemEDIFlowSchedule.start_date, systemEDIFlowSchedule.end_date, systemEDIFlowSchedule.SCHIntervalTime);
            }         
        }
    }
}
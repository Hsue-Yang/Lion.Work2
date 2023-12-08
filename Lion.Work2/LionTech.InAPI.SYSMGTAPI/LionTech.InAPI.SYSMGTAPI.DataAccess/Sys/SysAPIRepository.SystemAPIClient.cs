using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysAPIRepository
    {
        private class LogInApiTrace
        {
            public string SysID { get; set; }
            public string RequestID { get; set; }
            public string RequestContentType { get; set; }
            public string RequestHttpMethod { get; set; }
            public BsonDocument RequestHeaders { get; set; }
            [BsonElement("RequestHeaders.X-Forwarded-For")]
            public string XForwardedFor { get; set; }
            public string RequestUrl { get; set; }
            public string RequestUrlQuery { get; set; }
            public string RequestUrlLocalPath { get; set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public string ClientIpAddress { get; set; }
            public DateTime RequestDateTime { get; set; }
            public int DurationTime { get; set; }
            public string RequestContent { get; set; }
            public string ResponseStatusCode { get; set; }
            public string ResponseContent { get; set; }
            public string ServerMachine { get; set; }
        }

        public async Task<(int rowCount, IEnumerable<SystemAPIClient> SystemAPILogList)> GetSystemAPILogList(
            string sysID, string apiGroupID, string apiFunID, string apiClientSysID, string apiNo, string dtBegin, string dtEnd,
            string cultureID, int pageIndex, int pageSize)
        {
            List<SystemAPIClient> result = new List<SystemAPIClient>();
            var logInApiTraces = GetLogInApiTraceList(sysID, apiGroupID, apiFunID, apiClientSysID, apiNo, dtBegin, dtEnd, pageIndex, pageSize);

            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var systemApis = (await conn.QueryAsync<SystemAPI>("EXEC dbo.sp_GetSystemAPIByIds @sysID, @apiGroup, @cultureID; ", new
                {
                    sysID,
                    apiGroup = apiGroupID,
                    cultureID
                })).ToList();
                var systemSettings = (await conn.QueryAsync<SystemSetting>("EXEC dbo.sp_GetSystemMainByIds @cultureID;", new {cultureID})).ToList();
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemAPILogs @sysID, @apiGroupID, @apiFunID, @dtBegin, @dtEnd, @apiNo, @apiClientSysID, @cultureID, @pageIndex, @pageSize;", new {sysID, apiGroupID, apiFunID, dtBegin, dtEnd, apiNo, apiClientSysID, cultureID, pageIndex, pageSize});

                logInApiTraces.SystemAPIClientList.ToList().ForEach(row =>
                {
                    row.ClientSysNM = systemSettings.Find(f => f.SysID == row.ClientSysID)?.SysNM;
                    row.APIFunNM = systemApis.Find(f => f.APIFunID == row.APIFunID)?.APIFunNM;
                });

                var rowCount = multi.Read<int>().SingleOrDefault();
                rowCount += logInApiTraces.rowCount;

                var systemApiClientList = multi.Read<SystemAPIClient>();
                result.AddRange(systemApiClientList);
                result.AddRange(logInApiTraces.SystemAPIClientList);

                return (rowCount, result.OrderBy(o => o.ClientDTBegin).Take(pageSize));
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemAPIClient> SystemAPIClientList)> GetSystemAPIClientList(
            string sysID, string apiGroupID, string apiFunID, string dtBegin, string dtEnd, string cultureID, int pageIndex, int pageSize)
        {
            List<SystemAPIClient> result = new List<SystemAPIClient>();
            var logInApiTraces = GetLogInApiTraceList(sysID, apiGroupID, apiFunID, null, null, dtBegin, dtEnd, pageIndex, pageSize);

            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var systemSettings = (await conn.QueryAsync<SystemSetting>("EXEC dbo.sp_GetSystemMainByIds @cultureID;", new {cultureID})).ToList();
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemAPIClients @sysID, @apiGroupID, @apiFunID, @dtBegin, @dtEnd, @cultureID, @pageIndex, @pageSize;", new {sysID, apiGroupID, apiFunID, dtBegin, dtEnd, cultureID, pageIndex, pageSize});

                logInApiTraces.SystemAPIClientList.ToList().ForEach(row => { row.ClientSysNM = systemSettings.Find(f => f.SysID == row.ClientSysID)?.SysNM; });

                var rowCount = multi.Read<int>().SingleOrDefault();
                rowCount += logInApiTraces.rowCount;

                var systemApiClientList = multi.Read<SystemAPIClient>();
                result.AddRange(systemApiClientList);
                result.AddRange(logInApiTraces.SystemAPIClientList);

                return (rowCount, result.OrderBy(o => o.ClientDTBegin).Take(pageSize));
            }
        }

        public async Task<SystemAPIClient> GetSystemAPIClientDetail(string apiNo, string cultureID)
        {
            if (long.TryParse(apiNo, out _))
            {
                using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
                {
                    return await conn.QuerySingleOrDefaultAsync<SystemAPIClient>("EXEC dbo.sp_GetSystemAPIClient @apiNo, @cultureID;", new {apiNo, cultureID});
                }
            }

            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var systemSettings = (await conn.QueryAsync<SystemSetting>("EXEC dbo.sp_GetSystemMainByIds @cultureID;", new {cultureID})).ToList();

                string yearMonth = apiNo.Substring(0, 6);
                var startDt = DateTime.ParseExact(apiNo.Substring(0, 8), "yyyyMMdd", null);
                var endDt = startDt.AddDays(1).AddMilliseconds(-1);

                IMongoCollection<LogInApiTrace> col = _mongoConnectionProvider.LionGroupSERP.GetCollection<LogInApiTrace>($"Log.InApiTrace{yearMonth}");
                var queryResult =
                    (from row in col.AsQueryable()
                        where row.RequestID == apiNo &&
                              startDt <= row.RequestDateTime && row.RequestDateTime <= endDt
                        select new LogInApiTrace
                        {
                            SysID = row.SysID,
                            RequestID = row.RequestID,
                            RequestContentType = row.RequestContentType,
                            RequestHttpMethod = row.RequestHttpMethod,
                            RequestHeaders = row.RequestHeaders,
                            XForwardedFor = row.XForwardedFor,
                            RequestUrl = row.RequestUrl,
                            RequestUrlQuery = row.RequestUrlQuery,
                            RequestUrlLocalPath = row.RequestUrlLocalPath,
                            ControllerName = row.ControllerName,
                            ActionName = row.ActionName,
                            ClientIpAddress = row.ClientIpAddress,
                            RequestDateTime = row.RequestDateTime,
                            DurationTime = row.DurationTime,
                            RequestContent = row.RequestContent,
                            ResponseStatusCode = row.ResponseStatusCode,
                            ResponseContent = row.ResponseContent,
                            ServerMachine = row.ServerMachine
                        }).SingleOrDefault();

                if (queryResult != null)
                {
                    var systemApi =
                        await conn.QuerySingleOrDefaultAsync<SystemAPI>("EXEC dbo.sp_GetSystemAPIFullName @sysID, @apiGroupID, @apiFunID, @cultureID;", new
                        {
                            sysID = queryResult.SysID,
                            apiGroupID = queryResult.ControllerName,
                            apiFunID = queryResult.ActionName,
                            cultureID
                        });

                    var queryDictionary = HttpUtility.ParseQueryString(queryResult.RequestUrlQuery);
                    string clientSysId = queryDictionary["ClientSysID"];

                    return new SystemAPIClient
                    {
                        APINo = queryResult.RequestID,
                        SysID = queryResult.SysID,
                        SysNM = systemSettings.Find(f => f.SysID == queryResult.SysID)?.SysNM,
                        APIGroupID = queryResult.ControllerName,
                        APIGroupNM = systemApi?.APIGroupNM,
                        APIFunID = queryResult.ActionName,
                        APIFunNM = systemApi?.APIFunNM,
                        ClientSysID = queryDictionary["ClientSysID"],
                        ClientSysNM = systemSettings.Find(f => f.SysID == clientSysId)?.SysNM,
                        ClientUserNM = queryDictionary["ClientUserID"],
                        ClientDTBegin = queryResult.RequestDateTime.AddHours(8),
                        ClientDTEnd = queryResult.RequestDateTime.AddHours(8).AddMilliseconds(queryResult.DurationTime),
                        IPAddress = queryResult.XForwardedFor ?? queryResult.ClientIpAddress,
                        REQHeaders = queryResult.RequestHeaders.ToString(),
                        REQUrl = HttpUtility.UrlDecode(queryResult.RequestUrl),
                        REQReturn = queryResult.ResponseContent
                    };
                }

                return null;
            }
        }

        private (int rowCount, IEnumerable<SystemAPIClient> SystemAPIClientList) GetLogInApiTraceList(
            string sysID, string apiGroupID, string apiFunID, string apiClientSysID, string apiNo, string dtBegin, string dtEnd,
            int pageIndex, int pageSize)
        {
            List<SystemAPIClient> result = new List<SystemAPIClient>();
            bool crossMonth = false;
            int rowCount = 0, prevTotalPageIndex = 0, prevRemainRowCount = 0;
            var startDt = Convert.ToDateTime(dtBegin);
            var endDt = Convert.ToDateTime(dtEnd);

            foreach (var yearMonth in new[] {startDt.ToString("yyyyMM"), endDt.ToString("yyyyMM")}.Distinct())
            {
                IMongoCollection<LogInApiTrace> col = _mongoConnectionProvider.LionGroupSERP.GetCollection<LogInApiTrace>($"Log.InApiTrace{yearMonth}");

                var queryResult =
                    from row in col.AsQueryable()
                    where row.SysID == sysID &&
                          row.ControllerName == apiGroupID &&
                          startDt <= row.RequestDateTime && row.RequestDateTime <= endDt
                    select new LogInApiTrace
                    {
                        SysID = row.SysID,
                        RequestID = row.RequestID,
                        RequestContentType = row.RequestContentType,
                        RequestHttpMethod = row.RequestHttpMethod,
                        RequestHeaders = row.RequestHeaders,
                        XForwardedFor = row.XForwardedFor ?? null,
                        RequestUrl = row.RequestUrl,
                        RequestUrlQuery = row.RequestUrlQuery,
                        RequestUrlLocalPath = row.RequestUrlLocalPath,
                        ControllerName = row.ControllerName,
                        ActionName = row.ActionName,
                        ClientIpAddress = row.ClientIpAddress,
                        RequestDateTime = row.RequestDateTime,
                        DurationTime = row.DurationTime,
                        RequestContent = row.RequestContent,
                        ResponseStatusCode = row.ResponseStatusCode,
                        ServerMachine = row.ServerMachine
                    };

                if (string.IsNullOrWhiteSpace(apiFunID) == false)
                {
                    queryResult = queryResult.Where(w => w.ActionName == apiFunID);
                }

                if (string.IsNullOrWhiteSpace(apiNo) == false)
                {
                    queryResult = queryResult.Where(w => w.RequestID == apiNo);
                }

                if (string.IsNullOrWhiteSpace(apiClientSysID) == false)
                {
                    queryResult = queryResult.Where(w => w.RequestUrlQuery.Contains($"ClientSysID={apiClientSysID}"));
                }
                
                rowCount += queryResult.Count();
                
                if (result.Count < pageSize)
                {
                    int skipNum = (pageIndex - 1 - prevTotalPageIndex) * pageSize;

                    if (crossMonth && 0 < skipNum)
                    {
                        skipNum -= prevRemainRowCount;
                    }

                    result.AddRange(queryResult
                        .OrderBy(o => o.RequestDateTime)
                        .Skip(skipNum)
                        .Take(pageSize)
                        .AsEnumerable()
                        .Select(row =>
                        {
                            var queryDictionary = HttpUtility.ParseQueryString(row.RequestUrlQuery);
                            return new SystemAPIClient
                            {
                                APINo = row.RequestID,
                                APIGroupID = row.ControllerName,
                                APIFunID = row.ActionName,
                                ClientSysID = queryDictionary["ClientSysID"],
                                ClientDTBegin = row.RequestDateTime.AddHours(8),
                                ClientDTEnd = row.RequestDateTime.AddHours(8).AddMilliseconds(row.DurationTime),
                                IPAddress = row.XForwardedFor ?? row.ClientIpAddress,
                                REQHeaders = row.RequestHeaders.ToString(),
                            };
                        }));
                }

                prevTotalPageIndex = rowCount / pageSize;
                prevRemainRowCount = rowCount % pageSize;
                crossMonth = true;
            }

            return (rowCount, result);
        }
    }
}

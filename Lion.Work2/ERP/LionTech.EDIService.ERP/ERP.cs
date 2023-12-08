using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;

using System;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using LionTech.EDI;
using LionTech.Entity.EDI;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Entity;

namespace LionTech.EDIService.ERP
{
    partial class ERP : ServiceBase
    {
        public ERP()
        {
            InitializeComponent();
        }

        string _rootPath = string.Empty;
        string _runExName = ".run";
        string _serviceName = "LionTech.EDIService.ERP";
        string _systemID = "ERPAP";

        string _providerName = null;
        string _connectionString = null;
        string _updUserID = "EDIService.ERP";

        string _appSettingRootPath = "ERP.RootPath";
        string _appSettingEDIConnectionString = "ERP.EDI";

        List<string> _fileLogWrittenStreamList = new List<string>();
        List<Exception> _fileLogWrittenExceptionList = new List<Exception>();

        Flows _flows;

        private EDIServiceActionFilterBase _ediServiceActionFilter;

        enum EnumFlowStatus { idling, waitting, execute }

        enum EnumRootPathFile
        {
            [Description(@"Log\{0}.log")]
            Log,
            [Description(@"Log\{0}.err")]
            Exception
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this.GetAppSettings();
                this.timer2.AutoReset = true;
                this.timer2.Enabled = true;

                this.FileLogWrite(string.Concat(new object[] { "[start-up] PID:", System.Diagnostics.Process.GetCurrentProcess().Id }));

                //this.FileLogWrite("read image path.");
                //string subKeyPath = string.Concat(new object[] { @"SYSTEM\CurrentControlSet\services\", _serviceName });
                //string keyName = "ImagePath";
                //string imagePath = Common.ReadRegistry(Registry.LocalMachine, subKeyPath, keyName);
                //if (imagePath == null || imagePath == "")
                //{
                //    throw new EDIServiceException(EnumEDIServiceMessage.ImagePathNull, new string[] { string.Concat(
                //        new object[] { Registry.LocalMachine.ToString(), subKeyPath, "\\", keyName }) });
                //}

                this.FileLogWrite("load flows from XML.");
                string xmlPath = string.Concat(new object[] { _rootPath, _serviceName, ".exe.xml" });
                _flows = Flows.Load(xmlPath);

                this.FileLogWrite("build serverice action filter.");
                _ediServiceActionFilter = new EDIServiceActionFilter
                {
                    FileLogWriteString = FileLogWrite,
                    FileLogWriteException = FileLogWrite
                };

                this.FileLogWrite("execute service.");
                this.timer1.AutoReset = true;
                this.timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(EDIServiceException) &&
                    ((EDIServiceException)ex).EDIServiceMessage == EnumEDIServiceMessage.RootPathNull)
                {
                    this.EventLogWrite(ex);
                    throw;
                }
                else
                {
                    this.FileLogWrite(ex);
                }
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            try
            {
                while (_fileLogWrittenStreamList.Count > 0 || _fileLogWrittenExceptionList.Count > 0)
                {
                    this.timer2_Elapsed(null, null);
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(_rootPath))
                {
                    this.EventLogWrite(ex);
                    throw;
                }
                else
                {
                    this.FileLogWrite(ex);
                }
            }

            if (string.IsNullOrEmpty(_rootPath) == false)
            {
                FileLog.Write(FileLog.GetFilePath(_rootPath, Common.GetEnumDesc(EnumRootPathFile.Log), Common.GetDateString()), "[shutdown]");
            }
        }

        private void GetAppSettings()
        {
            _rootPath = ConfigurationManager.AppSettings[_appSettingRootPath];
            if (string.IsNullOrEmpty(_rootPath))
            {
                throw new EDIServiceException(EnumEDIServiceMessage.RootPathNull, new string[] { _appSettingRootPath });
            }
            if (_rootPath.EndsWith("\\") == false) { _rootPath = string.Concat(new object[] { _rootPath, "\\" }); }

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[_appSettingEDIConnectionString];
            if (connectionStringSettings == null)
            {
                throw new EDIServiceException(EnumEDIServiceMessage.ConnectionStringNull, new string[] { _appSettingEDIConnectionString });
            }
            else
            {
                if (connectionStringSettings.ProviderName == typeof(System.Data.SqlClient.SqlConnection).Namespace)
                {
                    _providerName = connectionStringSettings.ProviderName;
                    _connectionString = connectionStringSettings.ConnectionString;
                }
                else
                {
                    throw new EntityException(EnumEntityMessage.ProviderNameNotFound, new string[] { connectionStringSettings.ProviderName });
                }
            }
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.timer1.Enabled = false;

                this.CheckLogTimerAlive();

                this.ValidateSchedule(_flows);

                EntityEDI.Flows unexecutedFlows = new EntityEDI(_connectionString, _providerName)
                    .SelectUnexecutedFlow(new DBEDIFLOWID(_systemID));

                foreach (EntityEDI.Flow entityEDIFlow in unexecutedFlows)
                {
                    Flow flow = _flows[entityEDIFlow.EDI_FLOW_ID.GetValue()];

                    if (flow != null)
                    {
                        EnumFlowStatus flowStatus = this.ValidateStartDateTime(flow, entityEDIFlow);
                        this.FileLogWrite(string.Concat(new object[] { "[", flowStatus, "] ", Flow.GetID(flow) }));

                        if (flowStatus == EnumFlowStatus.execute)
                        {
                            string runFilePath = FileLog.GetFilePath(_rootPath, string.Concat(new object[] { flow.id, _runExName }));
                            FileInfo fileInfo = new FileInfo(runFilePath);
                            using (FileStream fileStream = fileInfo.Create()) { fileStream.Dispose(); }

                            flow.New();
                            flow.ediNo = entityEDIFlow.EDI_NO.GetValue();
                            flow.dataDate = entityEDIFlow.DATA_DATE.GetValue();

                            Thread ediFlowExecuteThread = new Thread(new ParameterizedThreadStart(this.EDIFlow_Execute));
                            ediFlowExecuteThread.IsBackground = true;
                            ediFlowExecuteThread.Start(flow);
                        }
                        
                        //Thread ediFlowDeleteLogThread = new Thread(new ParameterizedThreadStart(this.DeleteFlowLog));
                        //ediFlowDeleteLogThread.IsBackground = true;
                        //ediFlowDeleteLogThread.Start(flow);
                    }
                }
            }
            catch (Exception ex)
            {
                this.FileLogWrite(ex);
            }
            finally
            {
                this.timer1.AutoReset = true;
                this.timer1.Enabled = true;
            }
        }
        private void CheckLogTimerAlive()
        {
            if (_fileLogWrittenExceptionList.Count > 100 ||
                _fileLogWrittenStreamList.Count > 100)
            {
                timer2.Stop();
                timer2.Start();
                this.FileLogWrite("log timer restart");
            }
        }
        private void ValidateSchedule(Flows flows)
        {
            DateTime nowDateTime = DateTime.Now;
            foreach (Flow flow in flows)
            {
                DateTime todayDateTime;
                if (flow.schedule.IsExcute(nowDateTime, out todayDateTime))
                {
                    DBEDINO dbEDINO = new EntityEDI(_connectionString, _providerName)
                        .InsertNewFlow(new DBEDIFLOWID(_systemID),
                                       new DBEDIFLOWID(flow.id), new DBYMD(nowDateTime.AddDays(-1 * flow.schedule.dataDelay)),
                                       new DBSYSDT(todayDateTime), new DBUSERID(_updUserID));
                }
            }
        }

        private EnumFlowStatus ValidateStartDateTime(Flow flow, EntityEDI.Flow entityEDIFlow)
        {
            DateTime startDateTime = Common.GetDateTime(flow.schedule.startDate, flow.schedule.startTime);

            if (DateTime.Now < startDateTime)
            {
                return EnumFlowStatus.idling;
            }

            if (flow.schedule.frequency == EnumScheduleFrequency.Continuity)
            {
                string runFilePath = FileLog.GetFilePath(_rootPath, string.Concat(new object[] { flow.id, _runExName }));
                FileInfo fileInfo = new FileInfo(runFilePath);
                if (fileInfo.Exists == true)
                {
                    return EnumFlowStatus.waitting;
                }
                else
                {
                    return EnumFlowStatus.execute;
                }
            }
            else if (flow.schedule.frequency == EnumScheduleFrequency.Daily ||
                     flow.schedule.frequency == EnumScheduleFrequency.Weekly ||
                     flow.schedule.frequency == EnumScheduleFrequency.Monthly ||
                     flow.schedule.frequency == EnumScheduleFrequency.FixedTime)
            {
                if (entityEDIFlow.IS_AUTOMATIC.GetValue() == EnumYN.N ||
                    (entityEDIFlow.IS_AUTOMATIC.GetValue() == EnumYN.Y && DateTime.Now > entityEDIFlow.AUTO_SCHEDULE.GetValue()))
                {
                    string runFilePath = FileLog.GetFilePath(_rootPath, string.Concat(new object[] { flow.id, _runExName }));
                    FileInfo fileInfo = new FileInfo(runFilePath);
                    if (fileInfo.Exists == true)
                    {
                        return EnumFlowStatus.waitting;
                    }
                    else
                    {
                        return EnumFlowStatus.execute;
                    }
                }
                else
                {
                    return EnumFlowStatus.waitting;
                }
            }
            else
            {
                throw new EDIServiceException(EnumEDIServiceMessage.ScheduleFrequencyError,
                    new string[] { flow.schedule.frequency.ToString() });
            }
        }

        private void FileLogWrite(string writtenStream)
        {
            lock (_fileLogWrittenStreamList)
            {
                _fileLogWrittenStreamList.Add(writtenStream);
            }
        }

        private void FileLogWrite(Exception ex)
        {
            this.FileLogWrite(string.Concat(new object[] { "error: ", ex.Message }));
            lock (_fileLogWrittenExceptionList)
            {
                _fileLogWrittenExceptionList.Add(ex);
            }
        }

        private void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.timer2.Enabled = false;

                lock (_fileLogWrittenStreamList)
                {
                    if (_fileLogWrittenStreamList.Count > 0)
                    {
                        FileLog.Write(FileLog.GetFilePath(_rootPath, Common.GetEnumDesc(EnumRootPathFile.Log), Common.GetDateString()), _fileLogWrittenStreamList[0]);
                        _fileLogWrittenStreamList.RemoveAt(0);
                    }
                }
                lock (_fileLogWrittenExceptionList)
                {
                    if (_fileLogWrittenExceptionList.Count > 0)
                    {
                        FileLog.Write(FileLog.GetFilePath(_rootPath, Common.GetEnumDesc(EnumRootPathFile.Exception), Common.GetDateString()), _fileLogWrittenExceptionList[0]);
                        _fileLogWrittenExceptionList.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.EventLogWrite(ex);
            }
            finally
            {
                this.timer2.AutoReset = true;
                this.timer2.Enabled = true;
            }
        }

        private void EventLogWrite(Exception ex)
        {
            LionTech.Log.EventLog.Write(_serviceName, ex);
        }

        //private void DeleteFlowLog(object obj)
        //{
        //    try
        //    {
        //        Flow flow = (Flow)obj;
        //        if (flow.schedule.keepLogDay.HasValue &&
        //            flow.schedule.keepLogDay.Value > 0)
        //        {
        //            DateTime lastDate = DateTime.Now.AddDays(-(flow.schedule.keepLogDay.Value - 1));
        //            int lastDateNumber = int.Parse(Common.GetDateTimeFormattedText(lastDate, Common.EnumDateTimeFormatted.ShortDateNumber));

        //            string flowPath = flow.paths[EnumPathID.CMD.ToString()].value;
        //            flowPath = flowPath.Split(new [] { "{flowid}" }, StringSplitOptions.RemoveEmptyEntries)[0];
        //            flowPath += flow.id;
        //            string[] searchAllDir = Directory.GetFileSystemEntries(flowPath);

        //            foreach (string path in searchAllDir)
        //            {
        //                var fileName = System.IO.Path.GetFileName(path);
        //                if (Regex.IsMatch(fileName, "^[2][0][0-9]{6}$"))
        //                {
        //                    if (int.Parse(fileName) < lastDateNumber)
        //                    {
        //                        Directory.Delete(path, true);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLogWrite(ex);
        //    }
        //}

        private void EDIFlow_Execute(object obj)
        {
            try
            {
                Flow flow = (Flow)obj;
                EDIFlow ediFlow = new EDIFlow();

                ediFlow.FlowBegin = EDIFlow_OnFlowBegin;
                ediFlow.FlowBegin += _ediServiceActionFilter.OnFlowBegin;

                ediFlow.FlowEnded = EDIFlow_OnFlowEnded;
                ediFlow.FlowEnded += _ediServiceActionFilter.OnFlowEnded;

                ediFlow.JobBegin = EDIFlow_OnJobBegin;
                ediFlow.JobBegin += _ediServiceActionFilter.OnJobBegin;

                ediFlow.JobEnded = EDIFlow_OnJobEnded;
                ediFlow.JobEnded += _ediServiceActionFilter.OnJobEnded;

                ediFlow.Erred = EDIFlow_OnErred;
                ediFlow.Erred += _ediServiceActionFilter.OnErred;

                ediFlow.Execute(_systemID, flow);
            }
            catch (Exception ex)
            {
                this.FileLogWrite(ex);
                _ediServiceActionFilter.OnException(ex);
            }
        }

        private void EDIFlow_OnFlowBegin(Flow flow)
        {
            EntityEDI entityEDI = new EntityEDI(_connectionString, _providerName);
            entityEDI.UpdateFlowBeginData(
                new DBEDINO(flow.ediNo), new DBYMD(flow.ediDate), new DBHMSM(flow.ediTime), new DBUSERID(_updUserID));

            object[] objects = new object[] { "begin: \\", flow.ediDate, "\\", flow.ediTime, "\\(", flow.dataDate, ")" };

            string runFilePath = FileLog.GetFilePath(_rootPath, string.Concat(new object[] { flow.id, _runExName }));
            FileInfo fileInfo = new FileInfo(runFilePath);
            if (fileInfo.Exists == false)
            {
                throw new EDIServiceException(EnumEDIServiceMessage.FlowRunFileNotExist, new string[] { Flow.GetID(flow) });
            }
            else
            {
                FileLog.Write(runFilePath, string.Concat(objects));
            }
            this.FileLogWrite(string.Concat(objects));
        }

        private void EDIFlow_OnFlowEnded(Flow flow)
        {
            EnumResultID enumRESULT_ID = EnumResultID.F;
            if (flow.result == EnumFlowResult.Success) { enumRESULT_ID = EnumResultID.S; }
            else if (flow.result == EnumFlowResult.Cancel) { enumRESULT_ID = EnumResultID.C; }

            EntityEDI entityEDI = new EntityEDI(_connectionString, _providerName);
            entityEDI.UpdateFlowEndData(
                new DBEDINO(flow.ediNo), new DBCODEID(enumRESULT_ID), new DBUSERID(_updUserID));

            string runFilePath = FileLog.GetFilePath(_rootPath, string.Concat(new object[] { flow.id, _runExName }));
            FileInfo fileInfo = new FileInfo(runFilePath);
            if (fileInfo.Exists == false)
            {
                throw new EDIServiceException(EnumEDIServiceMessage.FlowRunFileNotExist, new string[] { Flow.GetID(flow) });
            }
            else
            {
                if (flow.result == EnumFlowResult.Success || flow.result == EnumFlowResult.Cancel) { fileInfo.Delete(); }
            }
            this.FileLogWrite(string.Concat(new object[] { "ended: result -> ", flow.result.ToString() }));
        }

        private void EDIFlow_OnJobBegin(Flow flow, Job job)
        {
            EntityEDI entityEDI = new EntityEDI(_connectionString, _providerName);
            entityEDI.InsertNewJob(
                new DBEDINO(flow.ediNo), new DBEDIJOBID(job.id), new DBUSERID(_updUserID));

            object[] objects = new object[] { "job begin: ", Job.GetID(flow, job) };

            string runFilePath = FileLog.GetFilePath(_rootPath, string.Concat(new object[] { flow.id, ".", job.id, _runExName }));
            FileInfo fileInfo = new FileInfo(runFilePath);
            if (fileInfo.Exists == true) { fileInfo.Delete(); }
            FileLog.Write(runFilePath, string.Concat(objects));

            this.FileLogWrite(string.Concat(objects));
        }

        private void EDIFlow_OnJobEnded(Flow flow, Job job)
        {
            EnumResultID enumRESULT_ID = EnumResultID.F;
            if (job.result == EnumJobResult.Success) { enumRESULT_ID = EnumResultID.S; }
            else if (job.result == EnumJobResult.Cancel) { enumRESULT_ID = EnumResultID.C; }

            EntityEDI entityEDI = new EntityEDI(_connectionString, _providerName);
            entityEDI.UpdateJobEndData(
                new DBEDINO(flow.ediNo), new DBEDIJOBID(job.id), new DBCODEID(enumRESULT_ID), new DBCOUNT(job.countRow), new DBUSERID(_updUserID));

            string runFilePath = FileLog.GetFilePath(_rootPath, string.Concat(new object[] { flow.id, ".", job.id, _runExName }));
            FileInfo fileInfo = new FileInfo(runFilePath);
            if (fileInfo.Exists == true)
            {
                if (job.result == EnumJobResult.Success || job.result == EnumJobResult.Cancel) { fileInfo.Delete(); }
            }
            this.FileLogWrite(string.Concat(new object[] { "job ended: result -> ", job.result.ToString() }));
        }

        private void EDIFlow_OnErred(Flow flow, string error)
        {
            this.FileLogWrite(string.Concat(new object[] { "error: ", error }));
        }
    }
}

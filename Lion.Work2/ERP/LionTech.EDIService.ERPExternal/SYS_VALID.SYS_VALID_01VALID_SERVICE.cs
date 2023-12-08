using System;
using System.Collections.Generic;
using System.ServiceProcess;
using LionTech.EDI;
using LionTech.Log;

namespace LionTech.EDIService.ERPExternal
{
    public partial class SYS_VALID
    {
        public static EnumJobResult SYS_VALID_01VALID_SERVICE(Flow flow, Job job)
        {
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            Dictionary<string, bool> targetService = new Dictionary<string, bool>()
                {
                    {"SQL Server (MSSQLSERVER)", false},
                    {"SQL Server Agent (MSSQLSERVER)", false}
                };

            try
            {
                string errLogPath = string.Empty;
                if (string.IsNullOrWhiteSpace(job.parameters["ErrLogPath"].value))
                {
                    throw new EDIException(EnumEDIMessage.JobParameterIsNull, new string[] { Job.GetID(flow, job), "ErrLogPath" });
                }
                else
                {
                    errLogPath = job.parameters["ErrLogPath"].value;
                }

                ServiceController[] serviceControllerArray = ServiceController.GetServices();

                foreach (ServiceController serviceController in serviceControllerArray)
                {
                    if (targetService.ContainsKey(serviceController.DisplayName))
                    {
                        if (serviceController.Status == ServiceControllerStatus.Running)
                        {
                            bool dependedStatus = true;

                            foreach (ServiceController dependedServiceController in serviceController.ServicesDependedOn)
                            {
                                try
                                {
                                    if (dependedServiceController.Status != ServiceControllerStatus.Running)
                                    {
                                        dependedStatus = false;
                                        break;
                                    }
                                }
                                catch (InvalidOperationException)
                                {
                                    //無法叫用的Win32不用處理
                                }
                            }

                            if (dependedStatus)
                            {
                                targetService[serviceController.DisplayName] = true;
                            }
                        }
                    }
                }

                foreach (KeyValuePair<string, bool> targetServiceKeyValuePair in targetService)
                {
                    if (targetServiceKeyValuePair.Value == false)
                    {
                        string writtenStream = string.Format("{0} is not running.", targetServiceKeyValuePair.Key);

                        FileLog.Write(exceptionPath, writtenStream);
                        FileLog.Write(errLogPath, writtenStream);
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
    }
}
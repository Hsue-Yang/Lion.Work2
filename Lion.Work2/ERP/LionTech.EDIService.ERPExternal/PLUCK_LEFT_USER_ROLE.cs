using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;

namespace LionTech.EDIService.ERPExternal
{
    public static class PLUCK_LEFT_USER_ROLE
    {
        private enum EnumJobParameter
        {
            TimeOut,
            ErrLogPath
        }

        private enum EnumConnectionString
        {
            LionGroupMSERP
        }

        private static string _errLogPath;
        private static string _summaryPath;
        private static int _ediServiceDistributorTimeOut;
        private static Entity_BaseAP _entity;
        private static Mongo_BaseAP _mongoEntity;
        private static EntityERPExternal _entityErpExternal;

        public static EnumJobResult LEFT_USER_AUTH_SYNC(Flow flow, Job job)
        {
            EnumJobResult jobResult = EnumJobResult.Failure;
            Connection connection = flow.connections[job.connectionID];
            Connection mongoConnection = flow.connections[EnumConnectionString.LionGroupMSERP.ToString()];
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;
            string cultureID = EnumCultureID.zh_TW.ToString();
            _summaryPath = flow.paths[EnumPathID.Summary.ToString()].value;

            try
            {
                _SetJobParameter(flow, job);

                _entity = new Entity_BaseAP(connection.value, connection.providerName);
                _entityErpExternal = new EntityERPExternal(connection.value, connection.providerName);
                _mongoEntity = new Mongo_BaseAP(mongoConnection.value, mongoConnection.providerName);

                string ipAddress =
                    (from ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList
                     where ip.AddressFamily == AddressFamily.InterNetwork
                     select ip.ToString()).FirstOrDefault();

                List<EntityERPExternal.AuthEventTargetPara.Event> paraEvents =
                    (from eventName in
                        new[]
                        {
                            EnumEDIServiceEventGroupID.SysUserSystemRole.ToString(),
                            EnumEDIServiceEventGroupID.SysSystemFunAssign.ToString()
                        }
                     select new EntityERPExternal.AuthEventTargetPara.Event
                     {
                         SysID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
                         EventGroupID = new DBVarChar(eventName),
                         EventID = new DBVarChar(EnumEDIServiceEventID.Edit.ToString())
                     }).ToList();
                
                var entityBasicInfo = _GetBasicInfo(cultureID);

                var modifyTypeNM = _GetModifyTypeName(cultureID);
                
                var authEventTargetList =
                    _entityErpExternal.SelectAuthEventTargetList(new EntityERPExternal.AuthEventTargetPara { EventList = paraEvents });

                var userLeftList = _entityErpExternal.SelectUserLeftList();

                if (userLeftList.Any())
                {
                    var deleteResult = _entityErpExternal.DeleteLeftUserRole(new EntityERPExternal.DeleteLeftUserRolePara
                    {
                        UserIDList = userLeftList.Select(n => n.UserID).ToList(),
                        UpdUserID = entityBasicInfo.UpdUserID,
                        IPAddress = new DBVarChar(ipAddress),
                        ExecSysID = entityBasicInfo.ExecSysID
                    });

                    if (deleteResult == EntityERPExternal.EnumDeleteLeftUserRoleResult.Success)
                    {
                        var delUserRoleLogList = userLeftList.Select(n => $"delete user role:{n.UserID.GetValue()}, IsLeft:{n.IsLeft.GetValue()}, LeftDate:{n.LeftDate.GetValue()}").ToList();
                        FileLog.Write(_summaryPath, string.Join(Environment.NewLine, delUserRoleLogList));
                    }
                }

                foreach (var userLeft in userLeftList)
                {
                    DBEntity.DBTableRow eventPara = null;

                    var targetSysIDList =
                        (from s in authEventTargetList
                         where s.EventGroupID.GetValue() == EnumEDIServiceEventGroupID.SysUserSystemRole.ToString() &&
                               s.EventID.GetValue() == EnumEDIServiceEventID.Edit.ToString()
                         select s.SysID).ToList();

                    var assignTargetSysIDList =
                        (from s in authEventTargetList
                         where s.EventGroupID.GetValue() == EnumEDIServiceEventGroupID.SysSystemFunAssign.ToString() &&
                               s.EventID.GetValue() == EnumEDIServiceEventID.Edit.ToString()
                         select s.SysID).ToList();

                    var userSystemFunAssignList =
                        _entityErpExternal.SelectUserSystemFunAssignList(new EntityERPExternal.UserSystemFunAssignPara
                        {
                            UserID = userLeft.UserID,
                            SysIDList = assignTargetSysIDList
                        });
                    
                    #region - 使用者角色清單 -

                    foreach (var targetSysID in targetSysIDList)
                    {
                        try
                        {
                            eventPara = new EntityEventPara.SysUserSystemRoleEdit
                            {
                                TargetSysIDList = new List<DBVarChar> { targetSysID },
                                UserID = userLeft.UserID,
                                RoleGroupID = new DBVarChar(null),
                                RoleIDList = new List<DBVarChar>()
                            };

                            string responseString = Utility.ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysUserSystemRole, EnumEDIServiceEventID.Edit, eventPara.SerializeToJson(), _ediServiceDistributorTimeOut);
                            _RecordHttpResponseString(responseString, eventPara);
                        }
                        catch (Exception ex)
                        {
                            if (eventPara != null)
                            {
                                FileLog.Write(exceptionPath, eventPara.SerializeToJson());
                            }
                            FileLog.Write(exceptionPath, ex);

                            if (eventPara != null)
                            {
                                FileLog.Write(_errLogPath, eventPara.SerializeToJson());
                            }
                            FileLog.Write(_errLogPath, ex);
                        }
                    }
                    #endregion

                    #region - 使用者指派檔 -
                    foreach (var targetSysID in 
                        (from s in userSystemFunAssignList
                         group s by new
                         {
                             SysID = s.SysID.GetValue(),
                             FunControllerID = s.FunControllerID.GetValue(),
                             FunActionName = s.FunActionName.GetValue()
                         }
                         into g
                         select new
                         {
                             g.Key.SysID,
                             g.Key.FunControllerID,
                             g.Key.FunActionName,
                             UserIDList = g.Select(s => s.UserID).ToList()
                         }))
                    {
                        try
                        {
                            eventPara = new EntityEventPara.SysSystemFunAssignEdit
                            {
                                TargetSysIDList = new List<DBVarChar> { new DBVarChar(targetSysID.SysID) },
                                SysID = new DBVarChar(targetSysID.SysID),
                                FunControllerID = new DBVarChar(targetSysID.FunControllerID),
                                FunActionName = new DBVarChar(targetSysID.FunActionName),
                                UserIDList = targetSysID.UserIDList
                            };

                            string responseString = Utility.ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemFunAssign, EnumEDIServiceEventID.Edit, eventPara.SerializeToJson(), _ediServiceDistributorTimeOut);
                            _RecordHttpResponseString(responseString, eventPara);
                        }
                        catch (Exception ex)
                        {
                            if (eventPara != null)
                            {
                                FileLog.Write(exceptionPath, eventPara.SerializeToJson());
                            }
                            FileLog.Write(exceptionPath, ex);

                            if (eventPara != null)
                            {
                                FileLog.Write(_errLogPath, eventPara.SerializeToJson());
                            }
                            FileLog.Write(_errLogPath, ex);
                        }
                    }
                    #endregion

                    #region - Record Log -
                    _mongoEntity.RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_FUN, new Mongo_BaseAP.RecordUserFunctionPara
                    {
                        ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.D.ToString()),
                        ModifyTypeNM = modifyTypeNM,
                        UserID = userLeft.UserID,
                        UserNM = userLeft.UserNM,
                        UpdUserID = entityBasicInfo.UpdUserID,
                        UpdUserNM = entityBasicInfo.UpdUserNM,
                        UpdDT = new DBDateTime(DateTime.Now),
                        ExecSysID = entityBasicInfo.ExecSysID,
                        ExecSysNM = entityBasicInfo.ExecSysNM,
                        ExecIPAddress = new DBVarChar(ipAddress)
                    });
                    #endregion

                }

                jobResult = EnumJobResult.Success;
            }
            catch (Exception ex)
            {
                FileLog.Write(exceptionPath, ex);
            }
            finally
            {
                _errLogPath = null;
                _summaryPath = null;
                _entity = null;
                _mongoEntity = null;
                _entityErpExternal = null;
            }

            return jobResult;
        }

        private static void _RecordHttpResponseString(string responseString, DBEntity.DBTableRow eventPara)
        {
            if (string.IsNullOrWhiteSpace(responseString) == false)
            {
                FileLog.Write(_summaryPath, string.Format("eventPara:{0}, responseString:{1}", eventPara.SerializeToJson(), responseString));
            }
            else
            {
                throw new ERPExternalException(EnumERPExternalMessage.HttpWebRequestGetResponseStringError,
                    new[]
                    {
                        (string.IsNullOrWhiteSpace(responseString) ? string.Empty : responseString)
                    });
            }
        }

        private static DBNVarChar _GetModifyTypeName(string cultureID)
        {
            Entity_BaseAP.CMCodePara codePara =
                new Entity_BaseAP.CMCodePara
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID)
                };

            DBNVarChar modifyTypeNM;
            List<Entity_BaseAP.CMCode> modifyTypeList = _entity.SelectCMCodeList(codePara);

            if (modifyTypeList != null &&
                modifyTypeList.Any())
            {
                modifyTypeNM = modifyTypeList.Find(e => e.CodeID.GetValue() == Mongo_BaseAP.EnumModifyType.D.ToString()).CodeNM;
            }
            else
            {
                modifyTypeNM = new DBNVarChar(null);
            }
            return modifyTypeNM;
        }

        private static Entity_BaseAP.BasicInfo _GetBasicInfo(string cultureID)
        {
            Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID)
            {
                UpdUserID = _entityErpExternal.UpdUserID,
                ExecSysID = new DBVarChar(EnumSystemID.ERPAP)
            };

            Entity_BaseAP.BasicInfo entityBasicInfo = _entity.SelectBasicInfo(basicInfoPara);
            return entityBasicInfo;
        }

        private static void _SetJobParameter(Flow flow, Job job)
        {
            _ediServiceDistributorTimeOut = 5000;

            if (string.IsNullOrWhiteSpace(job.parameters[EnumJobParameter.TimeOut.ToString()].value))
            {
                throw new EDIException(EnumEDIMessage.JobParameterIsNull, new[] { Job.GetID(flow, job), EnumJobParameter.TimeOut.ToString() });
            }

            _ediServiceDistributorTimeOut = int.Parse(job.parameters[EnumJobParameter.TimeOut.ToString()].value);

            _errLogPath = null;

            if (string.IsNullOrWhiteSpace(job.parameters[EnumJobParameter.ErrLogPath.ToString()].value))
            {
                throw new EDIException(EnumEDIMessage.JobParameterIsNull, new[] { Job.GetID(flow, job), EnumJobParameter.ErrLogPath.ToString() });
            }

            _errLogPath = job.parameters[EnumJobParameter.ErrLogPath.ToString()].value;
        }
    }
}
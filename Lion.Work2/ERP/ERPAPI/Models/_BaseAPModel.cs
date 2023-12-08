using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAPI.Models
{
    public class _BaseAPModel : _BaseModel
    {
        public _BaseAPModel()
        {
            _entity = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP);
            _mongoEntity = new Mongo_BaseAP(ConnectionStringMSERP, ProviderNameMSERP);
        }

        private readonly Entity_BaseAP _entity;
        private readonly Mongo_BaseAP _mongoEntity;

        internal override void OnException(Exception exception)
        {
            base.OnException(exception);

            try
            {
                var serpMailMessages =
                    new SERPMailMessages
                    {
                        MialAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SysSDMail.ToString()],
                        SmtpClientIPAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SmtpClientIPAddress.ToString()],
                        AppName = Common.GetEnumDesc(EnumAPISystemID.ERPAP),
                        Ex = exception
                    };

                PublicFun.SendErrorMailForSERP(serpMailMessages);

                string lineID = ConfigurationManager.AppSettings[EnumAppSettingKey.LineID.ToString()];
                if (string.IsNullOrWhiteSpace(lineID) == false)
                {
                    var to = ConfigurationManager.AppSettings[EnumAppSettingKey.LineTo.ToString()].Split(';');
                    PublicFun.SendErrorLineForSERP(Common.GetEnumDesc(EnumAPISystemID.ERPAP), lineID, to, exception);
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
        }

        #region - Log -

        internal virtual void RecordLog(Mongo_BaseAP.EnumLogDocName docName, Mongo_BaseAP.EnumModifyType modifyType, string apiNo, string updUserID, string execSysID, string ipAddress, Mongo_BaseAP.RecordLogPara source)
        {
            try
            {
                var mongoPara = _GetRecordLogPara(docName, modifyType, apiNo, updUserID, execSysID, ipAddress);
                _mongoEntity.RecordLog(docName, mongoPara, source);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        internal virtual void RecordLog(Mongo_BaseAP.EnumLogDocName docName, Mongo_BaseAP.EnumModifyType modifyType, string apiNo, string updUserID, string execSysID, string ipAddress, IEnumerable<Mongo_BaseAP.RecordLogPara> source)
        {
            try
            {
                var mongoPara = _GetRecordLogPara(docName, modifyType, apiNo, updUserID, execSysID, ipAddress);
                _mongoEntity.RecordLog(docName, mongoPara, source.ToList());
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        private Mongo_BaseAP.RecordLogPara _GetRecordLogPara(Mongo_BaseAP.EnumLogDocName docName, Mongo_BaseAP.EnumModifyType modifyType, string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            string cultureID = LionTech.Entity.ERP.EnumCultureID.zh_TW.ToString();

            Mongo_BaseAP.RecordLogPara recordPara = (Mongo_BaseAP.RecordLogPara)Activator.CreateInstance(_mongoEntity.GetRecordLogParaType(docName));

            Entity_BaseAP.CMCodePara codePara =
                new Entity_BaseAP.CMCodePara
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID)
                };

            List<Entity_BaseAP.CMCode> modifyTypeList = _entity.SelectCMCodeList(codePara);

            if (modifyTypeList != null &&
                modifyTypeList.Any())
            {
                recordPara.ModifyTypeNM = modifyTypeList.Find(e => e.CodeID.GetValue() == modifyType.ToString()).CodeNM;
            }
            else
            {
                recordPara.ModifyTypeNM = new DBNVarChar(null);
            }

            List<DBVarChar> methodFullNameList = new List<DBVarChar>();
            for (int methodIndex = 2; methodIndex < 10; methodIndex++)
            {
                MethodBase methodBase = Common.GetMethodBase(methodIndex);

                if (methodBase != null)
                {
                    var methodFullName = string.Concat(methodBase.ReflectedType.FullName, ".", methodBase.Name);
                    methodFullNameList.Add(new DBVarChar(methodFullName));
                    if (methodFullName.StartsWith(string.Format("{0}I.Controllers", EnumAPISystemID.ERPAP)))
                    {
                        break;
                    }
                }
            }

            LionTech.Entity.ERP.Entity_BaseAP.BasicInfoPara basicInfoPara = new LionTech.Entity.ERP.Entity_BaseAP.BasicInfoPara(cultureID)
            {
                UserID = new DBVarChar(null),
                UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(execSysID) ? null : execSysID))
            };

            LionTech.Entity.ERP.Entity_BaseAP.BasicInfo entityBasicInfo = _entity.SelectBasicInfo(basicInfoPara);

            recordPara.ModifyType = new DBChar(modifyType.ToString());
            recordPara.APINo = new DBChar(apiNo);
            recordPara.UpdUserID = entityBasicInfo.UpdUserID;
            recordPara.UpdUserNM = entityBasicInfo.UpdUserNM;
            recordPara.UpdDT = new DBDateTime(DateTime.Now);
            recordPara.ExecSysID = entityBasicInfo.ExecSysID;
            recordPara.ExecSysNM = entityBasicInfo.ExecSysNM;
            recordPara.ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress));
            recordPara.MethodFullNameList = methodFullNameList;
            return recordPara;
        }

        public virtual bool GetERPRecordUserFunctionResult(string userID, string apiNo, string updUserID, string sysID, string ipAddress)
        {
            try
            {
                string cultureID = LionTech.Entity.ERP.EnumCultureID.zh_TW.ToString();

                LionTech.Entity.ERP.Entity_BaseAP.UserFunctionPara para = new LionTech.Entity.ERP.Entity_BaseAP.UserFunctionPara(cultureID)
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                List<LionTech.Entity.ERP.Entity_BaseAP.UserFunction> userFunctionList = new LionTech.Entity.ERP.Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserFunctionList(para);

                List<LionTech.Entity.ERP.Mongo_BaseAP.RecordUserFunctionPara> mongoParaList = new List<LionTech.Entity.ERP.Mongo_BaseAP.RecordUserFunctionPara>();

                if (userFunctionList != null)
                {
                    LionTech.Entity.ERP.Entity_BaseAP.BasicInfoPara basicInfoPara = new LionTech.Entity.ERP.Entity_BaseAP.BasicInfoPara(cultureID)
                    {
                        UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                        UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                        ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                    };

                    LionTech.Entity.ERP.Entity_BaseAP.BasicInfo entityBasicInfo = new LionTech.Entity.ERP.Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectBasicInfo(basicInfoPara);

                    if (userFunctionList.Count == 0)
                    {
                        mongoParaList.Add(new LionTech.Entity.ERP.Mongo_BaseAP.RecordUserFunctionPara
                        {
                            UserID = entityBasicInfo.UserID,
                            UserNM = entityBasicInfo.UserNM,
                            SysID = new DBVarChar(null),
                            SysNM = new DBNVarChar(null),
                            FunControllerID = new DBVarChar(null),
                            FunGroupNM = new DBNVarChar(null),
                            FunActionNM = new DBVarChar(null),
                            FunNM = new DBNVarChar(null)
                        });
                    }
                    else
                    {
                        foreach (LionTech.Entity.ERP.Entity_BaseAP.UserFunction userFunction in userFunctionList)
                        {
                            mongoParaList.Add(new LionTech.Entity.ERP.Mongo_BaseAP.RecordUserFunctionPara
                            {
                                UserID = entityBasicInfo.UserID,
                                UserNM = entityBasicInfo.UserNM,
                                SysID = userFunction.SysID,
                                SysNM = userFunction.SysNM,
                                FunControllerID = userFunction.FunControllerID,
                                FunGroupNM = userFunction.FunGroupNM,
                                FunActionNM = userFunction.FunActionNM,
                                FunNM = userFunction.FunNM
                            });
                        }
                    }
                }

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_FUN, Mongo_BaseAP.EnumModifyType.U, apiNo, updUserID, sysID, ipAddress, mongoParaList);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public virtual bool GetERPRecordUserAccessResult(string userID, string apiNo, string updUserID, string sysID, string restrictType, string isLock, string isDisable, string ipAddress)
        {
            try
            {
                string cultureID = LionTech.Entity.ERP.EnumCultureID.zh_TW.ToString();

                LionTech.Entity.ERP.Entity_BaseAP.BasicInfoPara para = new LionTech.Entity.ERP.Entity_BaseAP.BasicInfoPara(cultureID)
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                    UpdUserID = new DBVarChar(updUserID),
                    ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                LionTech.Entity.ERP.Entity_BaseAP.BasicInfo entityBasicInfo = new LionTech.Entity.ERP.Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectBasicInfo(para);

                LionTech.Entity.ERP.Entity_BaseAP.CMCodePara codePara = new LionTech.Entity.ERP.Entity_BaseAP.CMCodePara
                {
                    ItemTextType = LionTech.Entity.ERP.Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.RestrictType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID)
                };

                List<LionTech.Entity.ERP.Entity_BaseAP.CMCode> restrictTypeList = new LionTech.Entity.ERP.Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectCMCodeList(codePara);

                LionTech.Entity.ERP.Mongo_BaseAP.RecordUserAccessPara recordPara = new LionTech.Entity.ERP.Mongo_BaseAP.RecordUserAccessPara
                {
                    UserID = entityBasicInfo.UserID,
                    UserNM = entityBasicInfo.UserNM,
                    RestrictType = new DBChar((string.IsNullOrWhiteSpace(restrictType) ? null : restrictType)),
                    RestrictTypeNM = new DBNVarChar(null),
                    IsLock = new DBChar((string.IsNullOrWhiteSpace(isLock) ? null : isLock)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(isDisable) ? null : isDisable)),
                    APINo = new DBChar(apiNo),
                    UpdUserID = entityBasicInfo.UpdUserID,
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = entityBasicInfo.ExecSysID,
                    ExecSysNM = entityBasicInfo.ExecSysNM,
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (restrictTypeList != null && restrictTypeList.Count > 0 &&
                    !string.IsNullOrWhiteSpace(restrictType))
                {
                    recordPara.RestrictTypeNM = (restrictTypeList.Find(e => e.CodeID.GetValue() == restrictType)).CodeNM;
                }

                if (new LionTech.Entity.ERP.Mongo_BaseAP(this.ConnectionStringMSERP, this.ProviderNameMSERP)
                    .RecordUserAccess(recordPara) == LionTech.Entity.ERP.Mongo_BaseAP.EnumRecordUserAccessResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public virtual bool GetB2PRecordUserSystemRoleResult(string userID, string updUserID, string sysID, string ipAddress)
        {
            try
            {
                LionTech.Entity.B2P.Entity_BaseAP.RecordUserSystemRolePara para = new LionTech.Entity.B2P.Entity_BaseAP.RecordUserSystemRolePara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID)),
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                };

                if (new LionTech.Entity.B2P.Entity_BaseAP(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .RecordUserSystemRole(para) == LionTech.Entity.B2P.Entity_BaseAP.EnumRecordUserSystemRoleResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Utility;

namespace B2PAP.Models.Sys
{
    public class SystemEDIFlowModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID
        }

        [Required]
        public string QuerySysID { get; set; }
        public string QueryEDIFlowID { get; set; }

        public SystemEDIFlowModel()
        {

        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QueryEDIFlowID = string.Empty;
        }

        List<EntitySystemEDIFlow.SystemEDIFlow> _entitySystemEDIFlowList;
        public List<EntitySystemEDIFlow.SystemEDIFlow> EntitySystemEDIFlowList { get { return _entitySystemEDIFlowList; } }

        public bool GetSystemEDIFlowList( EnumCultureID cultureID)
        {
            try
            {
                EntitySystemEDIFlow.SystemEDIFlowPara para = new EntitySystemEDIFlow.SystemEDIFlowPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID))
                };

                _entitySystemEDIFlowList = new EntitySystemEDIFlow(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemEDIFlowList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        //Output XML
        List<Entity_BaseAP.SystemEDIFlowDetail> _entitySystemEDIFlowDetail;
        public List<Entity_BaseAP.SystemEDIFlowDetail> EntitySystemEDIFlowDetail { get { return _entitySystemEDIFlowDetail; } }

        List<Entity_BaseAP.SystemEDIJobDetail> _entitySystemEDIJobDetail;
        public List<Entity_BaseAP.SystemEDIJobDetail> EntitySystemEDIJobDetail { get { return _entitySystemEDIJobDetail; } }

        List<Entity_BaseAP.SystemEDIConnectionDetail> _entitySystemEDIConDetail;
        public List<Entity_BaseAP.SystemEDIConnectionDetail> EntitySystemEDIConDetail { get { return _entitySystemEDIConDetail; } }

        List<Entity_BaseAP.SystemEDIParaDetail> _entitySystemEDIParDetail;
        public List<Entity_BaseAP.SystemEDIParaDetail> EntitySystemEDIParDetail { get { return _entitySystemEDIParDetail; } }

        List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> _entitySystemEDIFlowExecuteTimeDetail;
        public List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> EntitySystemEDIFlowExecuteTimeDetail { get { return _entitySystemEDIFlowExecuteTimeDetail; } }
        
        string FileDataPath;

        public string _FileDataPath { get { return FileDataPath; } }
        public bool SaveEDIXML(string Sys_ID, EnumCultureID cultureID, string UserID)
        {
            try
            {
                EntitySystemEDIFlowOutputXML.SystemEDIXMLPara para = new EntitySystemEDIFlowOutputXML.SystemEDIXMLPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(Sys_ID) ? null : Sys_ID)),

                };

                EntitySys.SysSystemSysIDPara Pathpara = new EntitySys.SysSystemSysIDPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(Sys_ID) ? null : Sys_ID)),

                };
                _entitySystemEDIFlowDetail = new EntitySystemEDIFlowOutputXML(this.ConnectionStringB2P, this.ProviderNameB2P).SysEDIFlowList(para);
                _entitySystemEDIJobDetail = new EntitySystemEDIFlowOutputXML(this.ConnectionStringB2P, this.ProviderNameB2P).SysEDIJobList(para);
                _entitySystemEDIConDetail = new EntitySystemEDIFlowOutputXML(this.ConnectionStringB2P, this.ProviderNameB2P).SysEDIConnectionList(para);
                _entitySystemEDIParDetail = new EntitySystemEDIFlowOutputXML(this.ConnectionStringB2P, this.ProviderNameB2P).SysEDIParaList(para);
                _entitySystemEDIFlowExecuteTimeDetail = new EntitySystemEDIFlowOutputXML(this.ConnectionStringB2P, this.ProviderNameB2P).SysEDIFlowFixedTimeList(para);
                if (_entitySystemEDIFlowDetail != null && _entitySystemEDIJobDetail != null && _entitySystemEDIConDetail != null)
                {
                    FileDataPath = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P).GetFileDataPath(Pathpara);
                    //FileDataPath = "E:\";
                    string FullfilePath =
                        Path.Combine(
                        new string[]
                        {
                            FileDataPath,                                                  
                            EnumFilePathKeyWord.FileData.ToString(),
                            Common.GetEnumDesc(Utility.GetEnumEDISystemID(Sys_ID)),
                            Common.GetEnumDesc(Utility.GetEnumEDISystemID(Sys_ID))+"."+UserID+"."+  Common.GetDateTimeString()+
                            Common.GetEnumDesc(EntitySystemEDIFlowOutputXML.EnumEDIXMLPathFile.xml)
                        });

                    if (Utility.GenerateEDIXML(_entitySystemEDIFlowDetail,
                        _entitySystemEDIJobDetail, _entitySystemEDIConDetail, _entitySystemEDIParDetail, _entitySystemEDIFlowExecuteTimeDetail
                        , FullfilePath) == Entity_BaseAP.EnumGenerateEDIXMLResult.Success)
                    {
                        return true;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetEDIFlowSettingResult(string userID, EnumCultureID cultureID, List<EntitySystemEDIFlow.EDIFlowValue> EDIFlowValueList)
        {
            try
            {
                EntitySystemEDIFlow.SystemEDIFlowPara para = new EntitySystemEDIFlow.SystemEDIFlowPara(cultureID.ToString())
                {
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID))
                };

                if (new EntitySystemEDIFlow(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditEDIFlowSetting(para, EDIFlowValueList) == EntitySystemEDIFlow.EnumEDIFlowSettingResult.Success)
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
    }
}
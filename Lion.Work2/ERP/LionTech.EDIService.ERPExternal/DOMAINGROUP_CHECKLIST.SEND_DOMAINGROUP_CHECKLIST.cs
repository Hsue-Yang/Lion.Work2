using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;
using LionTech.Utility;

namespace LionTech.EDIService.ERPExternal
{
    public class DOMAINGROUP_CHECKLIST
    {
        private enum EnumMsgUrl
        {
            [Description("{0}/Sys/DomainGroupUserProxy?DomainPath={1}&DomainGroupNM={2}")]
            DomainGroupUser
        }

        private enum EnumJobParamterKey
        {
            MessageContent
        }

        private static string MessageContent;

        public static EnumJobResult SEND_DOMAINGROUP_CHECKLIST(Flow flow, Job job)
        {
            Connection connection = flow.connections[job.connectionID];
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            try
            {
                if (job.parameters != null)
                {
                    List<Parameter> parameters = job.parameters.Where(w => w.id != EnumJobParamterKey.MessageContent.ToString()).ToList();
                    MessageContent = job.parameters[EnumJobParamterKey.MessageContent.ToString()].value;
                    if (parameters.Any(recipient => SendErpMsg(recipient, connection) == EntityERPExternal.EnumInsertErpMessageResult.Failure))
                    {
                        return EnumJobResult.Failure;
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

        private static EntityERPExternal.EnumInsertErpMessageResult SendErpMsg(Parameter parameter, Connection connection)
        {
            string[] paramters = parameter.value.Split('|');
            string domain = paramters[0];
            string ou = paramters[1];
            string targetUserID = paramters[2];

            string url = string.Format(Common.GetEnumDesc(EnumMsgUrl.DomainGroupUser),
                Common.GetEnumDesc(EnumSystemID.ERPAP),
                domain,
                Security.Encrypt(ou));

            EntityERPExternal.InsertErpMessagePara para = new EntityERPExternal.InsertErpMessagePara
            {
                MsgStfn = new DBVarChar(targetUserID),
                MsgMessage = new DBNVarChar(MessageContent),
                MsgMstfn = new DBVarChar(null),
                MsgUrl = new DBVarChar(url)
            };

            return new EntityERPExternal(connection.value, connection.providerName)
                .InsertErpMessageResult(para);
        }
    }
}

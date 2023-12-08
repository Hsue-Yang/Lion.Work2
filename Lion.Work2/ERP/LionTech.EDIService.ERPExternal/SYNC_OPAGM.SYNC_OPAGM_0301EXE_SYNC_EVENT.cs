using LionTech.EDI;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;
using System;

namespace LionTech.EDIService.ERPExternal
{
    public partial class SYNC_OPAGM
    {
        public static EnumJobResult SYNC_OPAGM_0301EXE_LEFT_USER(Flow flow, Job job)
        {
            EnumJobResult jobResult = EnumJobResult.Failure;
            Connection connection = flow.connections[job.connectionID];
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            try
            {
                EntityERPExternal entity = new EntityERPExternal(connection.value, connection.providerName);

                if (entity.UpdateLeftUser() == EntityERPExternal.EnumUpdateLeftUserResult.Success)
                {
                    jobResult = EnumJobResult.Success;
                }
            }
            catch (Exception ex)
            {
                FileLog.Write(exceptionPath, ex);
            }

            return jobResult;
        }
    }
}

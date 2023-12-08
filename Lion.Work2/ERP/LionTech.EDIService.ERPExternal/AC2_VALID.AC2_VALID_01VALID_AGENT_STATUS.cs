using LionTech.EDI;
using LionTech.Entity.ERP.EDIService;

namespace LionTech.EDIService.ERPExternal
{
    public partial class AC2_VALID
    {
        public static EnumJobResult AC2_VALID_01VALID_AGENT_STATUS(Flow flow, Job job)
        {
            Connection connection = flow.connections[job.connectionID];
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            bool isRuning = new EntityERPExternal(connection.value, connection.providerName).SelectSQLAgentStatus();

            if (isRuning)
            {
                return EnumJobResult.Success;
            }

            return EnumJobResult.Failure;
        }
    }
}
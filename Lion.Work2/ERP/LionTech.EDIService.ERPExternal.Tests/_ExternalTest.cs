using LionTech.EDI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.EDIService.ERPExternal.Tests
{
    public abstract class _ExternalTest
    {
        private enum EnumConnectionStringsKey
        {
            LionGroupSERP,
            LionGroupMSERP
        }
        public _ExternalTest()
        {
        }

        protected Flow GetFlow(string flowID)
        {
            var flows = Flows.Load(@"E:\azdevops\Git\LionTech\ERP\LionTech.EDIService.ERPExternal.Tests\LionTech.EDIService.ERP.exe.xml");
            var flow = (from f in flows
                        where f.id == flowID
                        select f).SingleOrDefault();

            flow?.connections.ForEach(connet =>
            {
                if (connet.id == EnumConnectionStringsKey.LionGroupSERP.ToString())
                {
                    //connet.value = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupSERP.ToString()].ConnectionString;
                }
                else if (connet.id == EnumConnectionStringsKey.LionGroupMSERP.ToString())
                {
                    //connet.value = ConfigurationManager.ConnectionStrings[EnumConnectionStringsKey.LionGroupMSERP.ToString()].ConnectionString;
                }
            });

            return flow;
        }

    }
}

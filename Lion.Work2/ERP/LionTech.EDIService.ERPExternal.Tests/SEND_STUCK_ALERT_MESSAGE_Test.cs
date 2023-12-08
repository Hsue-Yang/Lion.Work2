using LionTech.EDI;
using NUnit.Framework;

namespace LionTech.EDIService.ERPExternal.Tests
{
    internal class SEND_STUCK_ALERT_MESSAGE_Test : _ExternalTest
    {
        [Test]
        public void SendStuckAlertMessageTest()
        {
            //arrange
            Flow flow = GetFlow("EDI_CHECK");
            Job job = flow.jobs["SEND_STUCK_ALERT_MESSAGE"];
            //act
            EDI_CHECK.SEND_STUCK_ALERT_MESSAGE(flow, job);
        }
    }
}

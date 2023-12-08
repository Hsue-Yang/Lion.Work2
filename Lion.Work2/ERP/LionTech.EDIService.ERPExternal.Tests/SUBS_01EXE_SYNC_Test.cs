using LionTech.EDI;
using NUnit.Framework;

namespace LionTech.EDIService.ERPExternal.Tests
{
    internal class SUBS_01EXE_SYNC_Test : _ExternalTest
    {
        [Test]
        public void subEventFailSendMessage()
        {
            //arrange
            Flow flow = GetFlow("SUBS");
            Job job = flow.jobs["SUBS_01EXE_SYNC"];
            flow.ediNo = "201305160002";
            //act
            SUBS.SUBS_01EXE_SYNC(flow, job);

            //assert
        }
    }
}
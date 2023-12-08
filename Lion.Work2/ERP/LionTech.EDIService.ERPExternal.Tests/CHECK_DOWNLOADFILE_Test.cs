using LionTech.EDI;
using NUnit.Framework;

namespace LionTech.EDIService.ERPExternal.Tests
{
    internal class CHECK_DOWNLOADFILE_Test : _ExternalTest
    {
        [Test]
        public void checkAndDownloadFile()
        {
            //arrange
            Flow flow = GetFlow("DOTTED_SIGN");
            Job job = flow.jobs["CHECK_AND_DOWNLOAD_PDF"];
            flow.ediNo = "201305160002";
            //act
            DOTTED_SIGN.CHECK_AND_DOWNLOAD_PDF(flow, job);

            //assert
        }
    }
}
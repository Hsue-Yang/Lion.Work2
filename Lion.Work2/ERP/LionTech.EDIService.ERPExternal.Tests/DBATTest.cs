using LionTech.EDI;
using NUnit.Framework;

namespace LionTech.EDIService.ERPExternal.Tests
{
    public class DBATTest : _ExternalTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //arrange
            Flow flow = GetFlow("DBAT");
            Job job = flow.jobs["DBAT_0102EXE_REBUILD_MENU"];

            //act
            EnumJobResult actual = DBAT.DBAT_0102EXE_REBUILD_MENU(flow, job);

            //assert
        }
    }
}
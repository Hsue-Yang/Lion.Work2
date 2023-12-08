using LionTech.Entity.ERP;
using LionTech.Utility;
using System.Web;

namespace TRAININGAPI.Controllers
{
    public class _BaseAPController : _BaseController
    {
        protected override void OnActionExecutingSetAuthState()
        {

        }

        public enum EnumEDIServiceEventGroupID
        {

        }

        public enum EnumEDIServiceEventID
        {

        }

        protected string ExecEDIServiceDistributor(EnumEDIServiceEventGroupID eventGroup, EnumEDIServiceEventID eventID, string eventParaJsonString, string serviceUserID)
        {
            int ediServiceDistributorTimeOut = base.GetEDIServiceDistributorTimeOut();

            string ediServiceDistributorPath = base.GetEDIServiceDistributorPath(
                EnumSystemID.TRAININGAP.ToString(), eventGroup.ToString(), eventID.ToString(),
                serviceUserID, HttpUtility.UrlEncode(eventParaJsonString));

            string responseString = Common.HttpWebRequestGetResponseString(ediServiceDistributorPath, ediServiceDistributorTimeOut);
            if (string.IsNullOrWhiteSpace(responseString) == false)
            {
                return responseString;
            }

            return null;
        }
    }
}
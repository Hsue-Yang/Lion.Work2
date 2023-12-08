// 新增日期：2016-12-15
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

namespace ERPAPI.Models.LineBotService
{
    public class GetProfileModel : LineBotServiceModel
    {
        #region - Definitions -
        public class APIParaData
        {
            public string SysID { get; set; }
            public string LineID { get; set; }
            public string ReceiverID { get; set; }
        }
        #endregion
        
        #region - API Property -
        //[AllowHtml]
        public string APIPara { get; set; }

        public APIParaData APIData { get; set; }
        #endregion
    }
}
// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using LionTech.Entity.ERP.LoginEventService;

namespace ERPAPI.Models.LoginEventService
{
    public class LoginEventServiceModel : _BaseAPModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            Property1,
            Property2,
            Property3
        }
        #endregion

        #region - Constructor -
        public LoginEventServiceModel()
        {
            _entity = new EntityLoginEventService(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string Property1 { set; get; }
        public string Property2 { set; get; }
        public bool Property3 { set; get; }
        #endregion

        #region - Private -
        private readonly EntityLoginEventService _entity;
        #endregion

        #region - Validation -
        #endregion
    }
}
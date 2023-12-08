using LionTech.Entity.ERP.LionGroupAppService;

namespace ERPAPI.Models.LionGroupAppService
{
    public class QRCodeLoginAuthorizationModel : LionGroupAppServiceModel
    {
        #region - Constructor -
        public QRCodeLoginAuthorizationModel()
        {
            _entity = new EntityQRCodeLoginAuthorization(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string PingCode { get; set; }
        #endregion

        #region - Private -
        private readonly EntityQRCodeLoginAuthorization _entity;
        #endregion
    }
}
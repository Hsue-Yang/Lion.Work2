using System;
using LionTech.Entity;
using LionTech.Entity.ERP.LineBotService;

namespace ERPAPI.Models.LineBotService
{
    public class LineBotServiceModel : _BaseAPModel
    {
        #region - Constructor -
        public LineBotServiceModel()
        {
            _entity = new EntityLineBotService(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        public EntityLineBotService.LineClinet LineClinet { get; private set; }
        #endregion

        #region - Private -
        private readonly EntityLineBotService _entity;
        #endregion

        #region - 取得Line用戶端 -
        /// <summary>
        /// 取得Line用戶端
        /// </summary>
        /// <returns></returns>
        public bool GetLineClinet(string sysID, string lineID)
        {
            try
            {
                LineClinet =
                    _entity.SelectLineClinet(
                        new EntityLineBotService.LineClinetPara
                        {
                            SysID = new DBVarChar(sysID),
                            LineID = new DBVarChar(lineID)
                        });

                return LineClinet != null;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion
    }
}
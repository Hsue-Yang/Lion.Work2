using System;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Home;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Home
{
    public class QRCodeLoginModel : HomeModel, IUserAccountInfo
    {
        public new enum EnumCookieKey
        {
            LoginType
        }
        public string LoginType { get; set; }
        public string UserLocation { get; set; }

        [StringLength(20)]
        [InputType(EnumInputType.TextBox)]
        public string LocationDesc { get; set; }

        private string _IdNo;

        [StringLength(20)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string IdNo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_IdNo))
                {
                    return _IdNo;
                }
                return _IdNo.ToUpper();
            }
            set
            {
                _IdNo = value;
            }
        }

        [StringLength(8)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string Birthday { get; set; }
        
        public string PingCode { get; set; }

        public string CultureID { get; set; }

        public string UserID { get; set; }

        public string UserPassword { get; set; }
        
        public bool ValidateIP(string ip)
        {
            try
            {
                EntityHome.SystemIPPara para = new EntityHome.SystemIPPara
                {
                    SystemID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
                    IPAddress = new DBVarChar(ip)
                };

                DBInt ipAddressCount = new EntitySSOLogin(ConnectionStringSERP, ProviderNameSERP)
                    .SelectSystemIP(para);

                if (ipAddressCount.GetValue() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}
namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class UserConnect
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
        public string LastConnectDT { get; set; }
        public string CustLogout { get; set; }
        public string IPAddress { get; set; }
    }
}

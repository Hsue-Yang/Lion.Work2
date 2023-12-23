using LionTech.Entity.ERP;
namespace LionTech.Utility.TRAINING
{
    public static class API
    {
        public static class Leave
        {
            public static string GetLeave(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/Leave?ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string LeaveMenu(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/LeaveMenu?ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string LeaveMenuChild(string DetailMenuPara, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/LeaveMenuChild?DetailMenuPara={DetailMenuPara}&ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string GetLeaveDetail(int DetailPara, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/LeaveDetail?DetailPara={DetailPara}&ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string EditLeave(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/LeaveEdit?ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string DeleteLeave(int DetailPara, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/LeaveDelete?DetailPara={DetailPara}&ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
        }
    }
}
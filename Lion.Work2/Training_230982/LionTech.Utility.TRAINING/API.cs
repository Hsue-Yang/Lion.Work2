using LionTech.Entity.ERP;

namespace LionTech.Utility.TRAINING
{
    public static class API
    {
        public static class PracticeLeave
        {
            public static string GetLeaveList() => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/GetLeaveDataList?ClientSysID={EnumSystemID.TRAININGAP}";
            public static string GetPpm95List() => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/GetPpm95List?ClientSysID={EnumSystemID.TRAININGAP}";
            public static string GetPpd95List(string ppm95_id) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/GetPpd95List?ppm95_id={ppm95_id}&ClientSysID={EnumSystemID.TRAININGAP}";
            public static string EditLeaveData() => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/EditLeaveData?ClientSysID={EnumSystemID.TRAININGAP}";
            public static string DeleteLeaveData(int ppm96_id) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/DeleteLeaveData?ppm96_id={ppm96_id}&ClientSysID={EnumSystemID.TRAININGAP}";
            public static string GetLeaveData(int ppm96_id) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/GetLeaveDetail?ppm96_id={ppm96_id}&ClientSysID={EnumSystemID.TRAININGAP}";
        }
    }
}
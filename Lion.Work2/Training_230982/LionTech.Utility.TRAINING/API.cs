using LionTech.Entity.ERP;
using LionTech.Entity.TRAINING.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.Utility.TRAINING
{
    public static class API
    {
        public static class PracticeLeave
        {
            public static string GetLeaveList(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/GetLeaveDataList?ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string GetPpm95List(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/GetPpm95List?ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string GetPpd95List(string userID, string ppm95_id) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/GetPpd95List?ppm95_id={ppm95_id}&ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string SubmitFormData(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/EditLeaveData?ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
            public static string GetLeaveData(string userID, int ppm96_id) => $"{Common.GetEnumDesc(EnumAPISystemID.TRAININGAP)}/Leave/GetLeaveDetail?ppm96_id={ppm96_id}&ClientSysID={EnumSystemID.TRAININGAP}&ClientUserID={userID}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TRAININGAP.Models.Leave
{
    public class PracticeLeaveModel : PracticeModel
    {
        public PracticeLeaveModel() { }

        public List<Prapsppd95> Prapsppd95List { get; set; } //(假別子項目)
        public List<Prapsppm96> Prapsppm96List { get; set; } //(假單)
        public List<Prapsppm95> Prapsppm95List { get; set; } //(假別)

        #region
        public class Prapsppm96 //(假單)
        {
            public int ppm96_id { get; set; } //假單流水號
            public string ppm96_stfn { get; set; } //員編
            public DateTime ppm96_begin { get; set; } //開始時間
            public DateTime ppm96_end { get; set; } //結束時間
            public string ppm95_id { get; set; } //假別
            public string ppd95_id { get; set; } //假別子項目ID
            public string ppm96_sign { get; set; } //送審主管
        }
        public class Prapsppm95 //(假別)
        {
            public string ppm95_id { get; set; } //假別ID
            public string ppm95_name { get; set; } //假別名稱
        }
        public class Prapsppd95 //(假別子項目)
        {
            public string ppd95_id { get; set; } //假別子項目
            public string ppm95_id { get; set; } //假別ID
            public string ppd95_name { get; set; } //假別子項目名稱
        }
        #endregion

        public async Task<bool> InsertLeave(string stfn)
        {
            return true;
        }
    }
}
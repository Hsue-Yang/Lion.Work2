using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.Entity.TRAINING.Leave
{
    public class EntitiyLeave : Entity_Base
    {
        public EntitiyLeave(string connectionString, string providerName) : base(connectionString, providerName)
        {
        }

        public class LeavePara : DBCulture
        {
            public LeavePara(string cultureID) : base(cultureID)
            {
            }
            public enum ParaField //查詢語句的參數
            {
                ppm96_stfn, ppm96_begin, ppm96_end, ppm95_id, ppd95_id, ppm96_sign, ppm95_name, ppd95_name
            }
            public DBVarChar ppm96_stfn { get; set; }
            public DBDateTime ppm96_begin { get; set; }
            public DBDateTime ppm96_end { get; set; }
            public DBNVarChar ppm95_id { get; set; }
            public DBNVarChar ppd95_id { get; set; }
            public DBNVarChar ppm96_sign { get; set; }

            public DBNVarChar ppm95_name { get; set; }
            public DBNVarChar ppd95_name { get; set; }
        }
        public class Leave : DBTableRow //查詢結果及對應的值
        {
            public enum DataField
            {

            }
            //public DBVarChar
        }
    }
}

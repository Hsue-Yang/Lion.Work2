using System.Collections.Generic;
using System.Data;
using static LionTech.Entity.TRAINING.Leave.EntityLeaveDetail;
using System.Linq;

namespace LionTech.Entity.TRAINING.Leave
{
    public class EntityLeaveList : EntityLeave
    {
        public EntityLeaveList(string connectionString, string providerName) : base(connectionString, providerName) { }

        public class LeavePara
        {
            public LeavePara() { }

            public enum ParaField
            {
                ppm95_id
            }

            public DBVarChar ppm95_id;
        }

        public class Leave : DBTableRow
        {
            public enum DataField
            {
                ppm96_id, ppm96_stfn, ppm96_begin, ppm96_end, ppm95_name, ppd95_name, ppm96_sign
            }

            public DBInt ppm96_id;
            public DBVarChar ppm96_stfn;
            public DBDateTime ppm96_begin;
            public DBDateTime ppm96_end;
            public DBNVarChar ppm95_name;
            public DBNVarChar ppd95_name;
            public DBInt ppm96_sign;
        }
        public class Ppd95 : DBTableRow
        {
            public enum DataField
            {
                ppd95_id, ppd95_name
            }

            public DBVarChar ppd95_id;
            public DBNVarChar ppd95_name;
        }
        public class Ppm95 : DBTableRow
        {
            public enum DataField
            {
                ppm95_id, ppm95_name
            }

            public DBVarChar ppm95_id;
            public DBNVarChar ppm95_name;
        }

        public List<Leave> SelectLeaveList()
        {
            string commandText = string.Concat(new object[]
            {
             "SELECT ppm96_id",
             "     , ppm96_stfn",
             "     , ppm96_begin",
             "     , ppm96_end",
             "     , m95.ppm95_name",
             "     , d95.ppd95_name",
             "     , ppm96_sign ",
             "  FROM prapsppm96 m96 ",
             " INNER JOIN prapsppm95 m95 ON m95.ppm95_id = m96.ppm95_id ",
             " LEFT JOIN prapsppd95 d95 ON d95.ppd95_id = m96.ppd95_id"
              });

            return GetEntityList<Leave>(commandText, null);
        }
        public List<Ppm95> SelectPpmList()
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT ppm95_id",
                "     , ppm95_name ",
                "  FROM prapsppm95"
            });

            return GetEntityList<Ppm95>(commandText, null);
        }
        public List<Ppd95> SelectPpdList(LeavePara para)
        {
            List<DBParameter> dBParameters = new List<DBParameter>();
            string commandText = string.Concat(new object[]
            {
               "SELECT ppd95_id",
               "     , ppd95_name ",
               "  FROM prapsppd95 d95 ",
               " INNER JOIN prapsppm95 m95 ON d95.ppm95_id = m95.ppm95_id ",
               " WHERE m95.ppm95_id = {ppm95_id}"
            });

            dBParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm95_id.ToString(), Value = para.ppm95_id });
            return GetEntityList<Ppd95>(commandText, dBParameters);
        }
    }
}
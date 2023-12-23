using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ppm96_stfn, ppm96_begin, ppm96_end, ppm95_name, ppd95_name, ppm96_sign
            }
            public DBVarChar ppm96_stfn;
            public DBChar ppm96_begin;
            public DBChar ppm96_end;
            public DBNVarChar ppm95_name;
            public DBNVarChar ppd95_name;
            public DBVarChar pm96_sign;
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
             "SELECT ppm96_stfn" +
             "      ,ppm96_begin" +
             "      ,ppm96_end" +
             "      ,m95.ppm95_name" +
             "      ,d95.ppd95_name" +
             "      ,ppm96_sign " +
             "FROM prapsppm96 m96 " +
             "INNER JOIN prapsppm95 m95 ON m95.ppm95_id = m96.ppm95_id " +
             "LEFT JOIN prapsppd95 d95 ON d95.ppd95_id = m96.ppd95_id"
              });
            DataTable dataTable = base.GetDataTable(commandText, null);
            if (dataTable.Rows.Count > 0)
            {
                List<Leave> leaveList = new List<Leave>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Leave leave = new Leave()
                    {
                        ppm96_stfn = new DBVarChar(dataRow[Leave.DataField.ppm96_stfn.ToString()]),
                        ppm96_begin = new DBChar(dataRow[Leave.DataField.ppm96_begin.ToString()]),
                        ppm96_end = new DBChar(dataRow[Leave.DataField.ppm96_end.ToString()]),
                        ppd95_name = new DBNVarChar(dataRow[Leave.DataField.ppd95_name.ToString()]),
                        ppm95_name = new DBNVarChar(dataRow[Leave.DataField.ppm95_name.ToString()]),
                        pm96_sign = new DBVarChar(dataRow[Leave.DataField.ppm96_sign.ToString()]),
                    };
                    leaveList.Add(leave);

                }
                return leaveList;
            }

            return null;
        }
        public List<Ppm95> SelectPpmList()
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT  ppm95_id"+
                "       ,ppm95_name "+
                "FROM prapsppm95"
            });
            DataTable dataTable = base.GetDataTable(commandText, null);
            if (dataTable.Rows.Count > 0)
            {
                List<Ppm95> ppmList = new List<Ppm95>();
                foreach (DataRow datarow in dataTable.Rows)
                {
                    Ppm95 leave = new Ppm95()
                    {
                        ppm95_id = new DBVarChar(datarow[Ppm95.DataField.ppm95_id.ToString()]),
                        ppm95_name = new DBNVarChar(datarow[Ppm95.DataField.ppm95_name.ToString()]),
                    };
                    ppmList.Add(leave);
                }
                return ppmList;
            }
            return null;
        }
        public List<Ppd95> SelectPpdList(LeavePara para)
        {
            List<DBParameter> dBParameters = new List<DBParameter>();
            string commandText = string.Concat(new object[]
            {
               "SELECT  ppd95_id" +
               "       ,ppd95_name " +
               "FROM prapsppd95 d95 " +
               "INNER JOIN prapsppm95 m95 ON d95.ppm95_id = m95.ppm95_id " +
               "WHERE m95.ppm95_id = {ppm95_id}"
            });
            dBParameters.Add(new DBParameter { Name = LeavePara.ParaField.ppm95_id.ToString(), Value = para.ppm95_id });
            DataTable dataTable = base.GetDataTable(commandText, dBParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<Ppd95> ppdList = new List<Ppd95>();
                foreach (DataRow datarow in dataTable.Rows)
                {
                    Ppd95 leave = new Ppd95()
                    {
                        ppd95_id = new DBVarChar(datarow[Ppd95.DataField.ppd95_id.ToString()]),
                        ppd95_name = new DBNVarChar(datarow[Ppd95.DataField.ppd95_name.ToString()]),
                    };
                    ppdList.Add(leave);
                }
                return ppdList;
            }
            return null;
        }


    }
}

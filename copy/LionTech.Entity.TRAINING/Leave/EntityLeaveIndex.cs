using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.TRAINING.Leave
{
    public class EntityLeaveIndex : EntityLeave
    {
        public EntityLeaveIndex(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
        public class Leave : DBTableRow
        {
            public DBInt ppm96_id;
            public DBNVarChar ppm96_stfn;
            public DBDateTime ppm96_begin;
            public DBDateTime ppm96_end;
            public DBInt ppm96_sign;
            public DBNVarChar ppm95_name;
            public DBNVarChar ppd95_name;
        }

        public List<Leave> SelectLeaveList()
        {
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT",
                "p96.ppm96_id,",
                "p96.ppm96_stfn,",
                "p96.ppm96_begin,",
                "p96.ppm96_end,",
                "p95.ppm95_name,",
                "p95d.ppd95_name,",
                "ppm96_sign",
                "FROM",
                "[dbo].[prapsppm96] p96",
                "JOIN",
                "[dbo].[prapsppm95] p95",
                "ON",
                "p96.ppm95_id = p95.ppm95_id",
                "LEFT JOIN",
                "[dbo].[prapsppd95] p95d",
                "ON",
                "p96.ppd95_id = p95d.ppd95_id;",
            }));
            return GetEntityList<Leave>(commandText.ToString(), null);
        }
    }
}
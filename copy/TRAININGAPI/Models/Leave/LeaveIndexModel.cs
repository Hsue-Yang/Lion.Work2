using LionTech.Entity.TRAINING.Leave;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TRAININGAPI.Models.Leave
{
    public class LeaveIndexModel : LeaveModel
    {
        public LeaveIndexModel()
        {
            _entity = new EntityLeaveIndex(ConnectionStringTRAINING, ProviderNameTRAINING);
        }

        private readonly EntityLeaveIndex _entity;

        internal List<Leave> GetLeaveList()
        {
            return (from s in _entity.SelectLeaveList()
                    select new Leave
                    {
                        ppm96_id = s.ppm96_id.GetValue(),
                        ppm96_stfn = s.ppm96_stfn.GetValue(),
                        ppm96_begin = s.ppm96_begin.GetValue(),
                        ppm96_end = s.ppm96_end.GetValue(),
                        ppm96_sign_string = GetSignString(s.ppm96_sign.GetValue()),
                        ppm95_name = s.ppm95_name.GetValue(),
                        ppd95_name = s.ppd95_name.GetValue(),
                    }).ToList();
        }

        public class Leave
        {
            public int ppm96_id { get; set; }
            public string ppm96_stfn { get; set; }
            public DateTime ppm96_begin { get; set; }
            public DateTime ppm96_end { get; set; }
            public string ppm96_sign_string { get; set; }
            public string ppm95_name { get; set; }
            public string ppd95_name { get; set; }
        }
    }
}
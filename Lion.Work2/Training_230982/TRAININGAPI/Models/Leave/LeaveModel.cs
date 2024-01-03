using LionTech.Entity;
using LionTech.Entity.TRAINING.Leave;
using LionTech.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TRAININGAPI.Models.Leave
{
    public class LeaveModel : _BaseAPModel
    {
        public LeaveModel()
        {
            _entity = new EntityLeaveList(ConnectionStringTRAINING, ProviderNameTRAINING);
        }

        private readonly EntityLeaveList _entity;

        public class Prapsppm96
        {
            public int ppm96_id;
            public string ppm96_stfn;
            public DateTime ppm96_begin;
            public DateTime ppm96_end;
            public string ppm95_name;
            public string ppd95_name;
            public int ppm96_sign;
            public string ppd95_id;
            public string ppm95_id;
        }

        public class LeaveDetail
        {
            public int ppm96_id;
            public string ppm96_stfn;
            public string ppm96_begin;
            public string ppm96_end;
            public string ppm95_name;
            public string ppd95_name;
            public int ppm96_sign;
            public string ppd95_id;
            public string ppm95_id;
        }

        public class Prapsppd95
        {
            public string ppd95_id;
            public string ppd95_name;
        }

        public class Prapsppm95
        {
            public string ppm95_id;
            public string ppm95_name;
        }
        public string ppm95_id { get; set; }

        public List<LeaveDetail> GetLeaveList()
        {
            return (from s in _entity.SelectLeaveList()
                    select new LeaveDetail
                    {
                        ppm96_id = s.ppm96_id.GetValue(),
                        ppm96_stfn = s.ppm96_stfn.GetValue(),
                        ppm96_begin =s.ppm96_begin.GetFormattedValue(Common.EnumDateTimeFormatted.FullDateForMinutes),
                        ppm96_end = s.ppm96_end.GetFormattedValue(Common.EnumDateTimeFormatted.FullDateForMinutes),
                        ppm95_name = s.ppm95_name.GetValue(),
                        ppd95_name = s.ppd95_name.GetValue(),
                        ppm96_sign = s.ppm96_sign
                    }).ToList();
        }

        public List<Prapsppm95> GetPpmList()
        {
            return (from p in _entity.SelectPpmList()
                    select new Prapsppm95
                    {
                        ppm95_id = p.ppm95_id.GetValue(),
                        ppm95_name = p.ppm95_name.GetValue(),
                    }).ToList();
        }

        public List<Prapsppd95> GetPpdList()
        {
            var para = new EntityLeaveList.LeavePara
            {
                ppm95_id = new DBVarChar(ppm95_id)
            };

            if (_entity.SelectPpdList(para) != null)
            {
                return (from p in _entity.SelectPpdList(para)
                        select new Prapsppd95
                        {
                            ppd95_id = p.ppd95_id.GetValue(),
                            ppd95_name = p.ppd95_name.GetValue(),
                        }).ToList();
            }
            return null;
        }
    }
}
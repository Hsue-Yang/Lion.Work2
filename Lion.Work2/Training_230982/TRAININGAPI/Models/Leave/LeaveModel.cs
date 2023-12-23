using LionTech.Entity.TRAINING.Leave;
using LionTech.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
            public string ppm96_stfn;
            public string ppm96_begin;
            public string ppm96_end;
            public string ppm95_name;
            public string ppd95_name;
            public string ppm96_sign;
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

        public List<Prapsppm96> GetLeaveList()
        {
            return (from d in _entity.SelectLeaveList()
                    select new Prapsppm96
                    {
                        ppm96_stfn = d.ppm96_stfn.GetValue(),
                        ppm96_begin = d.ppm96_begin.GetValue(),
                        ppm96_end = d.ppm96_end.GetValue(),
                        ppd95_name = d.ppm95_name.GetValue(),
                        ppm95_name = d.ppd95_name.GetValue(),
                        ppm96_sign = d.pm96_sign != null ? d.pm96_sign.GetValue() : null
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
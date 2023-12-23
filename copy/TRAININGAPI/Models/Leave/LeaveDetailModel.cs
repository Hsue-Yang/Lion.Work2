using LionTech.Entity;
using LionTech.Entity.TRAINING.Leave;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TRAININGAPI.Models.Leave
{
    public class LeaveDetailModel : LeaveModel
    {
        public LeaveDetailModel()
        {
            _entity = new EntityLeaveDetail(ConnectionStringTRAINING, ProviderNameTRAINING);
        }

        private readonly EntityLeaveDetail _entity;
        public int DetailPara { get; set; }
        public string DetailMenuPara { get; set; }

        internal Leave GetLeaveListDetail()
        {
            EntityLeaveDetail.LeavePara para = new EntityLeaveDetail.LeavePara
            {
                ppm96_id = new DBInt(DetailPara)
            };
            var leaveInfo = _entity.SelectLeaveListDetail(para);
            if (leaveInfo != null)
            {
                Leave detailInfo = new Leave
                {
                    ppm96_id = leaveInfo.ppm96_id.GetValue(),
                    ppm96_stfn = leaveInfo.ppm96_stfn.GetValue(),
                    ppm96_begin = leaveInfo.ppm96_begin.GetValue(),
                    ppm96_end = leaveInfo.ppm96_end.GetValue(),
                    ppm95_id = leaveInfo.ppm95_id.GetValue(),
                    ppd95_id = leaveInfo.ppd95_id.GetValue(),
                    ppm96_sign_string = GetSignString(leaveInfo.ppm96_sign.GetValue()),
                };
                return detailInfo;
            }
            return null;
        }

        internal List<LeaveMenuChild> GetLeaveListMenuChild()
        {
            EntityLeaveDetail.LeavePara para = new EntityLeaveDetail.LeavePara
            {
                ppm95_id = new DBChar(DetailMenuPara)
            };
            return (from s in _entity.SelectLeaveListMenuChild(para)
                    select new LeaveMenuChild
                    {
                        ppd95_name = s.ppd95_name.GetValue(),
                        ppd95_id = s.ppd95_id.GetValue()
                    }).ToList();
        }

        internal List<LeaveMenu> GetLeaveListMenu()
        {
            return (from s in _entity.SelectLeaveListMenu()
                    select new LeaveMenu
                    {
                        ppm95_name = s.ppm95_name.GetValue(),
                        ppm95_id = s.ppm95_id.GetValue()
                    }).ToList();
        }

        internal bool EditLeave(LeaveEdit model)
        {
            EntityLeaveDetail.LeavePara para = new EntityLeaveDetail.LeavePara
            {
                ppm96_id = new DBInt(model.ppm96_id),
                ppm96_stfn = new DBNVarChar(model.ppm96_stfn),
                ppm96_begin = new DBDateTime(model.ppm96_begin),
                ppm96_end = new DBDateTime(model.ppm96_end),
                ppm95_id = new DBChar(model.ppm95_id),
                ppd95_id = new DBChar(model.ppd95_id),
                ppm96_sign = new DBInt(model.ppm96_sign)
            };
            return _entity.EditLeaveParaDetail(para) == EntityLeaveDetail.EnumEditLeaveDetailResult.Success;
        }

        internal bool DeleteLeave()
        {
            EntityLeaveDetail.LeavePara para = new EntityLeaveDetail.LeavePara
            {
                ppm96_id = new DBInt(DetailPara)
            };
            return _entity.DeleteLeaveParaDetail(para) == EntityLeaveDetail.EnumEditLeaveDetailResult.Success;
        }

        public class LeaveEdit
        {
            public int ppm96_id { get; set; }
            public string ppm96_stfn { get; set; }
            public DateTime ppm96_begin { get; set; }
            public DateTime ppm96_end { get; set; }
            public string ppm95_id { get; set; }
            public string ppd95_id { get; set; }
            public int ppm96_sign { get; set; }
        }

        public class LeaveMenuChild
        {
            public string ppd95_name { get; set; }
            public string ppd95_id { get; set; }
        }

        public class LeaveMenu
        {
            public string ppm95_id { get; set; }
            public string ppm95_name { get; set; }
        }

        public class Leave
        {
            public int ppm96_id { get; set; }
            public string ppm96_stfn { get; set; }
            public DateTime ppm96_begin { get; set; }
            public DateTime ppm96_end { get; set; }
            public string ppm96_sign_string { get; set; }
            public string ppm95_id { get; set; }
            public string ppd95_id { get; set; }
        }
    }
}
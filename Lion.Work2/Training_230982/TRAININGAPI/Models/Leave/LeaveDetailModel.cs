using LionTech.Entity;
using LionTech.Entity.TRAINING.Leave;
using System;
using System.Globalization;

namespace TRAININGAPI.Models.Leave
{
    public class LeaveDetailModel : LeaveModel
    {
        public LeaveDetailModel()
        {
            _entity = new EntityLeaveDetail(ConnectionStringTRAINING, ProviderNameTRAINING);
        }

        private readonly EntityLeaveDetail _entity;
        public int ppm96_id { get; set; }

        public bool EditLeaveData(Prapsppm96 leaveModel)
        {
            try
            {
                var para = new EntityLeaveDetail.LeaveDetailPara
                {
                    ppm96_id = new DBInt(leaveModel.ppm96_id),
                    ppm96_stfn = new DBVarChar(leaveModel.ppm96_stfn),
                    ppm96_begin = new DBDateTime(leaveModel.ppm96_begin),
                    ppm96_end = new DBDateTime(leaveModel.ppm96_end),
                    ppm95_id = new DBVarChar(leaveModel.ppm95_id),
                    ppd95_id = new DBVarChar(leaveModel.ppd95_id),
                    pm96_sign = new DBInt(leaveModel.ppm96_sign),
                };
                return _entity.EditLeaveDataDetail(para) == EntityLeaveDetail.EnumEditLeaveDetailResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public bool DeleteLeaveData()
        {
            try
            {
                EntityLeaveDetail.LeaveDetailPara para = new EntityLeaveDetail.LeaveDetailPara
                {
                    ppm96_id = new DBInt(ppm96_id)
                };
                
                return _entity.DeleteLeaveDataDetail(para) == EntityLeaveDetail.EnumEditLeaveDetailResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public Prapsppm96 GetLeaveData()
        {
            EntityLeaveDetail.LeaveDetailPara para = new EntityLeaveDetail.LeaveDetailPara
            {
                ppm96_id = new DBInt(ppm96_id)
            };

            var leaveData = _entity.GetLeaveDataDetail(para);
            if (leaveData != null)
            {
                return new Prapsppm96
                {
                    ppm96_id = leaveData.ppm96_id.GetValue(),
                    ppm96_stfn = leaveData.ppm96_stfn.GetValue(),
                    ppm96_begin = leaveData.ppm96_begin.GetValue(),
                    ppm96_end = leaveData.ppm96_end.GetValue(),
                    ppd95_id = leaveData.ppd95_id.GetValue(),
                    ppm95_id = leaveData.ppm95_id.GetValue(),
                    ppm96_sign = leaveData.ppm96_sign.GetValue(),
                };
            }
            return null;
        }
    }
}
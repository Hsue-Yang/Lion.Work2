using LionTech.Entity.ERP.LineBotService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRAININGAPI.Models.Leave
{
    public class LeaveDetailModel : LeaveModel
    {
        private readonly EntityLeave _entity;
        public LeaveDetailModel()
        {
            _entity = new EntityLeave(ConnectionStringTRAINING, ProviderNameTRAINING);
        }
    }
}
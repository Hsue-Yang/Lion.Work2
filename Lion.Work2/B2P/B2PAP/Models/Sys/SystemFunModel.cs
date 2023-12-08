using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;

namespace B2PAP.Models.Sys
{
    public class SystemFunModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QuerySubSysID,
            QueryFunControllerID, QueryFunControllerNM, QueryFunActionName,
            QueryFunName, QueryFunMenuSysID, QueryFunMenu
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QuerySubSysID { get; set; }

        public string QueryFunControllerID { get; set; }

        public string QueryFunName { get; set; }

        [StringLength(10)]
        [InputType(EnumInputType.TextBox)]
        public string QueryFunControllerNM { get; set; }

        [StringLength(10)]
        [InputType(EnumInputType.TextBox)]
        public string QueryFunActionName { get; set; }

        public string QueryFunMenuSysID { get; set; }

        public string QueryFunMenu { get; set; }

        public string PurviewID { get; set; }

        public List<string> PickList { get; set; }

        public SystemFunModel()
        {
        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QuerySubSysID = string.Empty;
            this.QueryFunControllerID = string.Empty;
            this.QueryFunControllerNM = string.Empty;
            this.QueryFunActionName = string.Empty;
            this.QueryFunName = string.Empty;
            this.QueryFunMenuSysID = string.Empty;
            this.QueryFunMenu = string.Empty;
            this.PickList = new List<string>();
        }

        List<EntitySystemFun.SystemFun> _entitySystemFunList;
        public List<EntitySystemFun.SystemFun> EntitySystemFunList { get { return _entitySystemFunList; } }

        public bool GetSystemFunList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFun.SystemFunPara para = new EntitySystemFun.SystemFunPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    SubSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySubSysID) ? null : this.QuerySubSysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunControllerID) ? null : this.QueryFunControllerID)),
                    FunControllerNM = new DBObject((string.IsNullOrWhiteSpace(this.QueryFunControllerNM) ? null : this.QueryFunControllerNM)),
                    FunActionNM = new DBObject((string.IsNullOrWhiteSpace(this.QueryFunActionName) ? null : this.QueryFunActionName)),
                    FunNM = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunName) ? null : this.QueryFunName)),
                    FunMenuSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunMenuSysID) ? null : this.QueryFunMenuSysID)),
                    FunMenu = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunMenu) ? null : this.QueryFunMenu))
                };

                _entitySystemFunList = new EntitySystemFun(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunList(para);

                if (_entitySystemFunList != null)
                {
                    EntitySystemFun.SystemMenuFunPara systemMenuFunPara = new EntitySystemFun.SystemMenuFunPara(cultureID.ToString())
                    {
                        SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                        FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunControllerID) ? null : this.QueryFunControllerID)),
                        FunActionName = new DBVarChar(null),
                        FunMenuSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunMenuSysID) ? null : this.QueryFunMenuSysID)),
                        FunMenu = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunMenu) ? null : this.QueryFunMenu))
                    };

                    List<EntitySystemFun.SystemMenuFun> systemMenuFunList = new EntitySystemFun(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .SelectSystemMenuFunList(systemMenuFunPara);

                    if (systemMenuFunList != null && systemMenuFunList.Count > 0)
                    {
                        foreach (EntitySystemFun.SystemFun data in _entitySystemFunList)
                        {
                            List<EntitySystemFun.SystemMenuFun> menuFunList = systemMenuFunList
                                .Where(menuFun => menuFun.SysID.GetValue() == data.SysID.GetValue() &&
                                                  menuFun.FunControllerID.GetValue() == data.FunControllerID.GetValue() &&
                                                  menuFun.FunActionName.GetValue() == data.FunActionName.GetValue()).ToList();

                            if (menuFunList != null)
                            {
                                data.MenuList = new List<EntitySystemFun.SystemMenuFun>(menuFunList);
                            }
                        }
                    }

                    _entitySystemFunList = base.GetEntitysByPage(_entitySystemFunList, pageSize);
                }
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public EntitySystemFun.EnumEditSystemFunResult GetEditSystemFunResult(string userID, EnumCultureID cultureID)
        {
            try
            {
                List<EntitySystemFun.SystemFunPara> paraList = new List<EntitySystemFun.SystemFunPara>();
                if (this.PickList != null && this.PickList.Count > 0)
                {
                    foreach (string pickData in this.PickList)
                    {
                        paraList.Add(new EntitySystemFun.SystemFunPara(cultureID.ToString())
                        {
                            SysID = new DBVarChar(pickData.Split('|')[0]),
                            PurviewID = new DBVarChar((string.IsNullOrWhiteSpace(this.PurviewID) ? null : this.PurviewID)),
                            FunControllerID = new DBVarChar(pickData.Split('|')[1]),
                            FunActionName = new DBVarChar(pickData.Split('|')[2]),
                            UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                        });
                    }
                }

                return new EntitySystemFun(this.ConnectionStringB2P, this.ProviderNameB2P).EditSystemFun(paraList);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return EntitySystemFun.EnumEditSystemFunResult.Failure;
        }
    }
}
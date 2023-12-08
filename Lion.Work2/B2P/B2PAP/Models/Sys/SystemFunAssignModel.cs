using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;

namespace B2PAP.Models.Sys
{
    public class SystemFunAssignModel : SysModel
    {
        public EnumCultureID CurrentCultureID { get; set; }

        [Required]
        public string SysID { get; set; }

        public string _SubSysID;

        public string SubSysID
        {
            get
            {
                if (_SubSysID == EnumSystemID.B2PAP.ToString())
                {
                    _SubSysID = string.Empty;
                }
                return _SubSysID;
            }
            set
            {
                _SubSysID = value;
            }
        }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunControllerID { get; set; }

        [Required]
        [StringLength(40)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunActionName { get; set; }

        public string FunType { get; set; }

        [Required]
        public string FunNM { get; set; }

        public SystemFunAssignModel()
        {

        }

        public void FormReset()
        {

        }

        List<EntitySystemFunAssign.RAWUser> _entityRAWUserList;
        public List<EntitySystemFunAssign.RAWUser> EntityRAWUserList { get { return _entityRAWUserList; } }
        public bool GetRAWUserList(string condition)
        {
            try
            {
                if (condition != null)
                {
                    EntitySystemFunAssign.RAWUserPara rawUserPara = new EntitySystemFunAssign.RAWUserPara()
                    {
                        UserCondition = new DBNVarChar(condition),
                    };

                    _entityRAWUserList = new EntitySystemFunAssign(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .SelectRAWUserList(rawUserPara);

                    if (_entityRAWUserList != null)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                _entityRAWUserList = null;
                base.OnException(ex);
            }
            return false;
        }

        EntitySystemFunAssign.SystemFun _entitySystemFunInfor;
        public EntitySystemFunAssign.SystemFun EntitySystemFunInfor { get { return _entitySystemFunInfor; } }
        public bool GetSystemFunInfor(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFunAssign.SystemFunPara para = new EntitySystemFunAssign.SystemFunPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName))
                };

                _entitySystemFunInfor = new EntitySystemFunAssign(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunInfor(para);

                if (_entitySystemFunInfor != null)
                {
                    this.FunNM = _entitySystemFunInfor.FunNM.GetValue();

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySystemFunAssign.SystemFunAssign> _entitySystemFunAssignList;
        public List<EntitySystemFunAssign.SystemFunAssign> EntitySystemFunAssignList { get { return _entitySystemFunAssignList; } }
        public bool GetSystemFunAssignList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFunAssign.SystemFunAssignPara para = new EntitySystemFunAssign.SystemFunAssignPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName))
                };

                _entitySystemFunAssignList = new EntitySystemFunAssign(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunAssignList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetEditSystemFunAssignResult(string userID, List<EntitySystemFunAssign.SystemFunAssignValue> systemFunAssignValueList, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFunAssign.SystemFunAssignPara para = new EntitySystemFunAssign.SystemFunAssignPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                    UserIDList = new List<DBVarChar>(),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                if (systemFunAssignValueList != null && systemFunAssignValueList.Count > 0)
                {
                    foreach (EntitySystemFunAssign.SystemFunAssignValue systemFunAssignValue in systemFunAssignValueList)
                    {
                        if (systemFunAssignValue.GetUserID().IsNull() == false)
                        {
                            para.UserIDList.Add(systemFunAssignValue.GetUserID());
                        }
                    }
                }

                if (new EntitySystemFunAssign(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditSystemFunAssign(para) == EntitySystemFunAssign.EnumEditSystemFunAssignResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;

namespace B2PAP.Models.Sys
{
    public class SystemFunDetailModel : SysModel
    {
        public EnumCultureID CurrentCultureID { get; set; }

        [Required]
        public string SysID { get; set; }

        public string SubSysID { get; set; }

        public string PurviewID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunControllerID { get; set; }

        [Required]
        [StringLength(40)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunActionName { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunNMJAJP { get; set; }

        [Required]
        public string FunType { get; set; }

        public string IsOutside { get; set; }

        public string IsDisable { get; set; }

        [StringLength(6, MinimumLength=6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<string> HasRole { get; set; }

        public SystemFunDetailModel()
        {

        }

        public void FormReset()
        {
            this.SubSysID = string.Empty;
            this.PurviewID = string.Empty;
            this.FunNMZHTW = string.Empty;
            this.FunNMZHCN = string.Empty;
            this.FunNMENUS = string.Empty;
            this.FunNMTHTH = string.Empty;
            this.FunNMJAJP = string.Empty;
            this.FunType = string.Empty;
            this.IsOutside = EnumYN.N.ToString();
            this.IsDisable = EnumYN.N.ToString();
            this.SortOrder = string.Empty;
            this.HasRole = new List<string>();
        }

        EntitySystemFunDetail.SystemFunDetail _entitySystemFunDetail;
        public EntitySystemFunDetail.SystemFunDetail EntitySystemFunDetail { get { return _entitySystemFunDetail; } }
        List<EntitySystemFun.SystemMenuFun> _entitySystemMenuFunList;
        public List<EntitySystemFun.SystemMenuFun> EntitySystemMenuFunList { get { return _entitySystemMenuFunList; } }
        public bool GetSystemFunDetail(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFunDetail.SystemFunDetailPara para = new EntitySystemFunDetail.SystemFunDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName))
                };
                
                _entitySystemFunDetail= new EntitySystemFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunDetail(para);

                if (_entitySystemFunDetail != null)
                {
                    EntitySystemFun.SystemMenuFunPara systemMenuFunPara = new EntitySystemFun.SystemMenuFunPara(cultureID.ToString())
                    {
                        SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                        FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                        FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                        FunMenuSysID = new DBVarChar(null),
                        FunMenu = new DBVarChar(null)
                    };

                    _entitySystemMenuFunList = new EntitySystemFun(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .SelectSystemMenuFunList(systemMenuFunPara);

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public List<EntitySys.SysSystemFunMenu> GetFunMenuList(string sysID)
        {
            try
            {
                EntitySys.SysSystemFunMenuPara para = new EntitySys.SysSystemFunMenuPara(this.CurrentCultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                return new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P).SelectSysSystemFunMenuList(para);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }

        List<EntitySystemFunDetail.SystemFunRole> _entitySystemFunRoleList;
        public List<EntitySystemFunDetail.SystemFunRole> EntitySystemFunRoleList { get { return _entitySystemFunRoleList; } }
        public bool GetSystemFunRoleList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFunDetail.SystemFunRolePara para = new EntitySystemFunDetail.SystemFunRolePara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName))
                };

                _entitySystemFunRoleList = new EntitySystemFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunRoleList(para);

                if (_entitySystemFunRoleList != null)
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

        public bool GetEditSystemFunDetailResult(string userID, List<EntitySystemFunDetail.SystemMenuFunValue> systemMenuFunValueList)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.SubSysID))
                {
                    this.SubSysID = this.SysID;
                }

                EntitySystemFunDetail.SystemFunDetailPara para = new EntitySystemFunDetail.SystemFunDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    SubSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SubSysID) ? null : this.SubSysID)),
                    PurviewID = new DBVarChar((string.IsNullOrWhiteSpace(this.PurviewID) ? null : this.PurviewID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                    FunNMZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunNMZHTW) ? null : this.FunNMZHTW)),
                    FunNMZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunNMZHCN) ? null : this.FunNMZHCN)),
                    FunNMENUS = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunNMENUS) ? null : this.FunNMENUS)),
                    FunNMTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunNMTHTH) ? null : this.FunNMTHTH)),
                    FunNMJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(this.FunNMJAJP) ? null : this.FunNMJAJP)),
                    FunType = new DBVarChar((string.IsNullOrWhiteSpace(this.FunType) ? null : this.FunType)),
                    IsOutside = new DBChar((string.IsNullOrWhiteSpace(this.IsOutside) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(this.IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                List<EntitySystemFunDetail.UserSystemFunRolePara> paraList = new List<EntitySystemFunDetail.UserSystemFunRolePara>();
                if (this.HasRole != null && this.HasRole.Count > 0)
                {
                    foreach (string roleString in this.HasRole)
                    {
                        paraList.Add(new EntitySystemFunDetail.UserSystemFunRolePara()
                        {
                            SysID = new DBVarChar((string.IsNullOrWhiteSpace(roleString.Split('|')[0]) ? null : roleString.Split('|')[0])),
                            RoleID = new DBVarChar((string.IsNullOrWhiteSpace(roleString.Split('|')[1]) ? null : roleString.Split('|')[1])),
                            FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                            FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                            UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                        });
                    }
                }

                if (new EntitySystemFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditSystemFunDetail(para, paraList, systemMenuFunValueList) == LionTech.Entity.B2P.Sys.EntitySystemFunDetail.EnumEditSystemFunDetailResult.Success)
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

        public EntitySystemFunDetail.EnumDeleteSystemFunDetailResult GetDeleteSystemFunDetailResult()
        {
            try
            {
                EntitySystemFunDetail.SystemFunDetailPara para = new EntitySystemFunDetail.SystemFunDetailPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                };

                return new EntitySystemFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .DeleteSystemFunDetail(para);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return LionTech.Entity.B2P.Sys.EntitySystemFunDetail.EnumDeleteSystemFunDetailResult.Failure;
        }
    }
}
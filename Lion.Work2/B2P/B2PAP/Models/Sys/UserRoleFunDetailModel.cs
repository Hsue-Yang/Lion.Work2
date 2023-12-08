using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class UserRoleFunDetailModel : SysModel
    {
        public string UserID { get; set; }

        public string RoleGroupID { get; set; }

        public List<string> HasRole { get; set; }

        public UserRoleFunDetailModel()
        {

        }

        public void FormReset()
        {
            this.HasRole = new List<string>();
        }

        EntityUserRoleFunDetail.UserRawData _entityUserRawData;
        public EntityUserRoleFunDetail.UserRawData EntityUserRawData { get { return _entityUserRawData; } }

        public bool GetUserRawData()
        {
            try
            {
                EntityUserRoleFunDetail.UserRawDataPara para = new EntityUserRoleFunDetail.UserRawDataPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserRawData = new EntityUserRoleFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserRawData(para);

                if (_entityUserRawData != null)
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

        List<EntityUserRoleFunDetail.SysSystemRoleGroup> _entitySysSystemRoleGroupList;
        public List<EntityUserRoleFunDetail.SysSystemRoleGroup> EntitySysSystemRoleGroupList { get { return _entitySysSystemRoleGroupList; } }

        public bool GetSysSystemRoleGroupList(EnumCultureID cultureID)
        {
            try
            {
                EntityUserRoleFunDetail.SysSystemRoleGroupPara para = new EntityUserRoleFunDetail.SysSystemRoleGroupPara(cultureID.ToString())
                {

                };

                _entitySysSystemRoleGroupList = new EntityUserRoleFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemRoleGroupList(para);

                if (_entitySysSystemRoleGroupList == null)
                {
                    _entitySysSystemRoleGroupList = new List<EntityUserRoleFunDetail.SysSystemRoleGroup>();
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntityUserRoleFunDetail.SysSystemRoleGroupCollect> _entitySysSystemRoleGroupCollectList = new List<EntityUserRoleFunDetail.SysSystemRoleGroupCollect>();
        public List<EntityUserRoleFunDetail.SysSystemRoleGroupCollect> EntitySysSystemRoleGroupCollectList { get { return _entitySysSystemRoleGroupCollectList; } }

        public bool GetSysSystemRoleGroupCollectList(string roleGroupID)
        {
            try
            {
                EntityUserRoleFunDetail.SysSystemRoleGroupCollectPara para = new EntityUserRoleFunDetail.SysSystemRoleGroupCollectPara()
                {
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(roleGroupID) ? null : roleGroupID))
                };

                _entitySysSystemRoleGroupCollectList = new EntityUserRoleFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSysSystemRoleGroupCollectList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntityUserRoleFunDetail.UserSystemRole> _entityUserSystemRoleList;
        public List<EntityUserRoleFunDetail.UserSystemRole> EntityUserSystemRoleList { get { return _entityUserSystemRoleList; } }

        public bool GetUserSystemRoleList(EnumCultureID cultureID)
        {
            try
            {
                EntityUserRoleFunDetail.UserSystemRolePara para = new EntityUserRoleFunDetail.UserSystemRolePara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserSystemRoleList = new EntityUserRoleFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserSystemRoleList(para);

                if (_entityUserSystemRoleList != null)
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

        public bool GetEditUserSystemRoleResult(string userID, string ip, EnumCultureID cultureID)
        {
            try
            {
                EntityUserRoleFunDetail.UserRawDataPara userRawDataPara = new EntityUserRoleFunDetail.UserRawDataPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                bool isDisable = new EntityUserRoleFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserIsDisable(userRawDataPara);

                EntityUserRoleFunDetail.UserSystemRolePara para = new EntityUserRoleFunDetail.UserSystemRolePara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleGroupID) ? null : this.RoleGroupID)),
                    IsDisable = new DBChar((isDisable ? EnumYN.Y.ToString() : EnumYN.N.ToString())),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ip) ? null : ip))
                };

                List<EntityUserRoleFunDetail.UserSystemRolePara> paraList = new List<EntityUserRoleFunDetail.UserSystemRolePara>();
                if (this.HasRole != null && this.HasRole.Count > 0)
                {
                    foreach (string roleString in this.HasRole)
                    {
                        paraList.Add(new EntityUserRoleFunDetail.UserSystemRolePara(cultureID.ToString())
                        {
                            UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                            SysID = new DBVarChar((string.IsNullOrWhiteSpace(roleString.Split('|')[0]) ? null : roleString.Split('|')[0])),
                            RoleID = new DBVarChar((string.IsNullOrWhiteSpace(roleString.Split('|')[1]) ? null : roleString.Split('|')[1]))
                        });
                    }
                }

                if (new EntityUserRoleFunDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditUserSystemRole(para, paraList) == EntityUserRoleFunDetail.EnumEditUserSystemRoleResult.Success)
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

        public bool GenerateUserMenuXML(string filePath, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.UserID))
                    return false;

                Entity_BaseAP.UserMenuFunPara para = new Entity_BaseAP.UserMenuFunPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                List<Entity_BaseAP.UserMenuFun> userFunMenuList = new Entity_BaseAP(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserMenuFunList(para);

                if (Utility.GenerateUserMenuXML(userFunMenuList, this.UserID, filePath) == Entity_BaseAP.EnumGenerateUserMenuXMLResult.Success)
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
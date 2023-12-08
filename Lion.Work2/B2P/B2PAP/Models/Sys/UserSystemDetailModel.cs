using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class UserSystemDetailModel : SysModel
    {
        public string UserID { get; set; }

        public List<string> HasSys { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysUserSystemDetail.TabText_UserSystemDetail,
                ImageURL=string.Empty
            }
        };

        public UserSystemDetailModel()
        {

        }

        public void FormReset()
        {
            this.HasSys = new List<string>();
        }

        EntityUserSystemDetail.UserRawData _entityUserRawData;
        public EntityUserSystemDetail.UserRawData EntityUserRawData { get { return _entityUserRawData; } }

        public bool GetUserRawData()
        {
            try
            {
                EntityUserSystemDetail.UserRawDataPara para = new EntityUserSystemDetail.UserRawDataPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserRawData = new EntityUserSystemDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
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

        List<EntityUserSystemDetail.UserSystemRole> _entityUserSystemRoleList;
        public List<EntityUserSystemDetail.UserSystemRole> EntityUserSystemRoleList { get { return _entityUserSystemRoleList; } }

        public bool GetUserSystemRoleList(EnumCultureID cultureID)
        {
            try
            {
                EntityUserSystemDetail.UserSystemRolePara para = new EntityUserSystemDetail.UserSystemRolePara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserSystemRoleList = new EntityUserSystemDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
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

        public bool GetEditUserSystemRoleResult(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntityUserSystemDetail.UserSystemRolePara para = new EntityUserSystemDetail.UserSystemRolePara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                List<EntityUserSystemDetail.UserSystemRolePara> paraList = new List<EntityUserSystemDetail.UserSystemRolePara>();
                if (this.HasSys != null && this.HasSys.Count > 0)
                {
                    foreach (string sysString in this.HasSys)
                    {
                        paraList.Add(new EntityUserSystemDetail.UserSystemRolePara(cultureID.ToString())
                        {
                            UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                            SysID = new DBVarChar(sysString)
                        });
                    }
                }

                if (new EntityUserSystemDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                        .EditUserSystemRole(para, paraList) == EntityUserSystemDetail.EnumEditUserSystemRoleResult.Success)
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
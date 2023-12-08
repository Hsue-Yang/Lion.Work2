using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Authorization;

namespace ERPAPI.Models.Authorization
{
    public class B2PUserModel : AuthorizationModel
    {
        #region API Property
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        //[AllowHtml]
        public string APIPara { get; set; }
        #endregion

        public class APIParaData
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string UserPWD { get; set; }

            public string ComID { get; set; }
            public string UnitID { get; set; }
            public string UnitNM { get; set; }

            public string UserGender { get; set; }
            public string UserTitle { get; set; }
            public string UserTel1 { get; set; }
            public string UserTel2 { get; set; }
            public string UserEmail { get; set; }
            public string Remark { get; set; }
            public string DefaultSysID { get; set; }
            public string DefaultPath { get; set; }
            public string IsGrantor { get; set; }
            public string IsDisable { get; set; }

            public List<string> RoleIDList { get; set; }
        }

        public APIParaData APIData { get; set; }

        public B2PUserModel()
        {

        }

        public bool CreateB2PUserAccount()
        {
            try
            {
                EntityB2PUser entityB2PUser = new EntityB2PUser(this.ConnectionStringB2P, this.ProviderNameB2P);

                EntityB2PUser.B2PUserAccountPara para = new EntityB2PUser.B2PUserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID)),
                    UserNM = new DBNVarChar((string.IsNullOrWhiteSpace(this.APIData.UserNM) ? null : this.APIData.UserNM)),
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserPWD) ? null : this.APIData.UserPWD)),

                    ComID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.ComID) ? null : this.APIData.ComID)),
                    UnitID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UnitID) ? null : this.APIData.UnitID)),
                    UnitNM = new DBNVarChar((string.IsNullOrWhiteSpace(this.APIData.UnitNM) ? null : this.APIData.UnitNM)),

                    UserGender = new DBChar((string.IsNullOrWhiteSpace(this.APIData.UserGender) ? null : this.APIData.UserGender)),
                    UserTitle = new DBNVarChar((string.IsNullOrWhiteSpace(this.APIData.UserTitle) ? null : this.APIData.UserTitle)),
                    UserTel1 = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserTel1) ? null : this.APIData.UserTel1)),
                    UserTel2 = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserTel2) ? null : this.APIData.UserTel2)),
                    UserEmail = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserEmail) ? null : this.APIData.UserEmail)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.APIData.Remark) ? null : this.APIData.Remark)),
                    DefaultSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.DefaultSysID) ? null : this.APIData.DefaultSysID)),
                    DefaultPath = new DBNVarChar((string.IsNullOrWhiteSpace(this.APIData.DefaultPath) ? null : this.APIData.DefaultPath)),
                    IsGrantor = new DBChar((string.IsNullOrWhiteSpace(this.APIData.IsGrantor) ? EnumYN.N.ToString() : this.APIData.IsGrantor)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(this.APIData.IsDisable) ? EnumYN.Y.ToString() : this.APIData.IsDisable)),

                    UpdUserID = new DBVarChar(entityB2PUser.UpdUserID.GetValue())
                };

                List<EntityB2PUser.SystemRolePara> paraList = new List<EntityB2PUser.SystemRolePara>();

                if (this.APIData.RoleIDList != null && this.APIData.RoleIDList.Count > 0)
                {
                    foreach (string roleID in this.APIData.RoleIDList)
                    {
                        paraList.Add(new EntityB2PUser.SystemRolePara()
                        {
                            UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID)),
                            SysID = new DBVarChar((string.IsNullOrWhiteSpace(roleID.Split('|')[0]) ? null : roleID.Split('|')[0])),
                            RoleID = new DBVarChar((string.IsNullOrWhiteSpace(roleID.Split('|')[1]) ? null : roleID.Split('|')[1])),
                            UpdUserID = new DBVarChar(entityB2PUser.UpdUserID.GetValue())
                        });
                    }
                }

                if (entityB2PUser.CreateAccount(para, paraList) == EntityB2PUser.EnumCreateAccountResult.Success)
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

        public bool DeleteB2PUserAccount()
        {
            try
            {
                EntityB2PUser entityB2PUser = new EntityB2PUser(this.ConnectionStringB2P, this.ProviderNameB2P);

                EntityB2PUser.B2PUserAccountPara para = new EntityB2PUser.B2PUserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID)),
                    UpdUserID = new DBVarChar(entityB2PUser.UpdUserID.GetValue())
                };

                if (entityB2PUser.DeleteAccount(para) == EntityB2PUser.EnumDeleteAccountResult.Success)
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

        public bool CheckB2PUserAccountIsExist()
        {
            try
            {
                EntityB2PUser.B2PUserAccountPara para = new EntityB2PUser.B2PUserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID))
                };

                if (new EntityB2PUser(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .CheckAccount(para) == EntityB2PUser.EnumCheckAccountResult.Success)
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

        public bool GenerateUserMenuXML(string userID, string filePath, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userID))
                    return false;

                Entity_BaseAP.UserMenuFunPara para = new Entity_BaseAP.UserMenuFunPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                List<Entity_BaseAP.UserMenuFun> userFunMenuList = new Entity_BaseAP(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserMenuFunList(para);

                if (Utility.GenerateUserMenuXML(userFunMenuList, userID, filePath) == Entity_BaseAP.EnumGenerateUserMenuXMLResult.Success)
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
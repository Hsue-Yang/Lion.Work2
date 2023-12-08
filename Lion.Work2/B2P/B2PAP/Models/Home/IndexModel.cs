using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Home;
using LionTech.Web.ERPHelper;

namespace B2PAP.Models.Home
{
    public class IndexModel : _BaseAPModel
    {
        public enum EnumUserLocation
        {
            COMP,
            HOME,
            OTHER
        }

        public string CultureID { get; set; }

        [AllowHtml]
        [InputType(EnumInputType.TextBoxHidden)]
        public string TargetUrl { get; set; }

        private string _UserID;

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string UserID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_UserID))
                {
                    return _UserID;
                }
                return _UserID.ToUpper();
            }
            set
            {
                _UserID = value;
            }
        }

        [Required]
        [StringLength(10)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string UserPassword { get; set; }

        public string UserLocation { get; set; }

        [StringLength(20)]
        [InputType(EnumInputType.TextBox)]
        public string LocationDesc { get; set; }

        [StringLength(10)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string IdNo { get; set; }

        [StringLength(8)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string Birthday { get; set; }

        public string EnvironmentRemind { get; set; }

        public IndexModel()
        {
            this.UserID = string.Empty;
            this.UserPassword = string.Empty;
            this.IdNo = string.Empty;
            this.Birthday = string.Empty;
            this.UserLocation = EnumUserLocation.COMP.ToString();
        }

        public void FormReset()
        {
            this.UserPassword = string.Empty;
            this.IdNo = string.Empty;
            this.Birthday = string.Empty;
        }

        List<Entity_BaseAP.CMCode> _entityBaseCultureIDList = new List<Entity_BaseAP.CMCode>();
        public List<Entity_BaseAP.CMCode> EntityBaseCultureIDList { get { return _entityBaseCultureIDList; } }

        public bool GetBaseCultureIDList(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.CMCodePara para = new Entity_BaseAP.CMCodePara()
                {
                    HasCodeID = new DBChar(EnumYN.N.ToString()),
                    CodeKind = new DBVarChar("0000"),
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                _entityBaseCultureIDList = new Entity_BaseAP(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectCMCodeList(para);

                if (_entityBaseCultureIDList != null)
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

        public bool AuthenticatingB2P()
        {
            try
            {
                EntityIndex.UserInforPara userInforPara = new EntityIndex.UserInforPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(this.UserPassword) ? null : LionTech.Utility.Validator.GetEncodeString(this.UserPassword))),
                };

                if (new EntityIndex(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .ValidateUserInfor(userInforPara) == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return false;
        }

        public bool GetValidateUserAuthorization(string sessionID, string ip)
        {
            try
            {
                EntityIndex.UserAuthorizationPara userAuthorizationPara = new EntityIndex.UserAuthorizationPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    SystemID = new DBVarChar(EnumSystemID.B2PAP.ToString()),
                    SessionID = new DBChar((string.IsNullOrWhiteSpace(sessionID) ? null : sessionID)),
                    IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ip) ? null : ip))
                };

                if (new EntityIndex(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .ValidateUserAuthorization(userAuthorizationPara) == EntityIndex.EnumValidateUserAuthorizationResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return false;
        }

        EntityIndex.UserMain _entityUserMain;
        public EntityIndex.UserMain EntityUserMain { get { return _entityUserMain; } }

        public bool GetUserMain()
        {
            try
            {
                EntityIndex.UserMainPara userMainPara = new EntityIndex.UserMainPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserMain = new EntityIndex(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserMain(userMainPara);

                if (_entityUserMain != null)
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

        List<EntityIndex.UserSystem> _entityUserSystemList;
        public List<EntityIndex.UserSystem> EntityUserSystemList { get { return _entityUserSystemList; } }

        public bool GetUserSystemList()
        {
            try
            {
                EntityIndex.UserSystemPara userSystemPara = new EntityIndex.UserSystemPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserSystemList = new EntityIndex(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserSystemList(userSystemPara);

                if (_entityUserSystemList != null)
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

        List<EntityIndex.UserSystemRole> _entityUserSystemRoleList;
        public List<EntityIndex.UserSystemRole> EntityUserSystemRoleList { get { return _entityUserSystemRoleList; } }

        public bool GetUserSystemRoleList()
        {
            try
            {
                EntityIndex.UserSystemRolePara userSystemRolePara = new EntityIndex.UserSystemRolePara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserSystemRoleList = new EntityIndex(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserSystemRoleList(userSystemRolePara);

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

        public EntityIndex.SCMAPB2PSettingSupB2PUserBulletin GetAPIParaSCMAPB2PSettingSupB2PUserBulletinEntity()
        {
            try
            {
                EntityIndex.SCMAPB2PSettingSupB2PUserBulletin entitySCMAPB2PSettingSupB2PUserBulletin = new EntityIndex.SCMAPB2PSettingSupB2PUserBulletin()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                return entitySCMAPB2PSettingSupB2PUserBulletin;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }
    }
}
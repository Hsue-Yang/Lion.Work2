using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.APIService;
using LionTech.Entity.B2P.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class UserSettingDetailModel : SysModel
    {
        public enum EnumUserGender
        {
            [Description("M")]
            Male,
            [Description("F")]
            Female
        }

        public string SystemID { get; set; }

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
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string UserNM { get; set; }

        [StringLength(10)]
        public string UserPWD { get; set; }

        public string UserGender { get; set; }

        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string UserTitle { get; set; }

        [StringLength(20)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string UserTel1 { get; set; }

        [StringLength(20)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string UserTel2 { get; set; }
        
        public bool MailFormat
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UserEmail) == false)
                {
                    return Common.GetTextValidationResult(UserEmail, Common.EnumRegexFormatted.Mail);
                }
                return false;
            }
        }

        [Required]
        [StringLength(40)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string UserEmail { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        public string IsDisable { get; set; }

        public string IsGrantor { get; set; }

        public string UpdInfor { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysUserSettingDetail.TabText_UserSettingDetail,
                ImageURL=string.Empty
            }
        };

        public UserSettingDetailModel()
        {

        }

        public void FormReset()
        {
            this.IsDisable = EnumYN.N.ToString();
        }

        EntityUserSettingDetail.UserSettingDetail _entityUserSettingDetail;
        public EntityUserSettingDetail.UserSettingDetail EntityUserSettingDetail { get { return _entityUserSettingDetail; } }

        public bool GetUserSettingDetail()
        {
            try
            {
                EntityUserSettingDetail.UserSettingDetailPara para = new EntityUserSettingDetail.UserSettingDetailPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserSettingDetail = new EntityUserSettingDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectUserSettingDetail(para);

                if (_entityUserSettingDetail != null)
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

        public bool GetEditUserSettingDetailResult(string userID, string comID, string unitID, string unitNM, string ip)
        {
            try
            {
                EntityUserSettingDetail.UserSettingDetailPara para = new EntityUserSettingDetail.UserSettingDetailPara()
                {
                    UserID = (string.IsNullOrWhiteSpace(this.UserID) ? null : new DBVarChar(this.UserID)),
                    UserNM = (string.IsNullOrWhiteSpace(this.UserNM) ? null : new DBNVarChar(this.UserNM)),
                    UserPWD = (string.IsNullOrWhiteSpace(this.UserPWD) ? null : new DBVarChar(LionTech.Utility.Validator.GetEncodeString(this.UserPWD))),

                    ComID = (string.IsNullOrWhiteSpace(comID) ? null : new DBVarChar(comID)),
                    UnitID = (string.IsNullOrWhiteSpace(unitID) ? null : new DBVarChar(unitID)),
                    UnitNM = (string.IsNullOrWhiteSpace(unitNM) ? null : new DBNVarChar(unitNM)),

                    UserGender = (string.IsNullOrWhiteSpace(this.UserGender) ? null : new DBChar(this.UserGender)),
                    UserTitle = (string.IsNullOrWhiteSpace(this.UserTitle) ? null : new DBNVarChar(this.UserTitle)),
                    UserTel1 = (string.IsNullOrWhiteSpace(this.UserTel1) ? null : new DBVarChar(this.UserTel1)),
                    UserTel2 = (string.IsNullOrWhiteSpace(this.UserTel2) ? null : new DBVarChar(this.UserTel2)),
                    UserEmail = (string.IsNullOrWhiteSpace(this.UserEmail) ? null : new DBVarChar(this.UserEmail)),
                    Remark = (string.IsNullOrWhiteSpace(this.Remark) ? null : new DBNVarChar(this.Remark)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(this.IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    IsGrantor = new DBChar((string.IsNullOrWhiteSpace(this.IsGrantor) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    GrantorUserID = (string.IsNullOrWhiteSpace(userID) ? null : new DBVarChar(userID)),
                    UpdUserID = (string.IsNullOrWhiteSpace(userID) ? null : new DBVarChar(userID)),
                    ExecIPAddress = (string.IsNullOrWhiteSpace(ip) ? null : new DBVarChar(ip))
                };

                if (new EntityUserSettingDetail(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .EditUserSettingDetail(para) == LionTech.Entity.B2P.Sys.EntityUserSettingDetail.EnumEditUserSettingDetailResult.Success)
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

        public EntityAPIPara.SCMAPB2PSettingSupB2PUser GetAPIParaSCMAPB2PSettingSupB2PUserEntity(string userID, string userPWD)
        {
            try
            {
                EntityAPIPara.SCMAPB2PSettingSupB2PUser entityAPIParaB2PSettingSupB2PUser = new EntityAPIPara.SCMAPB2PSettingSupB2PUser()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    UserNM = new DBNVarChar((string.IsNullOrWhiteSpace(this.UserNM) ? null : this.UserNM)),
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(userPWD) ? null : userPWD)),
                    UserGender = new DBChar((string.IsNullOrWhiteSpace(this.UserGender) ? null : this.UserGender)),
                    UserTitle = new DBNVarChar((string.IsNullOrWhiteSpace(this.UserTitle) ? null : this.UserTitle)),
                    UserTel1 = new DBVarChar((string.IsNullOrWhiteSpace(this.UserTel1) ? null : this.UserTel1)),
                    UserTel2 = new DBVarChar((string.IsNullOrWhiteSpace(this.UserTel2) ? null : this.UserTel2)),
                    UserEmail = new DBVarChar((string.IsNullOrWhiteSpace(this.UserEmail) ? null : this.UserEmail)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(this.Remark) ? null : this.Remark)),
                    IsGrantor = new DBChar((string.IsNullOrWhiteSpace(this.IsGrantor) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                    GrantorUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(this.IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString())),
                };

                return entityAPIParaB2PSettingSupB2PUser;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
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
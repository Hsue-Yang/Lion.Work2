using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class UserMainModel : SysModel
    {
        public enum EnumModifyResult
        {
            Success,
            Failure,
            SyncASPFailure
        }

        public enum EnumValidateUserPWD
        {
            Correctly, PWDIncorrectly, NewPWDIncorrectly
        }

        [InputType(EnumInputType.TextBoxHidden)]
        public bool IsForced { get; set; }
        public bool IsPermissible { get; set; }
        public string PWDValidDate { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string UserID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string UserNM { get; set; }

        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string UserENM { get; set; }

        [StringLength(20)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string UserTel { get; set; }

        [StringLength(5, MinimumLength = 4)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string UserExtension { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string UserMobile { get; set; }

        [StringLength(40)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string UserEMail { get; set; }

        [StringLength(10, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string UserPWD { get; set; }

        [StringLength(10, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string UserNewPWD { get; set; }

        [StringLength(10, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxPassword)]
        public string UserNewPWDCheck { get; set; }

        [StringLength(40)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string UserGoogleAccount { get; set; }

        public string IsGAccEnable { get; set; }

        public class UserParaData
        {
            public string stfn_stfn { get; set; }
            public string stfn_cname { get; set; }
            public string stfn_pswd { get; set; }
            public string stfn_sts { get; set; }

            public string stf4_alias { get; set; }
            public string pp00_idno { get; set; }
            public string pp00_bdate { get; set; }
            public string stf4_ptel { get; set; }
            public string stf4_extel { get; set; }
            public string stf4_mtel { get; set; }
            public string stf4_email2 { get; set; }
        }

        public UserParaData UserData { get; set; }

        public UserMainModel()
        {
        }

        public void FormReset()
        {
            this.IsForced = false;
            this.IsPermissible = true;
            this.UserNM = string.Empty;
            this.UserMobile = string.Empty;
            this.UserExtension = string.Empty;
            this.UserPWD = string.Empty;
            this.UserNewPWD = string.Empty;
            this.UserNewPWDCheck = string.Empty;
        }

        EntityUserMain.UserMain _entityUserMain;
        public EntityUserMain.UserMain EntityUserMain { get { return _entityUserMain; } }

        public bool GetUserMain()
        {
            try
            {
                EntityUserMain.UserMainPara para = new EntityUserMain.UserMainPara()
                {
                    UserID = new DBVarChar(this.UserID),
                };

                _entityUserMain = new EntityUserMain(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserMain(para);

                if (_entityUserMain != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public EnumValidateUserPWD ValidateUserPWD()
        {
            EnumValidateUserPWD result = EnumValidateUserPWD.Correctly;

            if (!string.IsNullOrWhiteSpace(this.UserPWD) ||
                !string.IsNullOrWhiteSpace(this.UserNewPWD) ||
                !string.IsNullOrWhiteSpace(this.UserNewPWDCheck))
            {
                if (_entityUserMain != null && _entityUserMain.UserPWD.GetValue() != Security.Encrypt(this.UserPWD))
                    result = EnumValidateUserPWD.PWDIncorrectly;

                if (this.UserNewPWD != this.UserNewPWDCheck)
                    result = EnumValidateUserPWD.NewPWDIncorrectly;
            }

            return result;
        }

        public bool GetPWDEffectiveResult()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.UserNewPWD))
                {
                    MongoUserMain.LogUserPWDPara para = new MongoUserMain.LogUserPWDPara()
                    {
                        UserID = new DBVarChar(string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID),
                        ModifyDate = new DBChar(Common.GetDateString())
                    };

                    List<MongoUserMain.LogUserPWD> entityLogUserPWDList = new MongoUserMain(this.ConnectionStringMSERP, this.ProviderNameMSERP)
                        .SelectUserPastPWDList(para);

                    if (entityLogUserPWDList != null && entityLogUserPWDList.Count > 0)
                    {
                        foreach (MongoUserMain.LogUserPWD entityLogUserPWD in entityLogUserPWDList)
                        {
                            if (entityLogUserPWD.UserPWD.GetValue() == Security.Encrypt(this.UserNewPWD))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public EnumModifyResult GetEditUserMainResult(string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string newPWD = string.Empty;
                if (!string.IsNullOrWhiteSpace(this.UserPWD) && !string.IsNullOrWhiteSpace(this.UserNewPWD) &&
                    !string.IsNullOrWhiteSpace(this.UserNewPWDCheck))
                {
                    this.PWDValidDate = Common.GetDateString(DateTime.Now.AddMonths(3));
                    newPWD = Security.Encrypt(this.UserNewPWD);
                }
                
                EntityUserMain.UserMainPara para = new EntityUserMain.UserMainPara()
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID),
                    UserNM = new DBNVarChar(string.IsNullOrWhiteSpace(this.UserNM) ? null : this.UserNM),
                    UserPWD = new DBVarChar(string.IsNullOrWhiteSpace(newPWD) ? null : newPWD),
                    //UserRawPWD = new DBVarChar(string.IsNullOrWhiteSpace(this.UserNewPWD) ? null : this.UserNewPWD),
                    PWDValidDate = new DBVarChar(string.IsNullOrWhiteSpace(this.PWDValidDate) ? null : this.PWDValidDate),

                    UserENM = new DBNVarChar(string.IsNullOrWhiteSpace(this.UserENM) ? null : this.UserENM),
                    UserIDNO = new DBVarChar(null),
                    UserBirthday = new DBChar(null),
                    UserTel = new DBVarChar(string.IsNullOrWhiteSpace(this.UserTel) ? null : this.UserTel),
                    UserExtension = new DBVarChar(string.IsNullOrWhiteSpace(this.UserExtension) ? null : this.UserExtension),
                    UserMoblie = new DBVarChar(string.IsNullOrWhiteSpace(this.UserMobile) ? null : this.UserMobile),
                    UserEMail = new DBVarChar(string.IsNullOrWhiteSpace(this.UserEMail) ? null : this.UserEMail),
                    UserGoogleAccount = new DBVarChar(string.IsNullOrWhiteSpace(this.UserGoogleAccount) ? null : this.UserGoogleAccount),
                    IsGAccEnable = new DBChar(string.IsNullOrWhiteSpace(this.IsGAccEnable) ? EnumYN.N.ToString() : this.IsGAccEnable),

                    UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)
                };

                if (new EntityUserMain(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditUserMain(para) == LionTech.Entity.ERP.Sys.EntityUserMain.EnumEditUserMainResult.Success)
                {
                    if (!string.IsNullOrWhiteSpace(newPWD))
                    {
                        this.GetRecordUserPWDResult(newPWD, updUserID, ipAddress, cultureID);
                    }

                    return EnumModifyResult.Success;

                    //if (new EntityUserMain(this.ConnectionStringERP, this.ProviderNameERP)
                    //    .SyncEditOpagm(para) == LionTech.Entity.ERP.Sys.EntityUserMain.EnumSyncEditOpagmResult.Success)
                    //{
                    //    return EnumModifyResult.Success;
                    //}
                    //else
                    //{
                    //    return EnumModifyResult.SyncASPFailure;
                    //}
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return EnumModifyResult.Failure;
        }

        private bool GetRecordUserPWDResult(string userPWD, string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara para = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID)),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString())
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectBasicInfo(para);

                if (entityBasicInfo != null)
                {
                    Mongo_BaseAP.RecordUserPWDPara recordPara = new Mongo_BaseAP.RecordUserPWDPara()
                    {
                        UserID = entityBasicInfo.UserID,
                        UserNM = entityBasicInfo.UserNM,
                        UserPWD = new DBVarChar(string.IsNullOrWhiteSpace(userPWD) ? null : userPWD),
                        IsReset = new DBChar(EnumYN.N.ToString()),
                        ModifyDate = new DBChar(Common.GetDateString()),
                        APINo = new DBChar(null),
                        UpdUserID = entityBasicInfo.UpdUserID,
                        UpdUserNM = entityBasicInfo.UpdUserNM,
                        UpdDT = new DBDateTime(DateTime.Now),
                        ExecSysID = entityBasicInfo.ExecSysID,
                        ExecSysNM = entityBasicInfo.ExecSysNM,
                        ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress))
                    };

                    // if (new Mongo_BaseAP(this.ConnectionStringMSERP, this.ProviderNameMSERP)
                    //     .RecordUserPWD(recordPara) == Mongo_BaseAP.EnumRecordUserPWDResult.Success)
                    // {
                    //     return true;
                    // }
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
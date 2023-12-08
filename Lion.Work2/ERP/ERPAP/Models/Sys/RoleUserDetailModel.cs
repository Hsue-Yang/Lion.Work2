// 新增日期：2017-04-26
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class RoleUserDetailModel : SysModel, IValidatableObject
    {
        #region - Definitions -
        public enum EnumRoleUserOperate
        {
            Attached,
            Cover
        }

        public enum EnumAddType
        {
            Comma,
            Auto
        }

        public class RoleUserInfo
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
        }
        #endregion

        #region - Constructor -
        public RoleUserDetailModel()
        {
            _entity = new EntityRoleUserDetail(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string SysID { get; set; }
        public string SysNMID { get; set; }
        public string RoleID { get; set; }
        public string RoleNMID { get; set; }
        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ErpWFNo { set; get; }
        [StringLength(1000)]
        public string Memo { set; get; }
        public string RoleUserOperate { get; set; }
        public string AddType { get; set; }
        [RegularExpression(@"^([0-9a-zA-Z]{4,8},)*([0-9a-zA-Z]{4,8})$",
            ErrorMessageResourceType = typeof(SysRoleUserDetail),
            ErrorMessageResourceName = nameof(SysRoleUserDetail.SystemMsg_UserMemoFormate_Error))]
        [StringLength(1000)]
        public string UserMemo { get; set; }
        public List<RoleUserInfo> RoleUserInfoList { get; set; }

        private Dictionary<string, string> _roleUserOperateDictionary;

        public Dictionary<string, string> RoleUserOperateDictionary => _roleUserOperateDictionary ?? (_roleUserOperateDictionary = new Dictionary<string, string>
        {
            { EnumRoleUserOperate.Attached.ToString(), SysRoleUserDetail.Label_Attached },
            { EnumRoleUserOperate.Cover.ToString(), SysRoleUserDetail.Label_Cover }
        });

        private Dictionary<string, string> _addTypeDictionary;

        public Dictionary<string, string> AddTypeDictionary => _addTypeDictionary ?? (_addTypeDictionary = new Dictionary<string, string>
        {
            { EnumAddType.Comma.ToString(), SysRoleUserDetail.Label_Comma },
            { EnumAddType.Auto.ToString(), SysRoleUserDetail.Label_Auto }
        });
        #endregion

        #region - Private -
        private readonly EntityRoleUserDetail _entity;
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            #region - 驗證使用者必填 -
            if (AddType == EnumAddType.Auto.ToString())
            {
                if (RoleUserInfoList == null ||
                    RoleUserInfoList.Any() == false)
                {
                    yield return new ValidationResult(SysRoleUserDetail.SystemMsg_UserID_Required);
                }
            }
            else if (AddType == EnumAddType.Comma.ToString())
            {
                if (string.IsNullOrWhiteSpace(UserMemo))
                {
                    yield return new ValidationResult(SysRoleUserDetail.SystemMsg_UserID_Required);
                }
            }
            #endregion
        }
        #endregion

        public async Task FormReset(string userID, EnumCultureID cultureId)
        {
            RoleUserOperate = EnumRoleUserOperate.Attached.ToString();
            AddType = EnumAddType.Comma.ToString();

            var sysSystemMain = GetSysSystemMain(SysID, cultureId);
            SysNMID = sysSystemMain?.SysNMID;

            await GetSysSystemRoleIDList(SysID, userID, cultureId);
            var sysSystemRoleId = EntitySysSystemRoleIDList.SingleOrDefault(s => s.RoleID == RoleID);
            if (sysSystemRoleId != null)
            {
                RoleNMID = $"{sysSystemRoleId.RoleNM} ({sysSystemRoleId.RoleID})" ;
            }
        }

        #region - 取得角色使用者權限設定參數 -
        /// <summary>
        /// 取得角色使用者權限設定參數
        /// </summary>
        /// <returns></returns>
        public string GetAuthRoleUserParaJsonString()
        {
            var result = new
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                RoleID = string.IsNullOrWhiteSpace(RoleID) ? null : RoleID,
                ErpWFNo = string.IsNullOrWhiteSpace(ErpWFNo) ? null : ErpWFNo,
                Memo = string.IsNullOrWhiteSpace(Memo) ? null : Memo,
                IsOverride = (RoleUserOperate == EnumRoleUserOperate.Cover.ToString()),
                UserIDList = (AddType == EnumAddType.Comma.ToString())
                    ? UserMemo.Split(',').ToList()
                    : RoleUserInfoList.Select(s => s.UserID).ToList()
            };

            return new JavaScriptSerializer().Serialize(result);
        }
        #endregion
    }
}
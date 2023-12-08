using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityPub : Entity_BaseAP
    {
        public EntityPub(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢工作流程類型 -
        public class PubFlowTypePara : DBCulture
        {
            public PubFlowTypePara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class PubFlowType : DBTableRow, ISelectItem
        {
            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 查詢工作流程類型
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<PubFlowType> SelectPubFlowTypeList(PubFlowTypePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    "SELECT CODE_ID AS CodeID ",
                    "     , dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CodeNM ",
                    "  FROM CM_CODE ",
                    " WHERE CODE_KIND = {WorkFlowType} ",
                    " ORDER BY SORT_ORDER "
                    );

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = PubFlowTypePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(PubFlowTypePara.ParaField.CODE_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = EnumCMCodeKind.WorkFlowType, Value = new DBVarChar(Common.GetEnumDesc(EnumCMCodeKind.WorkFlowType)) });
            return GetEntityList<PubFlowType>(commandText, dbParameters);
        }
        #endregion

        #region - 查詢使用者使用工作流程清單 -
        public class PubUserSystemWorkFlowIDPara : DBCulture
        {
            public PubUserSystemWorkFlowIDPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                USER_ID,
                FLOW_TYPE,
                WF_FLOW
            }

            public DBVarChar UserID;
            public DBVarChar FlowType;
        }

        public class PubUserSystemWorkFlowID : DBTableRow, ISelectItem
        {
            public DBVarChar WFFlowValue;
            public DBNVarChar WFFlowText;

            public string ItemText()
            {
                return WFFlowText.StringValue();
            }

            public string ItemValue()
            {
                return WFFlowValue.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 查詢使用者使用工作流程清單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<PubUserSystemWorkFlowID> SelectPubUserSystemWorkFlowIDList(PubUserSystemWorkFlowIDPara para)
        {
            string commandText =
                string.Concat(new object[]
                              {
                                  "SELECT DISTINCT (S.SYS_ID + '|' + S.WF_FLOW_ID + '|' + S.WF_FLOW_VER) AS WFFlowValue ",
                                  "     , dbo.FN_GET_NMID(S.SYS_ID + '/' +S.WF_FLOW_ID, S.{WF_FLOW}) + '-' + S.WF_FLOW_VER AS WFFlowText ",
                                  "     , M.SORT_ORDER",
                                  "     , S.SORT_ORDER",
                                  "     , S.WF_FLOW_ID",
                                  "     , S.WF_FLOW_VER",
                                  "  FROM SYS_SYSTEM_WF_FLOW S ",
                                  "  JOIN SYS_SYSTEM_MAIN M ",
                                  "    ON S.SYS_ID = M.SYS_ID ",
                                  "  JOIN SYS_SYSTEM_ROLE_FLOW R ",
                                  "    ON S.SYS_ID = R.SYS_ID ",
                                  "   AND S.WF_FLOW_ID = R.WF_FLOW_ID ",
                                  "   AND S.WF_FLOW_VER = R.WF_FLOW_VER ",
                                  "  JOIN SYS_USER_SYSTEM_ROLE U ",
                                  "    ON U.USER_ID = {USER_ID} ",
                                  "   AND R.SYS_ID = U.SYS_ID ",
                                  "   AND R.ROLE_ID = U.ROLE_ID ",
                                  " WHERE S.ENABLE_DATE <= dbo.FN_GET_SYSDATE(NULL) ",
                                  "   AND ISNULL(S.DISABLE_DATE,'99999999') > dbo.FN_GET_SYSDATE(NULL) ",
                                  "   AND S.FLOW_TYPE = {FLOW_TYPE} ",
                                  " ORDER BY M.SORT_ORDER",
                                  "     , S.SORT_ORDER",
                                  "     , S.WF_FLOW_ID",
                                  "     , S.WF_FLOW_VER"
                              });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = PubUserSystemWorkFlowIDPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = PubUserSystemWorkFlowIDPara.ParaField.FLOW_TYPE.ToString(), Value = para.FlowType });
            dbParameters.Add(new DBParameter { Name = PubUserSystemWorkFlowIDPara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(PubUserSystemWorkFlowIDPara.ParaField.WF_FLOW.ToString())) });
            return GetEntityList<PubUserSystemWorkFlowID>(commandText, dbParameters);
        }
        #endregion
        
        #region - 查詢工作流程資訊 -
        public class PubWorkFlowInfoPara : DBCulture
        {
            public PubWorkFlowInfoPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                WF_NO,
                CULTURE_ID,
                WF_FLOW
            }

            public DBChar WF_NO;
        }

        public class PubWorkFlowInfo : DBTableRow
        {
            public DBChar WFNo;
            public DBNVarChar WFSubject;
            public DBVarChar ResultID;
            public DBNVarChar ResultNM;
        }

        /// <summary>
        /// 查詢工作流程資訊
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public PubWorkFlowInfo SelectPubWorkFlowInfo(PubWorkFlowInfoPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT WF.WF_NO AS WFNo ",
                    "     , WF.WF_SUBJECT AS WFSubject ",
                    "     , WF.RESULT_ID AS ResultID ",
                    "     , dbo.FN_GET_NMID(WF.RESULT_ID, dbo.FN_GET_CM_NM({WorkFlowResultType}, WF.RESULT_ID, {CULTURE_ID})) AS ResultNM ",
                    "  FROM WF_FLOW WF",
                    "  JOIN SYS_SYSTEM_WF_FLOW SWF",
                    "    ON WF.SYS_ID = SWF.SYS_ID",
                    "   AND WF.WF_FLOW_ID = SWF.WF_FLOW_ID",
                    "   AND WF.WF_FLOW_VER = SWF.WF_FLOW_VER",
                    " WHERE WF.WF_NO = {WF_NO}"
                }));

            dbParameters.Add(new DBParameter { Name = PubWorkFlowInfoPara.ParaField.WF_NO, Value = para.WF_NO });
            dbParameters.Add(new DBParameter { Name = PubWorkFlowInfoPara.ParaField.CULTURE_ID, Value = new DBVarChar(para.CultureID) });
            dbParameters.Add(new DBParameter { Name = PubWorkFlowInfoPara.ParaField.WF_FLOW, Value = para.GetCultureFieldNM(new DBObject(PubWorkFlowInfoPara.ParaField.WF_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = EnumCMCodeKind.WorkFlowResultType, Value = new DBVarChar(Common.GetEnumDesc(EnumCMCodeKind.WorkFlowResultType)) });
            return GetEntityList<PubWorkFlowInfo>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

        #region - 查詢員工資料 -
        public class RawCMUserPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public List<DBVarChar> UserIDList;
        }

        public class RawCMUser : DBTableRow, IExtendedSelectItem
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;

            public string Background()
            {
                return null;
            }

            public string GroupBy()
            {
                return UserID.GetValue();
            }

            public string ItemText()
            {
                return UserNM.GetValue();
            }

            public string ItemValue(string key)
            {
                return UserID.GetValue();
            }

            public string ItemValue()
            {
                return UserID.GetValue();
            }

            public string PictureUrl()
            {
                return null;
            }
        }

        /// <summary>
        /// 查詢員工資料
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<RawCMUser> SelectRawCMUserList(RawCMUserPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT USER_ID AS UserID",
                    "     , USER_NM AS UserNM",
                    "  FROM RAW_CM_USER",
                    " WHERE USER_ID IN ({USER_ID})"
                }));

            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_ID, Value = para.UserIDList });
            return GetEntityList<RawCMUser>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢工作流程簽核角色 -
        public class WFNodeSigUserRolePara
        {
            public enum ParaField
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID, WF_SIG_SEQ
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar WFSigSeq;
        }

        /// <summary>
        /// 查詢工作流程簽核角色
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<RawCMUser> SelectWFNodeSigUserRoleList(WFNodeSigUserRolePara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT DISTINCT SR.USER_ID AS UserID",
                    "     , RCU.USER_NM AS UserNM",
                    "  FROM SYS_SYSTEM_ROLE_SIG WS",
                    "  JOIN SYS_USER_SYSTEM_ROLE SR",
                    "    ON WS.SYS_ID = SR.SYS_ID",
                    "   AND WS.ROLE_ID = SR.ROLE_ID",
                    "  JOIN RAW_CM_USER RCU",
                    "    ON SR.USER_ID = RCU.USER_ID",
                    " WHERE WS.SYS_ID = {SYS_ID}",
                    "   AND WS.WF_FLOW_ID = {WF_FLOW_ID}",
                    "   AND WS.WF_FLOW_VER = {WF_FLOW_VER}",
                    "   AND WS.WF_NODE_ID = {WF_NODE_ID}",
                    "   AND WS.WF_SIG_SEQ = {WF_SIG_SEQ}"
                }));

            dbParameters.Add(new DBParameter { Name = WFNodeSigUserRolePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = WFNodeSigUserRolePara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = WFNodeSigUserRolePara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = WFNodeSigUserRolePara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = WFNodeSigUserRolePara.ParaField.WF_SIG_SEQ.ToString(), Value = para.WFSigSeq });
            return GetEntityList<RawCMUser>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}
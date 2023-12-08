using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowChartModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID,
            WFFlowGroupID,
            WFCombineKey,
            WFFlowID,
            WFFlowVer
        }

        private enum EnumGraphicPath
        {
            [Description(@"~\Content\images\")]
            BasicCanvas
        }
        #endregion

        #region - Constructor -
        public SystemWorkFlowChartModel()
        {
            _entity = new EntitySystemWorkFlowChart(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        public class SystemWorkFlowNodePosition
        {
            public string WFNodeID { get; set; }
            public int NodePOSBeginX { get; set; }
            public int NodePOSBeginY { get; set; }
            public int NodePOSEndX { get; set; }
            public int NodePOSEndY { get; set; }
        }

        public class SystemWorkFlowArrowPosition
        {
            public string WFNodeID { get; set; }
            public string NextWFNodeID { get; set; }
            public int ArrowPOSBeginX { get; set; }
            public int ArrowPOSBeginY { get; set; }
            public int ArrowPOSEndX { get; set; }
            public int ArrowPOSEndY { get; set; }
        }

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        public string WFFlowGroupID { get; set; }

        [Required]
        public string WFCombineKey { get; set; }

        public string WFFlowID { get; set; }

        public string WFFlowVer { get; set; }

        public string FileDataURI { get; set; }

        public string NodePosition { get; set; }

        public string ArrowPosition { get; set; }

        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowChart _entity;
        #endregion

        #region - 表單初始 -
        /// <summary>
        /// 表單初始
        /// </summary>
        public void FormReset()
        {
            SysID = EnumSystemID.ERPAP.ToString();
            WFFlowGroupID = string.Empty;
            WFCombineKey = string.Empty;
            WFFlowID = string.Empty;
            WFFlowVer = string.Empty;
            FileDataURI = string.Empty;
            NodePosition = string.Empty;
            ArrowPosition = string.Empty;
        }
        #endregion

        #region - 取得工作流程結點位置 -
        /// <summary>
        /// 取得工作流程結點位置
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSystemWFNodePosition()
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;

                string apiUrl = API.SystemWorkFlowChart.QuerySystemWorkFlowNodePositions(SysID, WFFlowID, WFFlowVer);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<SystemWorkFlowNodePosition>());

                if (responseObj != null &&
                    responseObj.Count > 0)
                {
                    NodePosition = "[";
                    foreach (var systemWFNode in responseObj)
                    {
                        string node = string.Format("['{0}', {1}, {2}, {3}, {4}],",
                            systemWFNode.WFNodeID,
                            systemWFNode.NodePOSBeginX,
                            systemWFNode.NodePOSBeginY,
                            systemWFNode.NodePOSEndX,
                            systemWFNode.NodePOSEndY);
                        NodePosition = NodePosition + node;
                    }
                    NodePosition = NodePosition.TrimEnd(',') + "]";
                }
                else
                {
                    NodePosition = string.Empty;
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得工作流程指標位置 -
        /// <summary>
        /// 取得工作流程指標位置
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSystemWFArrowPosition()
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;

                string apiUrl = API.SystemWorkFlowChart.QuerySystemWorkFlowArrowPositions(SysID, WFFlowID, WFFlowVer);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<SystemWorkFlowArrowPosition>());

                if (responseObj != null &&
                    responseObj.Count > 0)
                {
                    ArrowPosition = "[";
                    foreach (var systemWFArrow in responseObj)
                    {
                        string arrow = string.Format("['{0}', '{1}', {2}, {3}, {4}, {5}],",
                            systemWFArrow.WFNodeID,
                            systemWFArrow.NextWFNodeID,
                            systemWFArrow.ArrowPOSBeginX,
                            systemWFArrow.ArrowPOSBeginY,
                            systemWFArrow.ArrowPOSEndX,
                            systemWFArrow.ArrowPOSEndY);
                        ArrowPosition = ArrowPosition + arrow;
                    }
                    ArrowPosition = ArrowPosition.TrimEnd(',') + "]";
                }

                if (string.IsNullOrWhiteSpace(ArrowPosition))
                {
                    ArrowPosition = string.Empty;
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得工作流程圖檔資料 -
        /// <summary>
        /// 取得工作流程圖檔資料
        /// </summary>
        /// <returns></returns>
        public bool GetFileDataURI()
        {
            try
            {
                string fullPath = Path.Combine(ConfigurationManager.AppSettings[EnumAppSettingKey.FilePath.ToString()],
                    Common.GetEnumDesc(EnumFilePathFolder.WorkFlow),
                    SysID,
                    string.Format("{0}.{1}.{2}.png", SysID, WFFlowID, WFFlowVer));

                if (File.Exists(fullPath) == false)
                {
                    fullPath = HttpContext.Current.Server.MapPath(
                        Path.Combine(Common.GetEnumDesc(EnumGraphicPath.BasicCanvas),
                            Common.GetEnumDesc(EnumFilePathFolder.WorkFlow),
                            string.Format("{0}.png", EnumGraphicPath.BasicCanvas)));
                }

                byte[] array = Common.FileReadByte(fullPath, 1024);
                FileDataURI = string.Format("{0},{1}", "data:image/png;base64", Convert.ToBase64String(array));

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

    }
}
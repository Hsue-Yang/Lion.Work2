using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;

namespace LionTech.EDIService.ERPExternal
{
    public class ERP_MENU_REBUILD
    {
        public static EnumJobResult ERP_USER_MENU_REBUILD(Flow flow, Job job)
        {
            Connection connection = flow.connections[job.connectionID];
            var exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;
            var userMenuFilePathFolder = job.parameters["UserMenuFilePathFolder"].value;

            if (string.IsNullOrWhiteSpace(userMenuFilePathFolder))
            {
                throw new EDIException(EnumEDIMessage.JobParameterIsNull, new[] { Job.GetID(flow, job), "UserMenuFilePathFolder" });
            }

            List<EntityERPExternal.UserIDInfo> userIDList = _GetUserIDList(connection);

            foreach (var user in userIDList)
            {
                string userID = user.UserID.GetValue();

                try
                {
                    for (int menuNO = 1; menuNO <= 3; menuNO++)
                    {
                        for (int isOutMenu = 0; isOutMenu <= 1; isOutMenu++)
                        {
                            var userErpMenuList = _GetUserErpMenuList(connection, userID, menuNO, isOutMenu);
                            _GenerateErpMenuHtmFile(userID, menuNO.ToString(), isOutMenu, userMenuFilePathFolder, userErpMenuList);
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Write(exceptionPath, string.Concat(new object[] { "ERP_USER_MENU_REBUILD Exception UserID: ", userID }));
                    FileLog.Write(exceptionPath, ex);
                }
            }

            return EnumJobResult.Success;
        }

        #region - 取得員工編號清單 -
        /// <summary>
        /// 取得員工編號清單
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static List<EntityERPExternal.UserIDInfo> _GetUserIDList(Connection connection)
        {
            return new EntityERPExternal(connection.value, connection.providerName)
                .SelectUserIDList();
        }
        #endregion

        #region - 取得員工ERP選單清單 -
        /// <summary>
        /// 取得員工ERP選單清單
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="userID"></param>
        /// <param name="menuNO"></param>
        /// <param name="isOutMenu"></param>
        /// <returns></returns>
        private static List<EntityERPExternal.ERPUserMenu> _GetUserErpMenuList(Connection connection, string userID, int menuNO, int isOutMenu)
        {
            EntityERPExternal.ERPUserMenuPara para = new EntityERPExternal.ERPUserMenuPara
            {
                UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID),
                Menu_NO = new DBChar(menuNO),
                mm00_all_use = new DBChar(isOutMenu)
            };

            return new EntityERPExternal(connection.value, connection.providerName)
                .SelectERPUserMenuList(para);
        }
        #endregion

        #region - 產生Erp選單Htm檔案 -
        /// <summary>
        /// 產生Erp選單Htm檔案
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="menuNO"></param>
        /// <param name="isOutMenu"></param>
        /// <param name="path"></param>
        /// <param name="erpUserMenuList"></param>
        private static void _GenerateErpMenuHtmFile(string userID, string menuNO, int isOutMenu, string path, List<EntityERPExternal.ERPUserMenu> erpUserMenuList)
        {
            var htmlContent = _CreateMenuHtmlContent(erpUserMenuList);
            var external = Convert.ToBoolean(isOutMenu) ? "o" : string.Empty;
            var completePath = $@"{path}{userID}{menuNO}{external}.htm";
            File.WriteAllText(completePath, htmlContent);
        }
        #endregion

        #region - 建立選單Htm檔內容 -
        /// <summary>
        /// 建立選單Htm檔內容
        /// </summary>
        /// <param name="erpUserMenuList"></param>
        /// <returns></returns>
        private static string _CreateMenuHtmlContent(List<EntityERPExternal.ERPUserMenu> erpUserMenuList)
        {
            var menuItem = string.Join("$$ ",
                erpUserMenuList.Select(e => $"{e.tabl_cname.GetValue().Trim()}, {e.mm00_name.GetValue().Trim()}<br>, ../{e.mm00_sys1.GetValue()}/{e.mm00_aspid.GetValue()}.asp").ToList());
            var menuTitleNMList = erpUserMenuList.Select(e => e.tabl_cname.GetValue()).Distinct().ToList();
            var menuItemInterval = Math.Round(100 / Convert.ToDouble(menuTitleNMList.Count), 0, MidpointRounding.AwayFromZero);
            var htmlTd = string.Join(string.Empty,
                menuTitleNMList
                    .Select((value, index) => new { Value = value, Index = index })
                    .Select(td => $"<TD width='{menuItemInterval}%' valign='top'><A HREF='#' onMouseOver='JavaM({td.Index + 1})' onMouseOut='Hide({td.Index + 1})'>{td.Value}</A></TD>"));

            return $"<input type='hidden' name='item2' value='{menuItem}'>" +
                   "<input type='Hidden' name='sMenu_y' value='10'>" +
                   $"<input type='hidden' name='len' value='{menuItemInterval}'>" +
                   "<SPAN ID='menubar' STYLE='position:relative; visibility:hidden;'>" +
                   "<TABLE ID='table1' WIDTH='100%'  BORDER=0 CELLPADDING=0 CELLSPACING=0 class='Label'>" +
                   $"<tr  class='ApName' >{htmlTd}</TR>";
        }
        #endregion
    }
}
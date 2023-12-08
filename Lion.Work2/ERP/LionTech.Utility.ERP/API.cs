using LionTech.Entity;
using LionTech.Entity.ERP;
using System.Collections.Generic;

namespace LionTech.Utility.ERP
{
    public static class API
    {
        #region Sys
        public static class SystemSetting
        {
            public static string IsITManager(string sysID, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/IsITManager/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string CodeManagement(string userID, string codeKind, string cultureID, List<string> codeIDs = null, string codeParent = null, bool? isFormatNMID = null)
            {
                string condition_codeIDs = codeIDs == null || codeIDs.Count == 0 ? null : $"&codeIDs={string.Join("&codeIDs=", codeIDs)}";
                string condition_codeParent = string.IsNullOrEmpty(codeParent) ? null : $"&codeParent={codeParent}";
                string condition_isFormatNMID = isFormatNMID == null ? null : $"&isFormatNMID={isFormatNMID}";
                return $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/CodeManagement?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&codeKind={codeKind}&CultureID={cultureID}{condition_codeIDs}{condition_codeParent}{condition_isFormatNMID}";
            }

            public static string QuerySystemFilePath(string sysID, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/FilePath/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QueryAllSystemByIds(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/System/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QueryUserSystemByIds(string userID, bool isExcludeOutsourcing, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/UserSystem/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&isExcludeOutsourcing={isExcludeOutsourcing}&CultureID={cultureID}";

            public static string UserSystems(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/UserSystems?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemSetting(string sysID, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemSetting(string userID, string clientIPAddress) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&ClientIPAddress={clientIPAddress}";

            public static string DeleteSystemSetting(string sysID, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemIPs(string sysID, string userID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/{sysID}/SystemIPs?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string EditSystemIP(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemIP?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemIP(string sysID, string userID, string subSysID, string ipAddress) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemIP/{sysID}/{subSysID}/{ipAddress}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemServices(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/{sysID}/SystemServices?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string EditSystemService(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemService?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemService(string sysID, string userID, string subSysID, string serviceID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemService/{sysID}/{subSysID}/{serviceID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QueryUserSystemSubs(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/UserSystem/SystemSubs?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemSubByIds(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/{sysID}/SystemSub/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemSubs(string sysID, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/{sysID}/SystemSubs?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemSub(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemSub?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemSub(string sysID, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemSub/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemUsers(string sysID, string userID, string queryUserID, string queryUserNM, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/{sysID}/SystemUsers?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&queryUserID={queryUserID}&queryUserNM={queryUserNM}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QueryUserSystemSysIDs(string userID, bool excludeOutsourcing, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/UserSystemSysIDs/{userID}?ClientSysID={EnumAPISystemID.ERPAP}&excludeOutsourcing={excludeOutsourcing}&cultureID={cultureID}";

            public static string QuerySystemSysIDs(bool excludeOutsourcing, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemSysIDs?ClientSysID={EnumAPISystemID.ERPAP}&excludeOutsourcing={excludeOutsourcing}&cultureID={cultureID}";

            public static string QuerySystemRoleGroups(string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemRoleGroups?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";

            public static string QuerySystemConditionIDs(string sysID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemSetting/SystemConditionIDs?ClientSysID={EnumAPISystemID.ERPAP}&sysID={sysID}&cultureID={cultureID}";
        }

        public static class SystemRole
        {
            public static string QuerySystemRoleByIds(string sysID, string userID, string roleCategoryID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&roleCategoryID={roleCategoryID}&CultureID={cultureID}";

            public static string QuerySystemRole(string sysID, string userID, string roleID, string roleCategoryID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&roleID={roleID}&roleCategoryID={roleCategoryID}&CultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemRole(string sysID, string userID, string roleID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/{roleID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemRoleByCategory(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/Category?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string EditSystemRole(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemRole(string sysID, string userID, string roleID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/{roleID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemRoleUsers
        {
            public static string QuerySystemRoleUsers(string sysID, string roleID, string userID, string userNM, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/{roleID}/Users?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&userID={userID}&userNM={userNM}&pageIndex={pageIndex}&pageSize={pageSize}";
        }

        public static class SystemRoleFuns
        {
            public static string QuerySystemRoleFunsList(string sysID, string userID, string roleID, string funControllerId, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/{roleID}/Funs?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&userID={userID}&funControllerId={funControllerId}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string EditSystemRoleFunsList(string sysID, string roleID, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/{roleID}/Funs?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemRoleElms
        {
            public static string QuerySystemRoleElms(string userID, string sysID, string roleID, string cultureID, string funControllerId, string funactionNM, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/{roleID}/Elms?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&userID={userID}&cultureID={cultureID}&funControllerId={funControllerId}&funactionNM={funactionNM}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string EditSystemRoleElms(string userID, string sysID, string roleID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/{roleID}/Elms?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemFunElmIds(string userID, string sysID, string cultureID, string funControllerId, string funactionNM) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRole/{sysID}/{cultureID}/ElmIds?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&funControllerId={funControllerId}&funactionNM={funactionNM}";
        }

        public static class SystemRoleCategory
        {
            public static string QuerySystemRoleCategoryByIds(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCategory/{sysID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemRoleCategories(string sysID, string userID, string roleCategoryNM, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCategory/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&roleCategoryNM={roleCategoryNM}&CultureID={cultureID}";

            public static string QuerySystemRoleCategory(string sysID, string userID, string roleCategoryID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCategory/{sysID}/{roleCategoryID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&roleCategoryID={roleCategoryID}";

            public static string EditSystemRoleCategory(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCategory?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemRoleCategory(string sysID, string userID, string roleCategoryID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCategory/{sysID}/{roleCategoryID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemTeams
        {
            public static string QuerySystemTeamss(string sysID, string userID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemTeams/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemTeams(string sysID, string userID, string teamsChannelID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemTeams/{sysID}/{teamsChannelID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemTeams(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemTeams?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemTeams(string sysID, string userID, string teamsChannelID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemTeams/{sysID}/{teamsChannelID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemPurview
        {
            public static string QuerySystemPurviewByIds(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemPurview/{sysID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemPurviews(string sysID, string userID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemPurview/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemPurview(string sysID, string userID, string purviewID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemPurview/{sysID}/{purviewID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemPurview(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemPurview?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemPurview(string sysID, string userID, string purviewID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemPurview/{sysID}/{purviewID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemFunMenu
        {
            public static string QuerySystemFunMenuByIds(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunMenu/{sysID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemFunMenus(string sysID, string userID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunMenu/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemFunMenu(string sysID, string userID, string funMenu) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunMenu/{sysID}/{funMenu}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemFunMenu(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunMenu?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemFunMenu(string sysID, string userID, string funMenu) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunMenu/{sysID}/{funMenu}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemFunGroup
        {
            public static string QueryUserSystemFunGroups(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunGroup/UserAuthorization?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemFunGroupByIds(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunGroup/Ids?ClientSysID={EnumAPISystemID.ERPAP}&sysID={sysID}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemFunGroups(string sysID, string userID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunGroup/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemFunGroup(string sysID, string userID, string funControllerID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunGroup/{sysID}/{funControllerID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemFunGroup(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunGroup?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemFunGroup(string sysID, string userID, string funControllerID, string execSysID, string execIpAddress) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunGroup/{sysID}/{funControllerID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&execSysID={execSysID}&execIpAddress={execIpAddress}";
        }

        public static class SystemFun
        {
            public static string QueryUserSystemFuns(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/UserAuthorization?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemFuns(string sysID, string userID, string subSysID, string funControllerID, string funActionName, string funGroupNM, string funNM, string funMenuSysID, string funMenu, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&subSysID={subSysID}&funControllerID={funControllerID}&funActionName={funActionName}&funGroupNM={funGroupNM}&funNM={funNM}&funMenuSysID={funMenuSysID}&funMenu={funMenu}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemFun(string sysID, string userID, string funControllerID, string funActionName, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/{sysID}/{funControllerID}/{funActionName}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemFunRoles(string sysID, string userID, string funControllerID, string funActionName, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/{sysID}/Roles?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&funControllerID={funControllerID}&funActionName={funActionName}&CultureID={cultureID}";

            public static string EditSystemFunByPurview(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/Purview?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemFun(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemFun(string sysID, string userID, string funControllerID, string funActionName) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/{sysID}/{funControllerID}/{funActionName}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemMenuFuns(string sysID, string userID, string funControllerID, string funActionName) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/MenuFuns/{sysID}/{funControllerID}/{funActionName}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemFunNames(string sysID, string funControllerID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/FunNames?ClientSysID={EnumAPISystemID.ERPAP}&sysID={sysID}&funControllerID={funControllerID}&CultureID={cultureID}";

            public static string QuerySystemFunActions(string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFun/FunActions?ClientSysID={EnumAPISystemID.ERPAP}&CultureID={cultureID}";

        }

        public static class SystemAPIGroup
        {
            public static string QuerySystemAPIGroupByIds(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPIGroup/{sysID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemAPIGroups(string sysID, string userID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPIGroup/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemAPIGroup(string sysID, string userID, string apiGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPIGroup/{sysID}/{apiGroupID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemAPIGroup(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPIGroup?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemAPIGroup(string sysID, string userID, string apiGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPIGroup/{sysID}/{apiGroupID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemAPI
        {
            public static string QuerySystemAPIFullName(string sysID, string userID, string apiGroupID, string apiFunID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{sysID}/Names?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&apiGroupID={apiGroupID}&apiFunID={apiFunID}&CultureID={cultureID}";

            public static string QuerySystemAPIByIds(string sysID, string userID, string apiGroupID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{sysID}/{apiGroupID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemAPIs(string sysID, string userID, string apiGroupID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&apiGroupID={apiGroupID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemAPI(string sysID, string userID, string apiGroupID, string apiFunID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{sysID}/{apiGroupID}/{apiFunID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemAPIRoles(string sysID, string userID, string apiGroupID, string apiFunID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{sysID}/Roles?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&apiGroupID={apiGroupID}&apiFunID={apiFunID}&CultureID={cultureID}";

            public static string EditSystemAPI(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemAPI(string sysID, string userID, string apiGroupID, string apiFunID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{sysID}/{apiGroupID}/{apiFunID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemAPIAuthorizes(string sysID, string userID, string apiGroupID, string apiFunID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{sysID}/{apiGroupID}/{apiFunID}/Authorizes?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string EditSystemAPIAuthorize(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/Authorize?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemAPIAuthorize(string sysID, string userID, string apiGroupID, string apiFunID, string apiClientSysID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/Authorize/{sysID}/{apiGroupID}/{apiFunID}/{apiClientSysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemAPILogs(string sysID, string userID, string apiGroupID, string apiFunID, string apiClientSysID, string apiNo, string dtBegin, string dtEnd, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/Logs/{sysID}/{apiGroupID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&apiFunID={apiFunID}&apiClientSysID={apiClientSysID}&apiNo={apiNo}&dtBegin={dtBegin}&dtEnd={dtEnd}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemAPIClients(string sysID, string userID, string apiGroupID, string apiFunID, string dtBegin, string dtEnd, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{sysID}/{apiGroupID}/{apiFunID}/Logs?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&dtBegin={dtBegin}&dtEnd={dtEnd}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemAPIClient(string userID, string apiNo, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/{apiNo}/Log?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemAPIFuntions(string sysID, string apiControllerID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemAPI/Funtions?ClientSysID={EnumAPISystemID.ERPAP}&sysID={sysID}&apiControllerID={apiControllerID}&CultureID={cultureID}";
        }

        public static class SystemEventGroup
        {
            public static string QuerySystemEventGroupByIds(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEventGroup/{sysID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemEventGroups(string sysID, string userID, string eventGroupID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEventGroup/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&eventGroupID={eventGroupID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemEventGroup(string sysID, string userID, string eventGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEventGroup/{sysID}/{eventGroupID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemEventGroup(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEventGroup?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemEventGroup(string sysID, string userID, string eventGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEventGroup/{sysID}/{eventGroupID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemEvent
        {
            public static string QuerySystemEventFullName(string sysID, string userID, string eventGroupID, string eventID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/{sysID}/Names?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&eventGroupID={eventGroupID}&eventID={eventID}&CultureID={cultureID}";

            public static string QuerySystemEventByIds(string sysID, string userID, string eventGroupID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/{sysID}/{eventGroupID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string QuerySystemEvents(string sysID, string userID, string eventGroupID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&eventGroupID={eventGroupID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemEvent(string sysID, string userID, string eventGroupID, string eventID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/{sysID}/{eventGroupID}/{eventID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemEvent(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemEvent(string sysID, string userID, string eventGroupID, string eventID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/{sysID}/{eventGroupID}/{eventID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemEventTargets(string sysID, string userID, string eventGroupID, string eventID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/Target/{sysID}/{eventGroupID}/{eventID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";

            public static string EditSystemEventTarget(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/Target?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemEventTarget(string sysID, string userID, string eventGroupID, string eventID, string targetSysID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/Target/{sysID}/{eventGroupID}/{eventID}/{targetSysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemEventEDIs(string sysID, string userID, string targetSysID, string eventGroupID, string eventID, string dtBegin, string dtEnd, string isOnlyFail, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/EDI?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&sysID={sysID}&targetSysID={targetSysID}&eventGroupID={eventGroupID}&eventID={eventID}&dtBegin={dtBegin}&dtEnd={dtEnd}&isOnlyFail={isOnlyFail}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemEventTargetEDIs(string sysID, string userID, string targetSysID, string eventGroupID, string eventID, string dtBegin, string dtEnd, string isOnlyFail, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/TargetEDI/{sysID}/{eventGroupID}/{eventID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}&targetSysID={targetSysID}&dtBegin={dtBegin}&dtEnd={dtEnd}&isOnlyFail={isOnlyFail}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string ExcuteSubscription(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/ExcuteSubscription?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string QuerySystemEventTargetIDs(string sysID, string eventGroupID, string eventID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEvent/Target/IDs/{sysID}/{eventGroupID}/{eventID}?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class LatestUseIP
        {
            public static string QueryLatestUseIPInfoList(string userID, string ipAddress, string codeNM, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/LatestUseIP/LatestUseIPView?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&ipAddress={ipAddress}&codeNM={codeNM}&pageIndex={pageIndex}&pageSize={pageSize}";
        }

        public static class UserConnect
        {
            public static string QueryUserConnectList(string userID, string connectDTBegin, string connectDTEnd, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserConnect?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&connectDTBegin={connectDTBegin}&connectDTEnd={connectDTEnd}&pageIndex={pageIndex}&pageSize={pageSize}";
        }

        public static class SystemLineBot
        {
            public static string QuerySystemLineBotIDList(string userID, string sysID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/{sysID}/LineIds?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}";

            public static string QuerySystemLineBotAccountList(string userID, string sysID, string lineID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&lineID={lineID}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";

            public static string QuerySystemLineBotAccountDetail(string userID, string sysID, string lineID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/{sysID}/{lineID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string EditSystemLineBotAccountDetail(string userID, string sysID, string lineID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/{sysID}/{lineID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string DeleteSystemLineBotByIds(string userID, string sysID, string lineID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/{sysID}/{lineID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";

            public static string CheckSystemLineBotIdIsExists(string sysID, string lineID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/{sysID}/{lineID}/IsExists?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class SystemLineBotReceiver
        {
            public static string QuerySystemLineBotReceiver(string userID, string sysID, string lineID, string cultureID, string queryReceiverNM, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/Receivers/{sysID}/{lineID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}&queryReceiverNM={queryReceiverNM}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string QuerySystemLineBotReceiverDetail(string userID, string sysID, string receiverID, string lineID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/Receiver/{sysID}/{lineID}/{receiverID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}";
            public static string EditSystemineBotReceiverDetail(string userID, string sysID, string receiverID, string lineID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/Receiver/{sysID}/{lineID}/{receiverID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string CheckSystemLineBotReceiverIdIsExists(string sysID, string lineID, string lineReceiverID, string receiverID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemLineBot/Receiver/{sysID}/{lineID}/{receiverID}/IsExists?ClientSysID={EnumAPISystemID.ERPAP}&lineReceiverID={lineReceiverID}";
        }

        public static class UserPermission
        {
            public static string QueryUserPermissionList(string userID, string userNM, string restrictType, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserPermission?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&userID={userID}&userNM={userNM}&restrictType={restrictType}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string QueryUserRawData(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserPermission/{userID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class ADSReport
        {
            public static string QueryADSReportsCsv(string userID, string reportType, string sysID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/ADSReport/csv/{reportType}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&sysID={sysID}";
        }

        public static class SysLoginEvent
        {
            public static string QuerySysLoginEventById(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SysLoginEvent/{sysID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&CultureID={cultureID}";
            public static string QuerySysLoginEventSettings(string sysID, string userID, string cultureID, string logineventID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SysLoginEvent?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&sysID={sysID}&CultureID={cultureID}&logineventID={logineventID}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string QueryLoginEventSettingDetail(string sysID, string userID, string logineventID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SysLoginEvent/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&logineventID={logineventID}";
            public static string EditSysLoginEventSettingSort(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SysLoginEvent/SettingSort?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditLoginEventSettingDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SysLoginEvent?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteLoginEventSettingDetail(string sysID, string userID, string logineventID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SysLoginEvent/{sysID}/{logineventID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string CheckSystemLineBotIdIsExists(string sysID, string logineventID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SysLoginEvent/{sysID}/{logineventID}/IsExists?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class UserBasicInfo
        {
            public static string QueryUserBasicInfoList(string clientUserID, string userID, string userNM, string isDisable, string isLeft, string connectDTBegin, string connectDTEnd, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserBasicInfo?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={clientUserID}&userID={userID}&userNM={userNM}&isDisable={isDisable}&isLeft={isLeft}&connectDTBegin={connectDTBegin}&connectDTEnd={connectDTEnd}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string QueryUserBasicInfoDetail(string clientUserID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserBasicInfo/{userID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={clientUserID}&cultureID={cultureID}";
        }

        public static class SystemEDIFlow
        {
            public static string QuerySystemEDIFlows(string userID, string sysID, string schFrequency, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&sysID={sysID}&schFrequency={schFrequency}&cultureID={cultureID}";
            public static string EditSystemEDIFlowSortOrder(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/Sort?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemEDIIPAddress(string userID, string SysID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/{SysID}/IP?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemEDIFlowDetail(string userID, string SysID, string ediflowID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/{SysID}/{ediflowID}/Detail?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemEDIFlowDetails(string userID, string sysID, string ediflowID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/{sysID}/{ediflowID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemEDIFlowDetails(string userID, string sysID, string ediflowID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/{sysID}/{ediflowID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryFlowNewSortOrder(string userID, string sysID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/{sysID}/SortOrder?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemEDIFlowScheduleList(string userID, string sysID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/ScheduleList/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&sysID={sysID}&cultureID={cultureID}";
            public static string QuerySystemEDIFlowByIds(string sysID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Flow/Ids/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
        }

        public static class SystemEDIFlowLog
        {
            public static string QuerySystemEDIFlowsLogs(string userID, string sysID, string ediNO, string ediFlowID, string ediDate, string dataDate, string resultID, string statusID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/FlowLog/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&ediFlowID={ediFlowID}&ediNO={ediNO}&ediDate={ediDate}&dataDate={dataDate}&resultID={resultID}&statusID={statusID}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string EditSystemEDIFlowWaitStatusLog(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/FlowLog?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemEDIFlowLogSetting
        {
            public static string EditEDIFlowLogSetting(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/FlowLogSetting?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemEDICon
        {
            public static string QuerySystemEDICons(string userID, string sysID, string cultureID, string ediflowID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Con/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&ediflowID={ediflowID}&cultureID={cultureID}";
            public static string EditSystemEDIConSortOrder(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Con/Sort?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemEDIConDetail(string userID, string SysID, string ediflowID, string ediconID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Con/{SysID}/{ediflowID}/{ediconID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemEDIConDetails(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Con?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemEDIConDetails(string userID, string sysID, string ediflowID, string ediconID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Con/{sysID}/{ediflowID}/{ediconID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryConNewSortOrder(string userID, string sysID, string ediflowID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Con/{sysID}/{ediflowID}/Sort?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SysFunElm
        {
            public static string QuerySysFunElmList(string sysID, string userID, string isDisable, string elmID, string elmName, string funControllerID, string funActionName, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunElm/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}&isDisable={isDisable}&elmID={elmID}&elmName={elmName}&funControllerID={funControllerID}&funActionName={funActionName}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string CheckSystemFunElmIdIsExists(string sysID, string elmID, string funControllerID, string funActionName) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunElm/{sysID}/{funControllerID}/{funActionName}/{elmID}/IsExists?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QuerySystemFunElmDetail(string sysID, string userID, string elmID, string funControllerID, string funActionName) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunElm/{sysID}/{funControllerID}/{funActionName}/{elmID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemFunElmRoleList(string sysID, string userID, string elmID, string funControllerID, string funActionName, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunElm/{sysID}/{funControllerID}/{funActionName}/{elmID}/Roles?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}";
            public static string QuerySystemFunElmInfo(string sysID, string userID, string elmID, string funControllerID, string funActionName, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunElm/{sysID}/{funControllerID}/{funActionName}/{elmID}/Info?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}";
            public static string EditSystemFunElmDetail(string userID, string sysID, string elmID, string funControllerID, string funActionName) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunElm/{sysID}/{funControllerID}/{funActionName}/{elmID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemFunElmRole(string userID, string sysID, string elmID, string funControllerID, string funActionName) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunElm/{sysID}/{funControllerID}/{funActionName}/{elmID}/Roles?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemEDIJobLog
        {
            public static string QuerySystemEDIJobLogs(string userID, string sysID, string ediNO, string ediFlowID, string ediJobID, string ediFlowIDSearch, string ediJobIDSearch, string edidate, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/JobLog/{sysID}/{ediFlowID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&ediNO={ediNO}&ediJobID={ediJobID}&ediFlowIDSearch={ediFlowIDSearch}&ediJobIDSearch={ediJobIDSearch}&edidate={edidate}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";
        }

        public static class SystemEDIJob
        {
            public static string QuerySystemEDIJobs(string userID, string SysID, string ediFlowID, string ediJobType, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/{SysID}/{ediFlowID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&ediJobType={ediJobType}&cultureID={cultureID}";
            public static string QuerySystemEDIJobDetail(string userID, string SysID, string ediFlowID, string ediJobID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/{SysID}/{ediFlowID}/{ediJobID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemEDIJobByIds(string userID, string SysID, string ediFlowID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/{SysID}/{ediFlowID}/Ids?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}";
            public static string QueryJobMaxSortOrder(string userID, string SysID, string ediFlowID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/{SysID}/{ediFlowID}/SortOrder?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemEDIParas(string userID, string SysID, string ediFlowID, string ediJobID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/{SysID}/{ediFlowID}/{ediJobID}/Paras?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}";
            public static string EditSystemEDIJobSortOrder(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/Sort?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemEDIJobDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/Detail?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemEDIJobImport(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/Import?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemEDIJobDetail(string userID, string sysID, string ediFlowID, string ediJobID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/{sysID}/{ediFlowID}/{ediJobID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemEDIParaSortOrder(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/Para/Sort?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemEDIPara(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/Para?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemEDIPara(string userID, string sysID, string ediFlowID, string ediJobID, string ediJobParaID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/Para/{sysID}/{ediFlowID}/{ediJobID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&ediJobParaID={ediJobParaID}";
            public static string QuerySystemEDIConByIds(string userID, string sysID, string ediFlowID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/{sysID}/{ediFlowID}/ConByIds?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}";
            public static string QuerySystemEDIConByIdsProviderCons(string userID, string sysID, string ediFlowID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemEDI/Job/{sysID}/{ediFlowID}/ByIdsProvider?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}";
        }

        public static class SystemWorkFlow
        {
            public static string QuerySystemWorkFlow(string userID, string sysID, string wfFlowGroupID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&wfFlowGroupID={wfFlowGroupID}&cultureID={cultureID}";
            public static string QuerySystemWorkFlowDetail(string userID, string sysID, string wfFlowID, string wfFlowVer) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Detail/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string CheckSystemWorkFlowExists(string sysID, string wfFlowID, string wfFlowVer) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/IsExists/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QuerySystemWorkFlowRoles(string userID, string sysID, string wfFlowID, string wfFlowVer, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Roles/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&wfFlowID={wfFlowID}&wfFlowVer={wfFlowVer}&cultureId={cultureID}";
            public static string EditSystemWorkFlowDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Detail/SystemWorkFlowDetail?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemWorkFlowDetail(string userID, string sysID, string wfFlowID, string wfFlowVer) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Detail/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryCustomMsgFunAction(EnumAPISystemID sysID, string funControllerID, string funActionName, string wfno, string msgContentType) => $"{Common.GetEnumDesc(sysID)}/{funControllerID}/{funActionName}?ClientSysID={EnumAPISystemID.ERPAP}&WFNo={wfno}&MsgContentType={msgContentType}";
        }

        public static class SystemWorkFlowGroup
        {
            public static string QuerySystemWorkFlowGroup(string userID, string sysID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Group/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&cultureID={cultureID}";
            public static string QuerySystemWorkFlowGroupDetail(string userID, string sysID, string wfFlowGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Group/Detail/{sysID}/{wfFlowGroupID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemWorkFlowGroupDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Group/Detail/SystemWorkFlowGroupDetail?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string CheckSystemWorkFlowGroupIsExists(string sysID, string wfFlowGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Group/IsExists/{sysID}/{wfFlowGroupID}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string CheckSystemWorkFlowIsExists(string sysID, string wfFlowGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Group/Flow/IsExists/{sysID}/{wfFlowGroupID}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string DeleteSystemWorkFlowGroupDetail(string userID, string sysID, string wfFlowGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Group/Detail/{sysID}/{wfFlowGroupID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemWorkFlowGroupIDs(string sysID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Group/IDs/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
        }

        public static class SystemWorkFlowNode
        {
            public static string QuerySystemWorkFlowNodes(string userID, string sysID, string cultureID, string wfFlowID, string wfFlowVer) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Node/{sysID}/{cultureID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&cultureID={cultureID}";
            public static string QuerySysUserSystemWorkFlowID(string userID, string sysID, string cultureID, string wfFlowGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Node/ByFlowID/{sysID}/{userID}/{cultureID}/{wfFlowGroupID}?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&cultureID={cultureID}";
            public static string QuerySystemWorkFlowNode(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlow/Node/Info/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
        }

        public static class SystemWorkFlowNodeDetail
        {
            public static string QuerySystemWorkFlowName(string sysID, string wfFlowID, string wfFlowVer, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/SystemWorkFlowName/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string QueryBackSystemWorkFlowNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/BackSystemWorkFlowNodes/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&wfNodeID={wfNodeID}&cultureID={cultureID}";
            public static string QuerySystemWorkFlowNodeRoles(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/Roles/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&wfNodeID={wfNodeID}&cultureID={cultureID}";
            public static string QuerySystemWorkFlowNode(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&wfNodeID={wfNodeID}";
            public static string QuerySystemWorkFlowNodes(string sysID, string wfFlowID, string wfFlowVer) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/List/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string EditSystemWorkFlowNodeDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string CheckWorkFlowChildsIsExist(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/{sysID}/{wfFlowID}/{wfFlowVer}/ChildsIsExists?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string DeleteSystemWorkFlowNodeDetail(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&wfNodeID={wfNodeID}&ClientUserID={userID}";
            public static string CheckWorkFlowHasRunTime(string sysID, string wfFlowID, string wfFlowVer) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/CheckRunTime/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QuerySystemWorkFlowNodeIDs(string sysID, string wfFlowID, string wfFlowVer, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNode/IDs/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
        }

        public static class SystemWorkFlowSignature
        {
            public static string QuerySystemWorkFlowSignatures(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowSig/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string EditSystemWorkFlowNode(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowSig?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemWorkFlowSignatureSeqs(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowSig/Seqs/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string QuerytSystemWorkFlowSignatureDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowSig/Detail/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}/{wfSigSeq}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string QuerytSystemRoleSignatures(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowSig/Roles/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&wfSigSeq={wfSigSeq}&cultureID={cultureID}";
            public static string InsertSystemWorkFlowSignatureDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowSig/Detail?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemWorkFlowSignatureDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowSig/Detail?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemWorkFlowSignatureDetail(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowSig/Detail/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&wfSigSeq={wfSigSeq}&ClientUserID={userID}";
        }

        public static class UserRoleFun
        {
            public static string QueryUserRoleFunGroups(string userID, string userNM, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserRoleFun?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&userNM={userNM}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string QueryUserMainInfo(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserRoleFun/UserMainInfo/{userID}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QuerySystemRoleGroupCollects(string roleGroupID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserRoleFun/SystemRoleGroupCollects/{roleGroupID}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QueryUserSystemRoles(string userID, string updUserID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserRoleFun/UserSystemRoles/{userID}/{updUserID}/{cultureID}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string EditUserSystemRole(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserRoleFun?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemUserPurview
        {
            public static string QuerySysUserPurviews(string userID, string updUserID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserPurview?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&updUserID={updUserID}&cultureID={cultureID}";
            public static string QueryPurviewNames(string sysID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserPurview/Detail?ClientSysID={EnumAPISystemID.ERPAP}&sysID={sysID}&cultureID={cultureID}";
            public static string QuerySysUserPurviewDetails(string sysID, string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserPurview/Detail/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&cultureID={cultureID}";
            public static string EditSysUserPurviewDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserPurview/Detail?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class RoleUser
        {
            public static string QueryRoleUsers(string sysID, string roleID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/RoleUser?ClientSysID={EnumAPISystemID.ERPAP}&sysID={sysID}&roleID={roleID}&cultureID={cultureID}";
        }

        public static class SystemRoleCondition
        {
            public static string QuerySystemRoleConditions(string roleConditionID, string roleID, string sysID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCondition?ClientSysID={EnumAPISystemID.ERPAP}&roleConditionID={roleConditionID}&roleID={roleID}&sysID={sysID}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string QuerySystemRoleConditionDetail(string sysID, string roleConditionID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCondition/Detail/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&roleConditionID={roleConditionID}";
            public static string EditSystemRoleConditionDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCondition/Detail?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemRoleConditionDetail(string sysID, string roleConditionID, string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCondition/Detail/{sysID}/{roleConditionID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            //public static string QuerySystemRoleConditionDetailMongoDB(string sysID, string roleConditionID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCondition/Detail/Mongo/{sysID}/{roleConditionID}?ClientSysID={EnumAPISystemID.ERPAP}";
            //public static string DeleteSystemRoleCondotionDetailMongoDB(string sysID, string roleConditionID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCondition/Detail/Mongo/{sysID}/{roleConditionID}?ClientSysID={EnumAPISystemID.ERPAP}";
            //public static string InsertSystemRoleCondotionDetailMongoDB() => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRoleCondition/Detail/Mongo?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class SystemWorkFlowNext
        {
            public static string QuerySystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNext/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string QuerySystemWFNodeList(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nodeTypeListstr, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNext/SystemWFNodes/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&nodeTypeListstr={nodeTypeListstr}&cultureID={cultureID}";
            public static string QuerySystemWFNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNext/WFNext/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&nextWFNodeID={nextWFNodeID}&cultureID={cultureID}";
            public static string EditSystemWFNext(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNext?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemWFNext(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowNext/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&nextWFNodeID={nextWFNodeID}&ClientUserID={userID}";
        }

        public static class SystemWorkFlowDocument
        {
            public static string QuerySystemWorkFlowDocuments(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowDoc/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string QuerySystemWorkFlowDocumentDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowDoc/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}/{wfDocSeq}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string InsertSystemWorkFlowDocument(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowDoc?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemWorkFlowDocument(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowDoc?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemWorkFlowDocument(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowDoc/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}/{wfDocSeq}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemFunAssign
        {
            public static string QuerySystemFunAssigns(string sysID, string funControllerID, string funActionName) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunAssign/{sysID}/{funControllerID}/{funActionName}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string EditSystemFunAssign(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunAssign?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryFunRawDatas(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunAssign/FunRawDatas?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&cultureID={cultureID}";
            public static string QueryUserRawDatas() => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunAssign/UserRawDatas?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class SystemWorkFlowChart
        {
            public static string QuerySystemWorkFlowNodePositions(string sysID, string wfFlowID, string wfFlowVer) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowChart/NodePositions/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QuerySystemWorkFlowArrowPositions(string sysID, string wfFlowID, string wfFlowVer) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemWorkFlowChart/ArrowPositions/{sysID}/{wfFlowID}/{wfFlowVer}?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class UserSystem
        {
            public static string QueryUserStatusList(string userID, string userNM, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserSystem?ClientSysID={EnumAPISystemID.ERPAP}&userID={userID}&userNM={userNM}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string QueryUserRawData(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserSystem/Detail/{userID}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QueryUserOutSourcingSystemRoles(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserSystem/Role/{userID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string EditUserOutSourcingSystemRoles() => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserSystem?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class SystemRecord
        {
            public static string QuerySystemRecord(string collectionNM, string userID, string sysID, string userIDListStr, string roleConditionID, string lineID, string funControllerID, string funActionName, string wfNo, string logNo, string sUpdDT, string eUpdDT) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemRecord?ClientSysID={EnumAPISystemID.ERPAP}&collectionNM={collectionNM}&userID={userID}&sysID={sysID}&userIDListStr={userIDListStr}&roleConditionID={roleConditionID}&lineID={lineID}&funControllerID={funControllerID}&funActionName={funActionName}&wfNo={wfNo}&logNo={logNo}&sUpdDT={sUpdDT}&eUpdDT={eUpdDT}";
        }

        public static class UserFunction
        {
            public static string QueryUserFunctions(string userID, string updUserID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserFunction/{userID}/{updUserID}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string EditUserFunction(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/UserFunction?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class SystemFunToolSetting
        {
            public static string QuerySystemFunToolSettings(string userID, string sysID, string funControllerID, string funActionName, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunToolSetting/{userID}/{sysID}/{funControllerID}/{funActionName}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string QuerySystemFunToolControllerIDs(string sysID, string condition, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunToolSetting/ControllerIDs/{sysID}?ClientSysID={EnumAPISystemID.ERPAP}&condition={condition}&cultureID={cultureID}";
            public static string QuerySystemFunToolFunNames(string sysID, string funControllerID, string condition, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunToolSetting/FunNames/{sysID}/{funControllerID}?ClientSysID={EnumAPISystemID.ERPAP}&condition={condition}&cultureID={cultureID}";
            public static string EditSystemFunToolSetting() => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunToolSetting?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string DeleteSystemFunToolSetting(string userID, string sysID, string funControllerID, string funActionName) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunToolSetting/{userID}/{sysID}/{funControllerID}/{funActionName}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string CopySystemUserFunTool(string toolNO) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunToolSetting/CopyFunTool/{toolNO}?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class TrustIP
        {
            public static string QueryTrustIPs() => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/TrustIP?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QueryTrustIPDetail(string IPBegin, string IPEnd, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/TrustIP/Detail/{IPBegin}/{IPEnd}?ClientSysID={EnumAPISystemID.ERPAP}&cultureID={cultureID}";
            public static string QueryValidTrustIPRepeated() => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/TrustIP/Valid?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string EditTrustIP() => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/TrustIP?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string DeleteTrustIP(string IPBegin, string IPEnd) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/TrustIP/{IPBegin}/{IPEnd}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string InsertTrustIPMongoDB() => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/TrustIP/MongoDB?ClientSysID={EnumAPISystemID.ERPAP}";
        }

        public static class SystemFunToolPara
        {
            public static string QuerySystemFunToolParaForms(string userID, string sysID, string funControllerID, string funActionName, string toolNo, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunToolPara/Forms/{userID}/{sysID}/{funControllerID}/{funActionName}/{toolNo}/{cultureID}?ClientSysID={EnumAPISystemID.ERPAP}";
            public static string QuerySystemFunToolParas(string userID, string sysID, string funControllerID, string funActionName, string toolNo, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemFunToolPara/{userID}/{sysID}/{funControllerID}/{funActionName}/{toolNo}?ClientSysID={EnumAPISystemID.ERPAP}&pageIndex={pageIndex}&pageSize={pageSize}";
        }

        public static class SystemCultureSetting
        {
            public static string QuerySystemCultureIDs(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemCultureSetting/IDs?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySystemCultures(string userID, string cultureID, int pageIndex, int pageSize) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemCultureSetting?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}&pageIndex={pageIndex}&pageSize={pageSize}";
            public static string QuerySystemCultureDetail(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemCultureSetting/{cultureID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string EditSystemCultureDetail(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemCultureSetting?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteSystemCultureDetail(string userID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemCultureSetting/{cultureID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string GenerateCultureJsonFile(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/SystemCultureSetting/GenerateCultureJsonFile?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
        }

        public static class Raw
        {
            public static string QueryRawUsers(string userID, string condition, int limit) => $"{Common.GetEnumDesc(EnumAPISystemID.SYSMGTAP)}/v1/Raw/RawUsers/?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&condition={condition}&limit={limit}";
        }
        #endregion

        #region Pub
        public static class Pub
        {
            public static string EditRemark(string userID, string sysID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/Remark?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryRemarks(string userID, string wfNo, string cultureID, string workFlowResultType, string signatureResultType) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/Remark/{wfNo}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}&workFlowResultType={workFlowResultType}&signatureResultType={signatureResultType}";
            public static string QuerySignatureInfo(string userID, string wfNo, string nodeNo, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/SetSignature/SignatureInfo/{wfNo}/{nodeNo}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&sysID={sysID}&wfFlowID={wfFlowID}&wfFlowVer={wfFlowVer}&wfNodeID={wfNodeID}&cultureID={cultureID}";
            public static string CheckDocIsExists(string userID, string wfNo) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/SetSignature/IsExists/{wfNo}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryNodeSignCheckAPI(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/SetSignature/WFNodeSignCheckAPI/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QuerySigMemo(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/SetSignature/SigMemo/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&cultureID={cultureID}";
            public static string QueryRawCMUsers(List<string> userID)
            {
                string condition_userID = (userID == null || userID.Count == 0) ? null : $"&userID={string.Join("&userID=", userID)}";
                return $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/RawCMUsers?ClientSysID={EnumAPISystemID.ERPAP}?userID={condition_userID}";
            }
            public static string QueryNodeSigUserRoles(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/SetSignature/WFNodeSigUserRoles/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}/{wfSigSeq}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryDocuments(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfNo, string IsDelete, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/Documents/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}/{wfNo}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&IsDelete={IsDelete}&cultureID={cultureID}";
            public static string QueryDocumentRemarks(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNo, string IsDelete, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/Documents/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNo}/Remarks?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&IsDelete={IsDelete}&cultureID={cultureID}";
            public static string QueryDocumentPath(string userID, string wfNo, string docNo) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/Document/Path/{wfNo}/{docNo}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string UploadDocument(string userID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/Document?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DeleteDocument(string userID, string wfNo, string docNo, string upUserID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/Document/{wfNo}/{docNo}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string DownloadDocument(string userID, string docEncodeName) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/Document/{docEncodeName}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryWFNodeUrl(string userID, string wfNo) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/ToDoList/WFNodeUrl/{wfNo}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryWFFlows(string wfNo, string wfBeginStart, string wfBeginStop, string userID, string cultrueID, string flowType, string sysID, string wfFlowID, string wfFlowVer, string wfEndStart, string wfEndStop, string wfSubject, string lot, string wfNewUserID, string nodeNewUserID, string sigUserID, bool isNullNodeNewUserID, List<string> wfResultIDs, List<string> nodeResultIDs, List<string> sigResultIDs, string sortOrder, int pageIndex, int pageSize)
            {
                string condition_WFResultIDs = (wfResultIDs == null || wfResultIDs.Count == 0) ? null : $"&wfResultIDs={string.Join("&wfResultIDs=", wfResultIDs)}";
                string condition_NodeResultIDs = (nodeResultIDs == null || nodeResultIDs.Count == 0) ? null : $"&nodeResultIDs={string.Join("&nodeResultIDs=", nodeResultIDs)}";
                string condition_SigResultIDs = (sigResultIDs == null || sigResultIDs.Count == 0) ? null : $"&sigResultIDs={string.Join("&sigResultIDs=", sigResultIDs)}";

                return $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/ToDoList/WFFlows?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}&wfNo={wfNo}&wfBeginStart={wfBeginStart}&wfBeginStop={wfBeginStop}&userID={userID}&cultrueID={cultrueID}&flowType={flowType}&sysID={sysID}&wfFlowID={wfFlowID}&wfFlowVer={wfFlowVer}&wfEndStart={wfEndStart}&wfEndStop={wfEndStop}&wfSubject={wfSubject}&lot={lot}&wfNewUserID={wfNewUserID}&nodeNewUserID={nodeNewUserID}&sigUserID={sigUserID}&isNullNodeNewUserID={isNullNodeNewUserID}{condition_WFResultIDs}{condition_NodeResultIDs}{condition_SigResultIDs}&sortOrder={sortOrder}&pageIndex={pageIndex}&pageSize={pageSize}";
            }
            public static string QueryWFStartFunUrl(string userID, string sysID, string wfFlowID, string wfFlowVer, string wfNodeID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/ToDoList/WFStartFunUrl/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}?ClientSysID={EnumAPISystemID.ERPAP}&ClientUserID={userID}";
            public static string QueryUserActivityWorkFlowIDs(string userID, string flowType, string cultureID) => $"{Common.GetEnumDesc(EnumAPISystemID.WFAP)}/v1/Pub/UserActivityWorkFlowIDs/{userID}?ClientSysID={EnumAPISystemID.ERPAP}&flowType={flowType}&cultureID={cultureID}";
        }
        #endregion
    }
}
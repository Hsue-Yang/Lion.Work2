using System;
using System.Collections.Generic;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;
using LionTech.Utility;

namespace LionTech.EDIService.ERPExternal
{
    public partial class SYNC_OPAGM
    {
        private static List<EntityERPExternal.UserDetailPara> UserDetailParaList = new List<EntityERPExternal.UserDetailPara>();

        public static EnumJobResult SYNC_OPAGM_0403EXE_ENCRYPT_INFO(Flow flow, Job job)
        {
            Connection connection = flow.connections[job.connectionID];
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            List<EntityERPExternal.UserDetail> userDetailList = GetSysUserDetailList(flow, job, connection);
            if (userDetailList != null && userDetailList.Count > 0)
            {
                EntityERPExternal.EnumUpdateUserDetailResult result = EntityERPExternal.EnumUpdateUserDetailResult.Failure;
                int execCNT = 0;

                foreach (EntityERPExternal.UserDetail userDetail in userDetailList)
                {
                    string userID = userDetail.UserID.GetValue();

                    try
                    {
                        if (UserDetailParaList.Count == 50)
                        {
                            result = GetUpdateUserDetailResult(flow, job, connection);
                            UserDetailParaList.Clear();
                        }

                        UserDetailParaList.Add(new EntityERPExternal.UserDetailPara()
                        {
                            UserID = new DBVarChar(string.IsNullOrWhiteSpace(userDetail.UserID.GetValue()) ? null : userDetail.UserID.GetValue()),
                            UserIDNo = new DBVarChar(string.IsNullOrWhiteSpace(userDetail.UserIDNo.GetValue()) ? null : Security.Encrypt(userDetail.UserIDNo.GetValue())),
                            UserBirthday = new DBChar(string.IsNullOrWhiteSpace(userDetail.UserBirthday.GetValue()) ? null : Security.Encrypt(userDetail.UserBirthday.GetValue())),
                            UpdUserID = new DBNVarChar(UpdUserID)
                        });

                        if (UserDetailParaList != null && UserDetailParaList.Count > 0 && execCNT + 1 == userDetailList.Count)
                        {
                            result = GetUpdateUserDetailResult(flow, job, connection);
                            UserDetailParaList.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        UserDetailParaList.Clear();

                        FileLog.Write(exceptionPath, string.Concat(new object[] { "SYNC_OPAGM_0403EXE_ENCRYP_INFO Exception UserID: ", userID }));
                        FileLog.Write(exceptionPath, ex);
                    }

                    execCNT++;
                }
            }

            return EnumJobResult.Success;
        }

        private static List<EntityERPExternal.UserDetail> GetSysUserDetailList(Flow flow, Job job, Connection connection)
        {
            return new EntityERPExternal(connection.value, connection.providerName)
                .SelectUserDetailList();
        }

        private static EntityERPExternal.EnumUpdateUserDetailResult GetUpdateUserDetailResult(Flow flow, Job job, Connection connection)
        {
            return new EntityERPExternal(connection.value, connection.providerName)
                .UpdateUserDetail(UserDetailParaList);
        }
    }
}
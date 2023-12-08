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
        private static List<EntityERPExternal.UserMainPara> UserMainParaList = new List<EntityERPExternal.UserMainPara>();

        public static EnumJobResult SYNC_OPAGM_0402EXE_ENCRYPT_PWD(Flow flow, Job job)
        {
            Connection connection = flow.connections[job.connectionID];
            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            List<EntityERPExternal.UserMain> userMainList = GetSysUserMainList(flow, job, connection);
            if (userMainList != null && userMainList.Count > 0)
            {
                EntityERPExternal.EnumUpdateUserMainResult result = EntityERPExternal.EnumUpdateUserMainResult.Failure;
                int execCNT = 0;

                foreach (EntityERPExternal.UserMain userMain in userMainList)
                {
                    string userID = userMain.UserID.GetValue();

                    try
                    {
                        if (UserMainParaList.Count == 50)
                        {
                            result = GetUpdateUserMainResult(flow, job, connection);
                            UserMainParaList.Clear();
                        }

                        UserMainParaList.Add(new EntityERPExternal.UserMainPara()
                        {
                            UserID = new DBVarChar(string.IsNullOrWhiteSpace(userMain.UserID.GetValue()) ? null : userMain.UserID.GetValue()),
                            UserPWD = new DBVarChar(string.IsNullOrWhiteSpace(userMain.UserPWD.GetValue()) ? null : Security.Encrypt(userMain.UserPWD.GetValue())),
                            UpdUserID = new DBNVarChar(UpdUserID)
                        });

                        if (UserMainParaList != null && UserMainParaList.Count > 0 && execCNT + 1 == userMainList.Count)
                        {
                            result = GetUpdateUserMainResult(flow, job, connection);
                            UserMainParaList.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        UserMainParaList.Clear();

                        FileLog.Write(exceptionPath, string.Concat(new object[] { "SYNC_OPAGM_0402EXE_ENCRYPT_PWD Exception UserID: ", userID }));
                        FileLog.Write(exceptionPath, ex);
                    }

                    execCNT++;
                }
            }

            return EnumJobResult.Success;
        }

        private static List<EntityERPExternal.UserMain> GetSysUserMainList(Flow flow, Job job, Connection connection)
        {
            return new EntityERPExternal(connection.value, connection.providerName)
                .SelectUserMainList();
        }

        private static EntityERPExternal.EnumUpdateUserMainResult GetUpdateUserMainResult(Flow flow, Job job, Connection connection)
        {
            return new EntityERPExternal(connection.value, connection.providerName)
                .UpdateUserMain(UserMainParaList);
        }
    }
}
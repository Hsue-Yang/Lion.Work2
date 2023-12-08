using System;
using System.Collections.Generic;
using System.Xml;
using LionTech.EDI;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Log;
using LionTech.Utility;
using StackExchange.Redis;

namespace LionTech.EDIService.ERPExternal
{
    public partial class DBAT
    {
        public static EnumJobResult DBAT_0102EXE_REBUILD_MENU(Flow flow, Job job)
        {
            Connection connection = flow.connections[job.connectionID];
            Connection redisConnection = flow.connections["redis9"];
            RedisConnection.Init(Security.Decrypt(redisConnection.value));

            string exceptionPath = flow.paths[EnumPathID.Exception.ToString()].value;

            string userMenuFilePathFolder;
            if (string.IsNullOrWhiteSpace(job.parameters["UserMenuFilePathFolder"].value))
            {
                throw new EDIException(EnumEDIMessage.JobParameterIsNull, new string[] { Job.GetID(flow, job), "UserMenuFilePathFolder" });
            }
            else
            {
                userMenuFilePathFolder = job.parameters["UserMenuFilePathFolder"].value;
            }

            List<EntityERPExternal.SysUserFunMenu> entitySysUserFunMenuList = GetSysUserFunMenuList(flow, job, connection);
            if (entitySysUserFunMenuList != null)
            {
                foreach (EntityERPExternal.SysUserFunMenu sysUserFunMenu in entitySysUserFunMenuList)
                {
                    string userID = sysUserFunMenu.UserID.GetValue();

                    try
                    {
                        string filePath = GetUserMenuFilePath(userMenuFilePathFolder, userID, LionTech.Entity.ERP.EnumCultureID.zh_TW);
                        string zh_TW = GenerateUserMenuXML(connection, userID, filePath, exceptionPath, LionTech.Entity.ERP.EnumCultureID.zh_TW);

                        if (string.IsNullOrWhiteSpace(zh_TW))
                        {
                            continue;
                        }

                        filePath = GetUserMenuFilePath(userMenuFilePathFolder, userID, LionTech.Entity.ERP.EnumCultureID.zh_CN);
                        string zh_CN = GenerateUserMenuXML(connection, userID, filePath, exceptionPath, LionTech.Entity.ERP.EnumCultureID.zh_CN);

                        filePath = GetUserMenuFilePath(userMenuFilePathFolder, userID, LionTech.Entity.ERP.EnumCultureID.en_US);
                        string en_US = GenerateUserMenuXML(connection, userID, filePath, exceptionPath, LionTech.Entity.ERP.EnumCultureID.en_US);

                        filePath = GetUserMenuFilePath(userMenuFilePathFolder, userID, LionTech.Entity.ERP.EnumCultureID.th_TH);
                        string th_TH = GenerateUserMenuXML(connection, userID, filePath, exceptionPath, LionTech.Entity.ERP.EnumCultureID.th_TH);

                        filePath = GetUserMenuFilePath(userMenuFilePathFolder, userID, LionTech.Entity.ERP.EnumCultureID.ja_JP);
                        string ja_JP = GenerateUserMenuXML(connection, userID, filePath, exceptionPath, LionTech.Entity.ERP.EnumCultureID.ja_JP);

                        filePath = GetUserMenuFilePath(userMenuFilePathFolder, userID, LionTech.Entity.ERP.EnumCultureID.ko_KR);
                        string ko_KR = GenerateUserMenuXML(connection, userID, filePath, exceptionPath, LionTech.Entity.ERP.EnumCultureID.ko_KR);

                        string redisKey = $"serp:usermenu:{userID}";
                        RedisConnection.Instance.ConnectionMultiplexer.GetDatabase().KeyDelete(redisKey);
                        RedisConnection.Instance.ConnectionMultiplexer.GetDatabase().HashSet(redisKey, new HashEntry[] {
                            new HashEntry(LionTech.Entity.ERP.EnumCultureID.zh_TW.ToString(),zh_TW),
                            new HashEntry(LionTech.Entity.ERP.EnumCultureID.zh_CN.ToString(),zh_CN),
                            new HashEntry(LionTech.Entity.ERP.EnumCultureID.en_US.ToString(),en_US),
                            new HashEntry(LionTech.Entity.ERP.EnumCultureID.th_TH.ToString(),th_TH),
                            new HashEntry(LionTech.Entity.ERP.EnumCultureID.ja_JP.ToString(),ja_JP),
                            new HashEntry(LionTech.Entity.ERP.EnumCultureID.ko_KR.ToString(),ko_KR),
                        });

                    }
                    catch (Exception ex)
                    {
                        FileLog.Write(exceptionPath, string.Concat(new object[] { "DBAT_0102EXE_REBUILD_MENU Exception UserID: ", userID }));
                        FileLog.Write(exceptionPath, ex);
                    }
                }
            }

            return EnumJobResult.Success;
        }

        private static List<EntityERPExternal.SysUserFunMenu> GetSysUserFunMenuList(Flow flow, Job job, Connection connection)
        {
            return new EntityERPExternal(connection.value, connection.providerName)
                .SelectSysUserFunMenuList();
        }

        protected static string GetUserMenuFilePath(string userMenuFilePathFolder, string userID, LionTech.Entity.ERP.EnumCultureID cultureID)
        {
            return System.IO.Path.Combine(
                new string[]
                    {
                        userMenuFilePathFolder,
                        userID,
                        string.Format("UserMenu.{0}{1}.xml", userID, cultureID == LionTech.Entity.ERP.EnumCultureID.zh_TW ? string.Empty : "." + Common.GetEnumDesc(cultureID))
                    });
        }

        public static string GenerateUserMenuXML(
            Connection connection, string userID, string filePath, string exceptionPath, EnumCultureID cultureID)
        {
            if (string.IsNullOrWhiteSpace(userID))
                return null;

            Entity_BaseAP.UserMenuFunPara para = new Entity_BaseAP.UserMenuFunPara(cultureID.ToString())
            {
                UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
            };

            List<Entity_BaseAP.UserMenuFun> userFunMenuList = new Entity_BaseAP(connection.value, connection.providerName)
                .SelectUserMenuFunList(para);

            var userMenu = LionTech.Entity.ERP.Utility.GetUserMenu(userFunMenuList, userID);

            if (userMenu == null)
                return null;

            try
            {
                //XMLFileEntity xmlFileEntity = new XMLFileEntity(filePath);
                //xmlFileEntity.SetContent(userMenu);
                //xmlFileEntity.Save();

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(userMenu.SerializeToXml());
                XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", null);
                xml.InsertBefore(xmlDeclaration, xml.FirstChild);
                return xml.OuterXml;
            }
            catch (Exception ex)
            {
                FileLog.Write(exceptionPath, string.Concat(new object[] { "DBAT_0102EXE_REBUILD_MENU Exception UserID: ", userID }));
                FileLog.Write(exceptionPath, ex);
                return null;
            }
        }
    }
}

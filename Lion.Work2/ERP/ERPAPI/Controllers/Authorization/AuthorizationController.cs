using ERPAPI.Models.Authorization;
using LionTech.Entity.ERP;
using LionTech.Utility;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ERPAPI.Controllers
{
    public partial class AuthorizationController : _BaseAPController
    {
        protected string userMenuRedisKey = "serp:usermenu:{0}";

        private void GenerateUserMenu(string userID, string zh_TW, string zh_CN, string en_US, string th_TH, string ja_JP, string ko_KR)
        {
            if (zh_TW != null && zh_CN != null && en_US != null && th_TH != null && ja_JP != null && ko_KR != null)
            {
                string redisKey = string.Format(userMenuRedisKey, userID);
                RedisConnection.RedisCache.KeyDelete(redisKey);
                RedisConnection.RedisCache.HashSet(redisKey, new HashEntry[] {
                    new HashEntry(LionTech.Entity.ERP.EnumCultureID.zh_TW.ToString(),zh_TW),
                    new HashEntry(LionTech.Entity.ERP.EnumCultureID.zh_CN.ToString(),zh_CN),
                    new HashEntry(LionTech.Entity.ERP.EnumCultureID.en_US.ToString(),en_US),
                    new HashEntry(LionTech.Entity.ERP.EnumCultureID.th_TH.ToString(),th_TH),
                    new HashEntry(LionTech.Entity.ERP.EnumCultureID.ja_JP.ToString(),ja_JP),
                    new HashEntry(LionTech.Entity.ERP.EnumCultureID.ko_KR.ToString(),ko_KR),
                });
            }
        }

        private string GetERPAPUserMenuFilePath(string userID, EnumCultureID cultureID)
        {
            return Path.Combine(
                string.Format(
                    ConfigurationManager.AppSettings[EnumAppSettingKey.FilePathERPAPUserMenu.ToString()],
                    userID,
                    cultureID == EnumCultureID.zh_TW ? string.Empty : "." + Common.GetEnumDesc(cultureID)
                ));
        }

        private string GetERPAPUserMenuDevEnvFilePath(string userID, EnumCultureID cultureID)
        {
            string fullFileName = GetERPAPUserMenuFilePath(userID, cultureID);
            string fileName = Path.GetFileName(fullFileName);
            int pos = fullFileName.LastIndexOf(@"\", StringComparison.Ordinal);
            return fullFileName.Substring(0, pos + 1) + @"DevEnv\" + fileName;
        }

        private string GetERPAPUserMenuDevEnvDirPath(string userID)
        {
            string fullFileName = GetERPAPUserMenuFilePath(userID, EnumCultureID.zh_TW);
            int pos = fullFileName.LastIndexOf(@"\", StringComparison.Ordinal);
            return fullFileName.Substring(0, pos + 1) + @"DevEnv";
        }

        private bool GenerateUserMenuXML(string userID, bool isDevEnv, Func<string, EnumCultureID, string> filePathFun)
        {
            AuthorizationModel model = new AuthorizationModel();
            string filePath = filePathFun(userID, EnumCultureID.zh_TW);
            var zh_TW = model.GenerateUserMenuXML(userID, filePath, EnumCultureID.zh_TW, isDevEnv);
            filePath = filePathFun(userID, EnumCultureID.zh_CN);
            var zh_CN = model.GenerateUserMenuXML(userID, filePath, EnumCultureID.zh_CN, isDevEnv);
            filePath = filePathFun(userID, EnumCultureID.en_US);
            var en_US = model.GenerateUserMenuXML(userID, filePath, EnumCultureID.en_US, isDevEnv);
            filePath = filePathFun(userID, EnumCultureID.th_TH);
            var th_TH = model.GenerateUserMenuXML(userID, filePath, EnumCultureID.th_TH, isDevEnv);
            filePath = filePathFun(userID, EnumCultureID.ja_JP);
            var ja_JP = model.GenerateUserMenuXML(userID, filePath, EnumCultureID.ja_JP, isDevEnv);
            filePath = filePathFun(userID, EnumCultureID.ko_KR);
            var ko_KR = model.GenerateUserMenuXML(userID, filePath, EnumCultureID.ko_KR, isDevEnv);

            if (isDevEnv == false)
            {
                GenerateUserMenu(userID, zh_TW, zh_CN, en_US, th_TH, ja_JP, ko_KR);
            }

            return true;
        }

        private byte[] GenerateUserMenuDevEnvXMLResult(string userID)
        {
            GenerateUserMenuXML(userID, true, GetERPAPUserMenuDevEnvFilePath);

            try
            {
                string fileDirPath = GetERPAPUserMenuDevEnvDirPath(userID);
                string zipPath = $"{fileDirPath}.zip";
                ZipFile.CreateFromDirectory(fileDirPath, zipPath);

                if (File.Exists(zipPath))
                {
                    byte[] fileByteArray = Common.FileReadByte(zipPath, 1024);
                    File.Delete(zipPath);
                    return fileByteArray;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }

        private bool GenerateUserMenuXMLResult(string userID)
        {
           return GenerateUserMenuXML(userID, false, GetERPAPUserMenuFilePath);
        }
    }
}
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.SystemSetting;
using LionTech.Entity;
using LionTech.Utility;

namespace ERPAPI.Controllers
{
    public partial class SystemSettingController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult SystemIconUploadEvent([FromUri]SystemIconModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<SystemIconModel.APIParaData>(model.APIPara);

                string targetPath = string.Empty;
                string fullFileName = string.Empty;
                if (model.APIData.UsedOnIIS == EnumYN.Y.ToString())
                {
                    targetPath = Common.GetEnumDesc(SystemIconModel.EnumTargetPath.SysIconUploadPath);
                    fullFileName = string.Format("{0}{1}", RandomString.Generate(48), model.APIData.FileExtension);
                }
                else
                {
                    targetPath = model.APIData.TargetPath;
                    fullFileName = string.Format("{0}{1}", model.APIData.FileName, model.APIData.FileExtension);
                }

                string filePath = Path.Combine(
                    new string[]
                        {
                            ConfigurationManager.AppSettings[EnumAppSettingKey.FilePath.ToString()],
                            targetPath, fullFileName
                        });

                MemoryStream memoryStream = new MemoryStream();
                int bytesRead;
                byte[] buffer = new byte[1024];
                while ((bytesRead = MSHttpContext.Request.InputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead);
                }
                byte[] byteArray = memoryStream.ToArray();

                Common.FileWriteStream(filePath, byteArray);
                
                return Text(string.Empty);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return Text(string.Empty);
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public Stream SystemIconDownloadEvent([FromUri]SystemIconModel model)
        {
            if (AuthState.IsAuthorized == false) return null;
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<SystemIconModel.APIParaData>(model.APIPara);

                string path = Path.Combine(
                    new string[]
                        {
                            ConfigurationManager.AppSettings[EnumAppSettingKey.FilePath.ToString()],
                            model.APIData.TargetFilePath
                        });

                MemoryStream stream = null;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    byte[] byteArray = Common.FileReadByte(path, 1024);

                    stream = new MemoryStream(byteArray);
                    stream.Seek(0, SeekOrigin.Begin);

                    byte[] buffer = new byte[1024];
                    stream.Position = 0;

                    int n;
                    while ((n = stream.Read(buffer, 0, 1024)) > 0)
                    {
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, n);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return null;
        }
    }
}
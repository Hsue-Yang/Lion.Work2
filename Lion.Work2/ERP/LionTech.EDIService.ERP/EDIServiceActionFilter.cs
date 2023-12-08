using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using LionTech.APIService.LineBot;
using LionTech.EDI;
using LionTech.Utility;

namespace LionTech.EDIService.ERP
{
    internal abstract class EDIServiceActionFilterBase
    {
        internal delegate void FileLogWriteStringDelegate(string writtenStream);

        internal delegate void FileLogWriteExceptionDelegate(Exception ex);

        internal FileLogWriteStringDelegate FileLogWriteString;
        internal FileLogWriteExceptionDelegate FileLogWriteException;

        internal abstract void OnFlowBegin(Flow flow);
        internal abstract void OnFlowEnded(Flow flow);
        internal abstract void OnJobBegin(Flow flow, Job job);
        internal abstract void OnJobEnded(Flow flow, Job job);
        internal abstract void OnErred(Flow flow, string error);

        internal abstract void OnException(Exception ex);
    }

    internal sealed class EDIServiceActionFilter : EDIServiceActionFilterBase
    {
        private enum EnumAppSettingKey
        {
            SubjectFormat,
            SysSDMail,
            SmtpClientIPAddress,
            LineTo,
            LineID
        }

        internal EDIServiceActionFilter()
        {
        }

        internal override void OnFlowBegin(Flow flow)
        {
        }

        internal override void OnFlowEnded(Flow flow)
        {
        }

        internal override void OnJobBegin(Flow flow, Job job)
        {
        }

        internal override void OnJobEnded(Flow flow, Job job)
        {
            if (job.result == EnumJobResult.Failure)
            {
                string message = string.Format("flow id: {0}, job ended: result -> {1}", flow.id, job.result);
                SendMail(message);
                SendLine(message);
            }
        }

        internal override void OnErred(Flow flow, string error)
        {
            SendMail(error);
            SendLine(error);
        }

        internal override void OnException(Exception ex)
        {
            string errorMsg = GetExceptionMessage(ex);

            SendMail(errorMsg);
            SendLine(errorMsg);
        }

        private void SendMail(string message)
        {
            try
            {
                string subjectFormat = ConfigurationManager.AppSettings[EnumAppSettingKey.SubjectFormat.ToString()];
                string mailAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SysSDMail.ToString()];
                string host = ConfigurationManager.AppSettings[EnumAppSettingKey.SmtpClientIPAddress.ToString()];

                SmtpClient client = new SmtpClient { Host = host };
                MailMessage mailMessages = new MailMessage { From = new MailAddress(mailAddress) };

                mailMessages.To.Add(new MailAddress(mailAddress));
                mailMessages.Priority = MailPriority.High;
                mailMessages.Subject = string.Format(subjectFormat, Common.GetDateTimeText());
                mailMessages.SubjectEncoding = Encoding.GetEncoding("utf-8");
                mailMessages.BodyEncoding = Encoding.GetEncoding("utf-8");
                mailMessages.IsBodyHtml = true;
                mailMessages.Body = string.Concat(message.Replace(Environment.NewLine, "<br>"), "<br>", "<br>", "<br>");
                client.Send(mailMessages);
            }
            catch (Exception ex)
            {
                FileLogWriteException(GetInnerException(ex));
            }
        }

        private void SendLine(string message)
        {
            string lineID = ConfigurationManager.AppSettings[EnumAppSettingKey.LineID.ToString()];

            if (string.IsNullOrWhiteSpace(lineID) == false)
            {
                try
                {
                    string subjectFormat = ConfigurationManager.AppSettings[EnumAppSettingKey.SubjectFormat.ToString()];
                    message = string.Concat(string.Format(subjectFormat, Common.GetDateTimeText()), Environment.NewLine, message);

                    ILineClient lineClient = LineClient.Create();
                    lineClient.SystemID = "ERPAP";
                    lineClient.LineID = lineID;

                    LineMessageText messageText = new LineMessageText
                    {
                        Text = message
                    };
                    
                    lineClient.AddToRange(ConfigurationManager.AppSettings[EnumAppSettingKey.LineTo.ToString()].Split(';'));
                    lineClient.AddMessage(messageText);
                    lineClient.SendMessage();
                }
                catch (Exception ex)
                {
                    FileLogWriteException(GetInnerException(ex));
                }
            }
        }

        private static string GetExceptionMessage(Exception ex)
        {
            string stackTraceMsg = null;
            Exception logException = GetInnerException(ex);
            
            if (logException.StackTrace != null)
            {
                stackTraceMsg = logException.StackTrace;
            }

            return
                string.Join(Environment.NewLine,
                    string.Format("Error Message: {0}", logException.Message),
                    stackTraceMsg);
        }

        private static Exception GetInnerException(Exception ex)
        {
            Exception logException = ex;

            if (logException.InnerException != null)
            {
                logException = logException.InnerException;
            }
            return logException;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ADSystem.Helpers
{
    public class InformationAccountEmail
    {
        public static InfoCorreo GetInfoAccountEmail()
        {
            return new InfoCorreo()
            {
                MailerName = WebConfigurationManager.AppSettings["MailerName"].ToString(),
                ServidorSMTP = WebConfigurationManager.AppSettings["ServidorSMTP"].ToString(),
                PuertoSMTP = Convert.ToInt32(WebConfigurationManager.AppSettings["PuertoSMTP"].ToString()),
                UsarSSL = WebConfigurationManager.AppSettings["UsaSSL"].ToString().ToUpper() == "TRUE",
                SmtpTimeout = Convert.ToInt32(WebConfigurationManager.AppSettings["SmtpTimeout"].ToString()),
                InformadorEmail = WebConfigurationManager.AppSettings["InformadorEmail"].ToString(),
                InformadorPassword = WebConfigurationManager.AppSettings["InformadorPassword"].ToString()
            };
        }
    }
    public class InfoCorreo
    {
        public string MailerName { get; set; }
        public string InformadorEmail { get; set; }
        public string InformadorPassword { get; set; }
        public string ServidorSMTP { get; set; }
        public int PuertoSMTP { get; set; }
        public bool UsarSSL { get; set; }
        public int SmtpTimeout { get; set; }
    }
    public class DBResponse<T>
    {
        public bool ExecutionOK;
        public string Message;
        public T Data;
        public int NumRows;
    }

    public class SendEmailNotifications
    {
        public string EmailEnvia { get; set; }
        public string EmailConCopiaEnvia { get; set; }
        public string EmailConCopiaOculta { get; set; }
        public string SubjectEmail { get; set; }
        public string BodyEmail { get; set; }
        public List<FileAttachment> Files { get; set; }
    }
    public class FileAttachment
    {
        public string FileName { get; set; }
        public MemoryStream memoryStreamFiles { get; set; }
    }
}
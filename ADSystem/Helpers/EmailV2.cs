using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ADSystem.Helpers
{
    public class EmailV2
    { 
        public static string FernandaEmail = WebConfigurationManager.AppSettings["FernandaEmail"].ToString();
        public static string AnnaEmail = WebConfigurationManager.AppSettings["AnnaEmail"].ToString();
        public static string ManagerEmail = WebConfigurationManager.AppSettings["ManagerEmail"].ToString();
        public static bool GetSendMail = Convert.ToBoolean(WebConfigurationManager.AppSettings["SendMail"].ToString()); 

        public void SendMail(SendEmailNotifications sendEmailNotifications, Boolean NotifyAdministrators)
        {
            var infoEmailServer = InformationAccountEmail.GetInfoAccountEmail();
            if (NotifyAdministrators)
            {
                sendEmailNotifications.EmailEnvia = FernandaEmail + "; " + AnnaEmail;
                sendEmailNotifications.EmailConCopiaEnvia = ManagerEmail;
            }
            else
            {
                 
                sendEmailNotifications.EmailConCopiaOculta = FernandaEmail + "; " + AnnaEmail + (ManagerEmail == "" ? "" : "; " + ManagerEmail) + "; soporte1@development.com.mx";
            }

            if (sendEmailNotifications.EmailEnvia == "")
            {
                sendEmailNotifications.EmailEnvia = FernandaEmail;
                sendEmailNotifications.EmailConCopiaOculta = "";
            }

            if (GetSendMail)
            {
                var enviaMail = NotificationsEmail.SendEmail(sendEmailNotifications, infoEmailServer);
                if (enviaMail.ExecutionOK)
                {
                    if (sendEmailNotifications.Files != null)
                    {
                        foreach (var item in sendEmailNotifications.Files)
                        {
                            var rutaArchivo = @"C:\Accents\" + item.FileName;

                            if (File.Exists(rutaArchivo))
                            {
                                File.Delete(rutaArchivo);
                            }
                        }
                    }
                }

                LogSendEmails.WriteLog(sendEmailNotifications, enviaMail);
            }
        }
    }
}
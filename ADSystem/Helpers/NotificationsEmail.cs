using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ADSystem.Helpers
{
    public class NotificationsEmail
    {
        public static DBResponse<Boolean> SendEmail(SendEmailNotifications envioEmail, InfoCorreo infoCorreo)
        {
            var dbResponse = new DBResponse<Boolean>();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string mailerNombre = infoCorreo.MailerName;
                string mailerEmail = infoCorreo.InformadorEmail;
                string mailerPassword = infoCorreo.InformadorPassword;

                string servidorSMTP = infoCorreo.ServidorSMTP;
                int puertoSMTP = infoCorreo.PuertoSMTP;
                int timeOut = infoCorreo.SmtpTimeout;
                bool usarSSL = infoCorreo.UsarSSL;

                //create the mail message
                MailMessage mail = new MailMessage();

                //Destinatarios
                List<MailAddress> destinatarios = new List<MailAddress>();
                var emails = envioEmail.EmailEnvia.Split(';');
                foreach (string em in emails)
                {
                    destinatarios.Add(new MailAddress(em));
                }

                // Con copia
                List<MailAddress> destinatariosConCopia = new List<MailAddress>();
                if (envioEmail.EmailConCopiaEnvia != null)
                {
                    if (envioEmail.EmailConCopiaEnvia.Length > 0)
                    {
                        var emailsConCopia = envioEmail.EmailConCopiaEnvia.Split(';');
                        foreach (string em in emailsConCopia)
                        {
                            destinatariosConCopia.Add(new MailAddress(em));
                        }
                    }
                }

                //Con copia oculta
                List<MailAddress> destinatariosConCopiaOculta = new List<MailAddress>();
                if (envioEmail.EmailConCopiaOculta != null)
                {
                    if (envioEmail.EmailConCopiaOculta.Length > 0)
                    {
                        var emailsConCopiaOculta = envioEmail.EmailConCopiaOculta.Split(';');
                        foreach (string em in emailsConCopiaOculta)
                        {
                            destinatariosConCopiaOculta.Add(new MailAddress(em));
                        }
                    }
                }
                //set the addresses
                try
                {
                    mail.From = new MailAddress(mailerEmail, mailerNombre);
                }
                catch (Exception exc)
                {
                    dbResponse.Message = "Función NotificationsEmail.SendEmail : Sección mail.From | " + exc.Message; ;
                    dbResponse.ExecutionOK = false;
                    return dbResponse;
                }

                try
                {
                    foreach (MailAddress direccion in destinatarios)
                    {
                        mail.To.Add(direccion);
                    }
                }
                catch (Exception exc)
                {
                    dbResponse.Message = "Función NotificationsEmail.SendEmail : Sección mail.To.Add | " + exc.Message; ;
                    dbResponse.Data = false;
                    dbResponse.ExecutionOK = false;
                    return dbResponse;
                }

                if (envioEmail.Files != null)
                {
                    foreach (var fileAtt in envioEmail.Files)
                    {
                        byte[] bytes = fileAtt.memoryStreamFiles.ToArray();
                        fileAtt.memoryStreamFiles.Close();
                        var file = Convert.ToBase64String(bytes);
                        if (!Directory.Exists(@"C:\AccentsArchivos\"))
                        {
                            Directory.CreateDirectory(@"C:\AccentsArchivos\");
                        }

                        var rutaArchivo = @"C:\AccentsArchivos\" + fileAtt.FileName;
                        File.WriteAllBytes(rutaArchivo, Convert.FromBase64String(file));

                        Attachment att = new Attachment(rutaArchivo)
                        {
                            Name = System.IO.Path.GetFileName(rutaArchivo)
                        };
                        mail.Attachments.Add(att);
                    }
                }

                //set the content
                mail.Subject = envioEmail.SubjectEmail;
                mail.IsBodyHtml = true;
                mail.Body = envioEmail.BodyEmail;
                if (destinatariosConCopia.Count > 0)
                {
                    foreach (MailAddress direccion in destinatariosConCopia)
                    {
                        mail.CC.Add(direccion);
                    }
                }

                if (destinatariosConCopiaOculta.Count > 0)
                {
                    foreach (MailAddress direccion in destinatariosConCopiaOculta)
                    {
                        mail.Bcc.Add(direccion);
                    }
                }

                try
                {
                    var smptClient = new SmtpClient(servidorSMTP, puertoSMTP)
                    {
                        Credentials = new System.Net.NetworkCredential(mailerEmail, mailerPassword),
                        EnableSsl = usarSSL,
                        Timeout = timeOut
                    };

                    smptClient.Send(mail);

                    dbResponse.Message = "Email enviado";
                    dbResponse.ExecutionOK = true;
                    dbResponse.Data = true;
                }
                catch (Exception exc)
                {
                    dbResponse.Data = false;
                    dbResponse.Message = "Función NotificationsEmail.SendEmail : Sección smtp.Send | " + exc.Message + "<br /> " + (exc.InnerException != null ? exc.InnerException.Message : "" + "  "); ;
                    dbResponse.ExecutionOK = false;
                    return dbResponse;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = ex.ToString();
                dbResponse.Data = false;
                dbResponse.ExecutionOK = false;
            }
            return dbResponse;
        }
    }
}
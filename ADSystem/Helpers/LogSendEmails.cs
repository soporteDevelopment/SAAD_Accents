using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ADSystem.Helpers
{
    public class LogSendEmails
    {
        public static string GetDirectory = WebConfigurationManager.AppSettings["DirectorioLogs"].ToString();
        public static void WriteLog(SendEmailNotifications envioEmail, DBResponse<Boolean> dBResponse)
        {
            string nombreArchivo = dBResponse.ExecutionOK ? "EnvioCorrecto_" : "EnvioError_";
            nombreArchivo = nombreArchivo + DateTime.Now.ToString("ddMMyyyyhhmmss");
            string path = GetDirectory + @"\" + nombreArchivo + ".txt";
            StreamWriter stream = null;
            try
            {
                if (!Directory.Exists(GetDirectory))
                {
                    Directory.CreateDirectory(GetDirectory);
                }
                stream = File.AppendText(path);
                stream.WriteLine(string.Format("Fecha {0}", DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt")));
                stream.WriteLine(string.Format("Destinatarios: {0}", envioEmail.EmailEnvia));
                stream.WriteLine(string.Format("Con Copia: {0}", envioEmail.EmailConCopiaEnvia));
                stream.WriteLine(string.Format("Con Copia Oculta: {0}", envioEmail.EmailConCopiaOculta));
                stream.WriteLine(string.Format("Asunto: {0}", envioEmail.SubjectEmail));
                stream.WriteLine(string.Format("Body Email: {0}", envioEmail.BodyEmail));
                stream.WriteLine(string.Format("Respuesta Correo: {0}", dBResponse.Message));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
    }
}
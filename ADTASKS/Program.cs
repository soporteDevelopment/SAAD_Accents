using ADTASKS.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ADTASKS
{
    class Program
    {
        public static string connection = "Data Source=40.117.232.162;Initial Catalog=SAADDB_DEV;user id=usersaaddev;password=ce!taz45";
        public static string AccentsEmail = "admin@accentsadmin.com";
        public static string SendGridKey = "";

        static void Main(string[] args)
        {
            SendNotificaction();
            SendNotificactionToAdmin();
        }

        public static void SendNotificaction()
        {
            try
            {
                var users = GetUsers();

                foreach (var user in users)
                {
                    if (user.idUsuario != 8 && user.idUsuario != 16)
                    {
                        var events = GetEvents(user.idUsuario);
                        var transfers = GetPendingsTransfers(user.idUsuario);
                        var views = GetPendingsViews(user.idUsuario);

                        if (!String.IsNullOrEmpty(events) || !String.IsNullOrEmpty(transfers) ||
                            !String.IsNullOrEmpty(views))
                        {
                            var client = new SendGridClient(SendGridKey);
                            var message = new SendGridMessage();

                            message.From = new EmailAddress(AccentsEmail, "Accents Decoration");
                            message.AddTo(new EmailAddress(user.Correo, user.Correo));
                            message.Subject = "Agenda " + DateTime.Now.ToString("dd/MM/yyyy"); 

                            message.HtmlContent = "<div style='font-size:15px; font-family: \"Libre Baskerville\";'>Buen día " +
                                user.Nombre + " " + user.Apellidos + "</div><br/>" + events + "<br/>" + transfers +
                                "<br/>" + views; 
                            message.TrackingSettings.OpenTracking.Enable = true;

                            client.SendEmailAsync(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendNotificactionToAdmin()
        {
            try
            {
                var transfers = GetPendingsTransfers(null);
                var views = GetPendingsViews(null);
                var birthdays = GetBirthDays();
                var aniversaries = GetAniversaries();

                if (!String.IsNullOrEmpty(transfers) || !String.IsNullOrEmpty(views) || !String.IsNullOrEmpty(birthdays) || !String.IsNullOrEmpty(aniversaries))
                {                    
                    var client = new SendGridClient(SendGridKey);
                    var message = new SendGridMessage();

                    message.From = new EmailAddress(AccentsEmail, "Accents Decoration");
                    message.AddTo(new EmailAddress("fernanda@accentsdecoration.com", "fernanda@accentsdecoration.com"));
                    message.Subject = "Agenda " + DateTime.Now.ToString("dd/MM/yyyy");
                    message.HtmlContent = "<div style='font-size:15px; font-family: \"Libre Baskerville\";'>Buen día </div><br />" + birthdays + "<br />" + aniversaries + "<br />" + transfers + " < br/>" + views;
                    message.TrackingSettings.OpenTracking.Enable = true;

                    client.SendEmailAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<UserModel> GetUsers()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                StringBuilder body = new StringBuilder();
                conn.ConnectionString = connection;

                conn.Open();

                SqlCommand command = new SqlCommand("GetUsers", conn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader dataReader = command.ExecuteReader();

                List<UserModel> lUsers = new List<UserModel>();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        lUsers.Add(new UserModel()
                        {
                            idUsuario = Convert.ToInt32(dataReader["idUsuario"].ToString()),
                            Nombre = dataReader["Nombre"].ToString(),
                            Apellidos = dataReader["Apellidos"].ToString(),
                            Correo = dataReader["Correo"].ToString()
                        });
                    }
                }

                dataReader.Close();
                conn.Close();
                
                return lUsers;
            }
        }

        public static string GetEvents(int idUser)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                StringBuilder body = new StringBuilder();
                conn.ConnectionString = connection;

                conn.Open();

                SqlCommand command = new SqlCommand("GetEvents", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = idUser;
                SqlDataReader dataReader = command.ExecuteReader();

                List<EventModel> lEvents = new List<EventModel>();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        lEvents.Add(new EventModel()
                        {
                            idEvento = Convert.ToInt32(dataReader["idEvento"].ToString()),
                            Nombre = dataReader["Nombre"].ToString(),
                            Fecha = Convert.ToDateTime(dataReader["Fecha"].ToString()),
                            HoraInicio = dataReader["HoraInicio"].ToString(),
                            HoraFin = dataReader["HoraFin"].ToString(),
                            Lugar = dataReader["Lugar"].ToString()
                        });
                    }
                }

                dataReader.Close();
                conn.Close();

                if (lEvents.Count > 0)
                {
                    body.Append("<section style='overflow-x:auto; border-bottom: none;overflow-x: auto;border-bottom: none; margin: 15px auto;min-width: 800px;width: 800px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";'>");
                    body.Append("<strong style='color: #ed303c;'>Eventos</strong>");
                    body.Append("<table style='border-collapse: collapse;border-spacing: 0;display: table;text-align: center;font-family: \"Libre Baskerville\"; margin-left:auto; margin-right:auto;'>");
                    body.Append("<thead>");
                    body.Append("<th style='width:200px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Evento</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Fecha</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Hora de Inicio</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Lugar</strong></th>");
                    body.Append("</thead>");
                    body.Append("<tbody>");

                    foreach (var mevent in lEvents)
                    {
                        body.Append("<tr><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mevent.Nombre + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mevent.Fecha.ToString("dd/MM/yyyy") + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + ((mevent.TodoDia == true) ? "TODO EL DÍA" : mevent.HoraInicio) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mevent.Lugar + "</td></tr>");
                    }

                    body.Append("</tbody>");
                    body.Append("</table>");
                    body.Append("</section>");
                }

                return body.ToString();
            }
        }

        public static string GetPendingsTransfers(int? idUser)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                StringBuilder body = new StringBuilder();
                conn.ConnectionString = connection;

                conn.Open();

                SqlCommand command = new SqlCommand("GetPendingsTransfers", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = idUser;
                SqlDataReader dataReader = command.ExecuteReader();

                List<TransferModel> lTransfers = new List<TransferModel>();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        lTransfers.Add(new TransferModel()
                        {
                            idTransferencia = Convert.ToInt32(dataReader["idTransferencia"].ToString()),
                            Numero = dataReader["Numero"].ToString(),
                            Usuario = dataReader["Vendedor"].ToString(),
                            Fecha = Convert.ToDateTime(dataReader["Fecha"].ToString()),
                            SucursalOrigen = dataReader["SucursalOrigen"].ToString(),
                            SucursalDestino = dataReader["SucursalDestino"].ToString()
                        });
                    }
                }

                dataReader.Close();
                conn.Close();

                if (lTransfers.Count > 0)
                {
                    body.Append("<section style='overflow-x:auto; border-bottom: none;overflow-x: auto;border-bottom: none; margin: 15px auto;min-width: 800px;width: 800px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";'>");
                    body.Append("<strong style='color: #ed303c;'>Transferencias pendientes</strong>");
                    body.Append("<table style='border-collapse: collapse;border-spacing: 0;display: table;text-align: center;font-family: \"Libre Baskerville\"; margin-left:auto; margin-right:auto;'>");
                    body.Append("<thead>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Transferencia</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Fecha</strong></th>");
                    body.Append("<th style='width:350px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Vendedor</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Origen</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Destino</strong></th>");
                    body.Append("</thead>");
                    body.Append("<tbody>");

                    foreach (var mtransfer in lTransfers)
                    {
                        body.Append("<tr><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mtransfer.Numero + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mtransfer.Fecha.Value.ToString("dd/MM/yyyy") + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mtransfer.Usuario + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mtransfer.SucursalOrigen + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mtransfer.SucursalDestino + "</td></tr>");
                    }

                    body.Append("</tbody>");
                    body.Append("</table>");
                    body.Append("</section>");
                }

                return body.ToString();
            }
        }

        public static string GetPendingsViews(int? idUser)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                StringBuilder body = new StringBuilder();
                conn.ConnectionString = connection;

                conn.Open();

                SqlCommand command = new SqlCommand("GetPendingsViews", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = idUser;
                SqlDataReader dataReader = command.ExecuteReader();

                List<ViewModel> lViews = new List<ViewModel>();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        lViews.Add(new ViewModel()
                        {
                            idVista = Convert.ToInt32(dataReader["idVista"].ToString()),
                            Remision = dataReader["Remision"].ToString(),
                            Vendedor = dataReader["Vendedor"].ToString(),
                            Cliente = dataReader["Cliente"].ToString(),
                            Fecha = Convert.ToDateTime(dataReader["Fecha"].ToString()),
                            Total = Convert.ToDecimal(dataReader["Total"].ToString()),
                            Proyecto = dataReader["Proyecto"].ToString(),
                        });
                    }
                }

                dataReader.Close();
                conn.Close();

                if (lViews.Count > 0)
                {
                    body.Append("<section style='overflow-x:auto; border-bottom: none;overflow-x: auto;border-bottom: none; margin: 15px auto;min-width: 800px;width: 800px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";'>");
                    body.Append("<strong style='color: #ed303c;'>Salidas a vista pendientes</strong>");
                    body.Append("<table style='border-collapse: collapse;border-spacing: 0;display: table;text-align: center;font-family: \"Libre Baskerville\"; margin-left:auto; margin-right:auto;'>");
                    body.Append("<thead>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Número</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Fecha</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Vendedor</strong></th>");
                    body.Append("<th style='width:350px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Cliente</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Total</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Proyecto</strong></th>");
                    body.Append("</thead>");
                    body.Append("<tbody>");

                    foreach (var mviews in lViews)
                    {
                        body.Append("<tr><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mviews.Remision + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mviews.Fecha.Value.ToString("dd/MM/yyyy") + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mviews.Vendedor + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mviews.Cliente + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mviews.Total + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + mviews.Proyecto + "</td></tr>");
                    }

                    body.Append("</tbody>");
                    body.Append("</table>");
                    body.Append("</section>");
                }

                return body.ToString();
            }
        }

        public static string GetBirthDays()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                StringBuilder body = new StringBuilder();
                conn.ConnectionString = connection;

                conn.Open();

                SqlCommand command = new SqlCommand("GetBirthDays", conn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader dataReader = command.ExecuteReader();

                List<BirthDayModel> lBirthDays = new List<BirthDayModel>();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        lBirthDays.Add(new BirthDayModel()
                        {
                            Nombre = dataReader["Nombre"].ToString(),
                            Apellidos = dataReader["Apellidos"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(dataReader["FechaNacimiento"].ToString())
                        });
                    }
                }

                dataReader.Close();
                conn.Close();

                if (lBirthDays.Count > 0)
                {
                    body.Append("<section style='overflow-x:auto; border-bottom: none;overflow-x: auto;border-bottom: none; margin: 15px auto;min-width: 800px;width: 800px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";'>");
                    body.Append("<strong style='color: #ed303c;'>Cumpleaños</strong>");
                    body.Append("<table style='border-collapse: collapse;border-spacing: 0;display: table;text-align: center;font-family: \"Libre Baskerville\"; margin-left:auto; margin-right:auto;'>");
                    body.Append("<thead>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Nombre</strong></th>");
                    body.Append("<th style='width:350px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Apellidos</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Fecha</strong></th>");
                    body.Append("</thead>");
                    body.Append("<tbody>");

                    foreach (var birthday in lBirthDays)
                    {
                        body.Append("<tr><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + birthday.Nombre + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + birthday.Apellidos + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + birthday.FechaNacimiento.ToString("dd/MM/yyyy") + "</td></tr>");
                    }

                    body.Append("</tbody>");
                    body.Append("</table>");
                    body.Append("</section>");
                }

                return body.ToString();
            }
        }

        public static string GetAniversaries()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                StringBuilder body = new StringBuilder();
                conn.ConnectionString = connection;

                conn.Open();

                SqlCommand command = new SqlCommand("GetAniversaries", conn);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader dataReader = command.ExecuteReader();

                List<AniversaryModel> lAniversaries = new List<AniversaryModel>();

                if (dataReader != null)
                {
                    while (dataReader.Read())
                    {
                        lAniversaries.Add(new AniversaryModel()
                        {
                            Nombre = dataReader["Nombre"].ToString(),
                            Apellidos = dataReader["Apellidos"].ToString(),
                            FechaIngreso = Convert.ToDateTime(dataReader["FechaIngreso"].ToString())
                        });
                    }
                }

                dataReader.Close();
                conn.Close();

                if (lAniversaries.Count > 0)
                {
                    body.Append("<section style='overflow-x:auto; border-bottom: none;overflow-x: auto;border-bottom: none; margin: 15px auto;min-width: 800px;width: 800px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";'>");
                    body.Append("<strong style='color: #ed303c;'>Aniversarios</strong>");
                    body.Append("<table style='border-collapse: collapse;border-spacing: 0;display: table;text-align: center;font-family: \"Libre Baskerville\"; margin-left:auto; margin-right:auto;'>");
                    body.Append("<thead>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Nombre</strong></th>");
                    body.Append("<th style='width:350px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Apellidos</strong></th>");
                    body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Fecha</strong></th>");
                    body.Append("</thead>");
                    body.Append("<tbody>");

                    foreach (var aniversary in lAniversaries)
                    {
                        body.Append("<tr><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + aniversary.Nombre + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + aniversary.Apellidos + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + aniversary.FechaIngreso.ToString("dd/MM/yyyy") + "</td></tr>");
                    }

                    body.Append("</tbody>");
                    body.Append("</table>");
                    body.Append("</section>");
                }

                return body.ToString();
            }
        }
    }
}

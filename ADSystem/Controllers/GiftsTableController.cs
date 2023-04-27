using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Controllers
{

    [SessionExpiredFilter]
    [AuthorizedModule]
    public class GiftsTableController : BaseController
    {
        // GET: GiftsTable
        public override ActionResult Index()
        {
            Session["Controller"] = "Mesa de Regalos";

            ViewBag.branches = tBranches.GetAllActivesBranches();

            ViewBag.seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            return View();
        }

        public ActionResult ListGiftsTable()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetGiftsTable(string dtDateSince, string dtDateUntil, string remision, string costumer, string codigo, int status, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { GiftsTable = tGiftsTable.GetGiftsTable(Convert.ToDateTime(dtDateSince), Convert.ToDateTime(dtDateUntil), remision, costumer, codigo, status, amazonas, guadalquivir, textura, page, pageSize), Count = tGiftsTable.CountRegisters() };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult SaveGiftsTable(GiftsTableViewModel oGiftsTable)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var remision = tGiftsTable.AddGiftsTable(oGiftsTable);

                this.SendGiftsTable(remision);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se registró la mesa de regalos con exito"), sRemision = remision };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public PartialViewResult GetDetailGiftsTable(int idGiftsTable)
        {
            GiftsTableViewModel oGiftsTable = tGiftsTable.GetGiftsTable(idGiftsTable);

            return PartialView("~/Views/GiftsTable/DetailGiftsTable.cshtml", oGiftsTable);
        }

        public ActionResult GetGiftsTable(int idGiftsTable)
        {
            GiftsTableViewModel oGiftsTable = tGiftsTable.GetGiftsTable(idGiftsTable);

            return View(oGiftsTable);
        }

        [HttpPost]
        public ActionResult UpdateGiftsTable(GiftsTableViewModel oGiftsTable)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (tGiftsTable.UpdateGiftsTable(oGiftsTable))
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó la mesa de regalos con exito") };

                }
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "No se actualizó la mesa de regalos con exito") };
                }
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult DeleteGiftsTable(int idGiftsTable)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var remision = tGiftsTable.DeleteGiftsTable(idGiftsTable);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se eliminó la mesa de regalos con exito"), sRemision = remision };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult UpdateStatusGiftsTable(int idGiftsTable, short status)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tGiftsTable.UpdateStatusGiftsTable(idGiftsTable, status))
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó el estatus de la mesa de regalos con exito") };

                }
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "No se actualizó el estatus de la mesa de regalos") };
                }

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult GeneratePrevNumberRem(int idBranch)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sRemision = tGiftsTable.GeneratePrevNumberRem(idBranch) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public PartialViewResult PrintGiftsTable(string remision)
        {
            var GiftsTable = tGiftsTable.GetGiftsTable(remision);

            ViewBag.Descuento = Convert.ToInt16(GiftsTable.lDetail.Exists(p => (p.Descuento > 0)));

            return PartialView(GiftsTable);
        }

        public void SendGiftsTable(string remision)
        {

            GiftsTableViewModel oGiftsTable = tGiftsTable.GetGiftsTable(remision);

            StringBuilder body = new StringBuilder();

            #region mailBody

            body.Append("<html lang='en'>");

            body.Append("<head>");
            body.Append("<meta charset='utf-8'>");
            body.Append("<meta http-equiv='X-UA-Compatible' content='IE=edge'>");
            body.Append("<meta name='viewport' content='width=device-width,user-scalable=no,initial-scale=1.0,maximum-scale=1.0,minimum-scale=1.0'>");
            body.Append("<title>Accents Decoration</title>");
            body.Append("</head>");

            body.Append("<body style='background: none;margin: 0 auto;'>");

            body.Append(@"<link href='https://fonts.googleapis.com/css?family=Libre+Baskerville:400,700|Open+Sans+Condensed:300,700' type='text/css' />");

            body.Append(@"<link href='https://fonts.googleapis.com/css?family=Libre+Baskerville|Open+Sans+Condensed:300,700' type='text/css' />");

            body.Append(@"<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css' type='text/css'>");

            body.Append("<header style='background:#fafafa;min-width:1024px;width:1024px;margin:0 auto;min-height: 53px;'>");
            body.Append("<div style='background:none;width:210px;display:inline-block;padding:15px 0 15px 40px;height: 53px;'>");
            body.Append("<img src='https://www.accentsadmin.com/content/images/logo_des.png' style='width: 210px;position: relative;'>");
            body.Append("</div>");
            body.Append("</header>");

            body.Append("<section style='min-width:1024px;width:1024px;text-align: center;margin: 0 auto; display: block; font-family: \"Libre Baskerville\"'>");
            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
            body.Append("Sucursal:<strong>" + oGiftsTable.Sucursal + "</strong>");
            body.Append("</div>");
            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
            body.Append("Vendedor:<strong>" + oGiftsTable.idVendedor1 + "</strong><br /><strong>" + oGiftsTable.idVendedor1 + "</strong>");
            body.Append("</div>");

            if (!String.IsNullOrEmpty(oGiftsTable.DespachoReferencia))
            {
                body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
                body.Append("Despacho:<strong>" + oGiftsTable.DespachoReferencia + "</strong>");
                body.Append("</div>");
            }

            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 259px;font-size: 15px;color: #464646;'>");
            body.Append("Fecha:<strong>" + oGiftsTable.Fecha + "</strong>");
            body.Append("</div>");
            body.Append("</section>");

            body.Append("<section style='min-width:900px;width:900px;margin: 20px auto;border-bottom: 1px solid #e0e0e0;text-align: center;padding: 10px 0;display:block; font-family: \"Libre Baskerville\"'>");
            body.Append("<div style='width: 49%;display: inline-block;text-align: center;'>");
            body.Append("<table style='width: 100%;display:table;border-collapse: separate;border-spacing: 2px;border-color: grey;'>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Novio:" + oGiftsTable.Novio + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Novio:" + oGiftsTable.Novia + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Email:</td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</div>");

            body.Append("<div style='display: inline-block; width: 1px; position: relative; bottom: 13px; text-align: center;'>");
            body.Append("<img src ='' />");
            body.Append("</div>");

            body.Append("<div style='width: 49%; display: inline-block; text-align: center;border-color: grey;'>");
            body.Append("<table style='display: table; border-collapse: separate;border-spacing: 2px;'>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Movil:</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Dirección:" + oGiftsTable.LugarBoda + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit; height:20px;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit;'></td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</div>");
            body.Append("</section>");

            body.Append("<section style='margin: 0 auto;text-align: center;padding: 15px 0;font-size: 20px;color: black;min-width: 1024px;width: 1024px;border-bottom: 3px solid #000;letter-spacing: 5px; font-family: \"Libre Baskerville\"; '>");
            body.Append("MESA DE REGALOS");
            body.Append("</section>");

            body.Append("<section style='text-align: center;padding: 15px 0;width: auto;font-size: 20px;color: black;letter-spacing: 5px;font-weight: lighter;display: block; font-family: \"Libre Baskerville\";'>");
            body.Append("DETALLE DE LA MESA DE REGALOS");
            body.Append("</section>");

            body.Append("<section style='margin: 15px auto;min-width: 950px;width: 950px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";' >");
            body.Append("<table style='border-collapse: collapse;border-spacing: 0;width: 100%;display: table;text-align: center; font-family: \"Libre Baskerville\";'>");
            body.Append("<thead>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>IMAGEN</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>CÓDIGO</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>DESCRIPCIÓN</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>CANT.</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>PREC. UNIT.</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>SUBTOTAL</strong></th>");

            var desc = oGiftsTable.lDetail.Exists(p => (p.Descuento > 0));

            if (desc)
            {

                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>DESCUENTO</strong></th>");

            }

            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>TOTAL</strong></th>");
            body.Append("</thead>");
            body.Append("<tbody>");

            foreach (var detail in oGiftsTable.lDetail)
            {

                decimal total = (detail.Cantidad * detail.Precio) - ((detail.Cantidad * detail.Precio) * (detail.Descuento / 100)) ?? 0;

                if (detail.Producto != null && detail.Producto.TipoImagen == 1)
                {

                    detail.IdGUID = Guid.NewGuid().ToString();

                    body.Append("<tr><td><img src='cid:" + detail.IdGUID + "' style='width: 80px;' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                }
                else
                {

                    if (detail.Producto != null)
                    {

                        body.Append("<tr><td><img src='" + detail.Producto.urlImagen + "' style='width: 80px;' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                    }
                    else
                    {

                        body.Append("<tr><td><img src='' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                    }

                }

                if (!String.IsNullOrEmpty(detail.Comentario))
                {

                    body.Append("<tr><td><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>COMENTARIOS:</strong></td><td  colspan='7'><p style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Comentario + "</p></td></tr>");

                }

            }

            body.Append("<tr><td colspan='5' style='text-align:right;'><span style='font-size:15px; font-family: \"Libre Baskerville\";'>SUMA:</span></td><td><p style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + oGiftsTable.SumSubtotal.Value.ToString("C", CultureInfo.CurrentCulture) + "</p></td></tr>");

            body.Append("</tbody>");
            body.Append("</table>");

            var Total = (oGiftsTable.Total > 0) ? oGiftsTable.Total.Value.ToString("C", CultureInfo.CurrentCulture) : "$0.00";
            body.Append("<br/>");
            body.Append("<table style='border-collapse: collapse; border-spacing:0; width: 100%; display: table; text-align:center; font-family: \"Libre Baskerville\";'>");
            body.Append("<tbody>");
            body.Append("<tr>");
            body.Append("<td style='width:300px;'></td>");
            body.Append("<td style='width:300px; color: #747474; text-align: right; font-size: 15px; font-family: \"Libre Baskerville\";'>Cantidad de productos: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + oGiftsTable.CantidadProductos + "</strong></td>");
            body.Append("<td style='width:300px; color: #464646; text-align: right; padding-right: 3%; font-size: 15px; font-family: \"Libre Baskerville\";'>Subtotal: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + oGiftsTable.Subtotal.Value.ToString("C", CultureInfo.CurrentCulture) + "</strong></td>");
            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");

            if (oGiftsTable.Descuento > 0)
            {

                body.Append("<td style='color: #464646; text-align: right; padding-right: 3%;'>Descuento: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + @Math.Truncate((decimal)oGiftsTable.Descuento) + "%</strong></td>");

            }
            else
            {

                body.Append("<td></td>");

            }

            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");

            if (oGiftsTable.IVA > 0)
            {
                decimal? subtotal = (oGiftsTable.Subtotal - (oGiftsTable.Subtotal * (oGiftsTable.Descuento / 100)));
                decimal? IVA = 0.16m;

                var result = subtotal * IVA;

                if (result < 0)
                {
                    result = 0;
                }

                body.Append("<td style='font-size:15px; font-family: \"Libre Baskerville\"; color: #464646; text-align: right; padding-right: 3%;'>16% IVA: " + result.Value.ToString("C", CultureInfo.CurrentCulture) + "</td>");
            }
            else
            {
                body.Append("<td></td>");
            }

            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");
            body.Append("<td style='font-size:15px; font-family: \"Libre Baskerville\"; color: #464646; text-align: right; padding-right: 3%;'> Total: <strong style='font-size:15px; font-family: \"Libre Baskerville\"; font-weight: bold; color: #000;'>" + oGiftsTable.Total.Value.ToString("C", CultureInfo.CurrentCulture) + "</strong></td>");
            body.Append("</tr>");
            body.Append("</tbody>");
            body.Append("</table>");
            body.Append("</section>");

            body.Append("<div style='width: 1024px;height: 3px;margin: 10px auto 0;background: #000;display: block;'></div>");

            body.Append("<p style='width: 100%;text-align: center'>");

            body.Append("<a href='https://www.facebook.com/accentsdecoration' style='text-decoration:none; background: #3b5998; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-facebook' aria-hidden='true'></i>&nbsp;FACEBOOK</a>&nbsp;&nbsp;");
            body.Append("<a href='https://www.instagram.com/accents_decoration' style='text-decoration:none; background: #9b6954; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-instagram' aria-hidden='true'></i>&nbsp;INSTRAGRAM</a>&nbsp;&nbsp;");
            body.Append("<a href='https://www.twitter.com/accentsdecor' style='text-decoration:none; background: #5bc0de; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-twitter' aria-hidden='true'></i>&nbsp;TWITTER</a>");
            body.Append("</p>");

            body.Append("<br/>");
            body.Append("<br/>");

            body.Append("<em style='font-size:15px; font-family: \"Libre Baskerville\";'>Para asegurarte de recibir todos nuestros correos agrega <b>admin@accentsadmin.com</b> a tu libro de direcciones.</em>");

            body.Append("<br/>");
            body.Append("<br/>");

            body.Append("<table cellspacing='0' cellpadding='3' style='width: 100%; font-family: \"Libre Baskerville\";'>");
            body.Append("<tr>");
            body.Append("<td style='text-align: center; font-family: \"Libre Baskerville\";'>");
            body.Append("<hr>");
            body.Append("<small style='font-size:15px; font-family: \"Libre Baskerville\";'>AMAZONAS 305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321</small>");
            body.Append("<br/>");
            body.Append("<small style='font-size:15px; font-family: \"Libre Baskerville\";'>RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591</small>");

            body.Append("<hr/>");
            body.Append("</td>");
            body.Append("<tr>");
            body.Append("</table>");
            body.Append("</section>");
            body.Append("</body>");
            body.Append("</html>");

            #endregion

            //this.SendMailGiftsTable(oGiftsTable.CorreoNovio.Trim().ToLower(), "Mesa de Regalos " + oGiftsTable.idMesaRegalo, body.ToString(), oGiftsTable.lDetail);
        }

        [HttpPost]
        public void SendGiftsTableAgain(string remision, string email)
        {
            GiftsTableViewModel oGiftsTable = tGiftsTable.GetGiftsTable(remision);

            StringBuilder body = new StringBuilder();

            #region mailBody

            body.Append("<html lang='en'>");

            body.Append("<head>");
            body.Append("<meta charset='utf-8'>");
            body.Append("<meta http-equiv='X-UA-Compatible' content='IE=edge'>");
            body.Append("<meta name='viewport' content='width=device-width,user-scalable=no,initial-scale=1.0,maximum-scale=1.0,minimum-scale=1.0'>");
            body.Append("<title>Accents Decoration</title>");
            body.Append("</head>");

            body.Append("<body style='background: none;margin: 0 auto;'>");

            body.Append(@"<link href='https://fonts.googleapis.com/css?family=Libre+Baskerville:400,700|Open+Sans+Condensed:300,700' type='text/css' />");

            body.Append(@"<link href='https://fonts.googleapis.com/css?family=Libre+Baskerville|Open+Sans+Condensed:300,700' type='text/css' />");

            body.Append(@"<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css' type='text/css'>");

            body.Append("<header style='background:#fafafa;min-width:1024px;width:1024px;margin:0 auto;min-height: 53px;'>");
            body.Append("<div style='background:none;width:210px;display:inline-block;padding:15px 0 15px 40px;height: 53px;'>");
            body.Append("<img src='https://www.accentsadmin.com/content/images/logo_des.png' style='width: 210px;position: relative;'>");
            body.Append("</div>");
            body.Append("</header>");

            body.Append("<section style='min-width:1024px;width:1024px;text-align: center;margin: 0 auto; display: block; font-family: \"Libre Baskerville\"'>");
            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
            body.Append("Sucursal:<strong>" + oGiftsTable.Sucursal + "</strong>");
            body.Append("</div>");
            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
            body.Append("Vendedor:<strong>" + oGiftsTable.idVendedor1 + "</strong><br /><strong>" + oGiftsTable.idVendedor1 + "</strong>");
            body.Append("</div>");

            if (!String.IsNullOrEmpty(oGiftsTable.DespachoReferencia))
            {
                body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
                body.Append("Despacho:<strong>" + oGiftsTable.DespachoReferencia + "</strong>");
                body.Append("</div>");
            }

            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 259px;font-size: 15px;color: #464646;'>");
            body.Append("Fecha:<strong>" + oGiftsTable.Fecha + "</strong>");
            body.Append("</div>");
            body.Append("</section>");

            body.Append("<section style='min-width:900px;width:900px;margin: 20px auto;border-bottom: 1px solid #e0e0e0;text-align: center;padding: 10px 0;display:block; font-family: \"Libre Baskerville\"'>");
            body.Append("<div style='width: 49%;display: inline-block;text-align: center;'>");
            body.Append("<table style='width: 100%;display:table;border-collapse: separate;border-spacing: 2px;border-color: grey;'>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Novio:" + oGiftsTable.Novio + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Novio:" + oGiftsTable.Novia + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Email:</td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</div>");

            body.Append("<div style='display: inline-block; width: 1px; position: relative; bottom: 13px; text-align: center;'>");
            body.Append("<img src ='' />");
            body.Append("</div>");

            body.Append("<div style='width: 49%; display: inline-block; text-align: center;border-color: grey;'>");
            body.Append("<table style='display: table; border-collapse: separate;border-spacing: 2px;'>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Movil:</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Dirección:" + oGiftsTable.LugarBoda + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit; height:20px;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit;'></td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</div>");
            body.Append("</section>");

            body.Append("<section style='margin: 0 auto;text-align: center;padding: 15px 0;font-size: 20px;color: black;min-width: 1024px;width: 1024px;border-bottom: 3px solid #000;letter-spacing: 5px; font-family: \"Libre Baskerville\"; '>");
            body.Append("MESA DE REGALOS");
            body.Append("</section>");

            body.Append("<section style='text-align: center;padding: 15px 0;width: auto;font-size: 20px;color: black;letter-spacing: 5px;font-weight: lighter;display: block; font-family: \"Libre Baskerville\";'>");
            body.Append("DETALLE DE LA MESA DE REGALOS");
            body.Append("</section>");

            body.Append("<section style='margin: 15px auto;min-width: 950px;width: 950px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";' >");
            body.Append("<table style='border-collapse: collapse;border-spacing: 0;width: 100%;display: table;text-align: center; font-family: \"Libre Baskerville\";'>");
            body.Append("<thead>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>IMAGEN</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>CÓDIGO</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>DESCRIPCIÓN</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>CANT.</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>PREC. UNIT.</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>SUBTOTAL</strong></th>");

            var desc = oGiftsTable.lDetail.Exists(p => (p.Descuento > 0));

            if (desc)
            {

                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>DESCUENTO</strong></th>");

            }

            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>TOTAL</strong></th>");
            body.Append("</thead>");
            body.Append("<tbody>");

            foreach (var detail in oGiftsTable.lDetail)
            {

                decimal total = (detail.Cantidad * detail.Precio) - ((detail.Cantidad * detail.Precio) * (detail.Descuento / 100)) ?? 0;

                if (detail.Producto != null && detail.Producto.TipoImagen == 1)
                {

                    detail.IdGUID = Guid.NewGuid().ToString();

                    body.Append("<tr><td><img src='cid:" + detail.IdGUID + "' style='width: 80px;' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                }
                else
                {

                    if (detail.Producto != null)
                    {

                        body.Append("<tr><td><img src='" + detail.Producto.urlImagen + "' style='width: 80px;' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                    }
                    else
                    {

                        body.Append("<tr><td><img src='' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Producto.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                    }

                }

                if (!String.IsNullOrEmpty(detail.Comentario))
                {

                    body.Append("<tr><td><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>COMENTARIOS:</strong></td><td  colspan='7'><p style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Comentario + "</p></td></tr>");

                }

            }

            body.Append("<tr><td colspan='5' style='text-align:right;'><span style='font-size:15px; font-family: \"Libre Baskerville\";'>SUMA:</span></td><td><p style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + oGiftsTable.SumSubtotal.Value.ToString("C", CultureInfo.CurrentCulture) + "</p></td></tr>");

            body.Append("</tbody>");
            body.Append("</table>");

            var Total = (oGiftsTable.Total > 0) ? oGiftsTable.Total.Value.ToString("C", CultureInfo.CurrentCulture) : "$0.00";
            body.Append("<br/>");
            body.Append("<table style='border-collapse: collapse; border-spacing:0; width: 100%; display: table; text-align:center; font-family: \"Libre Baskerville\";'>");
            body.Append("<tbody>");
            body.Append("<tr>");
            body.Append("<td style='width:300px;'></td>");
            body.Append("<td style='width:300px; color: #747474; text-align: right; font-size: 15px; font-family: \"Libre Baskerville\";'>Cantidad de productos: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + oGiftsTable.CantidadProductos + "</strong></td>");
            body.Append("<td style='width:300px; color: #464646; text-align: right; padding-right: 3%; font-size: 15px; font-family: \"Libre Baskerville\";'>Subtotal: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + oGiftsTable.Subtotal.Value.ToString("C", CultureInfo.CurrentCulture) + "</strong></td>");
            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");

            if (oGiftsTable.Descuento > 0)
            {

                body.Append("<td style='color: #464646; text-align: right; padding-right: 3%;'>Descuento: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + @Math.Truncate((decimal)oGiftsTable.Descuento) + "%</strong></td>");

            }
            else
            {

                body.Append("<td></td>");

            }

            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");

            if (oGiftsTable.IVA > 0)
            {
                decimal? subtotal = (oGiftsTable.Subtotal - (oGiftsTable.Subtotal * (oGiftsTable.Descuento / 100)));
                decimal? IVA = 0.16m;

                var result = subtotal * IVA;

                if (result < 0)
                {
                    result = 0;
                }

                body.Append("<td style='font-size:15px; font-family: \"Libre Baskerville\"; color: #464646; text-align: right; padding-right: 3%;'>16% IVA: " + result.Value.ToString("C", CultureInfo.CurrentCulture) + "</td>");
            }
            else
            {
                body.Append("<td></td>");
            }

            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");
            body.Append("<td style='font-size:15px; font-family: \"Libre Baskerville\"; color: #464646; text-align: right; padding-right: 3%;'> Total: <strong style='font-size:15px; font-family: \"Libre Baskerville\"; font-weight: bold; color: #000;'>" + oGiftsTable.Total.Value.ToString("C", CultureInfo.CurrentCulture) + "</strong></td>");
            body.Append("</tr>");
            body.Append("</tbody>");
            body.Append("</table>");
            body.Append("</section>");

            body.Append("<div style='width: 1024px;height: 3px;margin: 10px auto 0;background: #000;display: block;'></div>");

            body.Append("<p style='width: 100%;text-align: center'>");

            body.Append("<a href='https://www.facebook.com/accentsdecoration' style='text-decoration:none; background: #3b5998; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-facebook' aria-hidden='true'></i>&nbsp;FACEBOOK</a>&nbsp;&nbsp;");
            body.Append("<a href='https://www.instagram.com/accents_decoration' style='text-decoration:none; background: #9b6954; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-instagram' aria-hidden='true'></i>&nbsp;INSTRAGRAM</a>&nbsp;&nbsp;");
            body.Append("<a href='https://www.twitter.com/accentsdecor' style='text-decoration:none; background: #5bc0de; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-twitter' aria-hidden='true'></i>&nbsp;TWITTER</a>");
            body.Append("</p>");

            body.Append("<br/>");
            body.Append("<br/>");

            body.Append("<em style='font-size:15px; font-family: \"Libre Baskerville\";'>Para asegurarte de recibir todos nuestros correos agrega <b>admin@accentsadmin.com</b> a tu libro de direcciones.</em>");

            body.Append("<br/>");
            body.Append("<br/>");

            body.Append("<table cellspacing='0' cellpadding='3' style='width: 100%; font-family: \"Libre Baskerville\";'>");
            body.Append("<tr>");
            body.Append("<td style='text-align: center; font-family: \"Libre Baskerville\";'>");
            body.Append("<hr>");
            body.Append("<small style='font-size:15px; font-family: \"Libre Baskerville\";'>AMAZONAS 305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321</small>");
            body.Append("<br/>");
            body.Append("<small style='font-size:15px; font-family: \"Libre Baskerville\";'>RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591</small>");

            body.Append("<hr/>");
            body.Append("</td>");
            body.Append("<tr>");
            body.Append("</table>");
            body.Append("</section>");
            body.Append("</body>");
            body.Append("</html>");

            #endregion

            //this.SendMailGiftsTable(email, "Mesa de Regalos " + oGiftsTable.idMesaRegalo, body.ToString(), oGiftsTable.lDetail);
        }

        #region Cerrar Mesa de Regalos

        public ActionResult CloseGiftsTable(int idGiftsTable)
        {

            tGiftsTable.GenerateCredit(idGiftsTable, ((UserViewModel)Session["_User"]).idUsuario);

            ViewBag.branches = tBranches.GetAllActivesBranches();

            ViewBag.seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            GiftsTableViewModel oGiftsTable = tGiftsTable.GetGiftsTableForClose(idGiftsTable);

            return View(oGiftsTable);
        }

        #endregion

        #region Venta

        public ActionResult GiftsTableForSale(int idGiftsTable)
        {
            ViewBag.branches = tBranches.GetAllActivesBranches();

            ViewBag.seller = ((UserViewModel)Session["_User"]).NombreCompleto;

            GiftsTableViewModel oGiftsTable = tGiftsTable.GetGiftsTableForSale(idGiftsTable);

            return View(oGiftsTable);
        }

        [HttpPost]
        public ActionResult GeneratePrevNumberRemForSale(int idBranch)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { sRemision = tGiftsTable.GeneratePrevNumberRemForSale(idBranch) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult SaveGiftsTableForSale(SaleGiftsTableViewModel oSale, List<TypePaymentViewModel> lTypePayment)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                int idSale = 0;

                var remision = tGiftsTable.AddSale(oSale, out idSale);

                tGiftsTable.AddTypePayment(idSale, lTypePayment);

                this.SendMailSale(remision);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se registró la venta con exito"), sRemision = remision };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public void SendMailSale(string remision)
        {

            SaleGiftsTableViewModel oSale = tGiftsTable.GetGiftsTableSale(remision);

            StringBuilder body = new StringBuilder();

            body.Append("<html lang='en'>");

            body.Append("<head>");
            body.Append("<meta charset='utf-8'>");
            body.Append("<meta http-equiv='X-UA-Compatible' content='IE=edge'>");
            body.Append("<meta name='viewport' content='width=device-width,user-scalable=no,initial-scale=1.0,maximum-scale=1.0,minimum-scale=1.0'>");
            body.Append("<title>Accents Decoration</title>");
            body.Append("</head>");

            body.Append("<body style='background: none;margin: 0 auto;'>");

            body.Append(@"<link href='https://fonts.googleapis.com/css?family=Libre+Baskerville:400,700|Open+Sans+Condensed:300,700' type='text/css' />");

            body.Append(@"<link href='https://fonts.googleapis.com/css?family=Libre+Baskerville|Open+Sans+Condensed:300,700' type='text/css' />");

            body.Append(@"<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css' type='text/css'>");

            body.Append("<header style='background:#fafafa;min-width:1024px;width:1024px;margin:0 auto;min-height: 53px;'>");
            body.Append("<div style='background:none;width:210px;display:inline-block;padding:15px 0 15px 40px;height: 53px;'>");
            body.Append("<img src='https://www.accentsadmin.com/content/images/logo_des.png' style='width: 210px;position: relative;'>");
            body.Append("</div>");
            body.Append("<div style='margin: 0 auto;background: #e1e1e1;color: #1e1e1e;width: 253px;display: inline-block;text-align: center;float: right;padding: 19px 0; font-size:15px; font-family: \"Libre Baskerville\"'>");
            body.Append("Remisión:<strong style='color: #ed303c;'>" + oSale.Remision + "</strong>");
            body.Append("</div>");
            body.Append("</header>");

            body.Append("<section style='min-width:1024px;width:1024px;text-align: center;margin: 0 auto; display: block; font-family: \"Libre Baskerville\"'>");
            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
            body.Append("Sucursal:<strong>" + oSale.Sucursal + "</strong>");
            body.Append("</div>");
            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
            body.Append("Vendedor:<strong>" + oSale.Usuario1 + "</strong><br /><strong>" + oSale.Usuario2 + "</strong>");
            body.Append("</div>");

            if (oSale.DespachoReferencia != "")
            {
                body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
                body.Append("Despacho:<strong>" + oSale.DespachoReferencia + "</strong>");
                body.Append("</div>");
            }

            body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 259px;font-size: 15px;color: #464646;'>");
            body.Append("Fecha:<strong>" + oSale.Fecha + "</strong>");
            body.Append("</div>");
            body.Append("<div style='display: inline-block;background: #e1e1e1;padding: 5px 0;width: 253px;font-size: 15px;color: #464646;'>");
            body.Append("Factura:<strong>" + oSale.Factura + "</strong>");
            body.Append("</div>");
            body.Append("</section>");

            body.Append("<section style='min-width:900px;width:900px;margin: 20px auto;border-bottom: 1px solid #e0e0e0;text-align: center;padding: 10px 0;display:block; font-family: \"Libre Baskerville\"'>");
            body.Append("<div style='width: 49%;display: inline-block;text-align: center;'>");
            body.Append("<table style='width: 100%;display:table;border-collapse: separate;border-spacing: 2px;border-color: grey;'>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Cliente:" + oSale.ClienteFisico + "&nbsp;" + oSale.ClienteMoral + "&nbsp;" + oSale.Despacho + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Teléfono:" + oSale.oAddress.TelCasa + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Email:" + oSale.oAddress.Correo + "</td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</div>");

            body.Append("<div style='display: inline-block; width: 1px; position: relative; bottom: 13px; text-align: center;'>");
            body.Append("<img src ='' />");
            body.Append("</div>");

            body.Append("<div style='width: 49%; display: inline-block; text-align: center;border-color: grey;'>");
            body.Append("<table style='display: table; border-collapse: separate;border-spacing: 2px;'>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Movil:" + oSale.oAddress.TelCelular + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Dirección:" + oSale.oAddress.Direccion + "</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit; height:20px;'><img src='' /></td>");
            body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit;'></td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</div>");
            body.Append("</section>");

            body.Append("<section style='margin: 0 auto;text-align: center;padding: 15px 0;font-size: 20px;color: black;min-width: 1024px;width: 1024px;border-bottom: 3px solid #000;letter-spacing: 5px; font-family: \"Libre Baskerville\"; '>");
            body.Append("NOTA DE VENTA");
            body.Append("</section>");
            body.Append("<section style='text-align: center;padding: 15px 0;width: auto;font-size: 20px;color: black;letter-spacing: 5px;font-weight: lighter;display: block; font-family: \"Libre Baskerville\";'>");
            body.Append("FORMA DE PAGO");
            body.Append("</section>");

            body.Append("<section style='overflow-x:auto; border-bottom: none;overflow-x: auto;border-bottom: none; margin: 15px auto;min-width: 800px;width: 800px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";'>");
            body.Append("<table style='border-collapse: collapse;border-spacing: 0;display: table;text-align: center;font-family: \"Libre Baskerville\"; margin-left:auto; margin-right:auto;'>");
            body.Append("<thead>");
            body.Append("<th style='width:350px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Forma de pago</strong></th>");
            body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Cantidad</strong></th>");
            body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Fecha de pago</strong></th>");
            body.Append("<th style='width:350px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Banco</strong></th>");
            body.Append("</thead>");
            body.Append("<tbody>");

            List<TypePaymentViewModel> typesPayment = tSales.GetTypePayment(oSale.idVenta);

            foreach (var payment in typesPayment)
            {

                body.Append("<tr><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + payment.sTypePayment + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (payment.amount + (payment.amountIVA ?? 0)).ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + payment.DatePayment ?? "" + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + payment.bank + "</td></tr>");

            }

            body.Append("</tbody>");
            body.Append("</table>");
            body.Append("</section>");

            body.Append("<section style='text-align: center;padding: 15px 0;width: auto;font-size: 20px;color: black;letter-spacing: 5px;font-weight: lighter;display: block; font-family: \"Libre Baskerville\";'>");
            body.Append("DETALLE DE LA VENTA");
            body.Append("</section>");

            body.Append("<section style='margin: 15px auto;min-width: 950px;width: 950px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";' >");
            body.Append("<table style='border-collapse: collapse;border-spacing: 0;width: 100%;display: table;text-align: center; font-family: \"Libre Baskerville\";'>");
            body.Append("<thead>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>IMAGEN</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>CÓDIGO</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>DESCRIPCIÓN</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>CANT.</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>PREC. UNIT.</strong></th>");
            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>SUBTOTAL</strong></th>");

            var desc = oSale.oDetail.Exists(p => (p.Descuento > 0));

            if (desc)
            {

                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>DESCUENTO</strong></th>");

            }

            body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>TOTAL</strong></th>");
            body.Append("</thead>");
            body.Append("<tbody>");

            foreach (var detail in oSale.oDetail)
            {

                decimal total = (detail.Cantidad * detail.Precio) - ((detail.Cantidad * detail.Precio) * (detail.Descuento / 100)) ?? 0;

                if (detail.oProducto != null && detail.TipoImagen == 1)
                {

                    detail.IdGUID = Guid.NewGuid().ToString();

                    body.Append("<tr><td><img src='cid:" + detail.IdGUID + "' style='width: 80px;' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                }
                else
                {

                    if (detail.oProducto != null)
                    {

                        body.Append("<tr><td><img src='cid:" + detail.IdGUID + "' style='width: 80px;' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                    }
                    else
                    {

                        body.Append("<tr><td><img src='' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                    }

                }

                if (!String.IsNullOrEmpty(detail.Comentarios))
                {

                    body.Append("<tr><td><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>COMENTARIOS:</strong></td><td  colspan='7'><p style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Comentarios + "</p></td></tr>");

                }

            }

            body.Append("<tr><td colspan='5' style='text-align:right;'><span style='font-size:15px; font-family: \"Libre Baskerville\";'>SUMA:</span></td><td><p style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + oSale.SumSubtotal.ToString("C", CultureInfo.CurrentCulture) + "</p></td></tr>");

            body.Append("</tbody>");
            body.Append("</table>");

            var Total = (oSale.Total > 0) ? oSale.Total.Value.ToString("C", CultureInfo.CurrentCulture) : "$0.00";
            body.Append("<br/>");
            body.Append("<table style='border-collapse: collapse; border-spacing:0; width: 100%; display: table; text-align:center; font-family: \"Libre Baskerville\";'>");
            body.Append("<tbody>");
            body.Append("<tr>");
            body.Append("<td style='width:300px;'></td>");
            body.Append("<td style='width:300px; color: #747474; text-align: right; font-size: 15px; font-family: \"Libre Baskerville\";'>Cantidad de productos: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + oSale.CantidadProductos + "</strong></td>");
            body.Append("<td style='width:300px; color: #464646; text-align: right; padding-right: 3%; font-size: 15px; font-family: \"Libre Baskerville\";'>Subtotal: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + oSale.Subtotal.Value.ToString("C", CultureInfo.CurrentCulture) + "</strong></td>");
            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");

            if (oSale.Descuento > 0)
            {

                body.Append("<td style='color: #464646; text-align: right; padding-right: 3%;'>Descuento: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + @Math.Truncate((decimal)oSale.Descuento) + "%</strong></td>");

            }
            else
            {
                body.Append("<td></td>");
            }

            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");

            if (oSale.IVA > 0)
            {
                decimal? subtotal = (oSale.Subtotal - (oSale.Subtotal * (oSale.Descuento / 100))) + (oSale.TotalNotasCredito ?? 0);
                decimal? IVA = 0.16m;

                var result = subtotal * IVA;

                if (result < 0)
                {
                    result = 0;
                }

                body.Append("<td style='font-size:15px; font-family: \"Libre Baskerville\"; color: #464646; text-align: right; padding-right: 3%;'>16% IVA: " + result.Value.ToString("C", CultureInfo.CurrentCulture) + "</td>");
            }
            else
            {
                body.Append("<td></td>");
            }

            body.Append("</tr>");

            body.Append("<tr>");
            body.Append("<td></td>");
            body.Append("<td></td>");
            body.Append("<td style='font-size:15px; font-family: \"Libre Baskerville\"; color: #464646; text-align: right; padding-right: 3%;'> Total: <strong style='font-size:15px; font-family: \"Libre Baskerville\"; font-weight: bold; color: #000;'>" + oSale.Total.Value.ToString("C", CultureInfo.CurrentCulture) + "</strong></td>");
            body.Append("</tr>");
            body.Append("</tbody>");
            body.Append("</table>");
            body.Append("</section>");

            body.Append("<p style='width: 100%;text-align: center'>");

            body.Append("<a href='https://www.facebook.com/accentsdecoration' style='text-decoration:none; background: #3b5998; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-facebook' aria-hidden='true'></i>&nbsp;FACEBOOK</a>&nbsp;&nbsp;");
            body.Append("<a href='https://www.instagram.com/accents_decoration' style='text-decoration:none; background: #9b6954; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-instagram' aria-hidden='true'></i>&nbsp;INSTRAGRAM</a>&nbsp;&nbsp;");
            body.Append("<a href='https://www.twitter.com/accentsdecor' style='text-decoration:none; background: #5bc0de; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-twitter' aria-hidden='true'></i>&nbsp;TWITTER</a>");
            body.Append("</p>");

            body.Append("<br/>");

            body.Append("<table cellspacing='0' cellpadding='3' style='width: 100%; font-family: \"Libre Baskerville\";'>");
            body.Append("<tr>");
            body.Append("<td style='text-align: center; font-family: \"Libre Baskerville\";'>");
            body.Append("<hr>");
            body.Append("<small style='font-size:15px; font-family: \"Libre Baskerville\";'>AMAZONAS 305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321</small>");
            body.Append("<br/>");
            body.Append("<small style='font-size:15px; font-family: \"Libre Baskerville\";'>RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591</small>");

            body.Append("<hr/>");
            body.Append("</td>");
            body.Append("<tr>");
            body.Append("</table>");

            body.Append("<br/>");
            body.Append("<br/>");

            body.Append("<em style='font-size:15px; font-family: \"Libre Baskerville\";'>Para asegurarte de recibir todos nuestros correos agrega <b>admin@accentsadmin.com</b> a tu libro de direcciones.</em>");

            body.Append("<br/>");
            body.Append("<br/>");

            body.Append("</section>");

            body.Append("</body>");
            body.Append("</html>");

            string email = string.Empty;

            if (oSale.idClienteFisico > 0)
            {

                email = tPhysicalCustomers.GetPhysicalCustomer((int)oSale.idClienteFisico).Correo;

            }
            else if (oSale.idClienteMoral > 0)
            {

                email = tMoralCustomers.GetMoralCustomer((int)oSale.idClienteMoral).Correo;

            }
            else
            {

                email = tOffices.GetOffice((int)oSale.idDespacho).Correo;

            }

            if (email.Trim().ToLower() != "noreply@correo.com")
            {
                //this.SendMailSales("admin@accentsadmin.com", email, "Venta " + oSale.Remision, body.ToString(), oSale.oDetail);
            }
        }

        [HttpPost]
        public ActionResult SendMailSaleAgain(int idSale, string email)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                SaleViewModel oSale = tSales.GetSaleForIdSale(idSale);

                StringBuilder body = new StringBuilder();

                body.Append("<html lang='en'>");

                body.Append("<head>");
                body.Append("<meta charset='utf-8'>");
                body.Append("<meta http-equiv='X-UA-Compatible' content='IE=edge'>");
                body.Append("<meta name='viewport' content='width=device-width,user-scalable=no,initial-scale=1.0,maximum-scale=1.0,minimum-scale=1.0'>");
                body.Append("<title>Accents Decoration</title>");
                body.Append("</head>");

                body.Append("<body style='background: none;margin: 0 auto;'>");

                body.Append(@"<link href='https://fonts.googleapis.com/css?family=Libre+Baskerville:400,700|Open+Sans+Condensed:300,700' type='text/css' />");

                body.Append(@"<link href='https://fonts.googleapis.com/css?family=Libre+Baskerville|Open+Sans+Condensed:300,700' type='text/css' />");

                body.Append(@"<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css' type='text/css'>");

                body.Append("<header style='background:#fafafa;min-width:1024px;width:1024px;margin:0 auto;min-height: 53px;'>");
                body.Append("<div style='background:none;width:210px;display:inline-block;padding:15px 0 15px 40px;height: 53px;'>");
                body.Append("<img src='https://www.accentsadmin.com/content/images/logo_des.png' style='width: 210px;position: relative;'>");
                body.Append("</div>");
                body.Append("<div style='margin: 0 auto;background: #e1e1e1;color: #1e1e1e;width: 253px;display: inline-block;text-align: center;float: right;padding: 19px 0; font-size:15px; font-family: \"Libre Baskerville\"'>");
                body.Append("Remisión:<strong style='color: #ed303c;'>" + oSale.Remision + "</strong>");
                body.Append("</div>");
                body.Append("</header>");

                body.Append("<section style='min-width:1024px;width:1024px;text-align: center;margin: 0 auto; display: block; font-family: \"Libre Baskerville\"'>");
                body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
                body.Append("Sucursal:<strong>" + oSale.Sucursal + "</strong>");
                body.Append("</div>");
                body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
                body.Append("Vendedor:<strong>" + oSale.Usuario1 + "</strong><br /><strong>" + oSale.Usuario2 + "</strong>");
                body.Append("</div>");

                if (!String.IsNullOrEmpty(oSale.DespachoReferencia))
                {
                    body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 256px;font-size: 15px;color: #464646;'>");
                    body.Append("Despacho:<strong>" + oSale.DespachoReferencia + "</strong>");
                    body.Append("</div>");
                }

                body.Append("<div style='display: inline-block;background: #f7f7f7;padding: 5px 0;width: 259px;font-size: 15px;color: #464646;'>");
                body.Append("Fecha:<strong>" + oSale.Fecha + "</strong>");
                body.Append("</div>");
                body.Append("<div style='display: inline-block;background: #e1e1e1;padding: 5px 0;width: 253px;font-size: 15px;color: #464646;'>");
                body.Append("Factura:<strong>" + oSale.Factura + "</strong>");
                body.Append("</div>");
                body.Append("</section>");

                body.Append("<section style='min-width:900px;width:900px;margin: 20px auto;border-bottom: 1px solid #e0e0e0;text-align: center;padding: 10px 0;display:block; font-family: \"Libre Baskerville\"'>");
                body.Append("<div style='width: 49%;display: inline-block;text-align: center;'>");
                body.Append("<table style='width: 100%;display:table;border-collapse: separate;border-spacing: 2px;border-color: grey;'>");
                body.Append("<tr>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Cliente:" + oSale.ClienteFisico + "&nbsp;" + oSale.ClienteMoral + "&nbsp;" + oSale.Despacho + "</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Teléfono:" + oSale.oAddress.TelCasa + "</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Email:" + oSale.oAddress.Correo + "</td>");
                body.Append("</tr>");
                body.Append("</table>");
                body.Append("</div>");

                body.Append("<div style='display: inline-block; width: 1px; position: relative; bottom: 13px; text-align: center;'>");
                body.Append("<img src ='' />");
                body.Append("</div>");

                body.Append("<div style='width: 49%; display: inline-block; text-align: center;border-color: grey;'>");
                body.Append("<table style='display: table; border-collapse: separate;border-spacing: 2px;'>");
                body.Append("<tr>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Movil:" + oSale.oAddress.TelCelular + "</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit;'><img src='' /></td>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit; font-family: \"Libre Baskerville\"'>Dirección:" + oSale.oAddress.Direccion + "</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: right;width: 30%;display: table-cell;vertical-align: inherit; height:20px;'><img src='' /></td>");
                body.Append("<td style='font-size: 15px;font-weight: lighter;color: #464646;text-align: left;padding-left: 10px;width: 70%;display: table-cell;vertical-align: inherit;'></td>");
                body.Append("</tr>");
                body.Append("</table>");
                body.Append("</div>");
                body.Append("</section>");

                body.Append("<section style='margin: 0 auto;text-align: center;padding: 15px 0;font-size: 20px;color: black;min-width: 1024px;width: 1024px;border-bottom: 3px solid #000;letter-spacing: 5px; font-family: \"Libre Baskerville\"; '>");
                body.Append("NOTA DE VENTA");
                body.Append("</section>");
                body.Append("<section style='text-align: center;padding: 15px 0;width: auto;font-size: 20px;color: black;letter-spacing: 5px;font-weight: lighter;display: block; font-family: \"Libre Baskerville\";'>");
                body.Append("FORMA DE PAGO");
                body.Append("</section>");

                body.Append("<section style='overflow-x:auto; border-bottom: none;overflow-x: auto;border-bottom: none; margin: 15px auto;min-width: 800px;width: 800px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";'>");
                body.Append("<table style='border-collapse: collapse;border-spacing: 0;display: table;text-align: center;font-family: \"Libre Baskerville\"; margin-left:auto; margin-right:auto;'>");
                body.Append("<thead>");
                body.Append("<th style='width:350px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Forma de pago</strong></th>");
                body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Cantidad</strong></th>");
                body.Append("<th style='width:100px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Fecha de pago</strong></th>");
                body.Append("<th style='width:350px;'><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>Banco</strong></th>");
                body.Append("</thead>");
                body.Append("<tbody>");

                List<TypePaymentViewModel> typesPayment = tSales.GetTypePayment(oSale.idVenta);

                foreach (var payment in typesPayment)
                {

                    body.Append("<tr><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + payment.sTypePayment + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (payment.amount + (payment.amountIVA ?? 0)).ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + payment.DatePayment ?? "" + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + payment.bank + "</td></tr>");

                }

                body.Append("</tbody>");
                body.Append("</table>");
                body.Append("</section>");

                body.Append("<section style='text-align: center;padding: 15px 0;width: auto;font-size: 20px;color: black;letter-spacing: 5px;font-weight: lighter;display: block; font-family: \"Libre Baskerville\";'>");
                body.Append("DETALLE DE LA VENTA");
                body.Append("</section>");

                body.Append("<section style='margin: 15px auto;min-width: 950px;width: 950px;text-align: center;padding-bottom: 10px;display: block; font-family: \"Libre Baskerville\";' >");
                body.Append("<table style='border-collapse: collapse;border-spacing: 0;width: 100%;display: table;text-align: center; font-family: \"Libre Baskerville\";'>");
                body.Append("<thead>");
                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>IMAGEN</strong></th>");
                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>CÓDIGO</strong></th>");
                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>DESCRIPCIÓN</strong></th>");
                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>CANT.</strong></th>");
                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>PREC. UNIT.</strong></th>");
                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>SUBTOTAL</strong></th>");

                var desc = oSale.oDetail.Exists(p => (p.Descuento > 0));

                if (desc)
                {

                    body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>DESCUENTO</strong></th>");

                }

                body.Append("<th><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>TOTAL</strong></th>");
                body.Append("</thead>");
                body.Append("<tbody>");

                foreach (var detail in oSale.oDetail)
                {

                    decimal total = (detail.Cantidad * detail.Precio) - ((detail.Cantidad * detail.Precio) * (detail.Descuento / 100)) ?? 0;

                    if (detail.oProducto != null && detail.TipoImagen == 1)
                    {

                        detail.IdGUID = Guid.NewGuid().ToString();

                        body.Append("<tr><td><img src='cid:" + detail.IdGUID + "' style='width: 80px;' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                    }
                    else
                    {

                        if (detail.oProducto != null)
                        {

                            body.Append("<tr><td><img src='cid:" + detail.IdGUID + "' style='width: 80px;' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                        }
                        else
                        {

                            body.Append("<tr><td><img src='' /></td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Codigo + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Descripcion + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Cantidad + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + detail.Precio.Value.ToString("C", CultureInfo.CurrentCulture) + "</td><td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + (detail.Cantidad * detail.Precio).Value.ToString("C", CultureInfo.CurrentCulture) + "</td>" + ((desc == true) ? "<td style='font-size:15px; font-family: \"Libre Baskerville\";'>" + detail.Descuento + "%</td>" : "") + "<td style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: right;'>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td></tr>");

                        }

                    }

                    if (!String.IsNullOrEmpty(detail.Comentarios))
                    {

                        body.Append("<tr><td><strong style='font-size:15px; font-family: \"Libre Baskerville\";'>COMENTARIOS:</strong></td><td  colspan='7'><p style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + detail.Comentarios + "</p></td></tr>");

                    }

                }

                body.Append("<tr><td colspan='5' style='text-align:right;'><span style='font-size:15px; font-family: \"Libre Baskerville\";'>SUMA:</span></td><td><p style='font-size:15px; font-family: \"Libre Baskerville\"; text-align: left;'>" + oSale.SumSubtotal.Value.ToString("C", CultureInfo.CurrentCulture) + "</p></td></tr>");

                body.Append("</tbody>");
                body.Append("</table>");

                var Total = (oSale.Total > 0) ? oSale.Total.Value.ToString("C", CultureInfo.CurrentCulture) : "$0.00";
                body.Append("<br/>");
                body.Append("<table style='border-collapse: collapse; border-spacing:0; width: 100%; display: table; text-align:center; font-family: \"Libre Baskerville\";'>");
                body.Append("<tbody>");
                body.Append("<tr>");
                body.Append("<td style='width:300px;'></td>");
                body.Append("<td style='width:300px; color: #747474; text-align: right; font-size: 15px; font-family: \"Libre Baskerville\";'>Cantidad de productos: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + oSale.CantidadProductos + "</strong></td>");
                body.Append("<td style='width:300px; color: #464646; text-align: right; padding-right: 3%; font-size: 15px; font-family: \"Libre Baskerville\";'>Subtotal: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + oSale.Subtotal.Value.ToString("C", CultureInfo.CurrentCulture) + "</strong></td>");
                body.Append("</tr>");

                body.Append("<tr>");
                body.Append("<td></td>");
                body.Append("<td></td>");

                if (oSale.Descuento > 0)
                {

                    body.Append("<td style='color: #464646; text-align: right; padding-right: 3%;'>Descuento: <strong style='font-size:15px; font-family: \"Libre Baskerville\";'>" + @Math.Truncate((decimal)oSale.Descuento) + "%</strong></td>");

                }
                else
                {
                    body.Append("<td></td>");
                }

                body.Append("</tr>");

                body.Append("<tr>");
                body.Append("<td></td>");
                body.Append("<td></td>");

                if (oSale.IVA > 0)
                {
                    decimal? subtotal = (oSale.Subtotal - (oSale.Subtotal * (oSale.Descuento / 100))) + (oSale.TotalNotasCredito ?? 0);
                    decimal? IVA = 0.16m;

                    var result = subtotal * IVA;

                    if (result < 0)
                    {
                        result = 0;
                    }

                    body.Append("<td style='font-size:15px; font-family: \"Libre Baskerville\"; color: #464646; text-align: right; padding-right: 3%;'>16% IVA: " + result.Value.ToString("C", CultureInfo.CurrentCulture) + "</td>");
                }
                else
                {
                    body.Append("<td></td>");
                }

                body.Append("</tr>");

                body.Append("<tr>");
                body.Append("<td></td>");
                body.Append("<td></td>");
                body.Append("<td style='font-size:15px; font-family: \"Libre Baskerville\"; color: #464646; text-align: right; padding-right: 3%;'> Total: <strong style='font-size:15px; font-family: \"Libre Baskerville\"; font-weight: bold; color: #000;'>" + oSale.Total.Value.ToString("C", CultureInfo.CurrentCulture) + "</strong></td>");
                body.Append("</tr>");
                body.Append("</tbody>");
                body.Append("</table>");
                body.Append("</section>");

                body.Append("<p style='width: 100%;text-align: center'>");

                body.Append("<a href='https://www.facebook.com/accentsdecoration' style='text-decoration:none; background: #3b5998; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-facebook' aria-hidden='true'></i>&nbsp;FACEBOOK</a>&nbsp;&nbsp;");
                body.Append("<a href='https://www.instagram.com/accents_decoration' style='text-decoration:none; background: #9b6954; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-instagram' aria-hidden='true'></i>&nbsp;INSTRAGRAM</a>&nbsp;&nbsp;");
                body.Append("<a href='https://www.twitter.com/accentsdecor' style='text-decoration:none; background: #5bc0de; color: #fff; border: 0px;padding-left:2px;padding-right:2px;'><i class='fa fa-twitter' aria-hidden='true'></i>&nbsp;TWITTER</a>");
                body.Append("</p>");

                body.Append("<br/>");

                body.Append("<table cellspacing='0' cellpadding='3' style='width: 100%; font-family: \"Libre Baskerville\";'>");
                body.Append("<tr>");
                body.Append("<td style='text-align: center; font-family: \"Libre Baskerville\";'>");
                body.Append("<hr>");
                body.Append("<small style='font-size:15px; font-family: \"Libre Baskerville\";'>AMAZONAS 305 OTE. COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL.66220 MÉXICO. T: (81) 8356-9558 / (81) 8335-0321</small>");
                body.Append("<br/>");
                body.Append("<small style='font-size:15px; font-family: \"Libre Baskerville\";'>RIO GUADALQUIVIR #13. ESQ. CALZADA SAN PEDRO COLONIA DEL VALLE, SAN PEDRO GARZA GARCÍA, NL. 66220 MÉXICO. T: (81) 8335-5591</small>");

                body.Append("<hr/>");
                body.Append("</td>");
                body.Append("<tr>");
                body.Append("</table>");

                body.Append("<br/>");
                body.Append("<br/>");

                body.Append("<em style='font-size:15px; font-family: \"Libre Baskerville\";'>Para asegurarte de recibir todos nuestros correos agrega <b>admin@accentsadmin.com</b> a tu libro de direcciones.</em>");

                body.Append("<br/>");
                body.Append("<br/>");

                body.Append("</section>");
                body.Append("</body>");
                body.Append("</html>");

                //this.SendMailSales("admin@accentsadmin.com", email, "Venta " + oSale.Remision, body.ToString(), oSale.oDetail);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "El correo se ha enviado." };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        public PartialViewResult PrintGiftsTableSale(string remision)
        {
            var GiftsTable = tGiftsTable.GetGiftsTableSale(remision);

            ViewBag.Descuento = Convert.ToInt16(GiftsTable.oDetail.Exists(p => (p.Descuento > 0)));

            return PartialView(GiftsTable);
        }

        public ActionResult GetTypesPaymentForSale(int idSale)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { TypesPayment = tGiftsTable.GetTypePayment(idSale) };

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);

        }

        [HttpPost]
        public ActionResult UpdateStatusGiftsTableSale(int idSale, short status)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {

                if (tGiftsTable.UpdateStatusGiftsTableSale(idSale, status))
                {

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó el estatus de la venta con exito") };

                }
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Message = String.Format(Message.msgAdd = "No se actualizó el estatus de la venta") };
                }

            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        #endregion

    }
}
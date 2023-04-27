using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Helpers
{
    public static class UrlHelperExtension
    {
        public static string Js(this UrlHelper helper, string archivo)
        {
            return helper.Content(String.Format("~/Scripts/{0}.js?" + DateTime.Now.Millisecond, archivo));
            //return helper.Content(String.Format("~/Scripts/{0}.js", archivo));
        }

        public static string Css(this UrlHelper helper, string archivo)
        {
            return helper.Content(String.Format("~/{0}.css?" + DateTime.Now.Millisecond, archivo));
        }

        public static string RemoveLastChar(this UrlHelper helper, string cRuta)
        {
            return cRuta.Substring(0, cRuta.Length - 1);

        }
        public static string Ruta(this UrlHelper helper, string cLink)
        {
            return helper.Content(String.Format("~/{0}", cLink));
        }

        public static string Img(this UrlHelper helper, string archivo)
        {
            return helper.Content(String.Format("~/Content/Images/{0}", archivo));
        }

        public static string ImgNews(this UrlHelper helper, string archivo)
        {
            return helper.Content(String.Format("~/Content/Img/Noticias/{0}", archivo));
        }

        public static string Plugins(this UrlHelper helper, string archivo)
        {
            return helper.Content(String.Format("~/Content/Js/Jquery/Plugins/{0}", archivo));
        }

        public static StringBuilder TableTools(this UrlHelper helper)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("<link href='" + helper.Ruta("Scripts/jQuery/Plugins//DataTables/extras/TableTools/media/css/TableTools.css") + "'  rel='stylesheet' type='text/css' />");
            output.AppendLine("<script type='text/javascript' src='" + helper.Ruta("Scripts/jQuery/Plugins/DataTables/extras/TableTools/media/ZeroClipboard/ZeroClipboard.js") + "'></script>");
            output.AppendLine("<script type='text/javascript' src='" + helper.Ruta("Scripts/jQuery/Plugins/DataTables/extras/TableTools/media/js/TableTools.js") + "'></script>");

            return output;
        }

        public static StringBuilder Datatables(this UrlHelper helper, bool example = false)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("<script type='text/javascript' src='" + helper.Ruta("Scripts/jQuery/Plugins/DataTables/js/jquery.dataTables.js") + "'></script>");
            return output;
        }

        public static bool FileExists(this UrlHelper helper, string ruta)
        {
            string sourceFile = HttpContext.Current.Server.MapPath("~" + ruta);
            if (System.IO.File.Exists(@"" + sourceFile))
            {
                return true;
            }
            return false;
        }

        public static StringBuilder Uploadify(this UrlHelper helper)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("<link href='" + helper.Ruta("Scripts/jQuery/Plugins/UploadFy/css/uploadify.css") + "'  rel='stylesheet' type='text/css' />");
            output.AppendLine("<script type='text/javascript' src='" + helper.Ruta("Scripts/jQuery/Plugins/UploadFy/scripts/swfobject.js") + "'></script>");
            output.AppendLine("<script type='text/javascript' src='" + helper.Ruta("Scripts/jQuery/Plugins/UploadFy/scripts/jquery.uploadify.v2.1.0.min.js") + "'></script>");
            return output;
        }
    }
}

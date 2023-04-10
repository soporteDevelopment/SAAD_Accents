using ADEntities.Queries;
using ADEntities.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Helpers
{
    public static class HtmlHelperExtension
    {

        const int True = 1;

        const int False = 0;

        public static string Button(string controller, string action, string name = "", string id = "", string value = "", string model = "", string style = "", string click = "", string styleClass = "", string dataLoading = "")
        {
            HttpContext httpContext = HttpContext.Current;
            List<ActionViewModel> lExceptions = new List<ActionViewModel>();

            string salida = "";

            lExceptions.Add(new ActionViewModel { Accion = "Cancelar" });

            string input = string.Format("<button type='button' name='{0}' id='{1}' data-ng-click='{2}' class='{3}' data-loading-text='{4}'>{5}</button>", name, id, click, styleClass, dataLoading, value);

            if (lExceptions.Any(p => p.Accion.ToLower() == name.ToLower()))
            {
                salida = input;
            }
            else
            {
                UserViewModel sUsuario = (UserViewModel)httpContext.Session["_User"];
                if (sUsuario == null)
                {
                    return salida;
                }

                List<ActionViewModel> sAction = (List<ActionViewModel>)httpContext.Session["_Actions"];
                if (sAction.Any(p => p.Control.ToLower() == controller.ToLower() && p.Accion.ToLower() == action.ToLower()))
                {
                    salida = input;
                }
            }
            return salida;
        }

        public static int Icon(string controller, string action)
        {
            HttpContext httpContext = HttpContext.Current;
            List<ActionViewModel> lExceptions = new List<ActionViewModel>();

            UserViewModel sUsuario = (UserViewModel)httpContext.Session["_User"];
            if (sUsuario == null)
            {
                return False;
            }

            List<ActionViewModel> sAction = (List<ActionViewModel>)httpContext.Session["_Actions"];
            if (sAction.Any(p => p.Control.ToLower() == controller.ToLower() && p.Accion.ToLower() == action.ToLower()))
            {
                return True;
            }

            return False;
        }

        public static int ButtonMixedComboPromotion()
        {
            var promotions = new Promotions();

            if (promotions.GetMixedComboPromotion() != null)
            {
                return True;
            }

            return False;
        }

    }
}

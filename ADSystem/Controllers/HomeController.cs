using ADEntities.ViewModels;
using ADSystem.Common;
using System.Web.Mvc;

namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    [AuthorizedModule]
    public class HomeController : BaseController
    {
        // GET: Home
        public override ActionResult Index()
        {
            ViewData.Add("Title", "Inicio");
            
            UserViewModel user = (UserViewModel) Session["_User"];

            if(user.Ventas == Constants.sActivo)
            {

                return RedirectToAction("Index", "Sales");

            }

            return View();
        }               

    }
}
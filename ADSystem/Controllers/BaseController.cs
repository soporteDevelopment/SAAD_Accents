using ADEntities.Queries;
using ADEntities.ViewModels;
using ADSystem.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using ADEntities.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace ADSystem.Controllers
{
    public abstract class BaseController : Controller
    {
        #region General
        public string BaseURL { get; set; }

        #region Instancias
        public Users tUsers = new Users();
        public Branches tBranches = new Branches();
        public Modules tModules = new Modules();
        public Actions tActions = new Actions();
        public Categories tCategories = new Categories();
        public SubCategories tSubcategories = new SubCategories();
        public Products tProducts = new Products();
        public Provider tProviders = new Provider();
        public Materials tMaterials = new Materials();
        public Profiles tProfiles = new Profiles();
        public States tStates = new States();
        public Catalogs tCatalogs = new Catalogs();
        public BranchCatalogs tBranchCatalogs = new BranchCatalogs();
        public Brands tBrands = new Brands();
        public Events tEvents = new Events();
        public UserEvent tUserEvent = new UserEvent();
        public MoralCustomers tMoralCustomers = new MoralCustomers();
        public PhysicalCustomers tPhysicalCustomers = new PhysicalCustomers();
        public Orders tOrders = new Orders();
        public OutProducts tVistas = new OutProducts();
        public EraserOutProducts tBorradorVistas = new EraserOutProducts();
        public Offices tOffices = new Offices();
        public Sales tSales = new Sales();
        public Transfers tTransfers = new Transfers();
        public Receptions tReceptions = new Receptions();
        public EraserSales tEraserSales = new EraserSales();
        public Quotations tQuotations = new Quotations();
        public UnifiedQuotations tUnifiedQuotations = new UnifiedQuotations();
        public Credits tCredits = new Credits();
        public Services tServices = new Services();
        public Stocks tStocks = new Stocks();
        public Reports tReports = new Reports();
        public GiftsTable tGiftsTable = new GiftsTable();
        public Discounts tDiscounts = new Discounts();
        public Promotions tPromotions = new Promotions();
        public Repairs tRepairs = new Repairs();
        public Entries tEntries = new Entries();
        public Egresses tEgresses = new Egresses();
        public Binnacle tBinnacle = new Binnacle();
        public CreditHistory tCreditHistory = new CreditHistory();
        public ReportEntries tReportEntries = new ReportEntries();
        public TypeService tTypeService = new TypeService();
        public TypeMeasure tTypeMeasure = new TypeMeasure();
        public Textiles tTextiles = new Textiles();
        public PreQuotations tPreQuotations = new PreQuotations();
        /* 
        public PreCotizacionDetalle tPreCotizacionDetalle  = new PreCotizacionDetalle();
        public PreCotDetalleMedida tPreCotDetalleMedida  = new PreCotDetalleMedida();
        public PreCotDetalleTipoTela tPreCotDetalleTipoTela  = new PreCotDetalleTipoTela();
        public PreCotDetalleProveedore tPreCotDetalleProveedore  = new PreCotDetalleProveedore();*/


        #endregion

        public string AccentsEmail = WebConfigurationManager.AppSettings["FromEmail"].ToString();
        public string SendGridKey = WebConfigurationManager.AppSettings["SendGridKey"].ToString();
        public string FernandaEmail = WebConfigurationManager.AppSettings["FernandaEmail"].ToString();
        public string AnnaEmail = WebConfigurationManager.AppSettings["AnnaEmail"].ToString();
        public string ManagerEmail = WebConfigurationManager.AppSettings["ManagerEmail"].ToString();
        public bool GetSendMail = Convert.ToBoolean(WebConfigurationManager.AppSettings["SendMail"].ToString());
        public bool GetSendCC = Convert.ToBoolean(WebConfigurationManager.AppSettings["SendCC"].ToString());

        public MessagesTemplates Message = new MessagesTemplates();
        public abstract ActionResult Index();
        #endregion

        public virtual ActionResult GetStates()
        {
            return Json(tStates.GetStates(), JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult GetTowns(int idState)
        {
            return Json(tStates.GetTownsForState(idState), JsonRequestBehavior.AllowGet);
        }

        public virtual string CreateBodyMail(string password)
        {
            StringBuilder body = new StringBuilder();

            body.Append("<p>Está es la nueva contraseña que se le ha generado.</p>");
            body.Append("<br/>");
            body.Append("<p>Contraseña:&nbsp;<b class='brand-danger'>" + password + "</b></p>");
            body.Append("<br/>");
            body.Append("<p>Nota:Recuerde cambiar su contraseña al ingresar</p>");

            return body.ToString();
        }

        [HttpPost]
        public virtual ActionResult GetBranch()
        {
            return Json(tBranches.GetAllBranches());
        }

        protected string CleanCsvString(string input)
        {
            string output = "\"" + input.Replace("\"", "\"\"").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", "") + "\"";
            return output;
        }
    }
}
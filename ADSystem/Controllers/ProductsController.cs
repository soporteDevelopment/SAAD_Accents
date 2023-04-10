using ADEntities.Models;
using ADEntities.Queries;
using ADEntities.ViewModels;
using ADSystem.Common;
using ADSystem.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ADSystem.Controllers
{
    [SessionExpiredFilter]
    //[AuthorizedModule]
    public class ProductsController : BaseController
    {
        public Promotions promotions = new Promotions();

        public override ActionResult Index()
        {
            Session["Controller"] = "Productos";

            return View();
        }

        public ActionResult AssigningCategory()
        {
            Session["Controller"] = "Productos";

            return View();
        }

        [HttpPost]
        public ActionResult ListProducts(string description, string code, decimal? cost, string category, string subcategory, string color, string material, string brand, short? branch, int orderBy, bool ascending, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                Session["page"] = page;
                Session["pageSize"] = pageSize;

                List<ProductViewModel> products = new List<ProductViewModel>();
                int count = 0;

                switch (branch)
                {
                    case 2:
                    case 3:
                    case 4:
                        products = tProducts.GetProducts(description, code, cost, category, subcategory, color, material, brand, branch, orderBy, ascending, page, pageSize);
                        count = tProducts.CountRegistersWithFilters(description, code, cost, category, subcategory, color, material, brand, branch);
                        break;
                    case 5:
                        products = tProducts.GetProductsWithDamage(description, code, cost, category, subcategory, color, material, brand, orderBy, ascending, page, pageSize);
                        count = tProducts.CountRegistersWithFiltersWithDamage(description, code, cost, category, subcategory, color, material, brand);
                        break;
                    default:
                        products = tProducts.GetProducts(description, code, cost, category, subcategory, color, material, brand, orderBy, ascending, page, pageSize);
                        count = tProducts.CountRegistersWithFilters(description, code, cost, category, subcategory, color, material, brand);
                        break;
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = products, Count = count };
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
        public ActionResult ListProductsPrintBarCode(string description, string code, decimal? cost, string category, string color, string material, string order, string brand, short? branch, int page, int pageSize)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                Session["page"] = page;
                Session["pageSize"] = pageSize;

                List<ProductViewModel> products = new List<ProductViewModel>();
                int count = 0;

                switch (branch)
                {
                    case 2:
                    case 3:
                    case 4:
                        products = tProducts.GetProductsPrintBarCode(description, code, cost, category, color, material, order, brand, branch, page, pageSize);
                        count = tProducts.CountRegistersWithFiltersPrintBarCode(description, code, cost, category, color, material, order, brand, branch);
                        break;
                    default:
                        products = tProducts.GetProductsPrintBarCode(description, code, cost, category, color, material, order, brand, page, pageSize);
                        count = tProducts.CountRegistersWithFiltersPrintBarCode(description, code, cost, category, color, material, order, brand);
                        break;
                }

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = products, Count = count };
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
        public ActionResult ListProductsForBranch(int idStock, int branch, string description, string code, decimal? cost, string category, string color, string material, string brand, bool stockZero, int page, int pageSize, bool orderASC)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = tProducts.GetProductsForBranch(idStock, branch, description, code, cost, category, color, material, brand, stockZero, page, pageSize, orderASC), Count = tProducts.CountRegistersForBranch(branch, description, code, cost, category, color, material, brand) };

            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public PartialViewResult AddProduct()
        {
            ViewBag.ProductBranch = JsonConvert.SerializeObject(tProducts.GetBranches());

            return PartialView("AddProduct");
        }

        [HttpPost]
        public ActionResult SaveAddProduct(string Name, string ComercialName, string Description, int ProveedorId, decimal? BuyPrice, decimal SalePrice, int CategoryId, int? SubCategoryId, string Color, int? MaterialId, string Measure, decimal? Weight, string Code, string Comments, List<ProductBranchViewModel> Stock, string Filename, string urlImage, string ImgType, string Extension)
        {
            JsonMessenger jmResult = new JsonMessenger();

            int tipoImagen = 0;
            bool bandTipoImagen = false;
            tipoImagen = (ImgType == "file") ? 1 : 2;   /* file = 1, url = 2 */
            bandTipoImagen = ((tipoImagen == 1 && Filename != "") || (tipoImagen == 2 && urlImage != "")) ? true : false;

            if (ProveedorId == 0)
                ProveedorId = int.Parse(Session["idProvedor"].ToString());

            try
            {
                if (!tProducts.ValidateProductExist(Code))
                {
                    var cleanUrl = String.Empty;

                    if (urlImage != "")
                    {
                        cleanUrl = urlImage.Replace("url(\"", string.Empty);
                        cleanUrl = cleanUrl.Replace("\")", string.Empty);
                        urlImage = cleanUrl;
                    }

                    tProducto oProduct = new tProducto();

                    oProduct.Nombre = Name;
                    oProduct.NombreComercial = ComercialName;
                    oProduct.Descripcion = Description;
                    oProduct.PrecioCompra = BuyPrice;
                    oProduct.PrecioVenta = SalePrice;
                    oProduct.idProveedor = ProveedorId;
                    oProduct.idCategoria = CategoryId;
                    oProduct.idSubcategoria = SubCategoryId;
                    oProduct.Color = Color;
                    oProduct.idMaterial = MaterialId;
                    oProduct.Medida = Measure;
                    oProduct.Peso = Weight;
                    oProduct.Comentarios = Comments;
                    oProduct.Codigo = Code.Trim();
                    oProduct.Estatus = Constants.sActivo;
                    oProduct.Extension = (!String.IsNullOrEmpty(Extension)) ? Extension.ToUpper() : null;
                    oProduct.TipoImagen = (bandTipoImagen) ? (short)tipoImagen : (short)0;
                    oProduct.urlImagen = (tipoImagen == 2 && urlImage != "") ? urlImage : "";
                    oProduct.NombreImagen = Filename;
                    oProduct.CreadoPor = Convert.ToInt32(Session["_ID"]);
                    oProduct.Creado = DateTime.Now;

                    int iResult = tProducts.AddProduct(oProduct);

                    if (iResult > 0)
                    {
                        if (Stock != null)
                        {
                            tProducts.AddProductBranchExist(iResult, Stock, oProduct.PrecioVenta, (int)Session["_ID"]);
                        }

                        jmResult.success = 1;
                        jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se agregó un registro al módulo {0}", "Productos"), idProducto = iResult };
                    }
                    else
                    {
                        jmResult.success = 0;
                        jmResult.failure = 1;
                        jmResult.oData = new { Error = "El código del producto ya está registrado." };
                    }
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El código del producto ya está registrado." };
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

        public ViewResult UpdateProduct(int idProduct)
        {
            ProductViewModel oProduct = (ProductViewModel)tProducts.GetProduct(idProduct);
            ViewBag.ProductBranch = JsonConvert.SerializeObject(tProducts.GetBranchesForProducts(idProduct));

            return View(oProduct);
        }

        [HttpPost]
        public ActionResult SaveUpdateProduct(int idProduct, string Name, string ComercialName, string Description, int ProveedorId, decimal? BuyPrice, decimal SalePrice, int CategoryId, int? SubCategoryId, string Color, int? MaterialId, string Measure, decimal? Weight, string Code, string Comments, string Brand, string justify, List<ProductBranchViewModel> Stock, string urlImage, string ImgType, string ImageName)
        {
            JsonMessenger jmResult = new JsonMessenger();
            int tipoImagen = 0;
            tipoImagen = (ImgType == "file") ? 1 : 2;   /* file = 1, url = 2 */

            try
            {
                if (!tProducts.ValidateProductExist(idProduct, Code))
                {
                    var cleanUrl = String.Empty;

                    if (urlImage != "")
                    {
                        cleanUrl = urlImage.Replace("url(\"", string.Empty);
                        cleanUrl = cleanUrl.Replace("\")", string.Empty);
                        urlImage = cleanUrl;
                    }

                    ProductViewModel oProduct = new ProductViewModel();
                    oProduct.idProducto = idProduct;
                    oProduct.Nombre = Name;
                    oProduct.NombreComercial = ComercialName;
                    oProduct.Descripcion = Description;
                    oProduct.PrecioCompra = BuyPrice;
                    oProduct.PrecioVenta = SalePrice;
                    oProduct.idProveedor = ProveedorId;
                    oProduct.idCategoria = CategoryId;
                    oProduct.idSubcategoria = SubCategoryId;
                    oProduct.Color = Color;
                    oProduct.idMaterial = MaterialId;
                    oProduct.Medida = Measure;
                    oProduct.Peso = Weight;
                    oProduct.Comentarios = Comments;
                    oProduct.Codigo = Code.Trim();
                    oProduct.TipoImagen = (short)tipoImagen;
                    oProduct.urlImagen = (tipoImagen == 2 && urlImage != "") ? urlImage : "";
                    oProduct.NombreImagen = ImageName;
                    oProduct.ModificadoPor = Convert.ToInt32(Session["_ID"]);
                    oProduct.Modificado = DateTime.Now;

                    var oldPriceSale = tProducts.GetProduct(idProduct).PrecioVenta;

                    if (tProducts.UpdateProduct(oProduct) == Constants.sActivo)
                    {
                        tProducts.UpdateProductBranchExist(idProduct, Stock, oldPriceSale, oProduct.PrecioVenta, justify, Convert.ToInt32(Session["_ID"]));

                        jmResult.success = 1;
                        jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Productos") };
                    }
                    else
                    {
                        jmResult.success = 0;
                        jmResult.failure = 1;
                        jmResult.oData = new { Error = "El código ya se encuentra registrado" };
                    }
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El código ya se encuentra registrado" };
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

        public ActionResult UpdateProductOnLine(int idProduct)
        {
            ProductViewModel oProduct = (ProductViewModel)tProducts.GetProduct(idProduct);
            ViewBag.ProductBranch = JsonConvert.SerializeObject(tProducts.GetBranchesForProducts(idProduct));

            return PartialView("~/Views/Products/UpdateProductOnLine.cshtml", oProduct);
        }

        [HttpPost]
        public ActionResult SaveUpdateProductOnLine(int idProduct, string Name, string Description, int ProveedorId, decimal SalePrice, string Code, string Comments, string Justify, List<ProductBranchViewModel> Stock)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (!tProducts.ValidateProductExist(idProduct, Code))
                {
                    ProductViewModel oProduct = new ProductViewModel();
                    oProduct.idProducto = idProduct;
                    oProduct.Nombre = Name;
                    oProduct.Descripcion = Description;
                    oProduct.PrecioVenta = SalePrice;
                    oProduct.idProveedor = ProveedorId;
                    oProduct.Comentarios = Comments;
                    oProduct.Codigo = Code.Trim();
                    oProduct.ModificadoPor = Convert.ToInt32(Session["_ID"]);
                    oProduct.Modificado = DateTime.Now;

                    var oldPriceSale = tProducts.GetProduct(idProduct).PrecioVenta;

                    if (tProducts.UpdateProductOnLine(oProduct))
                    {
                        tProducts.UpdateProductBranchExist(idProduct, Stock, oldPriceSale, oProduct.PrecioVenta, Justify, Convert.ToInt32(Session["_ID"]));

                        jmResult.success = 1;
                        jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Productos") };
                    }
                    else
                    {
                        jmResult.failure = 1;
                        jmResult.oData = new { Error = "El código del producto ya está registrado." };
                    }
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El código ya se encuentra registrado" };
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
        public ActionResult SaveUpdateProductStock(int idProduct, string Name, string ComercialName, string Description, int ProveedorId, decimal? BuyPrice, decimal SalePrice, int CategoryId, int? SubCategoryId, string Color, int? MaterialId, string Measure, decimal? Weight, string Code, string Comments, string Brand, List<ProductBranchViewModel> Stock, string urlImage, string ImgType, string ImageName)
        {
            JsonMessenger jmResult = new JsonMessenger();
            int tipoImagen = 0;
            tipoImagen = (ImgType == "file") ? 1 : 2;   /* file = 1, url = 2 */

            try
            {
                if (!tProducts.ValidateProductExist(idProduct, Code))
                {
                    var cleanUrl = String.Empty;

                    if (urlImage != "")
                    {
                        cleanUrl = urlImage.Replace("url(\"", string.Empty);
                        cleanUrl = cleanUrl.Replace("\")", string.Empty);
                        urlImage = cleanUrl;
                    }

                    ProductViewModel oProduct = new ProductViewModel();
                    oProduct.idProducto = idProduct;
                    oProduct.Nombre = Name;
                    oProduct.NombreComercial = ComercialName;
                    oProduct.Descripcion = Description;
                    oProduct.PrecioCompra = BuyPrice;
                    oProduct.PrecioVenta = SalePrice;
                    oProduct.idProveedor = ProveedorId;
                    oProduct.idCategoria = CategoryId;
                    oProduct.idSubcategoria = SubCategoryId;
                    oProduct.Color = Color;
                    oProduct.idMaterial = MaterialId;
                    oProduct.Medida = Measure;
                    oProduct.Peso = Weight;
                    oProduct.Comentarios = Comments;
                    oProduct.Codigo = Code.Trim();
                    oProduct.TipoImagen = (short)tipoImagen;
                    oProduct.urlImagen = (tipoImagen == 2 && urlImage != "") ? urlImage : "";
                    oProduct.NombreImagen = ImageName;
                    oProduct.ModificadoPor = Convert.ToInt32(Session["_ID"]);
                    oProduct.Modificado = DateTime.Now;

                    var oldPriceSale = tProducts.GetProduct(idProduct).PrecioVenta;

                    if (tProducts.UpdateProduct(oProduct) == Constants.sActivo)
                    {
                        tProducts.UpdateProductBranchExist(idProduct, Stock, oldPriceSale, oProduct.PrecioVenta, "", Convert.ToInt32(Session["_ID"]));

                        jmResult.success = 1;
                        jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se actualizó un registro en el módulo {0}", "Productos") };
                    }
                    else
                    {
                        jmResult.success = 0;
                        jmResult.failure = 1;
                        jmResult.oData = new { Error = "El código ya se encuentra registrado" };
                    }
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "El código ya se encuentra registrado" };
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
        public ActionResult UpdateStatusProducto(int idProducto, short bStatus)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                short status = Convert.ToInt16(bStatus);
                bool bResult = tProducts.UpdateStatusProducto(idProducto, status, Convert.ToInt32(Session["_ID"]));

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = String.Format(Message.msgAdd = "Se eliminó un registro en el módulo {0}", "Productos") };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetCategoriesForName(string category)
        {
            return Json(new { Category = tCategories.GetCategoriesForName(category) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategories()
        {
            JsonMessenger jmResult = new JsonMessenger();


            jmResult.success = 1;
            jmResult.failure = 0;
            jmResult.oData = new { Category = tCategories.GetCategories() };

            return Json(jmResult);
        }

        [HttpPost]
        public ActionResult GetSubcategories(int idCategory)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Subcategories = tSubcategories.GetSubcategoryByCategory(idCategory) };
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
        public ActionResult GetMaterials()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Materials = tMaterials.GetMaterials() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetMaterials(string material)
        {
            return Json(new { Materials = tMaterials.GetMaterials(material) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProductsForName()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = tProducts.GetProducts() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetProductsForName(string description)
        {
            return Json(new { Descriptions = tProducts.GetDescription(description) }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetProductsForCodigo()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = tProducts.GetCodigos() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetProductsForCodigo(string codigo)
        {
            return Json(new { Codigos = tProducts.GetCodigos(codigo) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProductsForPrecio()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = tProducts.GetPrecios() };
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
        public ActionResult GetProductsForColors()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = tProducts.GetColors() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetProductsForColors(string color)
        {
            return Json(new { Colors = tProducts.GetColors(color) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProductsForBrands()
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Products = tProducts.GetBrands() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult GetProductsForBrands(string brand)
        {

            return Json(new { Brands = tProducts.GetBrands(brand) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetBrandsForProvider(int idProvider)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Brands = tBrands.GetBrands() };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };

            }

            return Json(jmResult);
        }

        public ActionResult Barcode(string pedido)
        {

            return File(DrawEan13(pedido), "image/jpg");
        }

        private static byte[] DrawEan13(string valueToCode)
        {
            var resultado = new Byte[] { };

            using (var stream = new MemoryStream())
            {

                var bitmap = new Bitmap(200, 60);

                var grafic = Graphics.FromImage(bitmap);

                grafic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.SystemColors.Control),
                new Rectangle(0, 0, 200, 60));

                // Create an instance of the Ean13 Class.        
                Ean13 upc = new Ean13();

                upc.CodePedido = valueToCode;
                upc.Scale = 1.5F;

                upc.DrawEan13Barcode(grafic, new System.Drawing.Point(0, 0));

                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                stream.Seek(0, SeekOrigin.Begin);

                resultado = stream.ToArray();

            }

            return resultado;
        }

        [HttpPost]
        public virtual ActionResult UploadFile(int productId, string option, int qtyRotates)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                HttpPostedFileBase myFile = Request.Files["file"];
                bool isUploaded = false;
                string message = "La carga de la imagen falló";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(myFile.FileName);
                string imgName = string.Empty;

                Image img = Image.FromStream(myFile.InputStream);
                Bitmap bmp = img as Bitmap;
                Graphics g = Graphics.FromImage(bmp);
                Bitmap bmpNew = new Bitmap(bmp);
                g.DrawImage(bmpNew, new Point(0, 0));
                g.Dispose();
                bmp.Dispose();
                img.Dispose();

                switch (qtyRotates)
                {
                    case 1:
                        bmpNew.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 2:
                        bmpNew.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 3:
                        bmpNew.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        break;
                }

                if ((myFile != null && myFile.ContentLength != 0) && (extension.ToUpper() == ".PNG" || extension.ToUpper() == ".GIF" || extension.ToUpper() == ".JPEG" || extension.ToUpper() == ".JPG"))
                {
                    string pathForSaving = Server.MapPath("~/Content/Products");
                    if (this.CreateFolderIfNeeded(pathForSaving))
                    {

                        imgName = string.Format("{0}{1}", fileName, extension);
                        bmpNew.Save(Path.Combine(pathForSaving, imgName));

                        isUploaded = true;
                        message = "Imagen cargada correctamente";

                        if (option == "UPD")
                        {
                            ProductViewModel oProduct = new ProductViewModel();
                            oProduct.idProducto = productId;
                            oProduct.Extension = extension.ToUpper();
                            tProducts.UpdateProductExtn(oProduct);
                        }

                    }
                }

                return Json(new { isUploaded, message = message, fileName = fileName, extension = extension }, "text/html");
            }
            catch (Exception ex)
            {
                return Json(new { message = string.Format("La carga de la imagen falló: {0}", ex.Message) });
            }
        }
        
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult GetProduct(int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                product._Existencias = tProducts.GetProductsBranches((int)product.idProducto);
                product.Stock = tProducts.GetProductsBranches((int)product.idProducto).Sum(x => x.Existencia);
                product.Vista = tProducts.GetProductsOutPro((int)product.idProducto);
                product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Product = product };
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
        public ActionResult GetForEraserProduct(int idProduct, int idView)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                product._Existencias = tProducts.GetProductsBranchesForEraserSale((int)product.idProducto, idView);
                product.Stock = tProducts.GetProductsBranchesForEraserSale((int)product.idProducto, idView).Sum(x => x.Existencia);
                product.Vista = tProducts.GetProductsOutProForEraser((int)product.idProducto, idView);
                product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Product = product };
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
        public ActionResult GetProductForOutProduct(int idView, int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                product._Existencias = tProducts.GetProductsBranchesForOutProduct(idView, (int)product.idProducto);
                product.Stock = tProducts.GetProductsBranchesForOutProduct(idView, (int)product.idProducto).Sum(x => x.Existencia);
                product.Vista = tProducts.GetProductsForOutProduct(idView, (int)product.idProducto);
                product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Product = product };
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
        public ActionResult GetProductQuotation(int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                product._Existencias = tProducts.GetProductsBranchesForQuotation((int)product.idProducto);
                product.Stock = tProducts.GetProductsBranchesForQuotation((int)product.idProducto).Sum(x => x.Existencia);
                product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Product = product };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        [HttpGet]
        public ActionResult GetProductQuotationOriginView(int idProduct, int idView)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                product._Existencias = tProducts.GetProductsBranchesForEraserSale((int)product.idProducto, idView);
                product.Stock = tProducts.GetProductsBranchesForEraserSale((int)product.idProducto, idView).Sum(x => x.Existencia);
                product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Product = product };
            }
            catch (Exception ex)
            {

                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProductByBranch(int idBranch, int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();
            string msg = "";

            try
            {
                if (tProducts.ValidateOutProduct(idBranch, idProduct, out msg))
                {
                    var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                    product._Existencias = tProducts.GetProductsBranches((int)product.idProducto);
                    product.Stock = tProducts.GetProductForIdAndBranch(idBranch, idProduct);
                    product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Product = product };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = msg };
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
        public ActionResult GetProductOutProduct(int idBranch, int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();
            string msg = "";

            try
            {

                if (tProducts.ValidateOutProduct(idBranch, idProduct, out msg))
                {

                    var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                    product._Existencias = tProducts.GetProductsBranches((int)product.idProducto);
                    product.Stock = tProducts.GetProductsBranches((int)product.idProducto).Sum(x => x.Existencia);
                    product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Product = product };

                }
                else
                {

                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = msg };

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
        public ActionResult GetProductForId(string idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                if (Ean13.CheckCode(idProduct))
                {
                    string sProduct = idProduct.TrimStart('0');

                    int iProduct = Convert.ToInt32(sProduct.Remove(sProduct.Length - 1));

                    var product = (ProductViewModel)tProducts.GetProduct(iProduct);

                    product._Existencias = tProducts.GetProductsBranches((int)product.idProducto);
                    product.Stock = tProducts.GetProductsBranches((int)product.idProducto).Sum(x => x.Existencia);
                    product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Product = product };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = "Código incorrecto" };
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
        public ActionResult GetProductForIdAndView(int idBranch, int idView, int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Stock = tProducts.GetProductForIdAndView(idBranch, idView, idProduct) };
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
        public ActionResult GetProductForIdAndBranch(int idBranch, int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Stock = tProducts.GetProductForIdAndBranch(idBranch, idProduct) };
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
        public ActionResult GetCostProduct(int idProduct, int amount)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Total = (idProduct * amount) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        public ActionResult PrintTickets()
        {
            Session["Controller"] = "Productos";

            return View();
        }

        public ActionResult PrintTicketsProducts(string lProducts)
        {

            List<KeyValuePair<int, int>> listProducts = new List<KeyValuePair<int, int>>();

            string[] splitOne = lProducts.Split('|');

            foreach (var a in splitOne)
            {

                string[] splitTwo = a.Split(',');

                listProducts.Add(new KeyValuePair<int, int>(Convert.ToInt32(splitTwo[0]), Convert.ToInt32(splitTwo[1])));

            }


            List<ProductViewModel> products = tProducts.GetProductsPrintTickets(listProducts);

            return PartialView(products);
        }

        public ActionResult PrintAllTicketsOfOrder(int idOrder)
        {

            List<ProductViewModel> products = tProducts.GetAllProductsPrintTickets(idOrder);

            return PartialView("~/Views/Products/PrintTicketsProducts.cshtml", products);

        }

        [HttpPost]
        public ActionResult GetProductOutProductUnify(int idBranch, int idProduct)
        {
            JsonMessenger jmResult = new JsonMessenger();
            string msg = "";

            try
            {

                if (tProducts.ValidateOutProduct(idBranch, idProduct, out msg))
                {
                    var product = (ProductViewModel)tProducts.GetProduct(idProduct);

                    product._Existencias = tProducts.GetProductsBranches((int)product.idProducto);
                    product.Stock = tProducts.GetProductsBranches((int)product.idProducto).Sum(x => x.Existencia);
                    product.Promotion = promotions.GetPromotionProduct(DateTime.Now, (int)product.idProducto);

                    jmResult.success = 1;
                    jmResult.failure = 0;
                    jmResult.oData = new { Product = product };
                }
                else
                {
                    jmResult.success = 0;
                    jmResult.failure = 1;
                    jmResult.oData = new { Error = msg };
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
        public ActionResult GetAmountProductByView(int viewID, int productID)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Amount = tProducts.GetAmountProductByView(productID, viewID) };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

        //UpdateCategoryOnLine
        public ActionResult UpdateCategoryOnLine(int id)
        {
            ProductViewModel oProduct = (ProductViewModel)tProducts.GetProduct(id);

            return PartialView("~/Views/Products/UpdateCategoryOnLine.cshtml", oProduct);
        }

        [HttpPatch]
        public ActionResult SaveUpdateCategoryOnLine(int id, int categoryId, int? subcategoryId)
        {
            JsonMessenger jmResult = new JsonMessenger();

            try
            {
                tProducts.UpdateCategory(id, categoryId, subcategoryId);

                jmResult.success = 1;
                jmResult.failure = 0;
                jmResult.oData = new { Message = "Producto actualizado" };
            }
            catch (Exception ex)
            {
                jmResult.success = 0;
                jmResult.failure = 1;
                jmResult.oData = new { Error = ex.Message };
            }

            return Json(jmResult);
        }

    }
}
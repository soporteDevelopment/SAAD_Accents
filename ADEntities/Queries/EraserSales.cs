using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class EraserSales : Base
    {

        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tBorradorVentas.Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public List<SaleViewModel> GetEraserSales(string costumer, int? iduser, string project, int idUser, short? restricted, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var sales = context.tBorradorVentas.Where(
                    p => (p.Proyecto.ToUpper().Contains(project.ToUpper()) || (String.IsNullOrEmpty(project))) &&
                    ((p.idUsuario1 == iduser || p.idUsuario2 == iduser) || iduser == null) &&
                    (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                    ((p.tClientesMorale.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                    ((p.tDespacho.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())))) &&
                    (((p.tSucursale.idSucursal == amazonas) || (p.tSucursale.idSucursal == guadalquivir) || (p.tSucursale.idSucursal == textura)) ||
                    (amazonas == null && guadalquivir == null && textura == null))

                ).Select(p => new SaleViewModel()
                {
                    idVenta = p.idBorradorVenta,
                    idUsuario1 = p.idUsuario1,
                    Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                    idUsuario2 = p.idUsuario2,
                    Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                    idClienteFisico = p.idClienteFisico,
                    ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
                    idClienteMoral = p.idClienteMoral,
                    ClienteMoral = p.tClientesMorale.Nombre,
                    idDespacho = p.idDespacho,
                    Despacho = p.tDespacho.Nombre,
                    Proyecto = p.Proyecto,
                    idDespachoReferencia = p.idDespachoReferencia,
                    DespachoReferencia = p.tDespacho1.Nombre,
                    idSucursal = p.idSucursal,
                    Sucursal = p.tSucursale.Nombre,
                    Fecha = p.Fecha,
                    CantidadProductos = p.CantidadProductos,
                    Subtotal = p.Subtotal,
                    Descuento = p.Descuento,
                    IVA = p.IVA,
                    Total = p.Total
                }).OrderByDescending(p => p.Fecha);

                    List<SaleViewModel> lSales = sales.Skip(page * pageSize).Take(pageSize).ToList();

                    if (restricted == 1)
                    {
                        lSales = lSales.Where(p => p.idUsuario1 == idUser).ToList();
                    }

                    return lSales;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public int AddEraserSale(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
            string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, int? idView, decimal IVATasa)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tBorradorVenta oVentas = new tBorradorVenta();

                    oVentas.idUsuario1 = idUser1;
                    oVentas.idUsuario2 = idUser2;
                    oVentas.idClienteFisico = idCustomerP;
                    oVentas.idClienteMoral = idCustomerM;
                    oVentas.idDespacho = idOffice;
                    oVentas.Proyecto = project;
                    oVentas.idDespachoReferencia = idOfficeReference;
                    oVentas.idSucursal = idBranch;
                    oVentas.Fecha = Convert.ToDateTime(dateSale);
                    oVentas.CantidadProductos = amountProducts;
                    oVentas.Subtotal = subtotal;
                    oVentas.Descuento = discount;
                    oVentas.IVA = IVA;
                    oVentas.IVATasa = IVATasa;
                    oVentas.Total = total;
                    oVentas.idVista = idView;
                    oVentas.TipoCliente = typeCustomer;

                    context.tBorradorVentas.Add(oVentas);

                    context.SaveChanges();

                    context.SaveChanges();

                    foreach (var prod in lProducts)
                    {
                        this.AddEraserProductSale(oVentas.idBorradorVenta, prod);

                        if (prod.credito == Base.iTRue)
                        {
                            Credits oCredits = new Credits();
                            oCredits.SetStatus(prod.codigo, TypesCredit.creditoSaldada);
                        }
                    }

                    return oVentas.idBorradorVenta;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void AddEraserProductSale(int idSale, ProductSaleViewModel oProduct)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tBorradorDetalleVenta oDetalleVenta = new tBorradorDetalleVenta();

                    oDetalleVenta.idBorradorVenta = idSale;

                    if (oProduct.Tipo == 2 && !String.IsNullOrEmpty(oProduct.imagen))
                    {
                        char delimiter = '/';

                        var imagen = oProduct.imagen.Split(delimiter);

                        oDetalleVenta.Imagen = imagen[3];
                    }

                    if (oProduct.Tipo != 3)
                    {
                        oDetalleVenta.idProducto = oProduct.idProducto;
                    }

                    oDetalleVenta.idServicio = oProduct.idServicio;

                    if (oProduct.Tipo == 3)
                    {
                        oDetalleVenta.idNotaCredito = oProduct.idProducto;
                    }

                    oDetalleVenta.Descripcion = oProduct.desc;
                    oDetalleVenta.Precio = oProduct.prec;
                    oDetalleVenta.Descuento = oProduct.descuento;
                    oDetalleVenta.Cantidad = oProduct.cantidad;
                    oDetalleVenta.idPromocion = oProduct.idPromocion;
                    oDetalleVenta.idTipoPromocion = oProduct.idTipoPromocion;
                    oDetalleVenta.idProductoPadre = oProduct.idProductoPadre;
                    oDetalleVenta.Comentarios = oProduct.comentarios;

                    context.tBorradorDetalleVentas.Add(oDetalleVenta);

                    context.SaveChanges();

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public SaleViewModel GetEraserSaleForId(int idEraserSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var eraserSale = context.tBorradorVentas.Where(p => p.idBorradorVenta == idEraserSale).Select(p => new SaleViewModel()
                    {

                        idVenta = p.idBorradorVenta,
                        idUsuario1 = p.idUsuario1,
                        Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        idUsuario2 = p.idUsuario2,
                        Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
                        idClienteFisico = p.idClienteFisico,
                        idClienteMoral = p.idClienteMoral,
                        ClienteFisico = (p.idClienteFisico != null) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : p.tClientesMorale.Nombre,
                        idDespacho = p.idDespacho,
                        Despacho = p.tDespacho1.Nombre,
                        Proyecto = p.Proyecto,
                        idDespachoReferencia = p.idDespachoReferencia,
                        DespachoReferencia = p.tDespacho.Nombre,
                        TipoCliente = p.TipoCliente,
                        idSucursal = p.idSucursal,
                        Sucursal = p.tSucursale.Nombre,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Descuento = p.Descuento,
                        IVA = p.IVA,  
                        IVATasa = p.IVATasa,
                        Total = p.Total,
                        idVista = p.idVista ?? 0,
                        oAddress = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? new AddressViewModel()
                        {

                            Correo = p.tClientesFisico.Correo,
                            TelCasa = p.tClientesFisico.Telefono,
                            TelCelular = p.tClientesFisico.TelefonoCelular,
                            Direccion = p.tClientesFisico.Calle + " " + p.tClientesFisico.NumExt + " " + p.tClientesFisico.NumInt + " " + p.tClientesFisico.Colonia + " " + ((p.tClientesFisico.tMunicipio != null) ? p.tClientesFisico.tMunicipio.nombre_municipio : "") + " " + p.tClientesFisico.CP

                        } : (p.TipoCliente == TypesCustomers.MoralCustomer) ? new AddressViewModel()
                        {

                            Correo = p.tClientesMorale.Correo,
                            TelCasa = p.tClientesMorale.Telefono,
                            TelCelular = p.tClientesMorale.TelefonoCelular,
                            Direccion = p.tClientesMorale.Calle + " " + p.tClientesMorale.NumExt + " " + p.tClientesMorale.NumInt + " " + p.tClientesMorale.Colonia + " " + ((p.tClientesMorale.tMunicipio != null) ? p.tClientesMorale.tMunicipio.nombre_municipio : "") + " " + p.tClientesMorale.CP

                        } : new AddressViewModel()
                        {

                            Correo = p.tDespacho.Correo,
                            TelCasa = p.tDespacho.Telefono,
                            TelCelular = "",
                            Direccion = p.tDespacho.Calle + " " + p.tDespacho.NumExt + " " + p.tDespacho.NumInt + " " + p.tDespacho.Colonia + " " + ((p.tDespacho.tMunicipio != null) ? p.tDespacho.tMunicipio.nombre_municipio : "") + " " + p.tDespacho.CP

                        },
                        oDetail = p.tBorradorDetalleVentas.Where(x => x.idBorradorVenta == p.idBorradorVenta).Select(x => new SaleDetailViewModel()
                        {

                            idDetalleVenta = x.idBorradorDetalleVenta,
                            idVenta = x.idBorradorVenta,
                            idProducto = x.idProducto,
                            Codigo = x.tProducto.Codigo,
                            idServicio = x.idServicio,
                            idNotaCredito = x.idNotaCredito,
                            NotaCredito = x.NotaCredito,
                            Descripcion = x.Descripcion,
                            Precio = x.Precio,
                            Descuento = x.Descuento,
                            Cantidad = x.Cantidad,
                            oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
                            {
                                idProducto = o.idProducto,
                                urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
                                idProveedor = o.idProveedor,
                                Extension = o.Extension
                            }).FirstOrDefault(),
                            Comentarios = x.Comentarios,
                            Imagen = x.Imagen,
                            idPromocion = x.idPromocion,
                            idTipoPromocion = x.idTipoPromocion,
                            idProductoPadre = x.idProductoPadre
                        }).ToList(),
                        SumSubtotal = p.tBorradorDetalleVentas.Where(x => x.idBorradorVenta == p.idBorradorVenta).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum()

                    }).FirstOrDefault();

                    return eraserSale;

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void DeleteEraserSale(int idEraserSale, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    //Actualizar Notas de Crédito
                    this.ReturnEreaserCreditNote(idEraserSale);

                    //Borrar los registros de la tabla tBorradorDetalleVenta
                    var oDeatil = context.tBorradorDetalleVentas.Where(p => p.idBorradorVenta == idEraserSale).ToList();

                    foreach (var detail in oDeatil)
                    {
                        context.tBorradorDetalleVentas.Remove(detail);
                        context.SaveChanges();
                    }

                    //Borrar el registro de la table tBorradorVentas
                    var oSale = context.tBorradorVentas.FirstOrDefault(p => p.idBorradorVenta == idEraserSale);

                    context.tBorradorVentas.Remove(oSale);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void CloseEraserSale(int idEraserSale, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    //Borrar los registros de la tabla tBorradorDetalleVenta
                    var oDeatil = context.tBorradorDetalleVentas.Where(p => p.idBorradorVenta == idEraserSale).ToList();

                    foreach (var detail in oDeatil)
                    {
                        context.tBorradorDetalleVentas.Remove(detail);
                        context.SaveChanges();
                    }

                    //Borrar el registro de la table tBorradorVentas
                    var oSale = context.tBorradorVentas.FirstOrDefault(p => p.idBorradorVenta == idEraserSale);

                    context.tBorradorVentas.Remove(oSale);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void SaveUpdateEraserSale(int idEraserSale, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch, string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, int? idView)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tBorradorVenta oVentas = context.tBorradorVentas.FirstOrDefault(p => p.idBorradorVenta == idEraserSale);

                    oVentas.idUsuario1 = idUser1;
                    oVentas.idUsuario2 = idUser2;
                    oVentas.idClienteFisico = idCustomerP;
                    oVentas.idClienteMoral = idCustomerM;
                    oVentas.idDespacho = idOffice;
                    oVentas.Proyecto = project;
                    oVentas.idDespachoReferencia = idOfficeReference;
                    oVentas.idSucursal = idBranch;
                    oVentas.Fecha = Convert.ToDateTime(dateSale);
                    oVentas.CantidadProductos = amountProducts;
                    oVentas.Subtotal = subtotal;
                    oVentas.Descuento = discount;
                    oVentas.IVA = IVA;
                    oVentas.Total = total;
                    oVentas.idVista = idView;
                    oVentas.TipoCliente = typeCustomer;

                    context.SaveChanges();

                    //Delete detail sale
                    while (context.tBorradorDetalleVentas.Where(p => p.idBorradorVenta == idEraserSale).Count() != 0)
                    {
                        tBorradorDetalleVenta oDetail = context.tBorradorDetalleVentas.FirstOrDefault(p => p.idBorradorVenta == idEraserSale);
                        context.tBorradorDetalleVentas.Remove(oDetail);
                        context.SaveChanges();
                    }

                    foreach (var prod in lProducts)
                    {
                        this.AddEraserProductSale(oVentas.idBorradorVenta, prod);

                        if (prod.credito == Base.iTRue)
                        {
                            Credits oCredits = new Credits();
                            oCredits.SetStatus(prod.codigo, TypesCredit.creditoSaldada);
                        }
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void ReturnEreaserProducts(int idSale, int idBranch)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    List<SaleDetailViewModel> lSaleDetail = context.tBorradorDetalleVentas.Where(p => p.idBorradorVenta == idSale).Select(p => new SaleDetailViewModel()
                    {
                        idProducto = p.idProducto,
                        Cantidad = p.Cantidad
                    }).ToList();


                    foreach (var product in lSaleDetail)
                    {
                        tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == idBranch && p.idProducto == product.idProducto);

                        if (oProductBranch != null)
                        {
                            oProductBranch.Existencia = oProductBranch.Existencia + product.Cantidad;
                            context.SaveChanges();
                        }
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void ReturnEreaserCreditNote(int idSale)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    List<int?> lCredits = context.tBorradorDetalleVentas.Where(p => p.idBorradorVenta == idSale && p.idNotaCredito.HasValue).Select(p => p.idNotaCredito).ToList();


                    foreach (var credit in lCredits)
                    {
                        tNotasCredito creditNote = context.tNotasCreditoes.FirstOrDefault(p => p.idNotaCredito == credit);
                        creditNote.Estatus = TypesCredit.creditoPendiente;
                        context.SaveChanges();
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

        public void DeleteEreaserProductsFromStock(int idSale, int idBranch)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {

                    List<SaleDetailViewModel> lSaleDetail = context.tBorradorDetalleVentas.Where(p => p.idBorradorVenta == idSale && p.idProducto != null).Select(p => new SaleDetailViewModel()
                    {
                        idProducto = p.idProducto,
                        Cantidad = p.Cantidad
                    }).ToList();


                    foreach (var product in lSaleDetail)
                    {
                        tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == idBranch && p.idProducto == product.idProducto);
                        oProductBranch.Existencia = oProductBranch.Existencia - product.Cantidad;
                        context.SaveChanges();
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;                    
                }
        }

    }
}

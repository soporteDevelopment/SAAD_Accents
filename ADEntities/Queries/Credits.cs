using ADEntities.Enums;
using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Credits : Base
    {
        public int CountRegisters()
        {
            using (var context = new admDB_SAADDBEntities())
                return context.tNotasCreditoes.Where(p => p.Estatus == TypesCredit.creditoPendiente).Count();
        }

        public List<CreditNoteViewModel> GetCredits(DateTime dtDateSince, DateTime dtDateUntil, string remision, string costumer, string codigo, string comments, int status, short? amazonas, short? guadalquivir, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
                    var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

                    if (!String.IsNullOrEmpty(costumer))
                    {
                        dtStart = Convert.ToDateTime("01/01/2000").Date + new TimeSpan(0, 0, 0);
                        dtEnd = DateTime.Now.Date + new TimeSpan(23, 59, 59);
                    }

                    var credits = context.tNotasCreditoes.Where(

                      p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
                      (p.Remision.Contains(remision) || (String.IsNullOrEmpty(remision))) &&
                      (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                      (p.tClientesMorale.Nombre.ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
                      (p.tDespacho.Nombre.ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())))) &&
                      (((p.tVenta.idSucursal == amazonas) ||
                      (p.tVenta.idSucursal == guadalquivir)) || (amazonas == null && guadalquivir == null)) &&
                      ((p.Estatus == status) || (status == TypesCredit.creditoTodos))
                    ).Select(p => new CreditNoteViewModel()
                    {

                        idNotaCredito = p.idNotaCredito,
                        idVenta = p.idVenta,
                        RemisionCredito = p.Remision,
                        RemisionVenta = p.tVenta.Remision,
                        ClienteVenta = (p.tVenta.tClientesMorale != null) ? p.tVenta.tClientesMorale.Nombre : (p.tVenta.tClientesFisico != null) ? p.tVenta.tClientesFisico.Nombre + " " + p.tVenta.tClientesFisico.Apellidos : p.tVenta.tDespacho.Nombre,
                        Cliente = (p.idCustomerM != null) ? p.tClientesMorale.Nombre : (p.idCustomerP != null) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : p.tDespacho.Nombre,
                        idVendedor = p.idVendedor,
                        Vendedor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha,
                        FechaVigencia = p.FechaVigencia,
                        idTipoNotaCredito = p.idTipoNotaCredito,
                        TipoNotaCredito = new CreditNoteTypeViewModel()
                        {
                            idTipoNotaCredito = p.tTipoNotaCredito.idTipoNotaCredito,
                            Nombre = p.tTipoNotaCredito.Name
                        },
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                    p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                    "red")

                    }).OrderByDescending(p => p.RemisionCredito);

                    return credits.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<CreditNoteViewModel> GetCreditForRemision(string remision)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tNotasCreditoes.Where(p => p.Remision.Contains(remision) && p.Estatus != TypesSales.ventaSaldada).Select(p => new CreditNoteViewModel()
                    {

                        idNotaCredito = p.idNotaCredito,
                        idVenta = p.idVenta,
                        RemisionCredito = p.Remision,
                        RemisionVenta = p.tVenta.Remision,
                        idVendedor = p.idVendedor,
                        Vendedor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha,
                        FechaVigencia = p.FechaVigencia,
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                    p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                    "red")

                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public CreditNoteViewModel GetCreditNote(string remision)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tNotasCreditoes.Where(p => (p.Remision.Trim() == remision.Trim()) && (p.Estatus == TypesSales.ventaPendiente)).Select(p => new CreditNoteViewModel()
                    {
                        idNotaCredito = p.idNotaCredito,
                        idVenta = p.idVenta,
                        RemisionCredito = p.Remision,
                        RemisionVenta = p.tVenta.Remision,
                        idVendedor = p.idVendedor,
                        Vendedor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha,
                        FechaVigencia = p.FechaVigencia,
                        Comentarios = p.Comentarios,
                        Estatus = p.Estatus
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public CreditNoteViewModel GetSingleCreditForRemision(int idCreditNote)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tNotasCreditoes.Where(p => p.idNotaCredito == idCreditNote).Select(p => new CreditNoteViewModel()
                    {

                        idNotaCredito = p.idNotaCredito,
                        idVenta = p.idVenta,
                        RemisionCredito = p.Remision,
                        RemisionVenta = p.tVenta.Remision,
                        idVendedor = p.idVendedor,
                        Vendedor = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                        ClienteVenta = (p.tVenta.tClientesMorale != null) ? p.tVenta.tClientesMorale.Nombre : (p.tVenta.tClientesFisico != null) ? p.tVenta.tClientesFisico.Nombre + " " + p.tVenta.tClientesFisico.Apellidos : p.tVenta.tDespacho1.Nombre,
                        Cliente = (p.idCustomerM != null) ? p.tClientesMorale.Nombre : (p.idCustomerP != null) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : p.tDespacho.Nombre,
                        Cantidad = p.Cantidad,
                        Fecha = p.Fecha,
                        FechaVigencia = p.FechaVigencia,
                        Comentarios = p.Comentarios,
                        idTipoNotaCredito = p.idTipoNotaCredito,
                        TipoNotaCredito = new CreditNoteTypeViewModel()
                        {
                            idTipoNotaCredito = p.tTipoNotaCredito.idTipoNotaCredito,
                            Nombre = p.tTipoNotaCredito.Name
                        },
                        Estatus = p.Estatus,
                        sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
                                    p.Estatus == TypesSales.ventaPendiente ? "yellow" :
                                    "red")
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Validates if sale has credit payment.
        /// </summary>
        /// <param name="idSale">The identifier sale.</param>
        /// <returns></returns>
        public bool ValidateIfSaleHasCreditPayment(int idSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tFormaPagoes.Where(p => p.idVenta == idSale && p.FormaPago == TypesPayment.iCredito && p.Estatus == TypesPayment.iPendiente).Any();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int AddCredit(int? idCreditNoteType, int? idSale, int? idSeller, int? idCustomerP, int? idCustomerM, int? idOffice, decimal? amount, DateTime? dtDate, DateTime? dtDateValidity, string comments, List<CreditDetailViewModel> lProducts, int? userId, out string remCredit)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tNotasCredito tCredit = new tNotasCredito();

                    tCredit.idTipoNotaCredito = idCreditNoteType;
                    tCredit.idVenta = idSale;
                    tCredit.idVendedor = idSeller;

                    if (idSale != null && idSale > 0)
                    {
                        tCredit.idCustomerP = context.tVentas.Where(p => p.idVenta == idSale).Select(p => p.idClienteFisico).FirstOrDefault();
                        tCredit.idCustomerM = context.tVentas.Where(p => p.idVenta == idSale).Select(p => p.idClienteMoral).FirstOrDefault();
                        tCredit.idDespacho = context.tVentas.Where(p => p.idVenta == idSale).Select(p => p.idDespacho).FirstOrDefault();
                    }
                    else
                    {
                        tCredit.idCustomerP = idCustomerP;
                        tCredit.idCustomerM = idCustomerM;
                        tCredit.idDespacho = idOffice;
                    }

                    tCredit.Cantidad = amount;
                    tCredit.Fecha = dtDate;
                    tCredit.FechaVigencia = dtDateValidity;
                    tCredit.Comentarios = comments;
                    tCredit.Estatus = TypesSales.ventaPendiente;
                    tCredit.CreadoPor = userId;
                    tCredit.Creado = DateTime.Now;

                    context.tNotasCreditoes.Add(tCredit);

                    context.SaveChanges();

                    iResult = tCredit.idNotaCredito;

                    int? idBranch = context.tVentas.Where(p => p.idVenta == idSale).Select(p => p.idSucursal).FirstOrDefault();

                    //Genera remisión
                    remCredit = GenerateNumberRem(idBranch ?? 0, (int)tCredit.idNotaCredito);

                    var oCredit = context.tNotasCreditoes.FirstOrDefault(p => p.idNotaCredito == tCredit.idNotaCredito);

                    oCredit.Remision = remCredit;

                    context.SaveChanges();

                    //Modificar inventario
                    if (idSale.HasValue && lProducts != null)
                    {
                        if (lProducts.Count > 0)
                        {
                            SetStock((int)idSale, lProducts);

                            foreach (var prod in lProducts)
                            {

                                if (prod.Tipo == 1)
                                {
                                    tProducto tProduct = context.tProductos.FirstOrDefault(p => p.idProducto == prod.idProducto);

                                    if (prod.Amazonas != null && prod.Amazonas != 0)
                                    {
                                        this.InsertProductInDetailCredit(tCredit.idNotaCredito, (int)idSale, prod.idProducto, prod.Amazonas, 2);
                                        tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == 2 && p.idProducto == tProduct.idProducto);
                                        this.AddRegisterProduct((int)prod.idProducto, 2, "Se regresa a inventario por Nota de Credito " + remCredit, (decimal)oProductBranch.Existencia, (decimal)oProductBranch.Existencia + (decimal)prod.Amazonas, tProduct.PrecioVenta, tProduct.PrecioVenta, String.Empty, (int)idSeller);
                                    }

                                    if (prod.Guadalquivir != null && prod.Guadalquivir != 0)
                                    {
                                        this.InsertProductInDetailCredit(tCredit.idNotaCredito, (int)idSale, prod.idProducto, prod.Guadalquivir, 3);
                                        tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == 3 && p.idProducto == tProduct.idProducto);
                                        this.AddRegisterProduct((int)prod.idProducto, 3, "Se regresa a inventario por Nota de Credito " + remCredit, (decimal)oProductBranch.Existencia, (decimal)oProductBranch.Existencia + (decimal)prod.Guadalquivir, tProduct.PrecioVenta, tProduct.PrecioVenta, String.Empty, (int)idSeller);
                                    }

                                    if (prod.Textura != null && prod.Textura != 0)
                                    {
                                        this.InsertProductInDetailCredit(tCredit.idNotaCredito, (int)idSale, prod.idProducto, prod.Textura, 4);
                                        tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == 4 && p.idProducto == tProduct.idProducto);
                                        this.AddRegisterProduct((int)prod.idProducto, 4, "Se regresa a inventario por Nota de Credito " + remCredit, (decimal)oProductBranch.Existencia, (decimal)oProductBranch.Existencia + (decimal)prod.Textura, tProduct.PrecioVenta, tProduct.PrecioVenta, String.Empty, (int)idSeller);
                                    }
                                }
                                else
                                {
                                    this.InsertServiceInDetailCredit(tCredit.idNotaCredito, (int)idSale, prod.idServicio, (decimal)prod.Cantidad);
                                }
                            }
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;
        }

        public int AddCreditFromPayment(int? idSale, decimal? amount, int? userId, out string remCredit)
        {
            int iResult;

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var sale = context.tVentas.FirstOrDefault(p => p.idVenta == idSale);

                    tNotasCredito tCredit = new tNotasCredito();

                    tCredit.idTipoNotaCredito = (int)CreditNoteType.RestanteFormaPago;
                    tCredit.idVenta = idSale;
                    tCredit.idVendedor = sale.idUsuario1;

                    if (sale.TipoCliente == TypesCustomers.PhysicalCustomer)
                    {
                        tCredit.idCustomerP = context.tVentas.Where(p => p.idVenta == idSale).Select(p => p.idClienteFisico).FirstOrDefault();
                    }
                    else if (sale.TipoCliente == TypesCustomers.MoralCustomer)
                    {
                        tCredit.idCustomerM = context.tVentas.Where(p => p.idVenta == idSale).Select(p => p.idClienteMoral).FirstOrDefault();
                    }
                    else if (sale.TipoCliente == TypesCustomers.OfficeCustomer)
                    {
                        tCredit.idDespacho = context.tVentas.Where(p => p.idVenta == idSale).Select(p => p.idDespacho).FirstOrDefault();
                    }

                    tCredit.Cantidad = amount;
                    tCredit.Fecha = DateTime.Now;
                    tCredit.FechaVigencia = DateTime.Now.AddYears(1);
                    tCredit.Comentarios = sale.Remision;
                    tCredit.Estatus = TypesSales.ventaPendiente;
                    tCredit.CreadoPor = userId;
                    tCredit.Creado = DateTime.Now;

                    context.tNotasCreditoes.Add(tCredit);

                    context.SaveChanges();

                    iResult = tCredit.idNotaCredito;

                    //Genera remisión
                    remCredit = GenerateNumberRem((int)sale.idSucursal, (int)tCredit.idNotaCredito);

                    var oCredit = context.tNotasCreditoes.FirstOrDefault(p => p.idNotaCredito == tCredit.idNotaCredito);

                    oCredit.Remision = remCredit;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }

            return iResult;
        }

        public void SetStock(int idSale, List<CreditDetailViewModel> lProducts)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    foreach (var prod in lProducts)
                    {
                        if (prod.Tipo == 1)
                        {
                            List<tProductosSucursal> oProductBranch = context.tProductosSucursals.Where(p => p.idProducto == prod.idProducto).ToList();

                            if (oProductBranch != null)
                            {
                                foreach (var product in oProductBranch)
                                {
                                    if (product.idSucursal == 2)
                                    {
                                        product.Existencia = product.Existencia + prod.Amazonas ?? 0;
                                    }

                                    if (product.idSucursal == 3)
                                    {
                                        product.Existencia = product.Existencia + prod.Guadalquivir ?? 0;
                                    }

                                    if (product.idSucursal == 4)
                                    {
                                        product.Existencia = product.Existencia + prod.Textura ?? 0;
                                    }

                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void InsertProductInDetailCredit(int idCredit, int idSale, int? idProduct, decimal? Amount, int idBranch)
        {

            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tDetalleVenta oDetailSale = context.tDetalleVentas.FirstOrDefault(p => p.idVenta == idSale && p.idProducto == idProduct);

                    if (oDetailSale != null)
                    {
                        tDetalleCredito oDetail = new tDetalleCredito();

                        oDetail.idNotaCredito = idCredit;
                        oDetail.idProducto = idProduct;
                        oDetail.idServicio = null;
                        oDetail.Descripcion = oDetailSale.Descripcion;
                        oDetail.Precio = oDetailSale.Precio;
                        oDetail.Cantidad = Amount;
                        oDetail.idSucursal = idBranch;

                        context.tDetalleCreditoes.Add(oDetail);

                        //Se indica que el producto está relacionado con una NC
                        oDetailSale.NotaCredito = true;
                        oDetailSale.idNotaCredito = idCredit;

                        context.SaveChanges();
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void InsertServiceInDetailCredit(int idCredit, int idSale, int? idService, decimal amount)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {

                    tDetalleVenta oDetailSale = context.tDetalleVentas.FirstOrDefault(p => p.idVenta == idSale && p.idServicio == idService);

                    tDetalleCredito oDetail = new tDetalleCredito();

                    oDetail.idNotaCredito = idCredit;
                    oDetail.idProducto = null;
                    oDetail.idServicio = idService;
                    oDetail.Descripcion = oDetailSale.Descripcion;
                    oDetail.Precio = oDetailSale.Precio;
                    oDetail.Cantidad = amount;
                    oDetail.idSucursal = oDetail.idSucursal;

                    context.tDetalleCreditoes.Add(oDetail);

                    //Se indica que el producto está relacionado con una NC
                    oDetailSale.NotaCredito = true;
                    oDetailSale.idNotaCredito = idCredit;

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public string GenerateNumberRem(int idBranch, int idSale)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    string month = DateTime.Now.ToString("MM");
                    string year = DateTime.Now.ToString("yy");

                    char pad = '0';

                    return string.Concat(string.Concat(month, string.Concat(string.Concat(year + "-", idSale.ToString().PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : "G"), "NC"));
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void SetStatus(string creditNote, short status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tNotasCredito oCredit = context.tNotasCreditoes.FirstOrDefault(p => p.Remision == creditNote);
                    oCredit.Estatus = status;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void SetStatus(int idCreditNote, short status)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    tNotasCredito oCredit = context.tNotasCreditoes.FirstOrDefault(p => p.idNotaCredito == idCreditNote);
                    oCredit.Estatus = status;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public string GetCreditNoteForIdventa(int idventa)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tNotasCreditoes.Where(p => (p.idVenta == idventa) && (p.Estatus == TypesSales.ventaPendiente)).Select(p => p.Remision).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Gets the credit detail.
        /// </summary>
        /// <param name="idCredit">The identifier credit.</param>
        /// <returns></returns>
        public List<CreditDetailViewModel> GetCreditDetail(int idCredit)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tDetalleCreditoes.Where(p => p.idNotaCredito == idCredit).Select(p => new CreditDetailViewModel()
                    {
                        idDetalleCredito = p.idDetalleCredito,
                        idProducto = p.idProducto,
                        Codigo = p.tProducto.Codigo,
                        idServicio = p.idServicio,
                        Descripcion = p.Descripcion,
                        Precio = p.Precio,
                        Cantidad = p.Cantidad,
                    }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Adds the credit note.
        /// </summary>
        /// <param name="creditNote">The credit note.</param>
        /// <returns></returns>
        public tNotasCredito AddCreditNote(tNotasCredito creditNote)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tNotasCreditoes.Add(creditNote);
                    context.SaveChanges();

                    int? idBranch = context.tVentas.Where(p => p.idVenta == creditNote.idNotaCredito).Select(p => p.idSucursal).FirstOrDefault();

                    //Genera remisión
                    var creditReference = GenerateNumberRem(idBranch ?? 0, creditNote.idNotaCredito);

                    var oCredit = context.tNotasCreditoes.FirstOrDefault(p => p.idNotaCredito == creditNote.idNotaCredito);
                    oCredit.Remision = creditReference;
                    context.SaveChanges();

                    return creditNote;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }
    }
}

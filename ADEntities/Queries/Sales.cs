using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace ADEntities.Queries
{
	public class Sales : Base
	{
		private const int CashAccount = 7;

		public bool VerifySale(int idUser, DateTime verifyDate)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var sale = context.tVentas.OrderByDescending(p => p.Fecha).FirstOrDefault(p => p.idUsuario1 == idUser && p.Estatus == TypesOutProducts.Pendiente);

					if (sale != null)
					{
						var totalMinute = verifyDate.Subtract(sale.Fecha ?? DateTime.Now);
						return context.tVentas.Any(p => p.idUsuario1 == idUser && totalMinute.TotalMinutes <= 2);
					}
					else
					{
						return false;
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int CountRegisters()
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tVentas.Count();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int GetCount(bool allTime, DateTime dtDateSince, DateTime dtDateUntil, string remision, string costumer, int? iduserforsearch, string codigo, string project, string billNumber, string voucher, int status, int? statusPayment, int idUser, short? restricted, decimal? payment, short? amazonas, short? guadalquivir, short? textura)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					DateTime dtStart;
					DateTime dtEnd;

					if (allTime)
					{
						dtStart = DateTime.Now.AddYears(-25) + new TimeSpan(0, 0, 0);
						dtEnd = DateTime.Now + new TimeSpan(23, 59, 59);
					}
					else
					{
						dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
						dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);
					}

					return context.tVentas.Where(
					  p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
					  (p.Remision.Contains(remision) || (String.IsNullOrEmpty(remision))) &&
					  (p.Proyecto.ToUpper().Contains(project.ToUpper()) || (String.IsNullOrEmpty(project))) &&
					  (p.NumeroFactura.Contains(billNumber) || (String.IsNullOrEmpty(billNumber))) &&
					  ((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null) &&
					  (((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
					  (p.tClientesMorale.Nombre.ToUpper().Contains(costumer) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
					  (p.tDespacho.Nombre.ToUpper().Contains(costumer) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())))) &&
					  ((context.tFormaPagoes.Any(f => f.Voucher.Contains(voucher)) || context.tHistorialCreditoes.Any(f => f.Voucher.Contains(voucher))) || String.IsNullOrEmpty(voucher)) &&
					  (((p.idSucursal == amazonas) ||
					  (p.idSucursal == guadalquivir) ||
					  (p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
					  (p.tDetalleVentas.Where(x => (context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo.Contains(codigo) || (String.IsNullOrEmpty(codigo)))).Any()) &&
					  ((p.Estatus == status) || (status == TypesSales.ventaTodos)) &&
					  ((context.tFormaPagoes.Any(f => f.idVenta == p.idVenta && f.Cantidad == payment) || payment == null) ||
					  (context.tHistorialCreditoes.Any(h => h.idVenta == p.idVenta && h.Cantidad == payment) || payment == null)) &&
					  ((context.tFormaPagoes.Any(f => f.idVenta == p.idVenta && f.Estatus == statusPayment)) ||
					   (context.tHistorialCreditoes.Any(f => f.idVenta == p.idVenta && f.Estatus == statusPayment)) || statusPayment == null)
					).Count();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<SaleViewModel> GetSales(bool allTime, bool searchPayment, DateTime dtDateSince, DateTime dtDateUntil, string remision, string costumer, int? iduserforsearch, string codigo, string project, string billNumber, string voucher, int status, int? statusPayment, int idUser, short? restricted, decimal? payment, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					DateTime dtStart;
					DateTime dtEnd;

					if (allTime)
					{
						dtStart = DateTime.Now.AddYears(-25) + new TimeSpan(0, 0, 0);
						dtEnd = DateTime.Now + new TimeSpan(23, 59, 59);
					}
					else
					{
						dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
						dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);
					}

					IOrderedQueryable<SaleViewModel> sales;

					if (!searchPayment)
					{
						sales = context.tVentas.Where(
						p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
						(p.Remision.Contains(remision) || (String.IsNullOrEmpty(remision))) &&
						(p.Proyecto.ToUpper().Contains(project.ToUpper()) || (String.IsNullOrEmpty(project))) &&
						(p.NumeroFactura.Contains(billNumber) || (String.IsNullOrEmpty(billNumber))) &&
						((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null) &&
						(((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
						(p.tClientesMorale.Nombre.ToUpper().Contains(costumer) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
						(p.tDespacho.Nombre.ToUpper().Contains(costumer) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())))) &&
						((context.tFormaPagoes.Any(f => f.Voucher.Contains(voucher)) || context.tHistorialCreditoes.Any(f => f.Voucher.Contains(voucher))) || String.IsNullOrEmpty(voucher)) &&
						(((p.idSucursal == amazonas) ||
						(p.idSucursal == guadalquivir) ||
						(p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
						(p.tDetalleVentas.Where(x => (context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo.Contains(codigo) || (String.IsNullOrEmpty(codigo)))).Any()) &&
						((p.Estatus == status) || (status == TypesSales.ventaTodos)) &&
						((p.tFormaPagoes.Any(f => f.idVenta == p.idVenta && f.Cantidad == payment) || payment == null) ||
						(p.tHistorialCreditoes.Any(h => h.idVenta == p.idVenta && h.Cantidad == payment) || payment == null))
					  ).Select(p => new SaleViewModel()
					  {
						  idVenta = p.idVenta,
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
						  TipoCliente = p.TipoCliente,
						  idSucursal = p.idSucursal,
						  Sucursal = p.tSucursale.Nombre,
						  Fecha = p.Fecha,
						  CantidadProductos = p.CantidadProductos,
						  Subtotal = p.Subtotal,
						  Descuento = p.Descuento,
						  Remision = p.Remision,
						  IVA = p.IVA,
						  Total = p.Total,
						  FormaPago = context.tFormaPagoes.Where(f => f.idVenta == p.idVenta && f.FormaPago == TypesPayment.iCredito).Select(f => f.FormaPago).FirstOrDefault(),
						  DescFormasPago = context.tFormaPagoes.Where(f => f.idVenta == p.idVenta).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
							   x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
							   x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
							   x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
							   x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
							   x.FormaPago == TypesPayment.iCredito && x.Estatus == TypesPayment.iPendiente ? TypesPayment.Credito :
							   x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
							   x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
							   x.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
							   x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago : ""
						  ).ToList().Union(
							context.tFormaPagoCreditoes.Where(x => x.tHistorialCredito.idVenta == p.idVenta).Select(y => y.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

							  y.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
							  y.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
							  y.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
							  y.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
							  y.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
							  y.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
							  y.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
							  y.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
							  y.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago : "")
						  ).ToList(),
						  Estatus = p.Estatus,
						  Factura = p.Factura,
						  NumberFactura = p.NumeroFactura
					  }).OrderByDescending(p => p.Fecha);
					}
					else
					{
						sales = context.tVentas.Where(p =>
						(p.Remision.Contains(remision) || (String.IsNullOrEmpty(remision))) &&
						(p.Proyecto.ToUpper().Contains(project.ToUpper()) || (String.IsNullOrEmpty(project))) &&
						(p.NumeroFactura.Contains(billNumber) || (String.IsNullOrEmpty(billNumber))) &&
						((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null) &&
						(((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
						(p.tClientesMorale.Nombre.ToUpper().Contains(costumer) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
						(p.tDespacho.Nombre.ToUpper().Contains(costumer) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())))) &&
						((context.tFormaPagoes.Any(f => f.Voucher.Contains(voucher)) || context.tHistorialCreditoes.Any(f => f.Voucher.Contains(voucher))) || String.IsNullOrEmpty(voucher)) &&
						(((p.idSucursal == amazonas) ||
						(p.idSucursal == guadalquivir) ||
						(p.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null)) &&
						(p.tDetalleVentas.Where(x => (context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo.Contains(codigo) || (String.IsNullOrEmpty(codigo)))).Any()) &&
						((p.Estatus == status) || (status == TypesSales.ventaTodos)) &&
						((p.tFormaPagoes.Any(f => f.idVenta == p.idVenta && f.Cantidad == payment) || payment == null) ||
						(p.tHistorialCreditoes.Any(h => h.idVenta == p.idVenta && h.Cantidad == payment) || payment == null)) &&
						((p.tFormaPagoes.Any(f => (f.Creado >= dtStart && f.Creado <= dtEnd) && f.FormaPago != 2 && f.Estatus == 2) ||
						 (p.tHistorialCreditoes.Any(f => (p.Fecha >= dtStart && p.Fecha <= dtEnd) && f.Estatus == 2))))
					  ).Select(p => new SaleViewModel()
					  {
						  idVenta = p.idVenta,
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
						  TipoCliente = p.TipoCliente,
						  idSucursal = p.idSucursal,
						  Sucursal = p.tSucursale.Nombre,
						  Fecha = p.Fecha,
						  CantidadProductos = p.CantidadProductos,
						  Subtotal = p.Subtotal,
						  Descuento = p.Descuento,
						  Remision = p.Remision,
						  IVA = p.IVA,
						  Total = p.Total,
						  FormaPago = context.tFormaPagoes.Where(f => f.idVenta == p.idVenta && f.FormaPago == TypesPayment.iCredito).Select(f => f.FormaPago).FirstOrDefault(),
						  DescFormasPago = context.tFormaPagoes.Where(f => f.idVenta == p.idVenta).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
							   x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
							   x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
							   x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
							   x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
							   x.FormaPago == TypesPayment.iCredito && x.Estatus == TypesPayment.iPendiente ? TypesPayment.Credito :
							   x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
							   x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
							   x.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
							   x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago : ""
						  ).ToList().Union(
							context.tFormaPagoCreditoes.Where(x => x.tHistorialCredito.idVenta == p.idVenta).Select(y => y.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

							  y.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
							  y.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
							  y.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
							  y.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
							  y.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
							  y.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
							  y.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
							  y.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
							  y.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago : "")
						  ).ToList(),
						  Estatus = p.Estatus,
						  Factura = p.Factura,
						  NumberFactura = p.NumeroFactura
					  }).OrderByDescending(p => p.Fecha);
					}

					List<SaleViewModel> lSales = sales.Skip(page * pageSize).Take(pageSize).ToList();

					foreach (var sale in lSales)
					{
						sale.sEstatus = this.GetStatusPaymentDetail(sale.idVenta) == true ? "orange" : sale.Estatus == TypesSales.ventaSaldada ? "grey" :
										sale.Estatus == TypesSales.ventaPendiente ? "yellow" :
										"red";
					}

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

		public void UpdatePaidOutSales(int idCommissionSale, int idSeller, List<int> sales)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					foreach (var idSale in sales)
					{

						if (context.tVentas.Where(p => p.idVenta == idSale && p.idUsuario1 == idSeller).Count() == 1)
						{
							tVenta sale = context.tVentas.FirstOrDefault(p => p.idVenta == idSale);

							sale.PagadoUno = true;
							sale.idVentasComisiones = idCommissionSale;
						}
						else if (context.tVentas.Where(p => p.idVenta == idSale && p.idUsuario2 == idSeller).Count() == 1)
						{
							tVenta sale = context.tVentas.FirstOrDefault(p => p.idVenta == idSale);

							sale.PagadoDos = true;
							sale.idVentasComisiones = idCommissionSale;
						}

						context.SaveChanges();
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void UpdatePaidOutSalesOffice(int idCommissionSale, int idOffice, List<ReportOfSalesViewModel> sales)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					foreach (var sale in sales)
					{
						tVenta venta = context.tVentas.FirstOrDefault(p => p.idVenta == sale.idVenta);

						venta.PagadoOffice = true;
						venta.idVentasComisionesDespacho = idCommissionSale;
						venta.ComisionGeneral = sale.ComisionGeneral;
						venta.ComisionArteriors = sale.ComisionArterios;
						venta.ComisionTotal = sale.ComisionTotal;
						venta.TotalNotaCredito = sale.TotalNotaCredito;

						if (venta.idDespachoReferencia == null)
						{
							venta.idDespachoReferencia = idOffice;
						}

						foreach (var detailsale in sale.oDetail)
						{
							tDetalleVenta detail = context.tDetalleVentas.FirstOrDefault(p => p.idDetalleVenta == detailsale.idDetalleVenta);

							if (detail != null)
							{
								detail.PorcentajePago = detailsale.Porcentaje;
							}

							context.SaveChanges();
						}

						context.SaveChanges();
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		private bool GetStatusPaymentDetail(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return ((context.tVentas.Where(s => s.idVenta == idSale && s.Estatus != TypesSales.ventaSaldada).Count() > 0) && (context.tFormaPagoes.Any(p => p.idVenta == idSale && p.Estatus == TypesSales.ventaSaldada && p.FormaPago != TypesPayment.iNotaCredito) || context.tHistorialCreditoes.Any(q => q.idVenta == idSale && q.Estatus == TypesSales.ventaSaldada && q.tFormaPagoCreditoes.Where(c => c.idHistorialCredito == q.idHistorialCredito).Select(c => c.FormaPago != TypesPayment.iNotaCredito).Any())));
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public string AddSale(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
			string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, int? idView, int? idQuotation, int? originCurrency, out int idSale)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tVenta oVentas = new tVenta();

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
					oVentas.Estatus = TypesSales.ventaPendiente;
					oVentas.idVista = idView;
					oVentas.idCotizacion = idQuotation;
					oVentas.TipoCliente = typeCustomer;
					oVentas.TipoVenta = (idView != null) ? TypesSales.vista : (idQuotation != null) ? TypesSales.cotizacion : TypesSales.normal;
					oVentas.OrigenMoneda = originCurrency;
					oVentas.Omitir = false;

					context.tVentas.Add(oVentas);

					context.SaveChanges();

					idSale = oVentas.idVenta;

					tVenta objVentas = context.tVentas.Where(p => p.idVenta == oVentas.idVenta).FirstOrDefault();
					if (idBranch == 1)
						objVentas.Remision = this.GenerateNumberRemC(objVentas.idVenta);
					else
						objVentas.Remision = this.GenerateNumberRem(idBranch, objVentas.idVenta);

					context.SaveChanges();

					foreach (var prod in lProducts)
					{
						if (prod.Tipo == TypesSales.credito)
						{
							this.AddCreditSale(oVentas.idVenta, prod);
							Credits oCredits = new Credits();
							oCredits.SetStatus(prod.codigo, TypesCredit.creditoSaldada);
						}
						else
						{
							this.AddProductSale(oVentas.idVenta, prod);
						}
					}

					if (idBranch == 1)
						this.DeleteProductsFromStock(oVentas.idVenta, idUser1);
					else
						this.DeleteProductsFromStock(oVentas.idVenta, (int)oVentas.idSucursal, idUser1);

					return objVentas.Remision;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public string AddUnifySale(int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
			string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, int originCurrency, out int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tVenta oVentas = new tVenta();

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
					oVentas.Estatus = TypesSales.ventaPendiente;
					oVentas.TipoCliente = typeCustomer;
					oVentas.TipoVenta = TypesSales.vista;
					oVentas.OrigenMoneda = originCurrency;
					oVentas.Omitir = false;

					context.tVentas.Add(oVentas);

					context.SaveChanges();

					idSale = oVentas.idVenta;

					tVenta objVentas = context.tVentas.Where(p => p.idVenta == oVentas.idVenta).FirstOrDefault();
					if (idBranch == 1)
						objVentas.Remision = this.GenerateNumberRemC(objVentas.idVenta);
					else
						objVentas.Remision = this.GenerateNumberRem(idBranch, objVentas.idVenta);

					context.SaveChanges();

					foreach (var prod in lProducts)
					{
						if (prod.Tipo == TypesSales.credito)
						{
							this.AddCreditSale(oVentas.idVenta, prod);
							Credits oCredits = new Credits();
							oCredits.SetStatus(prod.codigo, TypesCredit.creditoSaldada);
						}
						else
						{
							this.AddProductSale(oVentas.idVenta, prod);
						}
					}
					if (idBranch == 1)
						this.DeleteProductsFromStock(oVentas.idVenta, idUser1);
					else
						this.DeleteProductsFromStock(oVentas.idVenta, (int)oVentas.idSucursal, idUser1);

					return objVentas.Remision;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddTypePayment(int idSale, List<TypePaymentViewModel> lTypePayment)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					foreach (var typePayment in lTypePayment)
					{
						tFormaPago oFormaPago = new tFormaPago();

						oFormaPago.idVenta = idSale;
						oFormaPago.FormaPago = typePayment.typesPayment;
						oFormaPago.TipoTarjeta = typePayment.typesCard;
						oFormaPago.Banco = typePayment.bank;
						oFormaPago.Titular = typePayment.holder;
						oFormaPago.NumCheque = typePayment.numCheck;
						oFormaPago.NumIFE = typePayment.numIFE;
						oFormaPago.Cantidad = typePayment.amount;
						oFormaPago.FechaLimitePago = (String.IsNullOrEmpty(typePayment.dateMaxPayment)) ? DateTime.Now : Convert.ToDateTime(typePayment.dateMaxPayment);
						oFormaPago.CantidadIVA = typePayment.amountIVA;
						oFormaPago.IVA = typePayment.IVA;
						oFormaPago.Estatus = (typePayment.typesPayment == 8) ? TypesSales.ventaSaldada : TypesSales.ventaPendiente;
						oFormaPago.idNotaCredito = typePayment.idCreditNote;
						oFormaPago.idPagoMeses = (typePayment.typesPayment == 5) ? typePayment.paymentMonth : null;
						oFormaPago.idCuentaBancos = typePayment.idCatalogBankAccount;
						oFormaPago.idTipoTerminales = typePayment.idCatalogTerminalType;
						oFormaPago.FechaPago = DateTime.Now;
						oFormaPago.Creado = DateTime.Now;
						context.tFormaPagoes.Add(oFormaPago);
						context.SaveChanges();

						if (typePayment.idCreditNote != null)
						{
							Credits oCredits = new Credits();
							oCredits.SetStatus((int)typePayment.idCreditNote, TypesCredit.creditoSaldada);
						}

					}

				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int GetNumberSalesForSeller(DateTime dtDateSince, DateTime dtDateUntil, int idSeller)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
					var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

					return context.tVentas.Where(
					   p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) && p.idUsuario1 == idSeller
					).Count();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<SaleViewModel> GetSalesForSeller(DateTime dtDateSince, DateTime dtDateUntil, int idSeller)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
					var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

					var lSales = context.tVentas.Where(
					   p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) && p.idUsuario1 == idSeller
					).Select(p => new SaleViewModel()
					{

						idVenta = p.idVenta,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
						ClienteMoral = p.tClientesMorale.Nombre,
						Proyecto = p.Proyecto,
						Despacho = p.tDespacho.Nombre,
						DespachoReferencia = p.tDespacho1.Nombre,
						idSucursal = p.idSucursal,
						Sucursal = p.tSucursale.Nombre,
						Fecha = p.Fecha,
						CantidadProductos = p.CantidadProductos,
						Subtotal = p.Subtotal,
						Descuento = p.Descuento,
						IVA = p.IVA,
						Total = p.Total,
						TipoCliente = p.TipoCliente,
						oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
						{

							idDetalleVenta = x.idDetalleVenta,
							idVenta = x.idVenta,
							idProducto = x.idProducto,
							Codigo = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo,
							idServicio = x.idServicio,
							idNotaCredito = x.idNotaCredito,
							NotaCredito = x.NotaCredito,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento,
							Cantidad = x.Cantidad,
							TipoImagen = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().TipoImagen,
							Imagen = x.Imagen,
							Comentarios = x.Comentarios,
							idPromocion = x.idPromocion,
							CostoPromocion = x.CostoPromocion,
							idTipoPromocion = x.idTipoPromocion,
							idProductoPadre = x.idProductoPadre,
							oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
							{
								idProducto = o.idProducto,
								urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
								idProveedor = o.idProveedor,
								Extension = o.Extension
							}).FirstOrDefault()
						}).ToList(),
						SumSubtotal = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum()

					}).OrderByDescending(p => p.Fecha).ToList();

					foreach (var sale in lSales)
					{
						sale.sEstatus = this.GetStatusPaymentDetail(sale.idVenta) == true ? "orange" : sale.Estatus == TypesSales.ventaSaldada ? "grey" :
										sale.Estatus == TypesSales.ventaPendiente ? "yellow" :
										"red";
					}

					return lSales;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddProductSale(int idSale, ProductSaleViewModel oProduct)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tDetalleVenta oDetalleVenta = new tDetalleVenta();

					oDetalleVenta.idVenta = idSale;

					if (oProduct.Tipo == 2 && !String.IsNullOrEmpty(oProduct.imagen))
					{
						char delimiter = '/';

						var imagen = oProduct.imagen.Split(delimiter);

						oDetalleVenta.Imagen = imagen[3];
					}

					oDetalleVenta.idProducto = oProduct.idProducto;
					oDetalleVenta.idCotizacion = (oProduct.idCotizacion == 0) ? null : oProduct.idCotizacion;
					oDetalleVenta.idServicio = oProduct.idServicio;
					oDetalleVenta.idNotaCredito = oProduct.idCredito;
					oDetalleVenta.NotaCredito = false;
					oDetalleVenta.Descripcion = oProduct.desc;
					oDetalleVenta.Precio = oProduct.prec;
					oDetalleVenta.Descuento = oProduct.descuento;
					oDetalleVenta.Cantidad = oProduct.cantidad;
					oDetalleVenta.Comentarios = oProduct.comentarios;
					oDetalleVenta.idSucursal = (oProduct.idSucursal == 0) ? null : oProduct.idSucursal;
					oDetalleVenta.idPromocion = oProduct.idPromocion;
					oDetalleVenta.CostoPromocion = oProduct.CostoPromocion;
					oDetalleVenta.idTipoPromocion = oProduct.idTipoPromocion;
					oDetalleVenta.idProductoPadre = oProduct.idProductoPadre;

					context.tDetalleVentas.Add(oDetalleVenta);

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddCreditSale(int idSale, ProductSaleViewModel oProduct)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{

					tDetalleVenta oDetalleVenta = new tDetalleVenta();

					oDetalleVenta.idVenta = idSale;
					oDetalleVenta.idProducto = null;
					oDetalleVenta.idServicio = oProduct.idServicio;
					oDetalleVenta.idNotaCredito = oProduct.idProducto;
					oDetalleVenta.Descripcion = oProduct.desc;
					oDetalleVenta.Precio = oProduct.prec;
					oDetalleVenta.Descuento = oProduct.descuento;
					oDetalleVenta.Cantidad = oProduct.cantidad;
					oDetalleVenta.Comentarios = oProduct.comentarios;

					context.tDetalleVentas.Add(oDetalleVenta);

					context.SaveChanges();

				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void DeleteCreditSale(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					//Se eliminan los abonos para las forma de pago a Crédito
					List<tHistorialCredito> lHistory = context.tHistorialCreditoes.Where(p => p.idVenta == idSale).ToList();

					if (lHistory.Count() > 0)
					{
						foreach (var h in lHistory)
						{
							if (h != null)
							{
								List<tFormaPagoCredito> lPaymentWay = context.tFormaPagoCreditoes.Where(p => p.idHistorialCredito == h.idHistorialCredito).ToList();

								if (lPaymentWay.Count() > 0)
								{
									foreach (var pw in lPaymentWay)
									{
										if (pw != null)
										{
											context.tFormaPagoCreditoes.Remove(pw);
											context.SaveChanges();
										}
									}
								}

								context.tHistorialCreditoes.Remove(h);
								context.SaveChanges();
							}
						}
					}

					List<tFormaPago> lPayment = context.tFormaPagoes.Where(p => p.idVenta == idSale).ToList();

					if (lPayment.Count() > 0)
					{
						foreach (tFormaPago p in lPayment)
						{
							if (p != null)
							{
								context.tFormaPagoes.Remove(p);
								context.SaveChanges();
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

		public int GetLastID()
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var lastId = context.tVentas.Max(p => (int?) p.idVenta) ?? 0;

					return lastId;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public string GeneratePrevNumberRem(int idBranch)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					string month = DateTime.Now.ToString("MM");
					string year = DateTime.Now.ToString("yy");

					string ID = (this.GetLastID() + 1).ToString();
					char pad = '0';

					return string.Concat(month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T"));
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

					return string.Concat(month, string.Concat(string.Concat(year + "-", idSale.ToString().PadLeft(4, pad)), (idBranch == TypesBranch.Amazonas) ? "A" : (idBranch == TypesBranch.Guadalquivir) ? "G" : "T"));
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public SaleViewModel GetSaleForRemision(string remision)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var sale = context.tVentas.Where(p => p.Remision.ToUpper() == remision.ToUpper()).Select(p => new SaleViewModel()
					{
						idVenta = p.idVenta,
						Remision = p.Remision,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						idClienteFisico = p.idClienteFisico,
						oClienteFisico = p.tClientesFisico,
						idClienteMoral = p.idClienteMoral,
						oClienteMoral = p.tClientesMorale,
						ClienteFisico = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
						idDespacho = p.idDespacho,
						Despacho = p.tDespacho.Nombre,
						oDespacho = p.tDespacho,
						Proyecto = p.Proyecto,
						idDespachoReferencia = p.idDespachoReferencia,
						DespachoReferencia = p.tDespacho1.Nombre,
						idSucursal = p.idSucursal,
						Sucursal = p.tSucursale.Nombre,
						Fecha = p.Fecha,
						CantidadProductos = p.CantidadProductos,
						Subtotal = p.Subtotal,
						Descuento = p.Descuento ?? 0,
						IVA = p.IVA,
						Total = p.Total,
						TotalNotasCredito = p.tDetalleVentas.Where(x => x.idNotaCredito > 0).Select(x => x.Precio * x.Cantidad).DefaultIfEmpty(0).Sum(),
						Estatus = p.Estatus,
						NumberFactura = p.NumeroFactura,
						TipoCliente = p.TipoCliente,
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
						oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
						{
							idDetalleVenta = x.idDetalleVenta,
							idVenta = x.idVenta,
							idProducto = x.idProducto,
							Codigo = (context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo == null) ? x.Descripcion : context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo,
							idServicio = x.idServicio,
							idNotaCredito = x.idNotaCredito,
							NotaCredito = x.NotaCredito,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento ?? 0,
							Cantidad = x.Cantidad,
							Proveedor = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().tProveedore.NombreEmpresa,
							idPromocion = x.idPromocion,
							CostoPromocion = x.CostoPromocion,
							idTipoPromocion = x.idTipoPromocion,
							idProductoPadre = x.idProductoPadre,
							oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
							{
								idProducto = o.idProducto,
								urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
								idProveedor = o.idProveedor,
								Extension = o.Extension
							}).FirstOrDefault(),
							Comentarios = x.Comentarios,
							Imagen = x.Imagen,
							Omitir = x.Omitir ?? false
						}).ToList(),
						SumSubtotal = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum()

					}).FirstOrDefault();

					foreach (var product in sale.oDetail)
					{
						if (product.idServicio == 1 || product.idServicio == 11
							|| product.idServicio == 25 || product.idServicio == 60 || product.idServicio == 61)
						{
							product.Omitir = true;
						}
					}

					return sale;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public SaleViewModel GetSaleForIdSale(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					context.Configuration.LazyLoadingEnabled = false;

					var sales = context.tVentas.Where(p => p.idVenta == idSale).Select(p => new SaleViewModel()
					{
						idVenta = p.idVenta,
						Remision = p.Remision,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						idClienteFisico = p.idClienteFisico,
						oClienteFisico = p.tClientesFisico,
						idClienteMoral = p.idClienteMoral,
						oClienteMoral = p.tClientesMorale,
						ClienteFisico = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : "",
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
						Total = p.Total,
						TotalNotasCredito = p.tDetalleVentas.Where(x => x.idNotaCredito > 0).Select(x => x.Precio * x.Cantidad).DefaultIfEmpty(0).Sum(),
						TipoCliente = p.TipoCliente,
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
						oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
						{
							idDetalleVenta = x.idDetalleVenta,
							idVenta = x.idVenta,
							idProducto = x.idProducto,
							Codigo = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo,
							idServicio = x.idServicio,
							idNotaCredito = x.idNotaCredito,
							idSucursal = x.idSucursal,
							NotaCredito = x.NotaCredito,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento,
							Cantidad = x.Cantidad,
							TipoImagen = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().TipoImagen,
							idPromocion = x.idPromocion,
							CostoPromocion = x.CostoPromocion,
							idTipoPromocion = x.idTipoPromocion,
							idProductoPadre = x.idProductoPadre,
							oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
							{
								idProducto = o.idProducto,
								urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
								idProveedor = o.idProveedor,
								NombreImagen = o.NombreImagen + o.Extension,
								Extension = o.Extension
							}).FirstOrDefault(),
							Comentarios = x.Comentarios,
							Imagen = x.Imagen
						}).ToList(),
						SumSubtotal = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum(),
						Editar = p.Editar ?? false,
						Estatus = p.Estatus
					}).FirstOrDefault();

					return sales;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddTax(int id)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var sale = context.tVentas.Where(p => p.idVenta == id).FirstOrDefault();

					sale.IVA = 1;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public SaleViewModel GetSaleToCredit(string remision)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var sales = context.tVentas.Where(p => p.Remision.ToUpper() == remision.ToUpper()).Select(p => new SaleViewModel()
					{

						idVenta = p.idVenta,
						Remision = p.Remision,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						ClienteFisico = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : "",
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
						Total = p.Total,
						TotalNotasCredito = p.tDetalleVentas.Where(x => x.idNotaCredito > 0).Select(x => x.Precio * x.Cantidad).DefaultIfEmpty(0).Sum(),
						TipoCliente = p.TipoCliente,
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
						oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta && x.idNotaCredito.HasValue == false).Select(x => new SaleDetailViewModel()
						{

							idDetalleVenta = x.idDetalleVenta,
							idVenta = x.idVenta,
							idProducto = x.idProducto,
							Codigo = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo,
							idServicio = x.idServicio,
							idNotaCredito = x.idNotaCredito,
							NotaCredito = x.NotaCredito,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento,
							Cantidad = x.Cantidad,
							Comentarios = x.Comentarios,
							Imagen = x.Imagen,
							idPromocion = x.idPromocion,
							CostoPromocion = x.CostoPromocion,
							idTipoPromocion = x.idTipoPromocion,
							idProductoPadre = x.idProductoPadre
						}).ToList(),
						SumSubtotal = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum()

					}).FirstOrDefault();

					return sales;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public SaleViewModel GetSaleToCredit(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var sales = context.tVentas.Where(p => p.idVenta == idSale).Select(p => new SaleViewModel()
					{

						idVenta = p.idVenta,
						Remision = p.Remision,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						ClienteFisico = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : "",
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
						Total = p.Total,
						TipoCliente = p.TipoCliente,
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
						oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
						{

							idDetalleVenta = x.idDetalleVenta,
							idVenta = x.idVenta,
							idProducto = x.idProducto,
							Codigo = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo,
							idServicio = x.idServicio,
							idNotaCredito = x.idNotaCredito,
							NotaCredito = x.NotaCredito,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento,
							Cantidad = x.Cantidad,
							idPromocion = x.idPromocion,
							CostoPromocion = x.CostoPromocion,
							idTipoPromocion = x.idTipoPromocion,
							idProductoPadre = x.idProductoPadre,
							oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
							{
								idProducto = o.idProducto,
								urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
								idProveedor = o.idProveedor,
								Extension = o.Extension
							}).FirstOrDefault(),
							Comentarios = x.Comentarios,
							Imagen = x.Imagen
						}).ToList(),
						SumSubtotal = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum()

					}).FirstOrDefault();

					return sales;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void SetStatus(int idSale, short status, string comments)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tVenta oSale = context.tVentas.FirstOrDefault(p => p.idVenta == idSale);

					if (status == TypesSales.ventaCancelada && (context.tFormaPagoes.Any(p => (p.idVenta == oSale.idVenta) && (p.Estatus == TypesSales.ventaPendiente || p.Estatus == TypesSales.ventaSaldada)) || context.tHistorialCreditoes.Any(p => (p.idVenta == oSale.idVenta) && (p.Estatus == TypesSales.ventaPendiente))))
					{
						throw new System.ArgumentException("Existen pagos pendientes por cancelar");
					}

					if (status == TypesSales.ventaSaldada && (context.tFormaPagoes.Any(p => (p.idVenta == oSale.idVenta) && (p.Estatus == TypesSales.ventaPendiente)) || context.tHistorialCreditoes.Any(p => (p.idVenta == oSale.idVenta) && (p.Estatus == TypesSales.ventaPendiente))))
					{
						throw new System.ArgumentException("Existen pagos pendientes por saldar");
					}

					if (oSale.Estatus == TypesSales.ventaCancelada && (status == TypesSales.ventaSaldada || status == TypesSales.ventaPendiente))
					{
						this.DeleteProductsFromStock(idSale, oSale.idUsuario1);
					}
					else if (status == TypesSales.ventaCancelada && (oSale.Estatus == TypesSales.ventaSaldada || oSale.Estatus == TypesSales.ventaPendiente))
					{
						this.ReturnProducts(idSale, (int)oSale.idSucursal, oSale.idUsuario1);
					}

					oSale.Estatus = status;
					oSale.Comentarios = comments;

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void ReturnProducts(int idSale, int idBranch, int? idUser)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					if (idBranch == 1)
					{
						List<SaleDetailViewModel> lSaleDetail = context.tDetalleVentas.Where(p => p.idVenta == idSale && p.idProducto != null).Select(p => new SaleDetailViewModel()
						{
							idProducto = p.idProducto,
							Cantidad = p.Cantidad,
							idSucursal = p.idSucursal,
							idPromocion = p.idPromocion
						}).ToList();

						foreach (var product in lSaleDetail)
						{
							tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == product.idSucursal && p.idProducto == product.idProducto);

							if (oProductBranch != null)
							{
								this.AddRegisterProduct(oProductBranch.idProducto, idBranch, "Se actualiza el inventario por edición de venta " + this.GetSaleForIdSale(idSale).Remision, (decimal)oProductBranch.Existencia, (decimal)(oProductBranch.Existencia + product.Cantidad), product.Precio, product.Precio, String.Empty, (int)idUser);

								oProductBranch.Existencia = oProductBranch.Existencia + product.Cantidad;

								context.SaveChanges();
							}
						}
					}
					else
					{
						List<SaleDetailViewModel> lSaleDetail = context.tDetalleVentas.Where(p => p.idVenta == idSale && p.idProducto != null).Select(p => new SaleDetailViewModel()
						{
							idProducto = p.idProducto,
							Cantidad = p.Cantidad
						}).ToList();


						foreach (var product in lSaleDetail)
						{
							tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == idBranch && p.idProducto == product.idProducto);

							if (oProductBranch != null)
							{
								this.AddRegisterProduct(oProductBranch.idProducto, idBranch, "Se actualiza el inventario por edición de venta " + idSale, (decimal)oProductBranch.Existencia, (decimal)(oProductBranch.Existencia + product.Cantidad), product.Precio, product.Precio, String.Empty, (int)idUser);

								oProductBranch.Existencia = oProductBranch.Existencia + product.Cantidad;

								context.SaveChanges();
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

		public void ReturnProductsUnifySale(int idSale, int? idUser)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					List<SaleDetailViewModel> lSaleDetail = context.tDetalleVentas.Where(p => p.idVenta == idSale && p.idProducto != null).Select(p => new SaleDetailViewModel()
					{
						idProducto = p.idProducto,
						Cantidad = p.Cantidad,
						idSucursal = p.idSucursal,
						idPromocion = p.idPromocion
					}).ToList();

					foreach (var product in lSaleDetail)
					{
						tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == product.idSucursal && p.idProducto == product.idProducto);

						if (oProductBranch != null)
						{
							this.AddRegisterProduct(oProductBranch.idProducto, (int)product.idSucursal, "Se actualiza el inventario por edición de venta " + this.GetSaleForIdSale(idSale).Remision, (decimal)oProductBranch.Existencia, (decimal)(oProductBranch.Existencia + product.Cantidad), product.Precio, product.Precio, String.Empty, (int)idUser);
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

		public void DeleteProductsFromStock(int idSale, int idBranch, int? idUser)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					List<SaleDetailViewModel> lSaleDetail = context.tDetalleVentas.Where(p => p.idVenta == idSale && p.idProducto != null).Select(p => new SaleDetailViewModel()
					{
						idProducto = p.idProducto,
						Cantidad = p.Cantidad
					}).ToList();

					foreach (var product in lSaleDetail)
					{
						tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == idBranch && p.idProducto == product.idProducto);
						this.AddRegisterProduct(oProductBranch.idProducto, idBranch, "Se actualiza el inventario por venta " + this.GetSaleForIdSale(idSale).Remision, (decimal)oProductBranch.Existencia, (decimal)(oProductBranch.Existencia - product.Cantidad), product.Precio, product.Precio, String.Empty, (int)idUser);
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

		public void SetNumberBill(int idSale, string numberBill)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var oSale = context.tVentas.FirstOrDefault(p => p.idVenta == idSale);

					if (oSale != null)
					{
						oSale.NumeroFactura = numberBill;
						context.SaveChanges();
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<CreditHistoryViewModel> GetCreditHistory(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tHistorialCreditoes.Where(p => p.idVenta == idSale).Select(p => new CreditHistoryViewModel()
					{

						idHitorialCredito = p.idHistorialCredito,
						idVenta = p.idVenta,
						Cantidad = p.Cantidad,
						amountIVA = p.CantidadIVA,
						IVA = p.IVA,
						Restante = p.Restante,
						FormaPago = (p.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == p.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

							x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
							x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
							x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
							x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
							x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
							x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
							x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
							x.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
							x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
							""

						).FirstOrDefault()),
						Comentario = p.Comentario,
						Fecha = p.Fecha,
						Estatus = p.Estatus,
						sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
									p.Estatus == TypesSales.ventaPendiente ? "yellow" :
									"red"),
						idCuenta = p.idCuenta,
						_voucher = p.Voucher
					}).OrderBy(p => p.Fecha).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<TypePaymentViewModel> GetTypePayment(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var payment = context.tFormaPagoes.Where(p => p.idVenta == idSale).Select(p => new TypePaymentViewModel()
					{
						idTypePayment = p.idFormaPago,
						typesPayment = (short)p.FormaPago,
						sTypePayment = (p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
						p.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
						p.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
						p.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
						p.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
						p.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
						p.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
						p.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
						p.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
						p.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
						""
						),
						sTypesCard = (p.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
						p.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
						p.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
						amount = (decimal)p.Cantidad,
						maxPayment = (DateTime)p.FechaLimitePago,
						Estatus = p.Estatus,
						DatePayment = (DateTime)p.FechaPago,
						bank = p.Banco.ToUpper(),
						amountIVA = p.CantidadIVA,
						IVA = p.IVA,
						cuenta = context.tCuentas.Where(c => c.idCuenta == p.idCuenta).Select(c => new AccountViewModel()
						{
							idCuenta = c.idCuenta,
							Cuenta = c.Cuenta
						}).FirstOrDefault(),
						idCuenta = p.idCuenta,
						_Cuenta = p.tCuenta.Cuenta,
						_voucher = p.Voucher,
						idCreditNote = p.idNotaCredito,
						CreditNote = p.tNotasCredito.Remision,
						HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == idSale && p.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
						{
							FormaPago = (context.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

								x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
								x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
								x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
								x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
								x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
								x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
								x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
								x.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
								x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
								""
							).FirstOrDefault()),
							Cantidad = q.Cantidad,
							amountIVA = q.CantidadIVA,
							IVA = q.IVA,
							Restante = q.Restante,
							Estatus = q.Estatus,
							Fecha = q.Fecha,
							idVenta = q.idVenta,
							idHitorialCredito = q.idHistorialCredito,
							idCuenta = q.idCuenta,
							_Cuenta = q.tCuenta.Cuenta,
							_voucher = q.Voucher,
							idCreditNote = context.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.idNotaCredito).FirstOrDefault(),
							CreditNote = context.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.tNotasCredito.Remision).FirstOrDefault()
						}).OrderBy(q => q.Fecha).ToList()

					}).ToList();

					var paymentDistinct = payment.Where(p => p.typesPayment != TypesPayment.iCredito).Distinct().ToList();

					var paymentToAdd = payment.Where(p => p.typesPayment == TypesPayment.iCredito).ToList();

					foreach (var pay in paymentToAdd)
					{
						paymentDistinct.Add(pay);
					}

					return paymentDistinct.OrderBy(p => p.DatePayment).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<TypePaymentViewModel> GetTypePaymentForPrint(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var payment = context.tFormaPagoes.Where(p => p.idVenta == idSale).Select(p => new TypePaymentViewModel()

					{
						idTypePayment = p.idFormaPago,
						typesPayment = (short)p.FormaPago,
						sTypePayment = (p.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
						p.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
						p.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
						p.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
						p.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
						p.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
						p.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
						p.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
						p.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
						p.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
						""
						),
						sTypesCard = (p.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
						p.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
						p.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
						amount = (decimal)p.Cantidad,
						maxPayment = (DateTime)p.FechaLimitePago,
						Estatus = p.Estatus,
						DatePayment = (DateTime)p.FechaPago,
						bank = p.Banco.ToUpper(),
						amountIVA = p.CantidadIVA,
						IVA = p.IVA,
						cuenta = context.tCuentas.Where(c => c.idCuenta == p.idCuenta).Select(c => new AccountViewModel()
						{
							idCuenta = c.idCuenta,
							Cuenta = c.Cuenta
						}).FirstOrDefault(),
						idCuenta = p.idCuenta,
						_Cuenta = p.tCuenta.Cuenta,
						_voucher = p.Voucher,
						idCreditNote = p.idNotaCredito,
						CreditNote = p.tNotasCredito.Remision,
						HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == idSale && p.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
						{
							FormaPago = (q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
								x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
								x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
								x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
								x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
								x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
								x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
								x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
								x.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
								x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
								""
							).FirstOrDefault()),
							Cantidad = q.Cantidad,
							amountIVA = q.CantidadIVA,
							IVA = q.IVA,
							Restante = q.Restante,
							Estatus = q.Estatus,
							Fecha = q.Fecha,
							idVenta = q.idVenta,
							idHitorialCredito = q.idHistorialCredito,
							idCuenta = q.idCuenta,
							_Cuenta = q.tCuenta.Cuenta,
							_voucher = q.Voucher,
							idCreditNote = q.tFormaPagoCreditoes.Where(x => x.idNotaCredito == q.idHistorialCredito).Select(x => x.idNotaCredito).FirstOrDefault(),
							CreditNote = q.tFormaPagoCreditoes.Where(x => x.idNotaCredito == q.idHistorialCredito).Select(x => x.tNotasCredito.Remision).FirstOrDefault()
						}).OrderBy(q => q.Fecha).ToList()

					}).ToList();

					var paymentDistinct = payment.Where(p => p.typesPayment != TypesPayment.iCredito).Distinct().ToList();

					var paymentToAdd = payment.Where(p => p.typesPayment == TypesPayment.iCredito).ToList();

					foreach (var pay in paymentToAdd)
					{
						paymentDistinct.Add(pay);
					}

					List<TypePaymentViewModel> lTypePayments = new List<TypePaymentViewModel>();

					foreach (var register in paymentDistinct)
					{
						decimal suma = 0;

						if (register.HistoryCredit.Count > 0)
						{
							foreach (var history in register.HistoryCredit)
							{
								TypePaymentViewModel phistory = new TypePaymentViewModel();

								phistory.idTypePayment = history.idHitorialCredito;
								phistory.sTypePayment = history.FormaPago;
								phistory.amount = (decimal)history.Cantidad;
								phistory.DatePayment = history.Fecha;
								phistory.CreditNote = history.CreditNote;
								phistory._Cuenta = history._Cuenta;
								phistory._voucher = history._voucher;

								lTypePayments.Add(phistory);

								suma = suma - (decimal)history.Cantidad;
							}

							register.amount = register.amount + suma;
						}

						TypePaymentViewModel pregister = new TypePaymentViewModel();

						pregister.idTypePayment = register.idTypePayment;
						pregister.typesPayment = register.typesPayment;
						pregister.sTypePayment = register.sTypePayment;
						pregister.amount = (decimal)register.amount;
						pregister.DatePayment = register.DatePayment;
						pregister.CreditNote = register.CreditNote;
						pregister._Cuenta = register._Cuenta;
						pregister._voucher = register._voucher;

						lTypePayments.Add(pregister);
					}

					return lTypePayments.OrderBy(p => p.DatePayment).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public decimal? GetLeftoverCreditHistory(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tFormaPagoes.Where(p => p.idVenta == idSale && p.FormaPago == TypesPayment.iCredito).Sum(p => p.Cantidad);
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int InsertRecordCreditHistory(int idSale, decimal amount, decimal left, string comments, DateTime dtInsert, short? status = 2)
		{
			int iResult = 0;

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tHistorialCredito oHistory = new tHistorialCredito();

					oHistory.idVenta = idSale;
					oHistory.Cantidad = amount;
					oHistory.Restante = left;
					oHistory.Comentario = comments;
					oHistory.Fecha = dtInsert;
					oHistory.Estatus = status;

					context.tHistorialCreditoes.Add(oHistory);

					context.SaveChanges();

					iResult = oHistory.idHistorialCredito;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}

			return iResult;
		}

		public void SetStatusCreditHistory(int idCreditHistory, short status, DateTime dateModify, int? idCuenta, string voucher)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tHistorialCredito oHistory = context.tHistorialCreditoes.FirstOrDefault(p => p.idHistorialCredito == idCreditHistory);

					oHistory.Fecha = dateModify;
					oHistory.idCuenta = idCuenta;
					oHistory.Voucher = voucher;
					oHistory.Estatus = status;

					context.SaveChanges();

					//Se valida que todos los abonos esten con estatus SALDADO, si es así entonces el estatus de la venta se cambia a SALDADO
					int idSale = (int)context.tHistorialCreditoes.Where(p => p.idHistorialCredito == idCreditHistory).Select(p => p.idVenta).FirstOrDefault();

					List<CreditHistoryViewModel> lHistory = this.GetCreditHistory(idSale);

					var leftPayment = this.GetLeftoverCreditHistory(idSale) - lHistory.Sum(p => p.Cantidad);

					if ((!context.tHistorialCreditoes.Where(p => p.idVenta == idSale && p.Estatus == TypesSales.ventaPendiente).Any()) && (leftPayment <= 0))
					{
						tFormaPago oForma = context.tFormaPagoes.FirstOrDefault(p => p.idVenta == idSale && p.FormaPago == TypesPayment.iCredito);

						oForma.Estatus = TypesPayment.iCancelada;
					}

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddTypePaymentCredit(int idCreditHistory, int typePayment, int? typeCard, string bank, string holder, string check, int? idCreditNote)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tFormaPagoCredito oFormaPago = new tFormaPagoCredito();

					oFormaPago.idHistorialCredito = idCreditHistory;
					oFormaPago.FormaPago = typePayment;
					oFormaPago.TipoTarjeta = typeCard;
					oFormaPago.Banco = bank;
					oFormaPago.Titular = holder;
					oFormaPago.NumCheque = check;
					oFormaPago.idNotaCredito = idCreditNote;

					context.tFormaPagoCreditoes.Add(oFormaPago);

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int ValidateBill(string rfc)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tDatosFacturacions.Where(p => p.RFC.ToUpper().Trim() == rfc.ToUpper().Trim()).Count();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public BillViewModel GetBill(string rfc)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tDatosFacturacions.Where(p => p.RFC.ToUpper().Trim() == rfc.ToUpper().Trim()).Select(p => new BillViewModel()
					{
						idDatosFacturacion = p.idDatosFacturacion,
						Nombre = p.Nombre,
						Telefono = p.Telefono,
						Correo = p.Correo,
						RFC = p.RFC,
						Calle = p.Calle,
						NumExt = p.NumExt,
						NumInt = p.NumInt,
						Colonia = p.Colonia,
						Municipio = p.Municipio,
						Estado = p.Estado,
						CP = p.CP
					}).FirstOrDefault();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddBill(BillViewModel bill)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					if (context.tDatosFacturacions.Where(p => p.RFC.ToUpper().Trim() == bill.RFC.ToUpper().Trim()).Count() == 0)
					{
						tDatosFacturacion billTable = new tDatosFacturacion();

						billTable.Nombre = bill.Nombre;
						billTable.Telefono = bill.Telefono;
						billTable.Correo = bill.Correo;
						billTable.RFC = bill.RFC;
						billTable.Calle = bill.Calle;
						billTable.NumExt = bill.NumExt;
						billTable.NumInt = bill.NumInt;
						billTable.Colonia = bill.Colonia;
						billTable.Municipio = bill.Municipio;
						billTable.Estado = bill.Estado;
						billTable.CP = bill.CP;

						context.tDatosFacturacions.Add(billTable);

						context.SaveChanges();
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void UpdateBill(BillViewModel bill)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tDatosFacturacion billTable = context.tDatosFacturacions.FirstOrDefault(p => p.RFC.ToUpper().Trim() == bill.RFC.ToUpper().Trim());

					billTable.Nombre = bill.Nombre;
					billTable.Telefono = bill.Telefono;
					billTable.Correo = bill.Correo;
					billTable.RFC = bill.RFC;
					billTable.Calle = bill.Calle;
					billTable.NumExt = bill.NumExt;
					billTable.NumInt = bill.NumInt;
					billTable.Colonia = bill.Colonia;
					billTable.Municipio = bill.Municipio;
					billTable.Estado = bill.Estado;
					billTable.CP = bill.CP;

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public string GetRemission(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tVentas.Where(p => p.idVenta == idSale).Select(p => p.Remision).FirstOrDefault();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<SaleViewModel> GetRemissions(string remission)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tVentas.Where(p => p.Estatus == TypesSales.ventaPendiente && p.Remision.Contains(remission))
							.Select(p => new SaleViewModel()
							{
								idVenta = p.idVenta,
								Remision = p.Remision
							}).Distinct().OrderBy(p => p.Remision).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void BillSale(string remision)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tVenta oSale = context.tVentas.FirstOrDefault(p => p.Remision.Trim().ToUpper() == remision.Trim().ToUpper());

					oSale.Factura = TypesSales.factura;
					oSale.IVA = TypesSales.factura;

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void UpdateTotalSale(string remision, decimal amount)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tVenta oSale = context.tVentas.FirstOrDefault(p => p.Remision.Trim().ToUpper() == remision.Trim().ToUpper());

					oSale.Estatus = TypesSales.ventaPendiente;

					oSale.Total = oSale.Total + amount;

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void UpdateTotalSale(int idSale, decimal amount)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tVenta oSale = context.tVentas.FirstOrDefault(p => p.idVenta == idSale);
					oSale.Total = oSale.Total + amount;
					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void SaveDatePaymentDetail(int idTypePayment, short status, DateTime? datePayment, int? idcuenta, string voucher)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					DateTime? dtDate = null;

					if (datePayment != null)
						dtDate = datePayment.Value.Date + new TimeSpan(0, 0, 0);

					tFormaPago oPayment = context.tFormaPagoes.FirstOrDefault(p => p.idFormaPago == idTypePayment);

					oPayment.Estatus = status;

					if (dtDate != null)
						oPayment.FechaPago = Convert.ToDateTime(dtDate);

					oPayment.idCuenta = idcuenta;
					oPayment.Voucher = voucher;

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void SaveEditSale(SaleViewModel oSale, List<ProductSaleViewModel> lProducts)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var tVentas = context.tVentas.FirstOrDefault(p => p.idVenta == oSale.idVenta);

					if (tVentas == null) throw new ArgumentNullException(nameof(tVentas));

					tVentas.TipoCliente = oSale.TipoCliente;
					tVentas.idClienteFisico = oSale.idClienteFisico;
					tVentas.idClienteMoral = oSale.idClienteMoral;
					tVentas.idDespacho = oSale.idDespacho;
					tVentas.idDespachoReferencia = oSale.idDespachoReferencia;
					tVentas.Proyecto = oSale.Proyecto;
					tVentas.idUsuario1 = oSale.idUsuario1;
					tVentas.idUsuario2 = oSale.idUsuario2;
					tVentas.CantidadProductos = oSale.CantidadProductos;
					tVentas.Subtotal = oSale.Subtotal;
					tVentas.Descuento = oSale.Descuento;
					tVentas.Total = oSale.Total;
					tVentas.IVA = oSale.IVA;

					context.SaveChanges();

					//Se elimina detalle de Venta
					List<tDetalleVenta> lDetailSale = context.tDetalleVentas.Where(p => p.idVenta == oSale.idVenta).ToList();

					foreach (var detail in lDetailSale)
					{
						context.tDetalleVentas.Remove(detail);
						context.SaveChanges();
					}

					foreach (var prod in lProducts)
					{

						if (prod.Tipo == TypesSales.credito)
						{
							this.AddCreditSale(oSale.idVenta, prod);
							Credits oCredits = new Credits();
							oCredits.SetStatus(prod.codigo, TypesCredit.creditoSaldada);
						}
						else
						{
							this.AddProductSale(oSale.idVenta, prod);
						}

					}
					if (oSale.idSucursal == 1)
						this.DeleteProductsFromStock(oSale.idVenta, oSale.idUsuario1);
					else
						this.DeleteProductsFromStock(oSale.idVenta, (int)tVentas.idSucursal, oSale.idUsuario1);
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void SaveEditSale(SaleViewModel oSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var tVentas = context.tVentas.FirstOrDefault(p => p.idVenta == oSale.idVenta);

					if (tVentas == null) throw new ArgumentNullException(nameof(tVentas));

					var total = tVentas.Total;

					tVentas.TipoCliente = oSale.TipoCliente;
					tVentas.idClienteFisico = oSale.idClienteFisico;
					tVentas.idClienteMoral = oSale.idClienteMoral;
					tVentas.idDespacho = oSale.idDespacho;
					tVentas.idDespachoReferencia = oSale.idDespachoReferencia;
					tVentas.Proyecto = oSale.Proyecto;
					tVentas.idUsuario1 = oSale.idUsuario1;
					tVentas.idUsuario2 = oSale.idUsuario2;
					tVentas.Subtotal = oSale.Subtotal;
					tVentas.Descuento = oSale.Descuento;
					tVentas.Total = oSale.Total;
					tVentas.IVA = oSale.IVA;

					context.SaveChanges();

					if (total != oSale.Total)
					{
						UpdatePaymentWay(oSale.idVenta);
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void UpdatePaymentWay(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var venta = context.tVentas.FirstOrDefault(p => p.idVenta == idSale);

					if (venta == null) throw new ArgumentNullException(nameof(venta));

					var totalpayment = context.tFormaPagoes.Where(p => p.idVenta == idSale && p.FormaPago != TypesGeneric.TypesPayment.iCredito).Sum(p => p.Cantidad);

					//Se obtiene la diferencia
					var value = venta.Total - totalpayment;

					//Se obtiene el crédito
					var credit = context.tFormaPagoes.FirstOrDefault(p => p.idVenta == idSale && p.FormaPago == TypesGeneric.TypesPayment.iCredito);

					if (credit != null)
					{
						credit.Cantidad = value;
					}

					//Se guardan los cambios
					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void SaveEditDetailSale(List<SaleDetailViewModel> lDetail)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					foreach (var saleDetailViewModel in lDetail)
					{
						var detail = saleDetailViewModel;

						var firstOrDefault = context.tDetalleVentas.FirstOrDefault(p => p.idDetalleVenta == detail.idDetalleVenta);

						if (firstOrDefault !=
							null)
							firstOrDefault.Descuento = detail.Descuento;

						context.SaveChanges();
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddIVATypePayment(int idPayment, decimal amount, decimal amountIVA)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tFormaPago typePayment = context.tFormaPagoes.FirstOrDefault(p => p.idFormaPago == idPayment);

					typePayment.Cantidad = amount;
					typePayment.CantidadIVA = amountIVA;
					typePayment.IVA = true;

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddIVATypePaymentCredit(int idCreditHistory, decimal amount, decimal amountIVA)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tHistorialCredito typePayment = context.tHistorialCreditoes.FirstOrDefault(p => p.idHistorialCredito == idCreditHistory);

					typePayment.Cantidad = amount;
					typePayment.CantidadIVA = amountIVA;
					typePayment.IVA = true;

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<AccountViewModel> GetAccounts()
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tCuentas.Select(p => new AccountViewModel()
					{
						idCuenta = p.idCuenta,
						Cuenta = p.Cuenta
					}).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<int?> GetRemissionfromDetailSale(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tDetalleVentas.Where(p => p.idVenta == idSale && p.NotaCredito == true && p.idNotaCredito > 0).Select(p => p.idNotaCredito).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public string GeneratePrevNumberRemC()
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					string month = DateTime.Now.ToString("MM");
					string year = DateTime.Now.ToString("yy");

					string ID = (this.GetLastID() + 1).ToString();
					char pad = '0';

					return string.Concat(month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), "U"));
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public string GenerateNumberRemC(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					string month = DateTime.Now.ToString("MM");
					string year = DateTime.Now.ToString("yy");

					char pad = '0';

					return string.Concat(month, string.Concat(string.Concat(year + "-", idSale.ToString().PadLeft(4, pad)), "U"));
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void DeleteProductsFromStock(int idSale, int? idUser)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					List<SaleDetailViewModel> lSaleDetail = context.tDetalleVentas.Where(p => p.idVenta == idSale && p.idProducto != null).Select(p => new SaleDetailViewModel()
					{
						idProducto = p.idProducto,
						Cantidad = p.Cantidad,
						idSucursal = p.idSucursal
					}).ToList();


					foreach (var product in lSaleDetail)
					{
						tProductosSucursal oProductBranch = context.tProductosSucursals.FirstOrDefault(p => p.idSucursal == product.idSucursal && p.idProducto == product.idProducto);

						this.AddRegisterProduct(oProductBranch.idProducto, oProductBranch.idSucursal, "Se actualiza el inventario por venta " + this.GetSaleForIdSale(idSale).Remision, (decimal)oProductBranch.Existencia, (decimal)(oProductBranch.Existencia - product.Cantidad), context.tProductos.FirstOrDefault(p => p.idProducto == oProductBranch.idProducto).PrecioVenta, context.tProductos.FirstOrDefault(p => p.idProducto == oProductBranch.idProducto).PrecioVenta, String.Empty, (int)idUser);

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

		public void DeletePaymentWayOfSale(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					//Obtener historial credito
					if (context.tHistorialCreditoes.Where(p => p.idVenta == idSale).Any())
					{
						var histories = context.tHistorialCreditoes.Where(p => p.idVenta == idSale).ToList();

						foreach (var history in histories)
						{
							//Se elimina 
							var payments = context.tFormaPagoCreditoes.Where(p => p.idHistorialCredito == history.idHistorialCredito);

							if (payments != null)
							{
								foreach (var payment in payments)
								{
									if (payment.FormaPago == TypesPayment.iNotaCredito)
									{
										var creditnote = context.tNotasCreditoes.Where(p => p.idNotaCredito == payment.idNotaCredito).FirstOrDefault();

										if (creditnote != null)
										{
											creditnote.Estatus = TypesSales.ventaPendiente;
										}
									}

									context.tFormaPagoCreditoes.Remove(payment);
								}
							}

							context.tHistorialCreditoes.Remove(history);
						}
					}

					//Obtener formas de pago
					List<tFormaPago> listPaymentWay = context.tFormaPagoes.Where(p => p.idVenta == idSale).ToList();

					//Eliminar forma de pago  
					if (listPaymentWay != null)
					{
						foreach (var payment in listPaymentWay)
						{
							if (payment.FormaPago == TypesPayment.iNotaCredito)
							{
								var creditnote = context.tNotasCreditoes.Where(p => p.idNotaCredito == payment.idNotaCredito).FirstOrDefault();

								if (creditnote != null)
								{
									creditnote.Estatus = TypesSales.ventaPendiente;
									context.SaveChanges();
								}
							}

							context.tFormaPagoes.Remove(payment);
						}
					}

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void SaleIsEditing(int idSale)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var sale = context.tVentas.Where(p => p.idVenta == idSale).FirstOrDefault();

					if (sale != null)
					{
						sale.Editar = (sale.Editar == true) ? false : true;
						sale.Estatus = TypesSales.ventaPendiente;

						context.SaveChanges();
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<SalesChartViewModel> GetSalesForChart()
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var previousMonth = DateTime.Now.AddMonths(-1);
					var actualMonth = DateTime.Now;

					var sales = new List<SalesChartViewModel>();

					sales = context.tUsuarios
							.Where(p => p.Estatus == TypesGeneric.TypesUser.EstatusActivo &&
										(p.idPerfil == 9 || p.idPerfil == 12)).Select(p => new SalesChartViewModel()
										{
											VendedorId = p.idUsuario,
											Vendedor = p.Nombre
										}).ToList();

					foreach (var sale in sales)
					{
						sale.VentasMesPasado = this.GetSalesToChart(sale.VendedorId, previousMonth);
						sale.VentasMesActual = this.GetSalesToChart(sale.VendedorId, actualMonth);
					}

					return sales;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public decimal GetSalesToChart(int userId, DateTime dtMoth)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					#region Ventas
					var sales = context.tVentas.Where(
					   p =>
					  (p.Fecha.Value.Year == dtMoth.Year && p.Fecha.Value.Month == dtMoth.Month) &&
					  p.idUsuario1 == userId &&
					  p.idUsuario2 == null &&
					  ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
					  p.Remision.Length > 0 &&
					  p.Total > 0
					)
					.Select(p => new ReportOfSalesViewModel()
					{
						Compartida = false,
						idVenta = p.idVenta,
						Anio = p.Fecha.Value.Year,
						Mes = p.Fecha.Value.Month,
						Semana = SqlFunctions.DatePart("iso_week", p.Fecha),
						Dia = SqlFunctions.DatePart("Month", p.Fecha) + "/" + SqlFunctions.DatePart("Day", p.Fecha) + "/" + SqlFunctions.DatePart("Year", p.Fecha),
						Fecha = p.Fecha,
						Hora = p.Fecha,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						idDespacho = p.idDespacho,
						Despacho = p.tDespacho.Nombre,
						idSucursal = p.idSucursal,
						Sucursal = p.tSucursale.Nombre,
						Remision = p.Remision,
						TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
						Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
						ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
						ClienteMoral = p.tClientesMorale.Nombre,
						DespachoCliente = p.tDespacho.Nombre,
						ImporteVenta = p.Subtotal,
						Estatus = p.Estatus,
						sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
								p.Estatus == TypesSales.ventaPendiente ? "yellow" :
								"red"),
						IntDescuento = p.Descuento ?? 0,
						Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
						IVA = p.IVA,
						ImportePagadoCliente = 0,
						Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
						{

							idDetalleVenta = x.idDetalleVenta,
							idVenta = x.idVenta,
							idProducto = x.idProducto,
							idServicio = x.idServicio,
							idNotaCredito = x.idNotaCredito,
							NotaCredito = x.NotaCredito,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento ?? 0,
							Cantidad = x.Cantidad,
							idPromocion = x.idPromocion,
							CostoPromocion = x.CostoPromocion,
							idTipoPromocion = x.idTipoPromocion,
							idProductoPadre = x.idProductoPadre,
							oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
							{
								idProducto = o.idProducto,
								urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
								idProveedor = o.idProveedor,
								Extension = o.Extension
							}).FirstOrDefault(),
							Comentarios = x.Comentarios,
							Imagen = x.Imagen,
							Omitir = x.Omitir ?? false
						}).ToList(),
						listTypePayment = context.tFormaPagoes.Where(g => g.idVenta == p.idVenta).Select(g => new TypePaymentViewModel()
						{
							idTypePayment = g.idFormaPago,
							typesPayment = (short)g.FormaPago,
							sTypePayment = (g.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
						g.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
						g.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
						g.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
						g.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
						g.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
						g.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
						g.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
						g.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
						"Nota de Crédito"
						),
							typesCard = g.TipoTarjeta,
							sTypesCard = (g.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
							g.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
							g.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
							amount = (decimal)g.Cantidad,
							maxPayment = (DateTime)g.FechaLimitePago,
							Estatus = g.Estatus,
							DatePayment = (DateTime)g.FechaPago,
							bank = g.Banco.ToUpper(),
							amountIVA = g.CantidadIVA,
							IVA = g.IVA,
							HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == p.idVenta && g.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
							{
								idFormaPago = (int)(q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago).FirstOrDefault()),
								FormaPago = (q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

									x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
									x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
									x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
									x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
									x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
									x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
									x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
									x.FormaPago == TypesPayment.iNotaCredito ? TypesPayment.NotaCredito :
									x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
									""
								).FirstOrDefault()),
								Cantidad = q.Cantidad,
								amountIVA = q.CantidadIVA,
								IVA = q.IVA,
								Restante = q.Restante,
								Estatus = q.Estatus,
								Fecha = q.Fecha,
								idVenta = q.idVenta,
								idHitorialCredito = q.idHistorialCredito

							}).OrderBy(q => q.Fecha).ToList()
						}).ToList(),
						Pagado = p.PagadoUno ?? false,
						Omitir = p.Omitir ?? false
					}).OrderBy(p => p.idVenta).ToList();

					//Se revisa si existen pagos con American Express para descontar el 10%
					if (sales != null)
					{
						foreach (var sale in sales)
						{
							if (sale.listTypePayment.Where(g => g.typesCard == TypesCard.AmericanExpress).Any())
							{
								foreach (var typePayment in sale.listTypePayment)
								{

									if (typePayment.typesCard == TypesCard.AmericanExpress)
									{
										var com = ((typePayment.amount / 110) * 100);
										sale.comAmericanExpress = ((sale.comAmericanExpress ?? 0) + (typePayment.amount - com));
										typePayment.amount = com;
									}
								}
							}
						}
					}

					//Se elimina IVA
					if (sales != null)
					{
						foreach (var sale in sales)
						{
							if (!(bool)sale.Omitir && !(bool)sale.Compartida)
							{
								foreach (var typePayment in sale.listTypePayment)
								{
									//Si tiene IVA se le resta
									if (sale.IVA > 0)
									{
										sale.ImportePagadoCliente += ((typePayment.amount / 116) * 100);
									}
									else
									{
										sale.ImportePagadoCliente += typePayment.amount;
									}
								};

								//Quitar Total Pagado con Nota de Crédito        
								foreach (var typePayment in sale.listTypePayment)
								{
									//No es crédito y está saldada
									if (typePayment.typesPayment == 8 && typePayment.Estatus == 1)
									{
										if (sale.ImportePagadoCliente <= typePayment.amount)
										{
											sale.ImportePagadoCliente -= sale.ImportePagadoCliente;
										}
										else
										{
											//Si tiene IVA se le resta
											if (sale.IVA > 0)
											{
												sale.ImportePagadoCliente -= ((typePayment.amount / 116) * 100);
											}
											else
											{
												sale.ImportePagadoCliente -= typePayment.amount;
											}
										}
									}
									else if (typePayment.typesPayment == 2)
									{
										//Si es crédito se obtiene el historial 
										if (typePayment.HistoryCredit != null)
										{
											foreach (var history in typePayment.HistoryCredit)
											{
												//Está saldado
												if (history.idFormaPago == 8 && history.Estatus == 1)
												{
													sale.ImportePagadoCliente -= history.Cantidad;
												}
											};
										}
									}
								};
							}
						}
					}

					#endregion Ventas

					#region Ventas Compartidas

					//VentasCompartidas
					var salesShared = context.tVentas.Where(
					   p =>
					  (p.Fecha.Value.Year == dtMoth.Year && p.Fecha.Value.Month == dtMoth.Month) &&
					  ((p.idUsuario2 == userId) || ((p.idUsuario1 == userId) && (p.idUsuario2 != null))) &&
					  ((p.Estatus == TypesSales.ventaPendiente) || (p.Estatus == TypesSales.ventaSaldada)) &&
					  p.Remision.Length > 0 &&
					  p.Total > 0
					)
					.Select(p => new ReportOfSalesViewModel()
					{
						Compartida = true,
						idVenta = p.idVenta,
						Anio = p.Fecha.Value.Year,
						Mes = p.Fecha.Value.Month,
						Semana = SqlFunctions.DatePart("iso_week", p.Fecha),
						Dia = SqlFunctions.DatePart("Month", p.Fecha) + "/" + SqlFunctions.DatePart("Day", p.Fecha) + "/" + SqlFunctions.DatePart("Year", p.Fecha),
						Fecha = p.Fecha,
						Hora = p.Fecha,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						idDespacho = p.idDespacho,
						Despacho = p.tDespacho.Nombre,
						idSucursal = p.idSucursal,
						Sucursal = p.tSucursale.Nombre,
						Remision = p.Remision,
						TipoCliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? TypesCustomers.sPhysicalCustomer : (p.TipoCliente == TypesCustomers.MoralCustomer) ? TypesCustomers.sMoralCustomer : TypesCustomers.sOfficeCustomer,
						Cliente = (p.TipoCliente == TypesCustomers.PhysicalCustomer) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : (p.TipoCliente == TypesCustomers.MoralCustomer) ? p.tClientesMorale.Nombre : p.tDespacho.Nombre,
						ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
						ClienteMoral = p.tClientesMorale.Nombre,
						DespachoCliente = p.tDespacho.Nombre,
						ImporteVenta = p.Subtotal,
						Estatus = p.Estatus,
						sEstatus = (p.Estatus == TypesSales.ventaSaldada ? "grey" :
								p.Estatus == TypesSales.ventaPendiente ? "yellow" :
								"red"),
						IntDescuento = p.Descuento ?? 0,
						Descuento = (p.Subtotal * ((p.Descuento ?? 0) / 100)),
						IVA = p.IVA,
						ImportePagadoCliente = p.Total,
						Vendedor1 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						oDetail = p.tDetalleVentas.Where(x => x.idVenta == p.idVenta).Select(x => new SaleDetailViewModel()
						{

							idDetalleVenta = x.idDetalleVenta,
							idVenta = x.idVenta,
							idProducto = x.idProducto,
							idServicio = x.idServicio,
							idNotaCredito = x.idNotaCredito,
							NotaCredito = x.NotaCredito,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento ?? 0,
							Cantidad = x.Cantidad,
							idPromocion = x.idPromocion,
							CostoPromocion = x.CostoPromocion,
							idTipoPromocion = x.idTipoPromocion,
							idProductoPadre = x.idProductoPadre,
							oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
							{
								idProducto = o.idProducto,
								urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
								idProveedor = o.idProveedor,
								Extension = o.Extension
							}).FirstOrDefault(),
							Comentarios = x.Comentarios,
							Imagen = x.Imagen,
							Omitir = x.Omitir ?? false
						}).ToList(),
						listTypePayment = context.tFormaPagoes.Where(g => g.idVenta == p.idVenta).Select(g => new TypePaymentViewModel()
						{
							idTypePayment = g.idFormaPago,
							typesPayment = (short)g.FormaPago,
							sTypePayment = (g.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
						g.FormaPago == TypesPayment.iCredito ? TypesPayment.Credito :
						g.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
						g.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
						g.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
						g.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
						g.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
						g.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
						g.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
						"Nota de Crédito"
						),
							typesCard = g.TipoTarjeta,
							sTypesCard = (g.TipoTarjeta == TypesCard.AmericanExpress ? TypesCard.sAmericanExpress :
							g.TipoTarjeta == TypesCard.MasterCard ? TypesCard.sMasterCard :
							g.TipoTarjeta == TypesCard.Visa ? TypesCard.sVisa : ""),
							amount = (decimal)g.Cantidad,
							maxPayment = (DateTime)g.FechaLimitePago,
							Estatus = g.Estatus,
							DatePayment = (DateTime)g.FechaPago,
							bank = g.Banco.ToUpper(),
							amountIVA = g.CantidadIVA,
							IVA = g.IVA,
							HistoryCredit = context.tHistorialCreditoes.Where(q => q.idVenta == p.idVenta && g.FormaPago == TypesPayment.iCredito).Select(q => new CreditHistoryViewModel()
							{
								FormaPago = (q.tFormaPagoCreditoes.Where(x => x.idHistorialCredito == q.idHistorialCredito).Select(x => x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :

									x.FormaPago == TypesPayment.iDeposito ? TypesPayment.Deposito :
									x.FormaPago == TypesPayment.iEfectivo ? TypesPayment.Efectivo :
									x.FormaPago == TypesPayment.iTarjetaCredito ? TypesPayment.TarjetaCredito :
									x.FormaPago == TypesPayment.iTarjetaDebito ? TypesPayment.TarjetaDebito :
									x.FormaPago == TypesPayment.iCheque ? TypesPayment.Cheque :
									x.FormaPago == TypesPayment.iTransferencia ? TypesPayment.Transferencia :
									x.FormaPago == TypesPayment.iIntercambio ? TypesPayment.Intercambio :
									x.FormaPago == TypesPayment.iMercadoPago ? TypesPayment.MercadoPago :
									""

								).FirstOrDefault()),
								Cantidad = q.Cantidad,
								amountIVA = q.CantidadIVA,
								IVA = q.IVA,
								Restante = q.Restante,
								Estatus = q.Estatus,
								Fecha = q.Fecha,
								idVenta = q.idVenta,
								idHitorialCredito = q.idHistorialCredito
							}).OrderBy(q => q.Fecha).ToList()
						}).ToList(),
						Pagado = (p.idUsuario1 == userId) ? p.PagadoUno ?? false : p.PagadoDos ?? false,
						Omitir = p.Omitir ?? false
					}).OrderBy(p => p.idVenta);

					//Se revisa si existen pagos con American Express para descontar el 10%

					if (salesShared != null)
					{
						foreach (var sale in salesShared)
						{
							if (sale.listTypePayment.Where(g => g.typesCard == TypesCard.AmericanExpress).Any())
							{
								foreach (var typePayment in sale.listTypePayment)
								{
									if (typePayment.typesCard == TypesCard.AmericanExpress)
									{
										var com = ((typePayment.amount / 110) * 100);
										sale.comAmericanExpress = ((sale.comAmericanExpress ?? 0) + (typePayment.amount - com));
										typePayment.amount = com;
									}
								}
							}
						}
					}

					//Se elimina IVA
					if (salesShared != null)
					{
						foreach (var sale in salesShared)
						{
							if (!(bool)sale.Omitir && !(bool)sale.Compartida)
							{
								foreach (var typePayment in sale.listTypePayment)
								{
									//Si tiene IVA se le resta
									if (sale.IVA > 0)
									{
										sale.ImportePagadoCliente += ((typePayment.amount / 116) * 100);
									}
									else
									{
										sale.ImportePagadoCliente += typePayment.amount;
									}
								};

								//Quitar Total Pagado con Nota de Crédito        
								foreach (var typePayment in sale.listTypePayment)
								{
									//No es crédito y está saldada
									if (typePayment.typesPayment == 8 && typePayment.Estatus == 1)
									{
										if (sale.ImportePagadoCliente <= typePayment.amount)
										{
											sale.ImportePagadoCliente -= sale.ImportePagadoCliente;
										}
										else
										{
											//Si tiene IVA se le resta
											if (sale.IVA > 0)
											{
												sale.ImportePagadoCliente -= ((typePayment.amount / 116) * 100);
											}
											else
											{
												sale.ImportePagadoCliente -= typePayment.amount;
											}
										}
									}
									else if (typePayment.typesPayment == 2)
									{
										//Si es crédito se obtiene el historial 
										if (typePayment.HistoryCredit != null)
										{
											foreach (var history in typePayment.HistoryCredit)
											{
												//Está saldado
												if (history.idFormaPago == 8 && history.Estatus == 1)
												{
													sale.ImportePagadoCliente -= history.Cantidad;
												}
											};
										}
									}
								};
							}
						}
					}

					#endregion Ventas Compartidas

					if (salesShared != null)
					{
						foreach (var cant in salesShared.ToList())
						{
							sales.Add(cant);
						}
					}

					return (decimal)sales?.Sum(p => p.ImportePagadoCliente);
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<SaleViewModel> GetPendingSales()
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tVentas.Where(p => p.Estatus == TypesSales.ventaPendiente).Select(p => new SaleViewModel()
					{

						idVenta = p.idVenta,
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
						TipoCliente = p.TipoCliente,
						idSucursal = p.idSucursal,
						Sucursal = p.tSucursale.Nombre,
						Fecha = p.Fecha,
						CantidadProductos = p.CantidadProductos,
						Subtotal = p.Subtotal,
						Descuento = p.Descuento,
						Remision = p.Remision,
						IVA = p.IVA,
						Total = p.Total
					}).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		/// <summary>
		/// Gets the payment by sale.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public List<PaymentEntryViewModel> GetPaymentBySale(int id)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tFormaPagoes.Where(p => p.idVenta == id && p.Estatus == TypesSales.ventaPendiente && p.Entrada != true && (p.FormaPago == TypesPayment.iCheque || p.FormaPago == TypesPayment.iEfectivo)).Select(p => new PaymentEntryViewModel()
					{
						idPayment = (int)p.idFormaPago,
						Amount = (decimal)p.Cantidad,
						Direct = true,
						Selected = false
					}).Union(context.tHistorialCreditoes.Where(p => p.idVenta == id && p.Estatus == TypesSales.ventaPendiente && p.Entrada != true).Select(p => new PaymentEntryViewModel()
					{
						idPayment = (int)p.idHistorialCredito,
						Amount = (decimal)p.Cantidad,
						Direct = false,
						Selected = false
					})).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		/// <summary>
		/// Gets the payment by entry.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public List<PaymentEntryViewModel> GetPaymentByEntry(int idEntry)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tFormaPagoes.Where(p => p.idEntrada == idEntry && (p.FormaPago == TypesPayment.iCheque || p.FormaPago == TypesPayment.iEfectivo)).Select(p => new PaymentEntryViewModel()
					{
						idPayment = (int)p.idFormaPago,
						Amount = (decimal)p.Cantidad,
						Direct = true,
						Selected = false
					}).Union(context.tHistorialCreditoes.Where(p => p.idEntrada == idEntry).Select(p => new PaymentEntryViewModel()
					{
						idPayment = (int)p.idHistorialCredito,
						Amount = (decimal)p.Cantidad,
						Direct = false,
						Selected = false
					})).ToList();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		/// <summary>
		/// Updates the payment entry.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="payments">The payments.</param>
		public void UpdatePaymentEntry(int idEntry, List<PaymentEntryViewModel> payments)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					foreach (var payment in payments)
					{
						if (payment.Direct)
						{
							var entity = context.tFormaPagoes.Where(p => p.idFormaPago == payment.idPayment).FirstOrDefault();
							if (entity != null)
							{
								entity.idCuenta = CashAccount;
								entity.idEntrada = idEntry;
								entity.Entrada = true;
								context.SaveChanges();
							}
						}
						else
						{
							var entity = context.tHistorialCreditoes.Where(p => p.idHistorialCredito == payment.idPayment).FirstOrDefault();
							if (entity != null)
							{
								entity.idCuenta = CashAccount;
								entity.idEntrada = idEntry;
								entity.Entrada = true;
								context.SaveChanges();
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

		/// <summary>
		/// Updates the payment entry.
		/// </summary>
		/// <param name="idEntry">The identifier entry.</param>
		/// <param name="payments">The payments.</param>
		public void DeletePaymentEntry(int idEntry, List<PaymentEntryViewModel> payments)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					foreach (var payment in payments)
					{
						if (payment.Direct)
						{
							var entity = context.tFormaPagoes.Where(p => p.idEntrada == idEntry && p.idFormaPago == payment.idPayment).FirstOrDefault();
							if (entity != null)
							{
								entity.idEntrada = null;
								entity.Entrada = false;
								context.SaveChanges();
							}
						}
						else
						{
							var entity = context.tHistorialCreditoes.Where(p => p.idEntrada == idEntry && p.idHistorialCredito == payment.idPayment).FirstOrDefault();
							if (entity != null)
							{
								entity.idEntrada = null;
								entity.Entrada = false;
								context.SaveChanges();
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

		/// <summary>
		/// Sets the status payment by entry.
		/// </summary>
		/// <param name="idEntry">The identifier entry.</param>
		public void SetStatusPaymentByEntry(int idEntry)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					int? sale = null;

					var payments = context.tFormaPagoes.Where(p => p.idEntrada == idEntry && p.Estatus == TypesSales.ventaPendiente && p.Entrada == true).ToList();

					if (payments.Count > 0)
					{
						sale = payments.Select(p => p.idVenta).FirstOrDefault();

						foreach (var payment in payments)
						{
							payment.Estatus = TypesPayment.iSaldada;
						}

						context.SaveChanges();
					}

					var creditPayments = context.tHistorialCreditoes.Where(p => p.idEntrada == idEntry && p.Estatus == TypesSales.ventaPendiente && p.Entrada == true).ToList();

					if (creditPayments.Count > 0)
					{
						sale = creditPayments.Select(p => p.idVenta).FirstOrDefault();

						foreach (var payment in creditPayments)
						{
							payment.Estatus = TypesPayment.iSaldada;
						}

						context.SaveChanges();

						//Si ya no existe credito pendiente
						var idSale = creditPayments.Select(p => p.idVenta).FirstOrDefault();

						List<CreditHistoryViewModel> lHistory = this.GetCreditHistory((int)idSale);

						var leftPayment = this.GetLeftoverCreditHistory((int)idSale) - lHistory.Sum(p => p.Cantidad);

						if ((!context.tHistorialCreditoes.Where(p => p.idVenta == idSale && p.Estatus == TypesSales.ventaPendiente).Any()) && (leftPayment <= 0))
						{
							tFormaPago oForma = context.tFormaPagoes.FirstOrDefault(p => p.idVenta == idSale && p.FormaPago == TypesPayment.iCredito);
							oForma.Estatus = TypesPayment.iSaldada;

							context.SaveChanges();
						}
					}

					if (sale > 0)
					{
						//Si no existen pagos pendientes la venta es Saldada
						if (!(context.tFormaPagoes.Any(p => (p.idVenta == sale) && (p.Estatus == TypesSales.ventaPendiente)) || context.tHistorialCreditoes.Any(p => (p.idVenta == sale) && (p.Estatus == TypesSales.ventaPendiente))))
						{
							var entity = context.tVentas.Where(p => p.idVenta == sale).FirstOrDefault();
							entity.Estatus = TypesSales.ventaSaldada;
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
		
	}
}

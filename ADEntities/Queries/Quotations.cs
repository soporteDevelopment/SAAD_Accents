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
	public class Quotations : Base
	{
		public int CountRegisters(string number, string costumer, int? iduserforsearch, string project, int idUser, short? restricted, short? amazonas, short? guadalquivir, short? textura)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					return context.tCotizacions.Where(
						p => (p.Proyecto.ToUpper().Contains(project.ToUpper()) || String.IsNullOrEmpty(project)) &&
							 (p.Numero.ToUpper().Contains(number.ToUpper()) || String.IsNullOrEmpty(number)) &&
						((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null) &&
						(((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())) ||
						((p.tClientesMorale.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
						((p.tDespacho.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))))) &&
						(((p.tSucursale.idSucursal == amazonas) || (p.tSucursale.idSucursal == guadalquivir) || (p.tSucursale.idSucursal == textura)) ||
						(amazonas == null && guadalquivir == null && textura == null)
						&& (p.Estatus == TypesQuotations.activo))).Count();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<QuotationViewModel> GetQuotations(bool allTime, DateTime dtDateSince, DateTime dtDateUntil, string number, string costumer, int? iduserforsearch, string project, int idUser, short? restricted, short? amazonas, short? guadalquivir, short? textura, int page, int pageSize)
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
					IOrderedQueryable<QuotationViewModel> quotations;

					quotations = context.tCotizacions.Where(
					p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) &&
					(p.Proyecto.ToUpper().Contains(project.ToUpper()) || String.IsNullOrEmpty(project)) &&
					(p.Numero.ToUpper().Contains(number.ToUpper()) || String.IsNullOrEmpty(number)) &&
					((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null) &&
					(((p.tSucursale.idSucursal == amazonas) ||
					(p.tSucursale.idSucursal == guadalquivir) ||
					(p.tSucursale.idSucursal == textura)) || (amazonas == null && guadalquivir == null && textura == null) &&
					(((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())) ||
					((p.tClientesMorale.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
					((p.tDespacho.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())
					)))))
					&& (p.Estatus == TypesQuotations.activo)


				).Select(p => new QuotationViewModel()
				{

					idCotizacion = p.idCotizacion,
					Numero = p.Numero,
					idUsuario1 = p.idUsuario1,
					Usuario1 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
					idUsuario2 = p.idUsuario2,
					Usuario2 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
					idClienteFisico = p.idClienteFisico,
					ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
					idClienteMoral = p.idClienteMoral,
					ClienteMoral = p.tClientesMorale.Nombre,
					idDespacho = p.idDespacho,
					Proyecto = p.Proyecto,
					idDespachoReferencia = p.idDespachoReferencia,
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
					Dolar = p.Dolar,
					idTipoCotizacion = p.idTipoCotizacion

				}).OrderByDescending(p => p.Fecha);

					List<QuotationViewModel> lQuotations = quotations.Skip(page * pageSize).Take(pageSize).ToList();

					if (restricted == 1)
					{
						lQuotations = lQuotations.Where(p => p.idUsuario1 == idUser).ToList();
					}

					return lQuotations;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		/// <summary>
		/// Gets the quotations.
		/// </summary>
		/// <param name="costumer">The costumer.</param>
		/// <param name="iduserforsearch">The iduserforsearch.</param>
		/// <param name="project">The project.</param>
		/// <param name="idUser">The identifier user.</param>
		/// <param name="restricted">The restricted.</param>
		/// <param name="amazonas">The amazonas.</param>
		/// <param name="guadalquivir">The guadalquivir.</param>
		/// <param name="textura">The textura.</param>
		/// <param name="dollar">if set to <c>true</c> [dollar].</param>
		/// <param name="page">The page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
		public Tuple<List<QuotationViewModel>, int> GetQuotations(string costumer, int? iduserforsearch, string project, int idUser, short? restricted, short? amazonas, short? guadalquivir, short? textura, bool dollar, int page, int pageSize)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var quotations = context.tCotizacions.Where(
						p => (p.Proyecto.ToUpper().Contains(project.ToUpper()) || String.IsNullOrEmpty(project)) &&
						((p.idUsuario1 == iduserforsearch || p.idUsuario2 == iduserforsearch) || iduserforsearch == null) &&
						(((p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim())) ||
						((p.tClientesMorale.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))) ||
						((p.tDespacho.Nombre).ToUpper().Contains(costumer.ToUpper().Trim()) || (String.IsNullOrEmpty(costumer.ToUpper().Trim()))))) &&
						(((p.tSucursale.idSucursal == amazonas) || (p.tSucursale.idSucursal == guadalquivir) || (p.tSucursale.idSucursal == textura)) ||
						(amazonas == null && guadalquivir == null && textura == null)
						&& (p.Estatus == TypesQuotations.activo)
						&& (p.Dolar == dollar))
					).Select(p => new QuotationViewModel()
					{
						idCotizacion = p.idCotizacion,
						Numero = p.Numero,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idClienteFisico = p.idClienteFisico,
						ClienteFisico = p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos,
						idClienteMoral = p.idClienteMoral,
						ClienteMoral = p.tClientesMorale.Nombre,
						idDespacho = p.idDespacho,
						Proyecto = p.Proyecto,
						idDespachoReferencia = p.idDespachoReferencia,
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
						Dolar = p.Dolar,
						idTipoCotizacion = p.idTipoCotizacion
					}).OrderByDescending(p => p.Fecha);

					var total = quotations.Count();

					List<QuotationViewModel> lQuotations = quotations.Skip(page * pageSize).Take(pageSize).ToList();

					if (restricted == 1)
					{
						lQuotations = lQuotations.Where(p => p.idUsuario1 == idUser).ToList();
					}

					return new Tuple<List<QuotationViewModel>, int>(lQuotations, total);
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int AddQuotationDollar(string number, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
			string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oQuotation = new tCotizacion();

					oQuotation.Numero = number;
					oQuotation.idUsuario1 = idUser1;
					oQuotation.idUsuario2 = idUser2;
					oQuotation.idClienteFisico = idCustomerP;
					oQuotation.idClienteMoral = idCustomerM;
					oQuotation.idDespacho = idOffice;
					oQuotation.Proyecto = project;
					oQuotation.idDespachoReferencia = idOfficeReference;
					oQuotation.idSucursal = idBranch;
					oQuotation.Fecha = DateTime.Now;//Convert.ToDateTime(dateSale);
					oQuotation.CantidadProductos = amountProducts;
					oQuotation.Subtotal = subtotal;
					oQuotation.Descuento = discount;
					oQuotation.IVA = IVA;
					oQuotation.Total = total;
					oQuotation.Comentarios = comments;
					oQuotation.TipoCliente = typeCustomer;
					oQuotation.Estatus = TypesQuotations.activo;
					oQuotation.Dolar = true;
					oQuotation.idTipoCotizacion = (int)QuotationType.Regular;

					context.tCotizacions.Add(oQuotation);

					context.SaveChanges();

					foreach (var prod in lProducts)
					{
						this.AddQuotationProductSale(oQuotation.idCotizacion, prod);
					}

					return oQuotation.idCotizacion;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int AddQuotation(string number, int idUser1, int? idUser2, int? origin, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
			string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, decimal IVATasa)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oQuotation = new tCotizacion();

					oQuotation.Numero = number;
					oQuotation.idUsuario1 = idUser1;
					oQuotation.idUsuario2 = idUser2;
					oQuotation.idClienteFisico = idCustomerP;
					oQuotation.idClienteMoral = idCustomerM;
					oQuotation.idDespacho = idOffice;
					oQuotation.Proyecto = project;
					oQuotation.idDespachoReferencia = idOfficeReference;
					oQuotation.idSucursal = idBranch;
					oQuotation.Fecha = DateTime.Now;//Convert.ToDateTime(dateSale);
					oQuotation.CantidadProductos = amountProducts;
					oQuotation.Subtotal = subtotal;
					oQuotation.Descuento = discount;
					oQuotation.IVA = IVA;
					oQuotation.Total = total;
					oQuotation.Comentarios = comments;
					oQuotation.TipoCliente = typeCustomer;
					oQuotation.Estatus = TypesQuotations.activo;
					oQuotation.Dolar = false;
					oQuotation.idTipoCotizacion = (int)QuotationType.Regular;
					oQuotation.IVATasa = IVATasa;
					context.tCotizacions.Add(oQuotation);

					context.SaveChanges();

					foreach (var prod in lProducts)
					{
						prod.idSucursal = idBranch;
						this.AddQuotationProductSale(oQuotation.idCotizacion, prod);
					}

					return oQuotation.idCotizacion;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int AddQuotationFromView(string number, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
		   string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal IVATasa, decimal total, List<ProductSaleViewModel> lProducts, string comments, int? idView)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oQuotation = new tCotizacion();

					oQuotation.Numero = number;
					oQuotation.idUsuario1 = idUser1;
					oQuotation.idUsuario2 = idUser2;
					oQuotation.idClienteFisico = idCustomerP;
					oQuotation.idClienteMoral = idCustomerM;
					oQuotation.idDespacho = idOffice;
					oQuotation.Proyecto = project;
					oQuotation.idDespachoReferencia = idOfficeReference;
					oQuotation.idSucursal = idBranch;
					oQuotation.Fecha = DateTime.Now;//Convert.ToDateTime(dateSale);
					oQuotation.CantidadProductos = amountProducts;
					oQuotation.Subtotal = subtotal;
					oQuotation.Descuento = discount;
					oQuotation.IVA = IVA;
					oQuotation.IVATasa = IVATasa;
					oQuotation.Total = total;
					oQuotation.Comentarios = comments;
					oQuotation.TipoCliente = typeCustomer;
					oQuotation.Estatus = TypesQuotations.activo;
					oQuotation.Dolar = false;
					oQuotation.idVista = idView;
					oQuotation.idTipoCotizacion = (int)QuotationType.View;

					context.tCotizacions.Add(oQuotation);

					context.SaveChanges();

					//Verificar si producto pertenece a Salida a Vista
					var products = context.tDetalleVistas.Where(p => p.idVista == idView && p.Estatus == (int)StatusViewDetail.Pending).Select(p => p.idProducto).ToList();

					foreach (var prod in lProducts)
					{
						if (products.Any(p => p == prod.idProducto))
						{
							prod.idVista = idView;
						}
						else
						{
							prod.idVista = null;
						}

						this.AddQuotationProductSale(oQuotation.idCotizacion, prod);
					}

					return oQuotation.idCotizacion;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void UpdateQuotationFromView(int idQuotation, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer,
			int? idOfficeReference, int idBranch, string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts,
			string comments, int idView, decimal IVATasa)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oCotizacion = context.tCotizacions.FirstOrDefault(p => p.idCotizacion == idQuotation);

					oCotizacion.idUsuario1 = idUser1;
					oCotizacion.idUsuario2 = idUser2;
					oCotizacion.idClienteFisico = idCustomerP;
					oCotizacion.idClienteMoral = idCustomerM;
					oCotizacion.idDespacho = idOffice;
					oCotizacion.Proyecto = project;
					oCotizacion.idDespachoReferencia = idOfficeReference;
					oCotizacion.idSucursal = idBranch;
					oCotizacion.Fecha = Convert.ToDateTime(dateSale);
					oCotizacion.CantidadProductos = amountProducts;
					oCotizacion.Subtotal = subtotal;
					oCotizacion.Descuento = discount;
					oCotizacion.IVA = IVA;
					oCotizacion.IVATasa = IVATasa;
					oCotizacion.Total = total;
					oCotizacion.TipoCliente = typeCustomer;
					oCotizacion.Comentarios = comments;

					context.SaveChanges();

					this.DeleteDetailQuotation(idQuotation);

					//Verificar si producto pertenece a Salida a Vista
					var products = context.tDetalleVistas.Where(p => p.idVista == idView && p.Estatus == (int)StatusViewDetail.Pending).Select(p => p.idProducto).ToList();

					foreach (var prod in lProducts)
					{
						if (products.Any(p => p == prod.idProducto))
						{
							prod.idVista = idView;
						}
						else
						{
							prod.idVista = null;
						}

						this.AddQuotationProductSale(oCotizacion.idCotizacion, prod);
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int AddQuotationFromUnifiedView(string number, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
		  string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, decimal IVATasa)
		{

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oQuotation = new tCotizacion();

					oQuotation.Numero = number;
					oQuotation.idUsuario1 = idUser1;
					oQuotation.idUsuario2 = idUser2;
					oQuotation.idClienteFisico = idCustomerP;
					oQuotation.idClienteMoral = idCustomerM;
					oQuotation.idDespacho = idOffice;
					oQuotation.Proyecto = project;
					oQuotation.idDespachoReferencia = idOfficeReference;
					oQuotation.idSucursal = idBranch;
					oQuotation.Fecha = DateTime.Now;//Convert.ToDateTime(dateSale);
					oQuotation.CantidadProductos = amountProducts;
					oQuotation.Subtotal = subtotal;
					oQuotation.Descuento = discount;
					oQuotation.IVA = IVA;
					oQuotation.IVATasa = IVATasa;
					oQuotation.Total = total;
					oQuotation.Comentarios = comments;
					oQuotation.TipoCliente = typeCustomer;
					oQuotation.Estatus = TypesQuotations.activo;
					oQuotation.Dolar = false;
					oQuotation.idVista = null;
					oQuotation.idTipoCotizacion = (int)QuotationType.UnifiedView;

					context.tCotizacions.Add(oQuotation);

					context.SaveChanges();

					foreach (var prod in lProducts)
					{
						this.AddQuotationProductSale(oQuotation.idCotizacion, prod);
					}

					return oQuotation.idCotizacion;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void UpdateQuotationFromUnifiedView(int idQuotation, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer, int? idOfficeReference, int idBranch,
			string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA, decimal total, List<ProductSaleViewModel> lProducts, string comments, decimal IVATasa)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oCotizacion = context.tCotizacions.FirstOrDefault(p => p.idCotizacion == idQuotation);

					oCotizacion.idUsuario1 = idUser1;
					oCotizacion.idUsuario2 = idUser2;
					oCotizacion.idClienteFisico = idCustomerP;
					oCotizacion.idClienteMoral = idCustomerM;
					oCotizacion.idDespacho = idOffice;
					oCotizacion.Proyecto = project;
					oCotizacion.idDespachoReferencia = idOfficeReference;
					oCotizacion.idSucursal = idBranch;
					oCotizacion.Fecha = Convert.ToDateTime(dateSale);
					oCotizacion.CantidadProductos = amountProducts;
					oCotizacion.Subtotal = subtotal;
					oCotizacion.Descuento = discount;
					oCotizacion.IVA = IVA;
					oCotizacion.IVATasa = IVATasa;
					oCotizacion.Total = total;
					oCotizacion.TipoCliente = typeCustomer;
					oCotizacion.Comentarios = comments;

					context.SaveChanges();

					this.DeleteDetailQuotation(idQuotation);

					foreach (var prod in lProducts)
					{

						this.AddQuotationProductSale(oCotizacion.idCotizacion, prod);
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void AddQuotationProductSale(int idQuotation, ProductSaleViewModel oProduct)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacionDetalle oDetalleCotizacion = new tCotizacionDetalle();

					oDetalleCotizacion.idCotizacion = idQuotation;

					if (oProduct.Tipo == 2 && !String.IsNullOrEmpty(oProduct.imagen))
					{
						char delimiter = '/';
						var imagen = oProduct.imagen.Split(delimiter);
						oDetalleCotizacion.Imagen = imagen[3];
					}

					oDetalleCotizacion.idProducto = oProduct.idProducto;
					oDetalleCotizacion.idServicio = oProduct.idServicio;
					oDetalleCotizacion.idCredito = oProduct.idCredito;
					oDetalleCotizacion.Descripcion = oProduct.desc;
					oDetalleCotizacion.Precio = oProduct.prec;
					oDetalleCotizacion.Descuento = oProduct.descuento;
					oDetalleCotizacion.Cantidad = oProduct.cantidad;
					oDetalleCotizacion.Comentarios = oProduct.comentarios;
					oDetalleCotizacion.idSucursal = oProduct.idSucursal;
					oDetalleCotizacion.idVista = oProduct.idVista;
					oDetalleCotizacion.idPromocion = oProduct.idPromocion;
					oDetalleCotizacion.CostoPromocion = oProduct.CostoPromocion;
					oDetalleCotizacion.idTipoPromocion = oProduct.idTipoPromocion;
					oDetalleCotizacion.idProductoPadre = oProduct.idProductoPadre;

					context.tCotizacionDetalles.Add(oDetalleCotizacion);

					context.SaveChanges();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public QuotationViewModel GetQuotationId(int idQuotation)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var eraserSale = context.tCotizacions.Where(p => p.idCotizacion == idQuotation).Select(p => new QuotationViewModel()
					{
						Numero = p.Numero,
						idCotizacion = p.idCotizacion,
						idUsuario1 = p.idUsuario1,
						Usuario1 = p.tUsuario1.Nombre + " " + p.tUsuario1.Apellidos,
						idUsuario2 = p.idUsuario2,
						Usuario2 = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
						idClienteFisico = p.idClienteFisico,
						idClienteMoral = p.idClienteMoral,
						ClienteFisico = (p.idClienteFisico != null) ? p.tClientesFisico.Nombre + " " + p.tClientesFisico.Apellidos : p.tClientesMorale.Nombre,
						idDespacho = p.idDespacho,
						Proyecto = p.Proyecto,
						idDespachoReferencia = p.idDespachoReferencia,
						Despacho = p.tDespacho.Nombre,
						DespachoReferencia = p.tDespacho1.Nombre,
						idSucursal = p.idSucursal,
						Sucursal = p.tSucursale.Nombre,
						Fecha = p.Fecha,
						CantidadProductos = p.CantidadProductos,
						Subtotal = p.Subtotal,
						Descuento = p.Descuento,
						IVA = p.IVA,
						IVATasa = p.IVATasa,
						Total = p.Total,
						TipoCliente = p.TipoCliente,
						Comentarios = p.Comentarios,
						idTipoCotizacion = p.idTipoCotizacion,
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
						oDetail = p.tCotizacionDetalles.Where(x => x.idCotizacion == p.idCotizacion).Select(x => new DetailQuotationViewModel()
						{
							idDetalleCotizacion = x.idCotizacionDetalle,
							idCotizacion = x.idCotizacion,
							idProducto = x.idProducto,
							Codigo = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo,
							idServicio = x.idServicio,
							idCredito = x.idCredito,
							Credito = x.tNotasCredito.Remision,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento,
							Cantidad = x.Cantidad,
							TipoImagen = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().TipoImagen,
							Imagen = x.Imagen,
							Comentarios = x.Comentarios,
							idSucursal = x.idSucursal,
							idPromocion = x.idPromocion,
							CostoPromocion = x.CostoPromocion,
							idTipoPromocion = x.idTipoPromocion,
							idProductoPadre = x.idProductoPadre,
							Sucursal = (x.idSucursal == TypesBranch.Amazonas) ? "Amazonas" : (x.idSucursal == TypesBranch.Guadalquivir ? "Guadalquivir" : "Texturas"),
							idVista = x.idVista,
							Remision = context.tVistas.Where(o => o.idVista == x.idVista).Select(y => y.Remision).FirstOrDefault(),
							oProducto = context.tProductos.Where(o => o.idProducto == x.idProducto).Select(o => new ProductViewModel()
							{
								idProducto = o.idProducto,
								urlImagen = o.TipoImagen == 1 ? "/Content/Products/" + o.NombreImagen + o.Extension : o.urlImagen,
								idProveedor = o.idProveedor,
								NombreImagen = o.NombreImagen + o.Extension,
								Extension = o.Extension
							}).FirstOrDefault()
						}).ToList(),
						SumSubtotal = p.tCotizacionDetalles.Where(x => x.idCotizacion == p.idCotizacion).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum(),
						Dolar = p.Dolar,
						idVista = p.idVista,
					}).FirstOrDefault();

					return eraserSale;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void UpdateQuotationProductSale(int idQuotation, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer,
			int? idOfficeReference, int idBranch, string dateSale, int amountProducts, decimal subtotal, decimal discount, short IVA,
			decimal total, List<ProductSaleViewModel> lProducts, string comments, decimal IVATasa)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oCotizacion = context.tCotizacions.FirstOrDefault(p => p.idCotizacion == idQuotation);

					oCotizacion.idUsuario1 = idUser1;
					oCotizacion.idUsuario2 = idUser2;
					oCotizacion.idClienteFisico = idCustomerP;
					oCotizacion.idClienteMoral = idCustomerM;
					oCotizacion.idDespacho = idOffice;
					oCotizacion.Proyecto = project;
					oCotizacion.idDespachoReferencia = idOfficeReference;
					oCotizacion.idSucursal = idBranch;
					oCotizacion.Fecha = Convert.ToDateTime(dateSale);
					oCotizacion.CantidadProductos = amountProducts;
					oCotizacion.Subtotal = subtotal;
					oCotizacion.Descuento = discount;
					oCotizacion.IVA = IVA;
					oCotizacion.IVATasa = IVATasa;
					oCotizacion.Total = total;
					oCotizacion.TipoCliente = typeCustomer;
					oCotizacion.Comentarios = comments;

					context.SaveChanges();

					this.DeleteDetailQuotation(idQuotation);

					foreach (var prod in lProducts)
					{
						this.AddQuotationProductSale(oCotizacion.idCotizacion, prod);
					}

				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void DeleteDetailQuotation(int idQuotation)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					//Borrar los registros de la tabla tBorradorCotizacionDetalle
					var oDeatil = context.tCotizacionDetalles.Where(p => p.idCotizacion == idQuotation).ToList();

					foreach (var detail in oDeatil)
					{
						context.tCotizacionDetalles.Remove(detail);
						context.SaveChanges();
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public void DeleteQuotation(int idQuotation)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var oSale = context.tCotizacions.FirstOrDefault(p => p.idCotizacion == idQuotation);

					if (oSale != null)
					{
						oSale.Estatus = 0;
						context.SaveChanges();
					}
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int GetNumberQuotationsForSeller(DateTime dtDateSince, DateTime dtDateUntil, int idSeller)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
					var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

					return context.tCotizacions.Where(
					   p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) && p.idUsuario1 == idSeller
					).Count();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public int GetQuotationsToSaleForSeller(DateTime dtDateSince, DateTime dtDateUntil, int idSeller)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
					var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

					return context.tCotizacions.Join(context.tVentas, q => q.idCotizacion, v => v.idCotizacion,
						(q, v) => new { Quotations = q, Sales = v }).Where(qs => (qs.Quotations.Fecha >= dtStart && qs.Quotations.Fecha <= dtEnd) && qs.Quotations.idUsuario1 == idSeller)
						.Count();
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public decimal GetQuotationsSaled(DateTime dtDateSince, DateTime dtDateUntil, int idSeller)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
					var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

					return context.tCotizacions.Join(context.tVentas, q => q.idCotizacion, v => v.idCotizacion,
						(q, v) => new { Quotations = q, Sales = v }).Where(qs => (qs.Quotations.Fecha >= dtStart && qs.Quotations.Fecha <= dtEnd) && qs.Quotations.idUsuario1 == idSeller)
						.Select(p => p.Sales.Total).Sum() ?? 0;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public List<QuotationViewModel> GetQuotationsForSeller(DateTime dtDateSince, DateTime dtDateUntil, int idSeller)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var dtStart = dtDateSince.Date + new TimeSpan(0, 0, 0);
					var dtEnd = dtDateUntil.Date + new TimeSpan(23, 59, 59);

					return context.tCotizacions.Where(
					   p => (p.Fecha >= dtStart && p.Fecha <= dtEnd) && p.idUsuario1 == idSeller
					).Select(p => new QuotationViewModel()
					{

						idCotizacion = p.idCotizacion,
						Numero = p.Numero,
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
						oDetail = p.tCotizacionDetalles.Where(x => x.idCotizacion == p.idCotizacion).Select(x => new DetailQuotationViewModel()
						{

							idDetalleCotizacion = x.idCotizacionDetalle,
							idCotizacion = x.idCotizacion,
							idProducto = x.idProducto,
							Codigo = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().Codigo,
							idServicio = x.idServicio,
							idCredito = x.idCredito,
							Credito = x.tNotasCredito.Remision,
							Descripcion = x.Descripcion,
							Precio = x.Precio,
							Descuento = x.Descuento,
							Cantidad = x.Cantidad,
							TipoImagen = context.tProductos.Where(o => o.idProducto == x.idProducto).FirstOrDefault().TipoImagen,
							Imagen = x.Imagen,
							Comentarios = x.Comentarios,
							idSucursal = x.idSucursal,
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
						SumSubtotal = p.tCotizacionDetalles.Where(x => x.idCotizacion == p.idCotizacion).Select(x => ((x.Cantidad ?? 0) * (x.Precio ?? 0))).Sum(),
						Dolar = p.Dolar,
						idTipoCotizacion = p.idTipoCotizacion
					}).OrderByDescending(p => p.Fecha).ToList();

				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		//Generar número de cotización
		public int GetLastID()
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					var lastId = context.tCotizacions.Max(p => (int?)p.idCotizacion) ?? 0;

					return lastId;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}

		public string GeneratePrevNumberRem()
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					string month = DateTime.Now.ToString("MM");
					string year = DateTime.Now.ToString("yy");

					string ID = (this.GetLastID() + 1).ToString();
					char pad = '0';

					return string.Concat(month, string.Concat(string.Concat(year + "-", ID.PadLeft(4, pad)), "COT"));
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}


		/// <summary>Gets the branch information.</summary>
		/// <param name="IDBranch">The identifier branch.</param>
		/// <returns>
		///   <br />
		/// </returns>
		public decimal GetBranchInfo(int IDBranch)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					decimal iva = 0;
					tSucursale oSucursal = context.tSucursales.FirstOrDefault(p => p.idSucursal == IDBranch);
					iva = oSucursal.IVATasa;
					return iva;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}
		public int DuplicateQuotation(int idQuotation)
		{
			var quotation = GetQuotationId(idQuotation);
			var num = GeneratePrevNumberRem();

			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oQuotation = new tCotizacion();

					oQuotation.Numero = num;
					oQuotation.idUsuario1 = quotation.idUsuario1;
					oQuotation.idUsuario2 = quotation.idUsuario2;
					oQuotation.idClienteFisico = quotation.idClienteFisico;
					oQuotation.idClienteMoral = quotation.idClienteMoral;
					oQuotation.idDespacho = quotation.idDespacho;
					oQuotation.Proyecto = quotation.Proyecto;
					oQuotation.idDespachoReferencia = quotation.idDespachoReferencia;
					oQuotation.idSucursal = quotation.idSucursal;
					oQuotation.Fecha = DateTime.Now;
					oQuotation.CantidadProductos = quotation.CantidadProductos;
					oQuotation.Subtotal = quotation.Subtotal;
					oQuotation.Descuento = quotation.Descuento;
					oQuotation.IVA = quotation.IVA;
					oQuotation.Total = quotation.Total;
					oQuotation.Comentarios = quotation.Comentarios;
					oQuotation.TipoCliente = quotation.TipoCliente;
					oQuotation.Estatus = TypesQuotations.activo;
					oQuotation.Dolar = false;
					oQuotation.idTipoCotizacion = (int)QuotationType.Regular;
					oQuotation.IVATasa = quotation.IVATasa;
					context.tCotizacions.Add(oQuotation);

					context.SaveChanges();

					foreach (var prod in quotation.oDetail)
					{
						prod.idSucursal = quotation.idSucursal;

						this.AddQuotationProductSale(oQuotation.idCotizacion, new ProductSaleViewModel()
						{
							idProducto = prod.idProducto,
							codigo = prod.Codigo,
							imagen = prod.Imagen,
							desc = prod.Descripcion,
							prec = (decimal)prod.Precio,
							descuento = (decimal)prod.Descuento,
							cantidad = prod.Cantidad,
							idServicio = prod.idServicio,
							comentarios = prod.Comentarios,
							Tipo = prod.Tipo,
							idSucursal = prod.idSucursal,
							idVista = prod.idVista,
							idCotizacion = prod.idCotizacion,
							idPromocion = prod.idPromocion,
							CostoPromocion = prod.CostoPromocion,
							idTipoPromocion = prod.idTipoPromocion,
							idProductoPadre = prod.idProductoPadre
						});
					}

					return oQuotation.idCotizacion;
				}
				catch (DbEntityValidationException ex)
				{
					var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
					throw newException;
				}
		}
		public void UpdateQuotationDollar(int idQuotation, int idUser1, int? idUser2, int? idCustomerP, int? idCustomerM, int? idOffice, string project, byte typeCustomer,
			int? idOfficeReference, int idBranch, string dateSale, int amountProducts, decimal subtotal, decimal discount,
			decimal total, List<ProductSaleViewModel> lProducts, string comments)
		{
			using (var context = new admDB_SAADDBEntities())
				try
				{
					tCotizacion oCotizacion = context.tCotizacions.FirstOrDefault(p => p.idCotizacion == idQuotation);

					oCotizacion.idUsuario1 = idUser1;
					oCotizacion.idUsuario2 = idUser2;
					oCotizacion.idClienteFisico = idCustomerP;
					oCotizacion.idClienteMoral = idCustomerM;
					oCotizacion.idDespacho = idOffice;
					oCotizacion.Proyecto = project;
					oCotizacion.idDespachoReferencia = idOfficeReference;
					oCotizacion.idSucursal = idBranch;
					oCotizacion.Fecha = Convert.ToDateTime(dateSale);
					oCotizacion.CantidadProductos = amountProducts;
					oCotizacion.Subtotal = subtotal;
					oCotizacion.Descuento = discount;
					oCotizacion.Total = total;
					oCotizacion.TipoCliente = typeCustomer;
					oCotizacion.Comentarios = comments;

					context.SaveChanges();

					this.DeleteDetailQuotation(idQuotation);

					foreach (var prod in lProducts)
					{
						this.AddQuotationProductSale(oCotizacion.idCotizacion, prod);
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

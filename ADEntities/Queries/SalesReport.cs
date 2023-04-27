using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    /// <summary>
    /// SalesReport
    /// </summary>
    public class SalesReport
    {
        /// <summary>
        /// Totals the sales.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<VentasTotales_Result> TotalSales(DateTime startDate, DateTime endDate)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month)).Date + new TimeSpan(23, 59, 59);

                    return context.VentasTotales(startDate, endDate).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
        /// <summary>
        /// Totals the sales by day.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<VentasTotalesPorVendedor_Result> TotalSalesBySeller(int? idSeller, DateTime startDate, DateTime endDate)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month), 23, 59, 59);

                    return context.VentasTotalesPorVendedor(idSeller, startDate, endDate).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
        /// <summary>
        /// Viewses the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="branch">The branch.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<SalidasVista_Result> Views(int? idSeller, DateTime startDate, DateTime endDate, int? idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = endDate.Date + new TimeSpan(23, 59, 59);

                    return context.SalidasVista(idSeller, startDate, endDate, idBranch).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Saleses the type customer.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="branch">The branch.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<VentasTipoCliente_Result> SalesTypeCustomer(int? user, int? branch, DateTime startDate, DateTime endDate)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = endDate.Date + new TimeSpan(23, 59, 59);

                    return context.VentasTipoCliente(user, branch, startDate, endDate).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Cotizaciones a venta
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<CotizacionesVentas_Result> QuotationsSales(DateTime startDate, DateTime endDate)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = endDate.Date + new TimeSpan(23, 59, 59);

                    return context.CotizacionesVentas(startDate, endDate).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// CVentas credito
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<VentasCredito_Result> CreditSales(DateTime startDate, DateTime endDate)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = endDate.Date + new TimeSpan(23, 59, 59);

                    return context.VentasCredito(startDate, endDate).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var error in ex.EntityValidationErrors)
                    {
                        Console.WriteLine("====================");
                        Console.WriteLine("Entity {0} in state {1} has validation errors:",
                            error.Entry.Entity.GetType().Name, error.Entry.State);
                        foreach (var ve in error.ValidationErrors)
                        {
                            Console.WriteLine("\tProperty: {0}, Error: {1}",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                        Console.WriteLine();
                    }
                    throw;
                }
        }

        /// <summary>
        /// Inventario por dia
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<InventarioPorDia_Result> CurrentInventory(int? idProduct, DateTime queryDate)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var startDate = queryDate.Date + new TimeSpan(0, 0, 0);
                    var endDate = queryDate.Date + new TimeSpan(23, 59, 59);

                    return context.InventarioPorDia(idProduct, startDate, endDate).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }catch(Exception ex)
				{
                    throw ex;
                }
        }

        /// <summary>
        /// Ventas por servicio
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<VentasServicios_Result> SalesServices(DateTime startDate, DateTime endDate, Nullable<int> idService)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = endDate.Date + new TimeSpan(23, 59, 59);

                    return context.VentasServicios(startDate, endDate, idService).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Ventas por producto
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<VentasProductos_Result> SalesProduct(DateTime startDate, DateTime endDate, Nullable<int> idCategory, Nullable<int> idSubcategory)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = endDate.Date + new TimeSpan(23, 59, 59);

                    return context.VentasProductos(startDate, endDate, idCategory, idSubcategory).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Fecha de compra de producto
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<ObtenerProductosFechaCompra_Result> ProductsPurchasedDate()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {                 

                    return context.ObtenerProductosFechaCompra().ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        /// <summary>
        /// Origen de los clientes
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public List<ObtenerOrigenClientes_Result> OriginCustomers(DateTime startDate, DateTime endDate)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    startDate = startDate.Date + new TimeSpan(0, 0, 0);
                    endDate = endDate.Date + new TimeSpan(23, 59, 59);

                    return context.ObtenerOrigenClientes(startDate, endDate).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}
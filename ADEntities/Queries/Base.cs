using System;
using ADEntities.Models;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class Base
    {

        protected static short iTRue { get { return 1; } }

        protected static short iFalse { get { return 2; } }

        public void AddRegisterProduct(int idProducto, int idSucursal, string descripcion, 
            decimal inventarioAnterior, decimal inventarioActual, 
            decimal? precioVentaAnterior, decimal? precioVentaNuevo,
            string comentarios, int idUsuario)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var register = new tBitacoraProducto()
                    {
                        idProducto = idProducto,
                        idSucursal = idSucursal,
                        Descripcion = descripcion,
                        InventarioAnterior = inventarioAnterior,
                        InventarioActual = inventarioActual,
                        PrecioVentaAnterior = precioVentaAnterior,
                        PrecioVentaNuevo = precioVentaNuevo,
                        Comentarios = comentarios,
                        idUsuario = idUsuario,
                        Fecha = DateTime.Now
                    };

                    context.tBitacoraProductos.Add(register);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public decimal GetProductStockForBranch(int idProduct, int idBranch)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var product = context.tProductosSucursals.FirstOrDefault(p => p.idProducto == idProduct && p.idSucursal == idBranch);

                    if (product == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return product.Existencia??0;
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
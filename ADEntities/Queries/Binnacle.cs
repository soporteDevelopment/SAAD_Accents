using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class Binnacle : Base
    {
        public int CountRegisters(string code)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tBitacoraProductos.Where(p => p.tProducto.Codigo.Contains(code)).Count();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<BinnacleViewModel> GetBinnacle(string code, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var categories = context.tBitacoraProductos.Where(p => p.tProducto.Codigo == code)
                        .Select(p => new BinnacleViewModel()
                        {
                            idBitacora = p.idBitacoraProductos,
                            idUsuario = p.idUsuario,
                            Usuario = p.tUsuario.Nombre + " " + p.tUsuario.Apellidos,
                            Sucursal = p.tSucursale.Nombre,
                            Descripcion = p.Descripcion,
                            Producto = new ProductViewModel()
                            {
                                idProducto = p.tProducto.idProducto,
                                Codigo = p.tProducto.Codigo,
                                Descripcion = p.tProducto.Descripcion,
                                Proveedor = p.tProducto.tProveedore.Nombre
                            },
                            InventarioAnterior = p.InventarioAnterior,
                            InventarioActual = p.InventarioActual,
                            PrecioVentaAnterior = p.PrecioVentaAnterior,
                            PrecioVentaNuevo = p.PrecioVentaNuevo,
                            Comentarios = p.Comentarios,
                            Fecha = p.Fecha
                        }).OrderBy(p => p.Fecha);

                    return categories.Skip(page * pageSize).Take(pageSize).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}
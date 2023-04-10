using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class ShoppingCarts
    {
        public ViewModels.VirtualStore.ShoppingCartViewModel Get(int idCustomer)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tCarritoes
                    .Where(p => p.idCliente == idCustomer && p.Estatus == StatusShoppingCart.Active)
                    .Select(p => new ViewModels.VirtualStore.ShoppingCartViewModel()
                    {
                        idCarrito = p.idCarrito,
                        idCliente = p.idCliente,
                        Fecha = p.Fecha,
                        CantidadProductos = p.CantidadProductos,
                        Subtotal = p.Subtotal,
                        Descuento = p.Descuento,
                        Total = p.Total,
                        Estatus = p.Estatus,
                        Productos = context.tDetalleCarritoes.Where(q => q.idCarrito == p.idCarrito)
                        .Select(q => new DetailShoppingCartViewModel()
                        {
                            idDetalleCarrito = q.idDetalleCarrito,
                            idCarrito = q.idCarrito,
                            idProducto = q.idProducto,
                            Precio = q.Precio,
                            Cantidad = q.Cantidad,
                            Descuento = q.Descuento,
                            idPromocion = q.idPromocion,
                            Producto = new ProductViewModel()
                            {
                                idProducto = q.tProducto.idProducto,
                                Nombre = q.tProducto.Nombre,
                                Descripcion = q.tProducto.Descripcion,
                                PrecioVenta = q.tProducto.PrecioVenta,
                                PrecioCompra = q.tProducto.PrecioCompra,
                                idProveedor = q.tProducto.idProveedor,
                                idCategoria = q.tProducto.idCategoria,
                                idSubcategoria = q.tProducto.idSubcategoria,
                                Color = q.tProducto.Color,
                                idMaterial = q.tProducto.idMaterial,
                                Medida = q.tProducto.Medida,
                                Peso = q.tProducto.Peso,
                                Codigo = q.tProducto.Codigo,
                                Extension = q.tProducto.Extension,
                                urlImagen = q.tProducto.urlImagen,
                                TipoImagen = q.tProducto.TipoImagen,
                                NombreImagen = q.tProducto.NombreImagen,
                                Comentarios = q.tProducto.Comentarios
                            }
                        }).ToList()
                    }).FirstOrDefault();
            }
        }
    }
}
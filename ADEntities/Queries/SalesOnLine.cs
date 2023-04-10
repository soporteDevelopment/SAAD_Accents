using ADEntities.Models;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class SalesOnLine
    {
        private const string BasePath = "https://www.accentsadmin.com/Content/Products/";

        public SaleOnLineViewModel Get(int id)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return (from s in context.tVentasEnLineas
                        where s.idVenta == id
                        select new SaleOnLineViewModel
                        {
                            IdVenta = s.idVenta,
                            Remision = s.Remision,
                            IdCarrito = s.idCarrito,
                            IdCliente = s.idCliente,
                            Fecha = s.Fecha,
                            CantidadProductos = s.CantidadProductos,
                            Subtotal = s.Subtotal,
                            Descuento = s.Descuento,
                            Total = s.Total,
                            Estatus = s.Estatus,
                            NombreEstatus = (
                                s.Estatus == 1 ? "PENDIENTE POR CONFIRMAR" :
                                s.Estatus == 2 ? "COMPRA CONFIRMADA" :
                                s.Estatus == 3 ? "LISTO PARA RECOGER EN SUCURSAL" :
                                s.Estatus == 4 ? "COMPRA ENVIADA" :
                                s.Estatus == 5 ? "COMPRA ENTREGADA" :
                                s.Estatus == 6 ? "COMPRA CANCELADA" : "ESTATUS INDEFINIDO"
                            ),
                            EstatusOpenPay = s.EstatusOpenPay,
                            IdTipoEntrega = s.idTipoEntrega,
                            TipoEntrega = (s.idTipoEntrega == 1 ? "RECOGER EN SUCURSAL" : "ENVIO A DOMICILIO"),
                            Facturado = s.Facturado,
                            IdVendedor = s.idVendedor,
                            Vendedor = context.tUsuarios.Where(u => u.idUsuario == s.idVendedor).Select(u => new UserViewModel()
                            {
                                NombreCompleto = u.Nombre + " " + u.Apellidos
                            }).FirstOrDefault(),
                            IdSucursal = s.idSucursal,
                            Sucursal = (s.idSucursal == 2 ? "AMAZONAS" :
                                        s.idSucursal == 3 ? "GUADALQUIVIR" :
                                        s.idSucursal == 4 ? "TEXTURA" : "INDEFINIDO"),
                            Cliente = context.tClientesTiendaVirtuals
                                .Where(p => p.idCliente == s.idCliente).Select(p => new CustomerVirtualStoreViewModel()
                                {
                                    IdCliente = p.idCliente,
                                    Nombre = p.Nombre,
                                    Apellidos = p.Apellidos,
                                    Contrasena = p.Contrasena,
                                    TelefonoCelular = p.TelefonoCelular,
                                    Telefono = p.Telefono,
                                    Correo = p.Correo,
                                    //FechaNacimiento = p.FechaNacimiento.Value.ToString("yyyy/MM/dd"),
                                    Sexo = p.Sexo,
                                    Calle = p.Calle,
                                    NumInt = p.NumInt,
                                    NumExt = p.NumExt,
                                    Colonia = p.Colonia,
                                    IdMunicipio = p.idMunicipio,
                                    //IdEstado = p.tMunicipio.tEstado.id_estado,
                                    Cp = p.CP
                                }).FirstOrDefault(),
                            EmpresaEnvio = "",
                            NumeroGuia = "",
                            Productos = s.tDetalleVentaEnLineas
                                .Where(d => d.idVenta == s.idVenta)
                                .Select(d => new DetailSaleOnLineViewModel()
                                {
                                    IdDetalleVenta = d.idDetalleVenta,
                                    IdProducto = d.idProducto,
                                    Precio = d.Precio,
                                    Cantidad = d.Cantidad,
                                    Descuento = d.Descuento,
                                    Producto = new ProductViewModel()
                                    {
                                        idProducto = d.tProducto.idProducto,
                                        Nombre = d.tProducto.Nombre,
                                        Codigo = d.tProducto.Codigo,
                                        Descripcion = d.tProducto.Descripcion,
                                        Extension = d.tProducto.Extension,
                                        urlImagen = d.tProducto.urlImagen,
                                        TipoImagen = d.tProducto.TipoImagen,
                                        NombreImagen = d.tProducto.NombreImagen,
                                        Imagen = d.tProducto.TipoImagen == 1
                                            ? "/Content/Products/" + d.tProducto.NombreImagen + d.tProducto.Extension
                                            : d.tProducto.urlImagen
                                    },
                                    Distribucion = d.tDistribucionVentaEnLineas.Select(o => new DistributionDetailSaleOnLineCartViewModel()
                                    {
                                        IdDistribucionVenta = o.idDistribucionVenta,
                                        IdDetalleVenta = o.idDetalleVenta,
                                        IdSucursal = o.idSucursal,
                                        Sucursal = o.tSucursale.Nombre,
                                        IdProducto = o.idProducto,
                                        Cantidad = o.Cantidad
                                    }).ToList()
                                }).ToList()
                        }).FirstOrDefault();
            }
        }

        public List<SaleOnLineViewModel> GetBySellerId(int idSeller)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return (from s in context.tVentasEnLineas
                        where s.idVendedor == idSeller
                        && (s.Estatus != 5 && s.Estatus != 6)
                        select new SaleOnLineViewModel
                        {
                            IdVenta = s.idVenta,
                            Remision = s.Remision,
                            IdCarrito = s.idCarrito,
                            IdCliente = s.idCliente,
                            Fecha = s.Fecha,
                            CantidadProductos = s.CantidadProductos,
                            Subtotal = s.Subtotal,
                            Descuento = s.Descuento,
                            Total = s.Total,
                            Estatus = s.Estatus,
                            NombreEstatus = (
                                s.Estatus == 1 ? "PENDIENTE POR CONFIRMAR" :
                                s.Estatus == 2 ? "COMPRA CONFIRMADA" :
                                s.Estatus == 3 ? "LISTO PARA RECOGER EN SUCURSAL" :
                                s.Estatus == 4 ? "COMPRA ENVIADA" :
                                s.Estatus == 5 ? "COMPRA ENTREGADA" :
                                s.Estatus == 6 ? "COMPRA CANCELADA" : "ESTATUS INDEFINIDO"
                            ),
                            EstatusOpenPay = s.EstatusOpenPay,
                            IdTipoEntrega = s.idTipoEntrega,
                            TipoEntrega = (s.idTipoEntrega == 1 ? "RECOGER EN SUCURSAL" : "ENVIO A DOMICILIO"),
                            Facturado = s.Facturado,
                            IdVendedor = s.idVendedor,
                            Vendedor = context.tUsuarios.Where(u => u.idUsuario == s.idVendedor).Select(u => new UserViewModel()
                            {
                                NombreCompleto = u.Nombre + " " + u.Apellidos
                            }).FirstOrDefault(),
                            IdSucursal = s.idSucursal,
                            Sucursal = (s.idSucursal == 2 ? "AMAZONAS" :
                                        s.idSucursal == 3 ? "GUADALQUIVIR" :
                                        s.idSucursal == 4 ? "TEXTURA" : "INDEFINIDO"),                            
                            Cliente = context.tClientesTiendaVirtuals
                                .Where(p => p.idCliente == s.idCliente).Select(p => new CustomerVirtualStoreViewModel()
                                {
                                    IdCliente = p.idCliente,
                                    Nombre = p.Nombre,
                                    Apellidos = p.Apellidos,
                                    Contrasena = p.Contrasena,
                                    TelefonoCelular = p.TelefonoCelular,
                                    Telefono = p.Telefono,
                                    Correo = p.Correo,
                                    //FechaNacimiento = p.FechaNacimiento.Value.ToString("yyyy/MM/dd"),
                                    Sexo = p.Sexo,
                                    Calle = p.Calle,
                                    NumInt = p.NumInt,
                                    NumExt = p.NumExt,
                                    Colonia = p.Colonia,
                                    IdMunicipio = p.idMunicipio,
                                    //IdEstado = p.tMunicipio.tEstado.id_estado,
                                    Cp = p.CP
                                }).FirstOrDefault(),
                            EmpresaEnvio = s.EmpresaEnvio,
                            NumeroGuia = s.NumeroGuia,
                            Productos = s.tDetalleVentaEnLineas
                                .Where(d => d.idVenta == s.idVenta)
                                .Select(d => new DetailSaleOnLineViewModel()
                                {
                                    IdDetalleVenta = d.idDetalleVenta,
                                    IdProducto = d.idProducto,
                                    Precio = d.Precio,
                                    Cantidad = d.Cantidad,
                                    Descuento = d.Descuento,
                                    Producto = new ProductViewModel()
                                    {
                                        idProducto = d.tProducto.idProducto,
                                        Nombre = d.tProducto.Nombre,
                                        Codigo = d.tProducto.Codigo,
                                        Descripcion = d.tProducto.Descripcion,
                                        Extension = d.tProducto.Extension,
                                        urlImagen = d.tProducto.urlImagen,
                                        TipoImagen = d.tProducto.TipoImagen,
                                        NombreImagen = d.tProducto.NombreImagen,
                                        Imagen = d.tProducto.TipoImagen == 1
                                            ? "/Content/Products/" + d.tProducto.NombreImagen + d.tProducto.Extension
                                            : d.tProducto.urlImagen
                                    },
                                    Distribucion = d.tDistribucionVentaEnLineas.Select(o => new DistributionDetailSaleOnLineCartViewModel()
                                    {
                                        IdDistribucionVenta = o.idDistribucionVenta,
                                        IdDetalleVenta = o.idDetalleVenta,
                                        IdSucursal = o.idSucursal,
                                        Sucursal = o.tSucursale.Nombre,
                                        IdProducto = o.idProducto,
                                        Cantidad = o.Cantidad
                                    }).ToList()
                                }).ToList()
                        }).ToList();
            }
        }

        public int GetLastId()
        {
            using (var context = new admDB_SAADDBEntities())
            {
                if (context.tVentasEnLineas.Count() > 0)
                {
                    return context.tVentasEnLineas.Max(p => p.idVenta);
                }
                else
                {
                    return 0;
                }
            }
        }
        public Tuple<int, List<tVentasEnLinea>> GetAll(DateTime start, DateTime end, int status, string customer, string user, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var dtStart = start.Date + new TimeSpan(0, 0, 0);
                var dtEnd = end.Date + new TimeSpan(23, 59, 59);

                var sales = context.tVentasEnLineas.Where(p => ((p.tClientesTiendaVirtual.Nombre + " " + p.tClientesTiendaVirtual.Apellidos).Contains(customer) || String.IsNullOrEmpty(customer))
                    && ((p.tUsuario.Nombre + " " + p.tUsuario.Apellidos).Contains(user) || String.IsNullOrEmpty(user))
                    && p.Fecha >= dtStart && p.Fecha <= dtEnd && p.Estatus == status)
                    .Include(p => p.tClientesTiendaVirtual)
                    .Include(p => p.tUsuario)
                    .OrderBy(p => p.Fecha)
                    .Skip(page * pageSize).Take(pageSize).ToList();

                var total = context.tVentasEnLineas.Where(p => ((p.tClientesTiendaVirtual.Nombre + " " + p.tClientesTiendaVirtual.Apellidos).Contains(customer) || String.IsNullOrEmpty(customer))
                    && ((p.tUsuario.Nombre + " " + p.tUsuario.Apellidos).Contains(user) || String.IsNullOrEmpty(user))
                    && p.Fecha >= dtStart && p.Fecha <= dtEnd && p.Estatus == status).Count();

                return Tuple.Create(total, sales);
            }
        }
        public tVentasEnLinea Add(tVentasEnLinea saleOnLine)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                context.tVentasEnLineas.Add(saleOnLine);

                context.SaveChanges();

                return saleOnLine;
            }
        }
        public tVentasEnLinea Update(tVentasEnLinea saleOnLine)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tVentasEnLineas.Find(saleOnLine.idVenta);

                if (entity == null)
                {
                    throw new System.ArgumentException("Los datos de la venta no fueron encontrados");
                }

                entity.Remision = saleOnLine.Remision;
                entity.idCarrito = saleOnLine.idCarrito;
                entity.idCliente = saleOnLine.idCliente;
                entity.idTipoEntrega = saleOnLine.idTipoEntrega;
                entity.Fecha = saleOnLine.Fecha;
                entity.CantidadProductos = saleOnLine.CantidadProductos;
                entity.Subtotal = saleOnLine.Subtotal;
                entity.Descuento = saleOnLine.Descuento;
                entity.GastosEnvio = saleOnLine.GastosEnvio;
                entity.Total = saleOnLine.Total;
                entity.NumeroFactura = saleOnLine.NumeroFactura;
                entity.Estatus = saleOnLine.Estatus;

                context.SaveChanges();

                return entity;
            }
        }
        public tVentasEnLinea UpdateStatus(int id, int status, int? branch)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tVentasEnLineas.Find(id);

                if (entity == null)
                {
                    throw new System.ArgumentException("Los datos del carrito de compras no fueron encontrados");
                }

                entity.Estatus = status;
                entity.idSucursal = branch;

                context.SaveChanges();

                return entity;
            }
        }

        public tVentasEnLinea UpdateBill(int id, string bill)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tVentasEnLineas.Find(id);

                if (entity == null)
                {
                    throw new System.ArgumentException("Los datos del carrito de compras no fueron encontrados");
                }

                entity.NumeroFactura = bill;
                entity.Facturado = true;

                context.SaveChanges();

                return entity;
            }
        }

        public tVentasEnLinea UpdateAssignedUser(int id, int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tVentasEnLineas.Find(id);

                if (entity == null)
                {
                    throw new System.ArgumentException("Los datos del carrito de compras no fueron encontrados");
                }

                entity.idVendedor = idUser;

                context.SaveChanges();

                return entity;
            }
        }

        public tVentasEnLinea UpdateSendingData(int id, string sendingProvider, string guideNumber)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tVentasEnLineas.Find(id);

                if (entity == null)
                {
                    throw new System.ArgumentException("Los datos del carrito de compras no fueron encontrados");
                }

                entity.EmpresaEnvio = sendingProvider;
                entity.NumeroGuia = guideNumber;

                context.SaveChanges();

                return entity;
            }
        }
    }
}
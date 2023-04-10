using ADEntities.Models;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Converts
{
    public class SalesOnLine
    {
        public tVentasEnLinea PrepareAdd(SaleOnLineViewModel model)
        {
            tVentasEnLinea result = new tVentasEnLinea();

            result.Remision = model.Remision;
            result.idCarrito = model.IdCarrito;
            result.idCliente = model.IdCliente;
            result.idTipoEntrega = model.IdTipoEntrega;
            result.Fecha = model.Fecha;
            result.CantidadProductos = model.CantidadProductos;
            result.Subtotal = model.Subtotal;
            result.Descuento = model.Descuento;
            result.GastosEnvio = model.GastosEnvio;
            result.Total = model.Total;
            result.NumeroFactura = model.NumeroFactura;
            result.Estatus = 1;
            result.IdOrdenOpenPay = model.IdOrdenOpenPay;
            result.EstatusOpenPay = model.EstatusOpenPay;

            return result;
        }

        public tVentasEnLinea PrepareUpdate(SaleOnLineViewModel model)
        {
            tVentasEnLinea result = new tVentasEnLinea();

            result.idVenta = model.IdVenta;
            result.Remision = model.Remision;
            result.idCarrito = model.IdCarrito;
            result.idCliente = model.IdCliente;
            result.idTipoEntrega = model.IdTipoEntrega;
            result.Fecha = model.Fecha;
            result.CantidadProductos = model.CantidadProductos;
            result.Subtotal = model.Subtotal;
            result.Descuento = model.Descuento;
            result.GastosEnvio = model.GastosEnvio;
            result.Total = model.Total;
            result.NumeroFactura = model.NumeroFactura;
            result.Estatus = model.Estatus;
            result.IdOrdenOpenPay = model.IdOrdenOpenPay;
            result.EstatusOpenPay = model.EstatusOpenPay;

            return result;
        }

        public SaleOnLineViewModel TableToModel(tVentasEnLinea entity)
        {
            if (entity == null)
            {
                throw new System.ArgumentException("El registro no fue encontrado");
            }

            SaleOnLineViewModel result = new SaleOnLineViewModel();

            result.IdVenta = entity.idVenta;
            result.Remision = entity.Remision;
            result.IdCarrito = entity.idCarrito;
            result.IdCliente = entity.idCliente;
            result.IdTipoEntrega = entity.idTipoEntrega;
            result.Fecha = entity.Fecha;
            result.CantidadProductos = entity.CantidadProductos;
            result.Subtotal = entity.Subtotal;
            result.Descuento = entity.Descuento;
            result.GastosEnvio = entity.GastosEnvio;
            result.Total = entity.Total;
            result.NumeroFactura = entity.NumeroFactura;
            result.Estatus = entity.Estatus;
            result.IdOrdenOpenPay = entity.IdOrdenOpenPay;
            result.EstatusOpenPay = entity.EstatusOpenPay;

            return result;
        }

        public List<SaleOnLineViewModel> EntitiesToList(List<tVentasEnLinea> entities)
        {
            if (entities == null)
            {
                throw new System.ArgumentException("No existen registros");
            }

            return entities.Select(p => new SaleOnLineViewModel()
            {
                IdVenta = p.idVenta,
                Remision = p.Remision,
                Cliente = new CustomerVirtualStoreViewModel()
                {
                    IdCliente = p.tClientesTiendaVirtual.idCliente,
                    Nombre = p.tClientesTiendaVirtual.Nombre,
                    Apellidos = p.tClientesTiendaVirtual.Apellidos,
                    Contrasena = p.tClientesTiendaVirtual.Contrasena,
                    TelefonoCelular = p.tClientesTiendaVirtual.TelefonoCelular,
                    Telefono = p.tClientesTiendaVirtual.Telefono
                    //Correo = p.tClientesTiendaVirtual.Correo,
                    //FechaNacimiento = p.tClientesTiendaVirtual.FechaNacimiento.Value.ToString("yyyy/MM/dd"),
                    //Sexo = p.tClientesTiendaVirtual.Sexo,
                    //Calle = p.tClientesTiendaVirtual.Calle,
                    //NumInt = p.tClientesTiendaVirtual.NumInt,
                    //NumExt = p.tClientesTiendaVirtual.NumExt,
                    //Colonia = p.tClientesTiendaVirtual.Colonia,
                    //IdMunicipio = p.tClientesTiendaVirtual.idMunicipio,
                    //IdEstado = p.tClientesTiendaVirtual.tMunicipio.tEstado.id_estado,
                    //Cp = p.tClientesTiendaVirtual.CP
                },
                Vendedor = new UserViewModel()
                {
                    NombreCompleto = (p.tUsuario != null)? p.tUsuario.Nombre + " " + p.tUsuario.Apellidos : ""
                },
                IdCarrito = p.idCarrito,
                IdCliente = p.idCliente,
                IdTipoEntrega = p.idTipoEntrega,
                Fecha = p.Fecha,
                CantidadProductos = p.CantidadProductos,
                Subtotal = p.Subtotal,
                Descuento = p.Descuento,
                GastosEnvio = p.GastosEnvio,
                Total = p.Total,
                NumeroFactura = p.NumeroFactura,
                Estatus = p.Estatus,
                NombreEstatus = (
                    p.Estatus == 1 ? "PENDIENTE POR CONFIRMAR" :
                    p.Estatus == 2 ? "COMPRA CONFIRMADA" :
                    p.Estatus == 3 ? "LISTO PARA RECOGER EN SUCURSAL" :
                    p.Estatus == 4 ? "COMPRA ENVIADA" :
                    p.Estatus == 5 ? "COMPRA ENTREGADA" :
                    p.Estatus == 6 ? "COMPRA CANCELADA" : "ESTATUS INDEFINIDO"
                ),
                IdOrdenOpenPay = p.IdOrdenOpenPay,
                EstatusOpenPay = p.EstatusOpenPay,
                IdSucursal = p.idSucursal
            }).ToList();
        }

        public tVentasEnLinea ModelToTable(SaleOnLineViewModel model)
        {
            tVentasEnLinea result = new tVentasEnLinea();

            result.idVenta = model.IdVenta;
            result.Remision = model.Remision;
            result.idCarrito = model.IdCarrito;
            result.idCliente = model.IdCliente;
            result.idTipoEntrega = model.IdTipoEntrega;
            result.Fecha = model.Fecha;
            result.CantidadProductos = model.CantidadProductos;
            result.Subtotal = model.Subtotal;
            result.Descuento = model.Descuento;
            result.GastosEnvio = model.GastosEnvio;
            result.Total = model.Total;
            result.NumeroFactura = model.NumeroFactura;
            result.Estatus = model.Estatus;
            result.IdOrdenOpenPay = model.IdOrdenOpenPay;
            result.EstatusOpenPay = model.EstatusOpenPay;

            return result;
        }
    }
}
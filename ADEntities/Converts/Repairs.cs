using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADEntities.Models;
using ADEntities.ViewModels;

namespace ADEntities.Converts
{
    public static class Repairs
    {
        public static RepairViewModel TableToModel(tReparacione repair)
        {
            RepairViewModel result = new RepairViewModel();

            result.idReparacion = repair.idReparacion;
            result.idReparacion = repair.idReparacion;
            result.Numero = repair.Numero;
            result.idUsuario = repair.idUsuario;
            result.Destino = repair.Destino;
            result.Responsable = repair.Responsable;
            result.FechaSalida = repair.FechaSalida;
            result.FechaRegreso = repair.FechaRegreso;
            result.Estatus = repair.Estatus;
            result.Comentarios = repair.Comentarios;
            result.CreadoPor = repair.CreadoPor;
            result.Creado = repair.Creado;
            result.ModificadoPor = repair.ModificadoPor;
            result.Modificado = repair.Modificado;
            result.lDetalle = repair.tDetalleReparaciones.Select(o => new DetailRepairViewModel()
            {
                idDetalleReparacion = o.idDetalleReparacion,
                idProducto = o.idProducto,
                idSucursal = o.idSucursal,
                Cantidad = o.Cantidad,
                Pendiente = o.Pendiente,
                Comentarios = o.Comentarios
            }).ToList();

            return result;
        }

        public static tReparacione ModelToTable(RepairViewModel repair)
        {
            tReparacione result = new tReparacione();

            result.idReparacion = repair.idReparacion;
            result.Numero = repair.Numero;
            result.idUsuario = repair.idUsuario;
            result.Destino = repair.Destino;
            result.Responsable = repair.Responsable;
            result.FechaSalida = repair.FechaSalida;
            result.FechaRegreso = repair.FechaRegreso;
            result.Estatus = repair.Estatus;
            result.Comentarios = repair.Comentarios;
            result.CreadoPor = repair.CreadoPor;
            result.Creado = repair.Creado;
            result.ModificadoPor = repair.ModificadoPor;
            result.Modificado = repair.Modificado;
            result.tDetalleReparaciones = (repair.lDetalle == null)? null : repair.lDetalle.Select(o => new tDetalleReparacione()
            {
                idDetalleReparacion = o.idDetalleReparacion,
                idProducto = o.idProducto,
                idSucursal = o.idSucursal,
                Cantidad = o.Cantidad,
                Pendiente = o.Pendiente,
                Comentarios = o.Comentarios
            }).ToList();

            return result;
        }
    }
}
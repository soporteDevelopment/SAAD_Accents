using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADEntities.Models;

namespace ADEntities.Converts
{
    public static class Egresses
    {
        public static EgressViewModel TableToModel(tSalida entity)
        {
            EgressViewModel result = new EgressViewModel();

            result.idSalida = entity.idSalida;
            result.Tipo = entity.Tipo;
            result.idEntrada = entity.idEntrada;
            result.idVenta = entity.idVenta;
            result.Fecha = entity.Fecha;
            result.RecibidaPor = entity.RecibidaPor;
            result.RecibidaOtro = entity.RecibidaOtro;
            result.Cantidad = entity.Cantidad;
            result.Comentarios = entity.Comentarios;
            result.fkSalida = entity.fkSalida;
            result.Estatus = entity.Estatus;
            result.CreadoPor = entity.CreadoPor;
            result.Creado = entity.Creado;
            result.ModificadoPor = entity.ModificadoPor;
            result.Modificado = entity.Modificado;

            return result;
        }

        public static tSalida ModelToTable(EgressViewModel model)
        {
            tSalida entity = new tSalida();

            entity.idSalida = model.idSalida;
            entity.Tipo = model.Tipo;
            entity.idEntrada = model.idEntrada;
            entity.idVenta = model.idVenta;
            entity.Fecha = model.Fecha;
            entity.RecibidaPor = model.RecibidaPor;
            entity.RecibidaOtro = model.RecibidaOtro;
            entity.Cantidad = model.Cantidad;
            entity.Comentarios = model.Comentarios;
            entity.fkSalida = model.fkSalida;
            entity.Estatus = model.Estatus;
            entity.CreadoPor = model.CreadoPor;
            entity.Creado = model.Creado;
            entity.ModificadoPor = model.ModificadoPor;
            entity.Modificado = model.Modificado;

            return entity;
        }
    }
}
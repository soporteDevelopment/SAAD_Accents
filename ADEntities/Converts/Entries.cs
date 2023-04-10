using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADEntities.Models;
using ADEntities.ViewModels;

namespace ADEntities.Converts
{
    public static class Entries
    {
        public static EntryViewModel TableToModel(tEntrada entity)
        {
            EntryViewModel result = new EntryViewModel();

            result.idEntrada = entity.idEntrada;
            result.Tipo = entity.Tipo;
            result.idVenta = entity.idVenta;
            result.Fecha = entity.Fecha;
            result.EntregadaPor = entity.EntregadaPor;
            result.EntregadaOtro = entity.EntregadaOtro;
            result.Cantidad = entity.Cantidad;
            result.Comentarios = entity.Comentarios;
            result.Estatus = entity.Estatus;
            result.CreadoPor = entity.CreadoPor;
            result.Creado = entity.Creado;
            result.ModificadoPor = entity.ModificadoPor;
            result.Modificado = entity.Modificado;

            return result;
        }

        public static tEntrada ModelToTable(EntryViewModel model)
        {
            tEntrada entity = new tEntrada();

            entity.idEntrada = model.idEntrada;
            entity.Tipo = model.Tipo;
            entity.idVenta = model.idVenta;
            entity.Fecha = model.Fecha;
            entity.EntregadaPor = model.EntregadaPor;
            entity.EntregadaOtro = model.EntregadaOtro;
            entity.Cantidad = model.Cantidad;
            entity.Comentarios = model.Comentarios;
            entity.Estatus = model.Estatus;
            entity.CreadoPor = model.CreadoPor;
            entity.Creado = model.Creado;
            entity.ModificadoPor = model.ModificadoPor;
            entity.Modificado = model.Modificado;

            return entity;
        }
    }
}
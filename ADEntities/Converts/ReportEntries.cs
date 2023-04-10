using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADEntities.Models;

namespace ADEntities.Converts
{
    public class ReportEntries
    {
        public static ReportEntriesViewModel TableToModel(tReporteCaja entity)
        {
            ReportEntriesViewModel result = new ReportEntriesViewModel();

            result.idReporteCaja = entity.idReporteCaja;
            result.Fecha = entity.Fecha;
            result.Comentarios = entity.Comentarios;
            result.CantidadIngreso = entity.CantidadIngreso;
            result.CantidadEgreso = entity.CantidadEgreso;
            result.Revisado = entity.Revisado;
            result.CreadoPor = entity.CreadoPor;
            result.Creado = entity.Creado;
            result.ModificadoPor = entity.ModificadoPor;
            result.Modificado = entity.Modificado;

            return result;
        }

        public static tReporteCaja ModelToTable(ReportEntriesViewModel model)
        {
            tReporteCaja entity = new tReporteCaja();

            entity.idReporteCaja = model.idReporteCaja;
            entity.Fecha = model.Fecha;
            entity.Comentarios = model.Comentarios;
            entity.CantidadIngreso = model.CantidadIngreso;
            entity.CantidadEgreso = model.CantidadEgreso;
            entity.Revisado = model.Revisado;
            entity.CreadoPor = model.CreadoPor;
            entity.Creado = model.Creado;
            entity.ModificadoPor = model.ModificadoPor;
            entity.Modificado = model.Modificado;

            return entity;
        }
    }
}
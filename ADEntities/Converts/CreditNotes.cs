using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Converts
{
    /// <summary>
    /// Class convert to CreditNote
    /// </summary>
    public static class CreditNotes
    {
        /// <summary>
        /// Tables to model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static CreditNoteViewModel TableToModel(tNotasCredito entity)
        {
            CreditNoteViewModel result = new CreditNoteViewModel();

            result.idTipoNotaCredito = entity.idTipoNotaCredito;
            result.idNotaCredito = entity.idNotaCredito;
            result.RemisionCredito = entity.Remision;
            result.idVenta = entity.idVenta;
            result.idVendedor = entity.idVendedor;
            result.Cantidad = entity.Cantidad;
            result.Fecha = entity.Fecha;
            result.FechaVigencia = entity.FechaVigencia;
            result.Estatus = entity.Estatus;
            result.Comentarios = entity.Comentarios;

            return result;
        }

        /// <summary>
        /// Models to table.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static tNotasCredito ModelToTable(CreditNoteRequestViewModel model)
        {
            tNotasCredito result = new tNotasCredito();

            result.idTipoNotaCredito = model.idTipoNotaCredito;
            result.idVenta = model.idVenta;
            result.idVendedor = model.CreadoPor;
            result.Cantidad = model.Cantidad;
            result.Fecha = DateTime.Now;
            result.FechaVigencia = DateTime.Now.AddYears(1);
            result.Estatus = TypesCredit.creditoPendiente;
            result.Comentarios = model.Comentarios;
            result.idCustomerP = (model.idClienteFisico == 0)? null: (int?)model.idClienteFisico;
            result.idCustomerM = (model.idClienteMoral == 0)? null : (int?)model.idClienteMoral;
            result.idDespacho = (model.idDespacho == 0)? null : (int?)model.idDespacho;
            result.CreadoPor = model.CreadoPor;
            result.Creado = DateTime.Now;

            return result;
        }
    }
}
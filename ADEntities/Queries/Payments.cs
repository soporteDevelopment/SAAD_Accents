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
    public class Payments
    {
        /// <summary>
        /// Updates the payment.
        /// </summary>
        /// <param name="idPayment">The identifier payment.</param>
        /// <param name="idCreditNote">The identifier credit note.</param>
        public void UpdatePayment(int idPayment, int idCreditNote)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tFormaPagoes.Where(p => p.idFormaPago == idPayment).FirstOrDefault();
               
                if(entity != null)
                {
                    entity.idNotaCredito = idCreditNote;
                    entity.Estatus = TypesPayment.iCanceladaCredito;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Updates the history payment.
        /// </summary>
        /// <param name="idHistoryPayment">The identifier history payment.</param>
        /// <param name="idCreditNote">The identifier credit note.</param>
        public void UpdateHistoryPayment(int idHistoryPayment, int idCreditNote)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tFormaPagoCreditoes.Where(p => p.idHistorialCredito == idHistoryPayment).FirstOrDefault();

                if (entity != null)
                {
                    entity.idNotaCredito = idCreditNote;
                    context.SaveChanges();
                }

                var history = context.tHistorialCreditoes.Where(p => p.idHistorialCredito == idHistoryPayment).FirstOrDefault();

                if(history != null)
                {
                    history.Estatus = TypesPayment.iCanceladaCredito;
                    context.SaveChanges();
                }
            }
        }
    }
}
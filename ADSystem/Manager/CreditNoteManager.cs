using ADEntities.Models;
using ADEntities.Queries;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager
{
    /// <summary>
    /// Manager of CreditNote
    /// </summary>
    public class CreditNoteManager
    {
        public Credits _creditNotes = new Credits();
        public Payments _payments = new Payments();

        /// <summary>
        /// Adds the specified credit note.
        /// </summary>
        /// <param name="creditNote">The credit note.</param>
        /// <returns></returns>
        public CreditNoteViewModel Add(CreditNoteRequestViewModel creditNote)
        {
            if (_creditNotes.ValidateIfSaleHasCreditPayment(creditNote.idVenta))
            {
                throw new Exception("La venta cuenta con crédito pendiente por saldar");
            }

            var entity = ADEntities.Converts.CreditNotes.ModelToTable(creditNote);
            var result = _creditNotes.AddCreditNote(entity);

            PaymentCreditNote(creditNote.FormaPago, result.idNotaCredito);

            return ADEntities.Converts.CreditNotes.TableToModel(result);
        }

        /// <summary>
        /// Payments the credit note.
        /// </summary>
        /// <param name="payments">The payments.</param>
        /// <param name="creditNoteId">The credit note identifier.</param>
        public void PaymentCreditNote(List<PaymentCreditNoteViewModel> payments, int idCreditNote)
        {
            foreach (var payment in payments)
            {
                if (!payment.History)
                {
                    _payments.UpdatePayment(payment.idFormaPago, idCreditNote);
                }
                else
                {
                    _payments.UpdateHistoryPayment(payment.idFormaPago, idCreditNote);
                }
            }
        }
    }
}
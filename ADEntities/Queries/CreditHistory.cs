using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class CreditHistory : Base
    {
        public bool Delete(int? id)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var history = context.tHistorialCreditoes.Find(id);

                var payments = context.tFormaPagoCreditoes.Where(p => p.idHistorialCredito == id).ToList();

                foreach (var payment in payments)
                {
                    context.tFormaPagoCreditoes.Remove(payment);
                }

                context.tHistorialCreditoes.Remove(history);
                context.SaveChanges();

                return true;
            }
        }
    }
}
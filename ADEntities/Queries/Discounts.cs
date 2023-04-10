using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class Discounts : Base
    {

        public decimal GetDiscount(decimal amount)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    //return context.tDescuentos.Where(p => amount >= p.RangoInicial && amount <= p.RangoFinal && p.TipoCliente == 1).Select(p => p.Porcentaje).FirstOrDefault()??0;
                    return 0;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public decimal GetDiscountOffice(decimal amount)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    //return context.tDescuentos.Where(p => amount > p.RangoInicial && amount <= p.RangoFinal && p.TipoCliente == 2).Select(p => p.Porcentaje).FirstOrDefault() ?? 0;
                    return 0;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

        public decimal GetDiscountSpecial(decimal amount)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    //return context.tDescuentos.Where(p => amount > p.RangoInicial && amount <= p.RangoFinal && p.TipoCliente == 3).Select(p => p.Porcentaje).FirstOrDefault() ?? 0;
                    return 0;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;

                    
                }
        }

    }
}
using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class TypePromotions
    {
        public List<TypePromotionViewModel> Get()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tTipoPromocions.Where(p => p.Activo == true)
                            .Select(p => new TypePromotionViewModel()
                            {
                                idTipoPromocion = p.idTipoPromocion,
                                Descripcion = p.Descripcion,
                                Activo = p.Activo
                            }).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}
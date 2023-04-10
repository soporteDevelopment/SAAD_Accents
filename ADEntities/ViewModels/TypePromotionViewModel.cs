using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class TypePromotionViewModel
    {
        public int idTipoPromocion { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> Activo { get; set; }
    }
}
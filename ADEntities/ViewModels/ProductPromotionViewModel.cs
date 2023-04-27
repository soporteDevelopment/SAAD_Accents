using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class ProductPromotionViewModel
    {
        public int idPromocion { get; set; }
        public int idTipoPromocion { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> Costo { get; set; }
        public decimal Cantidad { get; set; }
    }
}
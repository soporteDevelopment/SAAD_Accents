using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class ProductStockViewModel: ProductViewModel
    {

        public int idProductosInventario { get; set; }

        public int idInventario { get; set; }

        public short Contabilizado { get; set; }

        public decimal Precio { get; set; }

        public bool Inventariado { get; set; }

    }
}
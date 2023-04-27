using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class DetailPromotionViewModel
    {
        public int idDetallePromocion { get; set; }
        public int idPromocion { get; set; }
        public int idProducto { get; set; }
        public decimal Cantidad { get; set; }
        public ProductViewModel Producto { get; set; }
    }
}
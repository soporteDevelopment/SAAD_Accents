using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class DetailSaleOnLineViewModel
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int? IdProducto { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Descuento { get; set; }
        public int? IdPromocion { get; set; }
        public ProductViewModel Producto { get; set; }
        public List<DistributionDetailSaleOnLineCartViewModel> Distribucion { get; set; }
    }
}
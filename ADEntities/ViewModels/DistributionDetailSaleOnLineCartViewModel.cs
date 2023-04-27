using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class DistributionDetailSaleOnLineCartViewModel
    {
        public int IdDistribucionVenta { get; set; }
        public int IdDetalleVenta { get; set; }
        public int IdSucursal { get; set; }
        public string Sucursal { get; set; }
        public int IdProducto { get; set; }
        public decimal? Cantidad { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels.VirtualStore
{
    public class DetailShoppingCartViewModel
    {
        public int idDetalleCarrito { get; set; }
        public Nullable<int> idCarrito { get; set; }
        public Nullable<int> idProducto { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public Nullable<decimal> Cantidad { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<int> idPromocion { get; set; }
        public ProductViewModel Producto { get; set; }
    }
}
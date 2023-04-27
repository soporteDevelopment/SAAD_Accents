using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels.VirtualStore
{
    public class ShoppingCartViewModel
    {
        public int idCarrito { get; set; }
        public Nullable<int> idCliente { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<decimal> CantidadProductos { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<int> Estatus { get; set; }
        public List<DetailShoppingCartViewModel> Productos { get; set; }
    }
}
using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class ShoppingCartViewModel
    {
        public int idProducto { get; set; }
        public string imagen { get; set; }
        public string codigo { get; set; }
        public string desc { get; set; }
        public decimal prec { get; set; }
        public int existencia { get; set; }
        public int stock { get; set; }
        public int cantidad { get; set; }
        public decimal costo { get; set; }
        public bool servicio { get; set; }
        public string comentarios { get; set; }
        
    }

    public class ShoppingCartList 
    {
        public List<ShoppingCartViewModel> ShoppingCartDetail { get; set; }
    }

}

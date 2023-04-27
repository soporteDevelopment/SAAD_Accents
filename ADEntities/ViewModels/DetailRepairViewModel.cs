using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class DetailRepairViewModel
    {
        public int idDetalleReparacion { get; set; }
        public int idReparacion { get; set; }
        public int idProducto { get; set; }
        public ProductViewModel Producto { get; set; }
        public int idSucursal { get; set; }
        public Nullable<decimal> Cantidad { get; set; }
        public Nullable<decimal> Pendiente { get; set; }
        public string Comentarios { get; set; }
    }
}
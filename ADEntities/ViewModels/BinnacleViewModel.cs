using System;

namespace ADEntities.ViewModels
{
    public class BinnacleViewModel
    {
        public int idBitacora { get; set; }

        public int idUsuario { get; set; }

        public string Usuario { get; set; }

        public string Sucursal { get; set; }

        public string Descripcion { get; set; }

        public ProductViewModel Producto { get; set; }

        public decimal? InventarioAnterior { get; set; }

        public decimal? InventarioActual { get; set; }

        public Nullable<decimal> PrecioVentaAnterior { get; set; }

        public Nullable<decimal> PrecioVentaNuevo { get; set; }

        public string Comentarios { get; set; }

        public DateTime? Fecha { get; set; }
    }
}